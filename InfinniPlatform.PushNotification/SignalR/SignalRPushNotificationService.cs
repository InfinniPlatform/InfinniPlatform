using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.PushNotification;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Session;

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace InfinniPlatform.PushNotification.SignalR
{
    /// <summary>
    /// Сервис для PUSH-уведомлений Web-клиентов на базе ASP.NET SignalR.
    /// </summary>
    [LoggerName("SignalR")]
    public class SignalRPushNotificationService : IPushNotificationService
    {
        public SignalRPushNotificationService(IDependencyResolver dependencyResolver,
                                              ITenantProvider tenantProvider,
                                              IJsonObjectSerializer jsonObjectSerializer,
                                              IPerformanceLog performanceLog,
                                              ILog log)
        {
            _dependencyResolver = dependencyResolver;
            _tenantProvider = tenantProvider;
            _jsonObjectSerializer = jsonObjectSerializer;
            _performanceLog = performanceLog;
            _log = log;
        }


        private readonly IDependencyResolver _dependencyResolver;
        private readonly ITenantProvider _tenantProvider;
        private readonly IJsonObjectSerializer _jsonObjectSerializer;
        private readonly IPerformanceLog _performanceLog;
        private readonly ILog _log;


        public Task NotifyAll(string messageType, object message)
        {
            return NotifyClients(nameof(NotifyAll), c => c.All, messageType, message);
        }

        public Task NotifyTenant(string tenantId, string messageType, object message)
        {
            return NotifyClients(nameof(NotifyTenant), c => c.Group(tenantId), messageType, message);
        }

        public Task NotifyCurrentTenant(string messageType, object message)
        {
            var tenantId = _tenantProvider.GetTenantId();

            return NotifyClients(nameof(NotifyCurrentTenant), c => c.Group(tenantId), messageType, message);
        }


        private async Task NotifyClients(string methodName, Func<IHubConnectionContext<dynamic>, dynamic> hubClientProxySelector, string messageType, object message)
        {
            var startTime = DateTime.Now;

            Exception error = null;

            try
            {
                var connectionManager = _dependencyResolver.Resolve<IConnectionManager>();

                if (connectionManager != null)
                {
                    var hubContext = connectionManager.GetHubContext<SignalRPushNotificationServiceHub>();

                    var hubConnectionContext = hubContext?.Clients;

                    if (hubConnectionContext != null)
                    {
                        var hubClientProxy = hubClientProxySelector(hubConnectionContext) as IClientProxy;

                        if (hubClientProxy != null)
                        {
                            var messageString = _jsonObjectSerializer.ConvertToString(message);

                            await hubClientProxy.Invoke(messageType, messageString);
                        }
                    }
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