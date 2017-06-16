using System.Threading;

namespace InfinniPlatform.ServiceHost.Interception
{
    public interface ISyncInterface
    {
        void DoWork();
        int DoGenericWork();
    }

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