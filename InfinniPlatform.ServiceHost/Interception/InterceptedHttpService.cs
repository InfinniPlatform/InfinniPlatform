using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using InfinniPlatform.Http;

namespace InfinniPlatform.ServiceHost.Interception
{
    public class InterceptedHttpService : IHttpService
    {
        private readonly IInterceptedTestInterface _interceptedTestClass;
        private readonly ITestInterface _testClass;

        public InterceptedHttpService(ITestInterface testClass, IInterceptedTestInterface interceptedTestClass)
        {
            _testClass = testClass;
            _interceptedTestClass = interceptedTestClass;
        }

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Get["/test"] = Test;
        }


        public async Task<object> Test(IHttpRequest httpRequest)
        {
            var cleanAsyncTime = await MeasureElapsedTime(async () => await _testClass.DoAsyncWork());
            var cleanTaskTime = await MeasureElapsedTime(async () => await _testClass.DoTaskWork());
            var cleanSyncTime = MeasureElapsedTime(() => _testClass.DoSyncWork());

            var interceptedAsyncTime = await MeasureElapsedTime(async () => await _interceptedTestClass.DoAsyncWork());
            var interceptedTaskTime = await MeasureElapsedTime(async () => await _interceptedTestClass.DoTaskWork());
            var interceptedSyncTime = MeasureElapsedTime(() => _interceptedTestClass.DoSyncWork());

            File.AppendAllLines("Clean.csv", new[] {$"{cleanAsyncTime};{cleanTaskTime};{cleanSyncTime}"});
            File.AppendAllLines("Intercepted.csv", new[] {$"{interceptedAsyncTime};{interceptedTaskTime};{interceptedSyncTime}"});

            return "OK";
        }

        private static async Task<double> MeasureElapsedTime(Func<Task> action)
        {
            var startNew = Stopwatch.StartNew();

            await action.Invoke();

            return startNew.Elapsed.TotalMilliseconds;
        }

        private static double MeasureElapsedTime(Action action)
        {
            var startNew = Stopwatch.StartNew();

            action.Invoke();

            return startNew.Elapsed.TotalMilliseconds;
        }
    }
}