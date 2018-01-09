using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using InfinniPlatform.Dynamic;
using InfinniPlatform.Http;
using InfinniPlatform.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace InfinniPlatform.Diagnostics
{
    /// <summary>
    /// Provides HTTP API for subsystem status.
    /// </summary>
    public sealed class SystemInfoContoller : Controller
    {
        private const int SubsystemTimeout = 1000;
        private readonly ILogger _logger;


        private readonly Dictionary<string, ISubsystemStatusProvider> _subsystemStatusProviders;

        /// <summary>
        /// Initializes a new instance of <see cref="SystemInfoContoller" />.
        /// </summary>
        /// <param name="subsystemStatusProviders">List of <see cref="ISubsystemStatusProvider"/> instances.</param>
        /// <param name="logger">Logger.</param>
        public SystemInfoContoller(IEnumerable<ISubsystemStatusProvider> subsystemStatusProviders,
                                   ILogger<SystemInfoContoller> logger)
        {
            _subsystemStatusProviders = subsystemStatusProviders.ToDictionary(p => p.Name, p => p);
            _logger = logger;
        }

        /// <inheritdoc />
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Запрос статуса разрешен только с локального узла
            if (!context.HttpContext.IsLocal())
            {
                _logger.LogWarning("Attemp to access to system info page from remote machine.");

                context.HttpContext.Response.StatusCode = 403;
                await context.HttpContext.Response.WriteAsync("Forbidden.");

                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }

        /// <summary>
        /// Returns summary status of subsystems.
        /// </summary>
        public async Task<IActionResult> GetStatus()
        {
            return Json(await GetStatus(false));
        }

        /// <summary>
        /// Returns full status of subsystems.
        /// </summary>
        /// <returns></returns>
        [HttpGet("info")]
        [HttpPost("info")]
        public async Task<IActionResult> GetExpandedStatus()
        {
            return Json(await GetStatus(true));
        }

        /// <summary>
        /// Returns subsystem status by name.
        /// </summary>
        /// <param name="subsystemName">Subsystem name.</param>
        [HttpGet("info/{subsystemName}")]
        [HttpPost("info/{subsystemName}")]
        public async Task<IActionResult> GetSubsystemStatus(string subsystemName)
        {
            _subsystemStatusProviders.TryGetValue(subsystemName, out var subsystem);

            var statusExpanded = (await GetSubsystemStatus(subsystem, ParseTimeout(Request.Query["timeout"])))?.Item1;

            return Json(statusExpanded);
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
                {"ok", true},
                {"version", version.Item1},
                {"versionHash", version.Item2}
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
                            status[subsystemName] = new DynamicDocument {{"ref", $"{Request.Scheme}://{Request.Host}/info/{subsystemName}"}};
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
                    status = new DynamicDocument {{"error", Resources.SubsystemIsNotResponding}};
                }
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception);

                // При определении статуса произошло исключение
                status = new DynamicDocument {{"error", exception.GetFullMessage()}}.ToDictionary();
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
                    result = int.Parse(timeout);
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