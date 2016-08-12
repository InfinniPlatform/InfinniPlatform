using System;
using System.Threading.Tasks;

using InfinniPlatform.PushNotification.Properties;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Session;

using Microsoft.AspNet.SignalR;

namespace InfinniPlatform.PushNotification.SignalR
{
    /// <summary>
    /// Точка обмена ASP.NET SignalR для реализации <see cref="SignalRPushNotificationService"/>.
    /// </summary>
    /// <remarks>
    /// При создании нового подключения - при вызове метода <see cref="OnConnected"/> или <see cref="OnReconnected"/> -
    /// осуществляется попытка идентифицировать текущего пользователя. Если пользователь известен, то происходит
    /// определение идентификатора его организации. Идентификатор организации выступает именем группы, в которую
    /// добавляется текущее подключение. Таким образом, все соединения группируются по организациям, к которым
    /// они относятся, благодаря чему появляется возможность делать отправку сообщений пользователям определенной
    /// организации. При удалении существующего подключения никаких действий не производится, поскольку подключение
    /// будет удалено из групп средствами самого SignalR.
    /// </remarks>
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


        public override Task OnConnected()
        {
            return AddConnectionToTenant(nameof(OnConnected));
        }

        public override Task OnReconnected()
        {
            return AddConnectionToTenant(nameof(OnReconnected));
        }


        private async Task AddConnectionToTenant(string methodName)
        {
            var startTime = DateTime.Now;

            Exception error = null;

            try
            {
                // Идентификационные данные текущего пользователя
                var identity = Context.User?.Identity;

                // Анонимные соединения будут проигнорированы
                if (identity != null && identity.IsAuthenticated)
                {
                    // Идентификатор организации текущего пользователя
                    var tenantId = _tenantProvider.GetTenantId(identity);

                    if (!string.IsNullOrEmpty(tenantId))
                    {
                        // Изменение связи соединения с группой организации пользователя
                        await Groups.Add(Context.ConnectionId, tenantId);
                    }
                }
                else
                {
                    _log.Warn(string.Format(Resources.UnknownUser, methodName));
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
                _performanceLog.Log(methodName, startTime, error);
            }
        }
    }
}