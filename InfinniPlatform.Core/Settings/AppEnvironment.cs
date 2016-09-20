using System;

using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Core.Settings
{
    internal sealed class AppEnvironment : IAppEnvironment
    {
        public const string SectionName = "app";

        public AppEnvironment()
        {
            Name = "InfinniPlatform";
            _instanceId = Guid.NewGuid().ToString("N");
        }

        private string _instanceId;

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
    }
}