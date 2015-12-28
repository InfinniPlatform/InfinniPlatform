using System;

using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    /// Пакет компонентов платформы
    /// </summary>
    [Obsolete("Use IoC")]
    public sealed class PlatformComponentsPack : IPlatformComponentsPack
    {
        public PlatformComponentsPack(IContainerResolver containerResolver)
        {
            _containerResolver = containerResolver;
        }


        private readonly IContainerResolver _containerResolver;


        [Obsolete("Use IoC")]
        public T GetComponent<T>() where T : class
        {
            return _containerResolver.Resolve<T>();
        }
    }
}