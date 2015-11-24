using System;

using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Sdk.Global
{
    [Obsolete("Use IoC")]
    public sealed class CustomServiceGlobalContext : ICustomServiceGlobalContext
    {
        public CustomServiceGlobalContext(Func<IContainerResolver> containerResolverFactory)
        {
            _containerResolver = containerResolverFactory();
        }


        private readonly IContainerResolver _containerResolver;


        [Obsolete("Use IoC")]
        public T GetComponent<T>() where T : class
        {
            return _containerResolver.Resolve<T>();
        }
    }
}