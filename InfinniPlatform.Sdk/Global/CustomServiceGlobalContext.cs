using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Sdk.Global
{
    public sealed class CustomServiceGlobalContext : ICustomServiceGlobalContext
    {
        public CustomServiceGlobalContext(IPlatformComponentsPack platformComponentsPack)
        {
            _platformComponentsPack = platformComponentsPack;
        }

        private readonly IPlatformComponentsPack _platformComponentsPack;

        public T GetComponent<T>() where T : class
        {
            if (typeof(T) == typeof(ScriptContext))
            {
                return new ScriptContext() as T;
            }
            return _platformComponentsPack.GetComponent<T>();
        }
    }
}