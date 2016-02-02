using System;

namespace InfinniPlatform.SystemConfig.Utils
{
    internal sealed class DocumentLink
    {
        public string DocumentId { get; set; }

        public string InstanceId { get; set; }

        public Action<dynamic> SetValue { get; set; }
    }
}