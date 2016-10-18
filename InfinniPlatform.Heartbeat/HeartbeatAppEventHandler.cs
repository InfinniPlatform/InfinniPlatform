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
        }

        private readonly HeartbeatSettings _settings;

        public override void OnAfterStart()
        {
            //send to server
            var period = _settings.Period;

            Task.Run(async () =>
                     {
                         while (true)
                         {
                             await Task.Delay(period);
                         }
                         // ReSharper disable once FunctionNeverReturns
                     });
        }

        public override void OnAfterStop()
        {
            base.OnAfterStop();
            //send to server
        }
    }
}