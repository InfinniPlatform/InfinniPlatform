using System.Threading.Tasks;

namespace InfinniPlatform.ServiceHost.Interception
{
    public interface IAsyncInterface
    {
        Task DoWork();
        Task<int> DoGenericWork();
    }

    public class AsyncClass : IAsyncInterface
    {
        public async Task DoWork()
        {
            await Task.Delay(1000);
        }

        public async Task<int> DoGenericWork()
        {
            await Task.Delay(1000);
            return 1000;
        }
    }
}