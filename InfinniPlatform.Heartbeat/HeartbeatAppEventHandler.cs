using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Heartbeat.Properties;
using InfinniPlatform.Heartbeat.Settings;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Heartbeat
{
    public class HeartbeatAppEventHandler : AppEventHandler
    {
        public HeartbeatAppEventHandler(HeartbeatSettings settings,
                                        IJsonObjectSerializer serializer,
                                        IAppEnvironment environment,
                                        ILog log)
        {
            _serializer = serializer;
            _environment = environment;
            _log = log;

            _httpClient = new HttpClient
                          {
                              BaseAddress = new Uri($"http://{settings.ServerAddress}/server/heartbeat")
                          };

            _timer = new Timer(SendWorkingMessage, null, 0, Timeout.Infinite);

            _period = settings.Period * 1000;
        }

        private readonly IAppEnvironment _environment;
        private readonly HttpClient _httpClient;
        private readonly ILog _log;
        private readonly int _period;
        private readonly IJsonObjectSerializer _serializer;
        private readonly Timer _timer;

        public override void OnAfterStart()
        {
            Task.Run(async () =>
                     {
                         var beatMessage = new HeartbeatMessage(Resources.AppStarted, _environment.Name, _environment.InstanceId);
                         var requestContent = new StringContent(_serializer.ConvertToString(beatMessage), _serializer.Encoding, HttpConstants.JsonContentType);

                         var startedMessageResponse = await _httpClient.PostAsync(string.Empty, requestContent);

                         if (startedMessageResponse.IsSuccessStatusCode)
                         {
                             _timer.Change(0, _period);
                         }

                         var startResponce = await startedMessageResponse.Content.ReadAsStringAsync();
                         _log.Error(Resources.UnableSendMessage, () => new Dictionary<string, object> { { "Content", startResponce } });
                     });
        }

        public override void OnBeforeStop()
        {
            Task.Run(async () =>
                     {
                         var requestUri = new Uri(string.Empty, UriKind.Relative);
                         var beatMessageResponse = new HeartbeatMessage(Resources.AppStopping, _environment.Name, _environment.InstanceId);

                         var requestContent = new StringContent(_serializer.ConvertToString(beatMessageResponse), _serializer.Encoding, HttpConstants.JsonContentType);

                         await _httpClient.PostAsync(requestUri, requestContent);
                     });
        }

        private async void SendWorkingMessage(object state)
        {
            var beatMessage = new HeartbeatMessage(Resources.AppWorking, _environment.Name, _environment.InstanceId);
            var requestContent = new StringContent(_serializer.ConvertToString(beatMessage), _serializer.Encoding, HttpConstants.JsonContentType);

            var workingMessage = await _httpClient.PostAsync(string.Empty, requestContent);

            if (!workingMessage.IsSuccessStatusCode)
            {
                var workResponce = await workingMessage.Content.ReadAsStringAsync();
                _log.Error(Resources.UnableSendMessage, () => new Dictionary<string, object> { { "Content", workResponce } });
            }
        }
    }
}