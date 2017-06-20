using System.Threading;
using InfinniPlatform.Aspects;

namespace InfinniPlatform.ServiceHost.Interception
{
    public interface ISyncInterface
    {
        void DoWork();
        int DoGenericWork();
    }

    [Aspect(typeof(PerformanceLoggerInterceptor))]
    public class SyncClass : ISyncInterface
    {
        public void DoWork()
        {
            Thread.Sleep(1000);
        }

        public int DoGenericWork()
        {
            Thread.Sleep(1000);
            return 1000;
        }
    }
}