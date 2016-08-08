using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Session;

using Microsoft.AspNet.SignalR;

namespace InfinniPlatform.PushNotification.SignalR
{
    /// <summary>
    /// Точка обмена ASP.NET SignalR для реализации <see cref="SignalRPushNotificationService"/>.
    /// </summary>
    [LoggerName("SignalR")]
    public class SignalRPushNotificationServiceHub : Hub
    {
        public SignalRPushNotificationServiceHub(ITenantProvider tenantProvider,
                                                 IPerformanceLog performanceLog,
                                                 ILog log)
        {
            _tenantProvider = tenantProvider;
            _performanceLog = performanceLog;
            _log = log;
        }


        private readonly ITenantProvider _tenantProvider;
        private readonly IPerformanceLog _performanceLog;
        private readonly ILog _log;


        /// <summary>
        /// Добавляет пользователю подписку на уведомления текущей организации.
        /// </summary>
        /// <remarks>
        /// Должен вызываться Web-клиентом после входа пользователя в систему.
        /// </remarks>
        public async Task SubscribeTenant()
        {
            var startTime = DateTime.Now;

            Exception error = null;

            try
            {
                var tenantId = _tenantProvider.GetTenantId();

                if (!string.IsNullOrEmpty(tenantId))
                {
                    await Groups.Add(Context.ConnectionId, tenantId);
                }
            }
            catch (Exception exception)
            {
                error = exception;

                _log.Error(exception.Message, null, exception);

                throw;
            }
            finally
            {
                _performanceLog.Log(nameof(SubscribeTenant), startTime, error);
            }
        }
    }
}