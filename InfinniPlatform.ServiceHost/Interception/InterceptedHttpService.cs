using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using InfinniPlatform.Http;

namespace InfinniPlatform.ServiceHost.Interception
{
    public class InterceptedHttpService : IHttpService
    {
        private readonly ITestInterface _testClass;
        private readonly IInterceptedTestInterface _interceptedTestClass;

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
            var asyncTime = await MeasureElapsedTime(async () => await _testClass.DoAsyncWork());
            var taskTime = await MeasureElapsedTime(async () => await _testClass.DoTaskWork());
            var syncTime = MeasureElapsedTime(() => _testClass.DoSyncWork());

            var asyncTime1 = await MeasureElapsedTime(async () => await _interceptedTestClass.DoAsyncWork());
            var taskTime1 = await MeasureElapsedTime(async () => await _interceptedTestClass.DoTaskWork());
            var syncTime1 = MeasureElapsedTime(() => _interceptedTestClass.DoSyncWork());

            var row = $"{asyncTime};{taskTime};{syncTime}";

            File.AppendAllLines("Clean.csv", new []{ $"{asyncTime};{taskTime};{syncTime}" });
            File.AppendAllLines("Intercepted.csv", new []{ $"{asyncTime1};{taskTime1};{syncTime1}" });
            
            return row;
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