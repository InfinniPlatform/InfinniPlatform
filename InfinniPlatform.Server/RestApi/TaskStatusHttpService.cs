using System;
using System.IO;
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
            string log = httpRequest.Form.Log;

            File.AppendAllText("1.txt",$"From {taskId}:");
            File.AppendAllText("1.txt",log);

            return null;
        }
    }
}