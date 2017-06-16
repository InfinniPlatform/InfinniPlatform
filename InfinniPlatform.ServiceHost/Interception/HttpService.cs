using System.Threading.Tasks;
using InfinniPlatform.Http;

namespace InfinniPlatform.ServiceHost.Interception
{
    public class HttpService : IHttpService
    {
        private readonly IAsyncInterface _asyncClass;
        private readonly ISyncInterface _syncClass;
        private readonly ITaskInterface _taskClass;

        public HttpService(IAsyncInterface asyncClass,
                           ITaskInterface taskClass,
                           ISyncInterface syncClass)
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

        public async Task<object> AsyncWork(IHttpRequest httpRequest)
        {
            await _asyncClass.DoWork();

            return "OK.";
        }

        public async Task<object> TaskWork(IHttpRequest httpRequest)
        {
            await _taskClass.DoWork();

            return "OK.";
        }

        public Task<object> SyncWork(IHttpRequest httpRequest)
        {
            _syncClass.DoWork();

            return Task.FromResult<object>("OK");
        }

        public async Task<object> AsyncWorkGeneric(IHttpRequest httpRequest)
        {
            await _asyncClass.DoGenericWork();

            return "OK.";
        }

        public async Task<object> TaskWorkGeneric(IHttpRequest httpRequest)
        {
            await _taskClass.DoGenericWork();

            return "OK.";
        }

        public Task<object> SyncWorkGeneric(IHttpRequest httpRequest)
        {
            _syncClass.DoGenericWork();

            return Task.FromResult<object>("OK");
        }
    }
}