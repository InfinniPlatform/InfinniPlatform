using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Server.RestApi
{
    public class TaskStatusHttpService : IHttpService
    {
        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "server";
            builder.Post["taskStatus"] = HandleTaskStatus;
        }

        private static Task<object> HandleTaskStatus(IHttpRequest httpRequest)
        {
            string taskId = httpRequest.Form.TaskId;
            List<object> log = httpRequest.Form.Log;

            File.AppendAllLines("1.txt", log.Select(o => o.ToString()));

            return null;
        }
    }
}