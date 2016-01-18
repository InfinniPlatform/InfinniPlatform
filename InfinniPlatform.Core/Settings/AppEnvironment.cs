using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Core.Settings
{
    internal sealed class AppEnvironment : IAppEnvironment
    {
        public const string SectionName = "app";

        public AppEnvironment()
        {
            Name = "InfinniPlatform";
        }

        public string Name { get; }
    }
}