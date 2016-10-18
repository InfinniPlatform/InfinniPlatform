using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Heartbeat.Settings;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Heartbeat
{
    public class HeartbeatAppEventHandler : AppEventHandler
    {
        private const string ServerHeartbeatPath = "/server/heartbeat";

        public HeartbeatAppEventHandler(HeartbeatSettings settings,
                                        IJsonObjectSerializer serializer,
                                        IAppEnvironment environment,
                                        ILog log)
        {
            _settings = settings;
            _serializer = serializer;
            _environment = environment;
            _log = log;

            var uri = new Uri(new Uri(settings.ServerAddress), ServerHeartbeatPath);

            _httpClient = new HttpClient
                          {
                              BaseAddress = uri
                          };
        }

        private readonly IAppEnvironment _environment;
        private readonly HttpClient _httpClient;
        private readonly ILog _log;
        private readonly IJsonObjectSerializer _serializer;
        private readonly HeartbeatSettings _settings;

        public override void OnAfterStart()
        {
            Task.Run(async () => { await SendBeats(); });
        }

        private async Task SendBeats()
        {
            var period = _settings.Period;


            var beatMessage = new DynamicWrapper
                              {
                                  { "Message", "Application started." },
                                  { "Name", _environment.Name },
                                  { "InstanceId", _environment.InstanceId }
                              };

            var requestContent = new StringContent(_serializer.ConvertToString(beatMessage), _serializer.Encoding, HttpConstants.JsonContentType);

            var startedMessage = await _httpClient.PostAsync(string.Empty, requestContent);

            if (startedMessage.IsSuccessStatusCode)
            {
                while (true)
                {
                    await Task.Delay(period * 1000);

                    beatMessage = new DynamicWrapper
                                  {
                                      { "Message", "Application is working." },
                                      { "Name", _environment.Name },
                                      { "InstanceId", _environment.InstanceId }
                                  };

                    requestContent = new StringContent(_serializer.ConvertToString(beatMessage), _serializer.Encoding, HttpConstants.JsonContentType);

                    var workingMessage = await _httpClient.PostAsync(string.Empty, requestContent);

                    if (!workingMessage.IsSuccessStatusCode)
                    {
                        var workResponce = await startedMessage.Content.ReadAsStringAsync();
                        _log.Error("Unable to send message.", () => new Dictionary<string, object> { { "Content", workResponce } });
                    }
                }
                // ReSharper disable once FunctionNeverReturns
            }

            var startResponce = await startedMessage.Content.ReadAsStringAsync();
            _log.Error("Unable to send message.", () => new Dictionary<string, object> { { "Content", startResponce } });
        }

        public override void OnBeforeStop()
        {
            var requestUri = new Uri("/server/heartbeat", UriKind.Relative);

            var beatMessage = new DynamicWrapper
                              {
                                  { "Message", "Application is stopping." },
                                  { "Name", _environment.Name },
                                  { "InstanceId", _environment.InstanceId }
                              };

            var requestContent = new StringContent(_serializer.ConvertToString(beatMessage), _serializer.Encoding, HttpConstants.JsonContentType);

            Task.Run(() => { _httpClient.PostAsync(requestUri, requestContent); })
                .Wait();
        }
    }
}