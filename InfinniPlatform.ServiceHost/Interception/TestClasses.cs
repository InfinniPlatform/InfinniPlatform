using System;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Aspects;

namespace InfinniPlatform.ServiceHost.Interception
{
    public class TestClasses
    {
        [Aspect(typeof(PerformanceLoggerInterceptor))]
        public class SyncClass : TestInterfaces.ISyncInterface
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


        [Aspect(typeof(PerformanceLoggerInterceptor))]
        public class AsyncClass : TestInterfaces.IAsyncInterface
        {
            public async Task DoWork()
            {
                throw new NullReferenceException();
                await Task.Delay(1000);
            }

            public async Task<int> DoGenericWork()
            {
                await Task.Delay(1000);
                return 1000;
            }
        }


        [Aspect(typeof(PerformanceLoggerInterceptor))]
        public class TaskClass : TestInterfaces.ITaskInterface
        {
            public Task DoWork()
            {
                return Task.Delay(1000);
            }

            public Task<int> DoGenericWork()
            {
                return Task.Delay(1000).ContinueWith(o => 1000);
            }
        }
    }
}