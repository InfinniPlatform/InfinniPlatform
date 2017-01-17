using System;
using System.IO;
using System.Linq;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Core.Settings
{
    internal sealed class AppEnvironment : IAppEnvironment
    {
        public const string SectionName = "app";

        private string _instanceId;

        public AppEnvironment()
        {
            Name = "InfinniPlatform";
            IsInCluster = false;
            _instanceId = $"{Environment.MachineName}_{Directory.GetCurrentDirectory().Split(Path.DirectorySeparatorChar).Last()}";
        }

        public string Name { get; set; }

        public string InstanceId
        {
            get { return _instanceId; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _instanceId = value;
                }
            }
        }

        public bool IsInCluster { get; set; }
    }
}