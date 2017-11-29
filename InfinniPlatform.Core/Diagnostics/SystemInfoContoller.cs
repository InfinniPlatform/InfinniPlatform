using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using InfinniPlatform.Dynamic;
using InfinniPlatform.Http;
using InfinniPlatform.Properties;
using InfinniPlatform.Serialization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace InfinniPlatform.Diagnostics
{
    /// <summary>
    /// Реализует REST-сервис для получения информации о системе.
    /// </summary>
    public sealed class SystemInfoContoller : Controller
    {
        private const int SubsystemTimeout = 1000;

        public SystemInfoContoller(IEnumerable<ISubsystemStatusProvider> subsystemStatusProviders,
                                   IHostAddressParser hostAddressParser,
                                   ILogger<SystemInfoContoller> logger)
        {
            _subsystemStatusProviders = subsystemStatusProviders.ToDictionary(p => p.Name, p => p);
            _hostAddressParser = hostAddressParser;
            _logger = logger;
        }


        private readonly Dictionary<string, ISubsystemStatusProvider> _subsystemStatusProviders;
        private readonly IHostAddressParser _hostAddressParser;
        private readonly ILogger _logger;

        public async Task<object> GetStatus()
        {
            var onBefore = await OnBefore();

            if (onBefore != null)
            {
                return onBefore;
            }

            return Json(await GetStatus(false));
        }

        [HttpGet("info")]
        [HttpPost("info")]
        public async Task<IActionResult> GetExpandedStatus()
        {
            var onBefore = await OnBefore();

            if (onBefore != null)
            {
                return onBefore;
            }

            return Json(await GetStatus(true));
        }

        [HttpGet("info/{subsystemName}")]
        [HttpPost("info/{subsystemName}")]
        public async Task<object> GetSubsystemStatus(string subsystemName)
        {
            var onBefore = await OnBefore();

            if (onBefore != null)
            {
                return onBefore;
            }

            _subsystemStatusProviders.TryGetValue(subsystemName, out var subsystem);

            var statusExpanded = (await GetSubsystemStatus(subsystem, ParseTimeout(Request.Query["timeout"])))?.Item1;

            return Json(statusExpanded);
        }

        private async Task<IActionResult> OnBefore()
        {
            // TODO On before
            // Запрос статуса разрешен только с локального узла
            if (!await _hostAddressParser.IsLocalAddress(Request.Host.Host))
            {
                return Forbid();
            }

            return null;
        }


        /// <summary>
        /// Возвращает объект с описанием статуса системы.
        /// </summary>
        private async Task<IDictionary<string, object>> GetStatus(bool expanded)
        {
            var ok = true;

            // Версия системы
            var version = GetSystemVersion();

            var status = new DynamicDocument
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
                        var subsystemName = subsystem.Key;

                        if (expanded)
                        {
                            // Определение статуса подсистемы
                            var subsystemStatus = await GetSubsystemStatus(subsystem.Value, ParseTimeout(Request.Query["timeout"]));
                            status[subsystemName] = subsystemStatus.Item1;
                            ok &= subsystemStatus.Item2;
                        }
                        else
                        {
                            // Формирование ссылки на статусную страницу подсистемы
                            status[subsystemName] = new DynamicDocument { { "ref", $"{Request.Scheme}://{Request.Host}/info/{subsystemName}" } };
                        }
                    }
                    catch (Exception exception)
                    {
                        _logger.LogWarning(exception);

                        ok = false;
                    }
                }
            }

            status["ok"] = ok;

            return status.ToDictionary();
        }


        /// <summary>
        /// Возвращает объект с описанием статуса подсистемы и успешности его определения.
        /// </summary>
        private async Task<Tuple<object, bool>> GetSubsystemStatus(ISubsystemStatusProvider subsystem, int timeout)
        {
            var ok = false;

            object status;

            try
            {
                var statusTask = subsystem.GetStatus(Request);

                // Исключается возможность бесконечного ожидания ответа от подсистемы
                if (await Task.WhenAny(statusTask, Task.Delay(timeout)) == statusTask)
                {
                    // Подсистема успешно вернула свой статус
                    status = statusTask.Result ?? new DynamicDocument();
                    ok = true;
                }
                else
                {
                    // Подсистема не отвечала длительное время
                    status = new DynamicDocument { { "error", Resources.SubsystemIsNotResponding } };
                }
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception);

                // При определении статуса произошло исключение
                status = new DynamicDocument { { "error", exception.GetFullMessage() } }.ToDictionary();
            }

            return new Tuple<object, bool>(status, ok);
        }


        /// <summary>
        /// Возвращает версию системы.
        /// </summary>
        private static Tuple<string, string> GetSystemVersion()
        {
            var assembly = typeof(SystemInfoContoller).GetTypeInfo().Assembly;
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new Tuple<string, string>(versionInfo.FileVersion, versionInfo.ProductVersion);
        }


        private static int ParseTimeout(StringValues timeout)
        {
            var result = 0;

            if (!StringValues.IsNullOrEmpty(timeout))
            {
                try
                {
                    result =  int.Parse(timeout);
                }
                catch
                {
                    // ignored
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