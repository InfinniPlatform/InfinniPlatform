using System;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Heartbeat.Settings;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Heartbeat
{
    public class HeartbeatAppEventHandler : AppEventHandler
    {
        public HeartbeatAppEventHandler(HeartbeatSettings settings,
                                        IJsonObjectSerializer serializer,
                                        IAppEnvironment environment)
        {
            _settings = settings;
            _serializer = serializer;
            _environment = environment;
            _httpClient = new HttpClient
                          {
                              BaseAddress = new Uri(settings.ServerAddress)
                          };
        }

        private readonly IAppEnvironment _environment;
        private readonly HttpClient _httpClient;
        private readonly IJsonObjectSerializer _serializer;
        private readonly HeartbeatSettings _settings;

        public override void OnAfterStart()
        {
            Task.Run(async () => { await SendBeats(); });
        }

        private async Task SendBeats()
        {
            var period = _settings.Period;
            var requestUri = new Uri("/server/beat", UriKind.Relative);

            var beatMessage = new DynamicWrapper
                              {
                                  { "Message", "Application started." },
                                  { "Name", _environment.Name },
                                  { "InstanceId", _environment.InstanceId }
                              };

            var requestContent = new StringContent(_serializer.ConvertToString(beatMessage), _serializer.Encoding, HttpConstants.JsonContentType);

            var startedMessage = await _httpClient.PostAsync(requestUri, requestContent);

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

                    var workingMessage = await _httpClient.PostAsync(requestUri, requestContent);

                    if (!workingMessage.IsSuccessStatusCode)
                    {
                    }
                }
                // ReSharper disable once FunctionNeverReturns
            }
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