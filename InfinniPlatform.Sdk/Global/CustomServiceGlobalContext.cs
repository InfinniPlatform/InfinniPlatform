using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public T GetComponent<T>(string version) where T : class
        {
            if (typeof (T) == typeof (ScriptContext))
            {
                return new ScriptContext(version) as T;
            }
            if (typeof (T) == typeof (ScriptContextApp))
            {
                return new ScriptContextApp(version) as T;
            }
            return _platformComponentsPack.GetComponent<T>(version);
        }
    }
}
