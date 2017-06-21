using System.Threading.Tasks;

namespace InfinniPlatform.ServiceHost.Interception
{
    public class TestInterfaces
    {
        public interface ISyncInterface
        {
            void DoWork();
            int DoGenericWork();
        }

        public interface ITaskInterface
        {
            Task DoWork();
            Task<int> DoGenericWork();
        }

        public interface IAsyncInterface
        {
            Task DoWork();
            Task<int> DoGenericWork();
        }
    }
}