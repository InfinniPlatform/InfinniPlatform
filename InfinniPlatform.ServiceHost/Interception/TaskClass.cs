using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Aspects;

namespace InfinniPlatform.ServiceHost.Interception
{
    public interface ITaskInterface
    {
        Task DoWork();
        Task<int> DoGenericWork();
    }

    [Aspect(typeof(PerformanceLoggerInterceptor))]
    public class TaskClass : ITaskInterface
    {
        public Task DoWork()
        {
            return Task.Run(() => Thread.Sleep(1000));
        }

        public Task<int> DoGenericWork()
        {
            return Task.Run(() =>
            {
                Thread.Sleep(1000);
                return 1000;
            });
        }
    }
}