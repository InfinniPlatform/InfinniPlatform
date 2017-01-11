using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

using InfinniPlatform.Core.Properties;
using InfinniPlatform.Sdk.Diagnostics;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Logging;

namespace InfinniPlatform.Core.Diagnostics
{
    /// <summary>
    /// Реализует REST-сервис для получения информации о системе.
    /// </summary>
    internal sealed class SystemInfoHttpService : IHttpService
    {
        private const int SubsystemTimeout = 1000;


        public SystemInfoHttpService(IEnumerable<ISubsystemStatusProvider> subsystemStatusProviders, IHostAddressParser hostAddressParser, ILog log)
        {
            _subsystemStatusProviders = subsystemStatusProviders;
            _hostAddressParser = hostAddressParser;
            _log = log;
        }


        private readonly IEnumerable<ISubsystemStatusProvider> _subsystemStatusProviders;
        private readonly IHostAddressParser _hostAddressParser;
        private readonly ILog _log;


        public void Load(IHttpServiceBuilder builder)
        {
            builder.OnBefore = async r =>
                               {
                                   // Запрос статуса разрешен только с локального узла
                                   if (!await _hostAddressParser.IsLocalAddress(r.UserHostAddress))
                                   {
                                       return Task.FromResult<object>(HttpResponse.Forbidden);
                                   }

                                   return Task.FromResult<object>(null);
                               };

            // Краткая информация о статусе системы
            builder.Get["/"] = async r => await GetStatus(r, false, ParseTimeout(r.Query.timeout));
            builder.Post["/"] = async r => await GetStatus(r, false, ParseTimeout(r.Query.timeout));

            // Подробная информация о статусе системы
            builder.Get["/info"] = async r => await GetStatus(r, true, ParseTimeout(r.Query.timeout));
            builder.Post["/info"] = async r => await GetStatus(r, true, ParseTimeout(r.Query.timeout));

            // Подробная информация о статусе подсистем

            if (_subsystemStatusProviders != null)
            {
                foreach (var subsystem in _subsystemStatusProviders)
                {
                    try
                    {
                        var subsystemName = subsystem.Name;
                        builder.Get[$"/info/{subsystemName}"] = async r => (await GetSubsystemStatus(subsystem, r, ParseTimeout(r.Query.timeout)))?.Item1;
                        builder.Post[$"/info/{subsystemName}"] = async r => (await GetSubsystemStatus(subsystem, r, ParseTimeout(r.Query.timeout)))?.Item1;
                    }
                    catch (Exception exception)
                    {
                        _log.Warn(exception);
                    }
                }
            }
        }


        /// <summary>
        /// Возвращает объект с описанием статуса системы.
        /// </summary>
        private async Task<object> GetStatus(IHttpRequest request, bool expanded, int timeout)
        {
            var ok = true;

            // Версия системы
            var version = GetSystemVersion();

            var status = new DynamicWrapper
                         {
                             { "ok", true },
                             { "version", version.Item1 },
                             { "versionHash", version.Item2 }
                         };

            if (_subsystemStatusProviders != null)
            {
                foreach (var subsystem in _subsystemStatusProviders)
                {
                    try
                    {
                        var subsystemName = subsystem.Name;

                        if (expanded)
                        {
                            // Определение статуса подсистемы
                            var subsystemStatus = await GetSubsystemStatus(subsystem, request, timeout);
                            status[subsystemName] = subsystemStatus.Item1;
                            ok &= subsystemStatus.Item2;
                        }
                        else
                        {
                            // Формирование ссылки на статусную страницу подсистемы
                            status[subsystemName] = new DynamicWrapper { { "ref", $"{request.BasePath}/info/{subsystemName}" } };
                        }
                    }
                    catch (Exception exception)
                    {
                        _log.Warn(exception);

                        ok = false;
                    }
                }
            }

            status["ok"] = ok;

            return status;
        }


        /// <summary>
        /// Возвращает объект с описанием статуса подсистемы и успешности его определения.
        /// </summary>
        private async Task<Tuple<object, bool>> GetSubsystemStatus(ISubsystemStatusProvider subsystem, IHttpRequest request, int timeout)
        {
            var ok = false;

            object status;

            try
            {
                var statusTask = subsystem.GetStatus(request);

                // Исключается возможность бесконечного ожидания ответа от подсистемы
                if (await Task.WhenAny(statusTask, Task.Delay(timeout)) == statusTask)
                {
                    // Подсистема успешно вернула свой статус
                    status = statusTask.Result ?? new DynamicWrapper();
                    ok = true;
                }
                else
                {
                    // Подсистема не отвечала длительное время
                    status = new DynamicWrapper { { "error", Resources.SubsystemIsNotResponding } };
                }
            }
            catch (Exception exception)
            {
                _log.Warn(exception);

                // При определении статуса произошло исключение
                status = new DynamicWrapper { { "error", exception.GetFullMessage() } };
            }

            return new Tuple<object, bool>(status, ok);
        }


        /// <summary>
        /// Возвращает версию системы.
        /// </summary>
        private static Tuple<string, string> GetSystemVersion()
        {
            var assembly = typeof(SystemInfoHttpService).GetTypeInfo().Assembly;
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new Tuple<string, string>(versionInfo.FileVersion, versionInfo.ProductVersion);
        }


        private static int ParseTimeout(dynamic timeout)
        {
            var result = 0;

            if (timeout != null)
            {
                try
                {
                    result = (int)timeout;
                }
                catch
                {
                }
            }

            if (result < SubsystemTimeout)
            {
                result = SubsystemTimeout;
            }

            return result;
        }
    }
}