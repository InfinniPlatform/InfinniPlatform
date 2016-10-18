using System;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Heartbeat.Settings;
using InfinniPlatform.Sdk.Hosting;

namespace InfinniPlatform.Heartbeat
{
    public class HeartbeatAppEventHandler : AppEventHandler
    {
        public HeartbeatAppEventHandler(HeartbeatSettings settings)
        {
            _settings = settings;
            _httpClient = new HttpClient
                          {
                              BaseAddress = new Uri(settings.ServerAddress)
                          };
        }

        private readonly HttpClient _httpClient;
        private readonly HeartbeatSettings _settings;

        public override void OnAfterStart()
        {
            Task.Run(async () =>
                     {
                         var period = _settings.Period;
                         var requestUri = new Uri("/beat");
                         var startedMessage = await _httpClient.PostAsync(requestUri, new StringContent("Started."));

                         if (startedMessage.IsSuccessStatusCode)
                         {
                             while (true)
                             {
                                 await Task.Delay(period);
                                 var workingMessage = await _httpClient.PostAsync(requestUri, new StringContent("Working."));

                                 if (!workingMessage.IsSuccessStatusCode)
                                 {
                                     
                                 }
                             }
                             // ReSharper disable once FunctionNeverReturns
                         }


                     });
        }

        public override void OnAfterStop()
        {
            base.OnAfterStop();
            //send to server
        }
    }
}