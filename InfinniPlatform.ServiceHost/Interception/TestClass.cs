using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Aspects;

namespace InfinniPlatform.ServiceHost.Interception
{
    public class TestClass : ITestInterface
    {
        private const int Time = 100;

        public void DoSyncWork()
        {
            Thread.Sleep(Time);
        }

        public int DoGenericSyncWork()
        {
            Thread.Sleep(Time);
            return Time;
        }

        public async Task DoAsyncWork()
        {
            await Task.Delay(Time);
        }

        public async Task<int> DoGenericAsyncWork()
        {
            await Task.Delay(Time);
            return Time;
        }

        public Task DoTaskWork()
        {
            return Task.Delay(Time);
        }

        public Task<int> DoGenericTaskWork()
        {
            return Task.Delay(Time).ContinueWith(o => Time);
        }
    }

    [Aspect(typeof(PerformanceLoggerInterceptor))]
    public class InterceptedTestClass : IInterceptedTestInterface
    {
        private const int Time = 100;

        public void DoSyncWork()
        {
            Thread.Sleep(Time);
        }

        public int DoGenericSyncWork()
        {
            Thread.Sleep(Time);
            return Time;
        }

        public async Task DoAsyncWork()
        {
            await Task.Delay(Time);
        }

        public async Task<int> DoGenericAsyncWork()
        {
            await Task.Delay(Time);
            return Time;
        }

        public Task DoTaskWork()
        {
            return Task.Delay(Time);
        }

        public Task<int> DoGenericTaskWork()
        {
            return Task.Delay(Time).ContinueWith(o => Time);
        }
    }
}