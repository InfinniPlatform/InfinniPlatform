using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Фабрика для получения нормализатора ключей <see cref="ILookupNormalizer"/>.
    /// </summary>
    public interface ILookupNormalizerFactory
    {
        ILookupNormalizer Get(IContainerResolver resolver);
    }
}