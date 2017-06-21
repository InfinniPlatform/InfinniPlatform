using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using InfinniPlatform.Http;

namespace InfinniPlatform.ServiceHost.Interception
{
    public class InterceptedHttpService : IHttpService
    {
        private const string AsyncFileName = "Async.csv";
        private const string SyncFileName = "Sync.csv";
        private const string TaskFileName = "Task.csv";
        private readonly TestInterfaces.IAsyncInterface _asyncClass;
        private readonly TestInterfaces.ISyncInterface _syncClass;
        private readonly TestInterfaces.ITaskInterface _taskClass;

        public InterceptedHttpService(TestInterfaces.IAsyncInterface asyncClass,
                                      TestInterfaces.ITaskInterface taskClass,
                                      TestInterfaces.ISyncInterface syncClass)
        {
            _asyncClass = asyncClass;
            _taskClass = taskClass;
            _syncClass = syncClass;
        }

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Get["/async"] = AsyncWork;
            builder.Get["/task"] = TaskWork;
            builder.Get["/sync"] = SyncWork;

            builder.Get["/asyncg"] = AsyncWorkGeneric;
            builder.Get["/taskg"] = TaskWorkGeneric;
            builder.Get["/syncg"] = SyncWorkGeneric;
        }

        // NOT GENERIC

        public Task<object> AsyncWork(IHttpRequest httpRequest)
        {
            return MeasureElapsedTime(async () => await _asyncClass.DoWork(), AsyncFileName);
        }

        public Task<object> TaskWork(IHttpRequest httpRequest)
        {
            return MeasureElapsedTime(async () => await _taskClass.DoWork(), TaskFileName);
        }

        public Task<object> SyncWork(IHttpRequest httpRequest)
        {
            return MeasureElapsedTime(() => _syncClass.DoWork(), SyncFileName);
        }

        // GENERIC

        public Task<object> AsyncWorkGeneric(IHttpRequest httpRequest)
        {
            return MeasureElapsedTime(async () => await _asyncClass.DoGenericWork(), AsyncFileName);
        }

        public Task<object> TaskWorkGeneric(IHttpRequest httpRequest)
        {
            return MeasureElapsedTime(async () => await _taskClass.DoGenericWork(), TaskFileName);
        }

        public Task<object> SyncWorkGeneric(IHttpRequest httpRequest)
        {
            return MeasureElapsedTime(() => _syncClass.DoGenericWork(), SyncFileName);
        }

        private static async Task<object> MeasureElapsedTime(Func<Task> action, string filename)
        {
            var startNew = Stopwatch.StartNew();
            await action.Invoke();
            var milliseconds = startNew.Elapsed.TotalMilliseconds;
            File.AppendAllText(filename, $"{milliseconds}{Environment.NewLine}");

            return "OK";
        }

        private static Task<object> MeasureElapsedTime(Action action, string filename)
        {
            var startNew = Stopwatch.StartNew();
            action.Invoke();
            var milliseconds = startNew.Elapsed.TotalMilliseconds;
            File.AppendAllText(filename, $"{milliseconds}{Environment.NewLine}");

            return Task.FromResult<object>("OK");
        }
    }
}