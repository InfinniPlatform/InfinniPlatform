using System;
using System.Linq;
using System.Text.RegularExpressions;

using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Server.Tasks.Agents;

namespace InfinniPlatform.Server.Tasks.Infinni.Node
{
    public class NodeOutputParser : INodeOutputParser
    {
        private const string OutputInfoRegex = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2},\d{3}\s\[PID\s\d+\]\sINFO\s+-\s+";

        public NodeOutputParser(IJsonObjectSerializer serializer)
        {
            _serializer = serializer;
        }

        private readonly IJsonObjectSerializer _serializer;

        public ServiceResult<AgentTaskStatus> FormatAppsInfoOutput(ServiceResult<AgentTaskStatus> serviceResult)
        {
            var processResult = serviceResult.Result;

            var appsInfoJson = Regex.Replace(processResult.Output, OutputInfoRegex, string.Empty, RegexOptions.Multiline, Regex.InfiniteMatchTimeout)
                                    .Split(new[] { System.Environment.NewLine }, StringSplitOptions.None)
                                    .FirstOrDefault(s => s.StartsWith("["));

            processResult.FormattedOutput = _serializer.Deserialize<object[]>(appsInfoJson);

            serviceResult.Result = processResult;

            return serviceResult;
        }

        public ServiceResult<AgentTaskStatus> FormatPackagesOutput(ServiceResult<AgentTaskStatus> serviceResult)
        {
            var processResult = serviceResult.Result;

            var packagesJson = Regex.Replace(processResult.Output, OutputInfoRegex, string.Empty, RegexOptions.Multiline, Regex.InfiniteMatchTimeout)
                                    .Split(new[] { System.Environment.NewLine }, StringSplitOptions.None)
                                    .FirstOrDefault(s => s.StartsWith("["));

            processResult.FormattedOutput = _serializer.Deserialize<string[]>(packagesJson);

            serviceResult.Result = processResult;

            return serviceResult;
        }
    }
}