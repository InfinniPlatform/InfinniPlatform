using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// ������� ��� ��������� ������������� ������ <see cref="ILookupNormalizer"/>.
    /// </summary>
    public interface ILookupNormalizerFactory
    {
        ILookupNormalizer Get(IContainerResolver resolver);
    }
}