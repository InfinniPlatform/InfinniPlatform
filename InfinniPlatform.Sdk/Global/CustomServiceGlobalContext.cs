using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Sdk.Global
{
    public sealed class CustomServiceGlobalContext : ICustomServiceGlobalContext
    {
        private readonly IPlatformComponentsPack _platformComponentsPack;

        public CustomServiceGlobalContext(IPlatformComponentsPack platformComponentsPack)
        {
            _platformComponentsPack = platformComponentsPack;
        }

        public T GetComponent<T>() where T : class
        {
            if (typeof (T) == typeof (ScriptContext))
            {
                return new ScriptContext() as T;
            }
            return _platformComponentsPack.GetComponent<T>();
        }

        public string GetVersion(string configuration, string userName)
        {
            var configVersions = GetComponent<IMetadataConfigurationProvider>().ConfigurationVersions;
            return GetComponent<IVersionStrategy>().GetActualVersion(configuration, configVersions, userName);
        }
    }
}
