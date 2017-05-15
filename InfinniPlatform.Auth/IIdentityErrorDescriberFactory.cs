using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// ������� ��� ��������� ������� ����������� ������ ��������������/����������� <see cref="Microsoft.AspNetCore.Identity.IdentityErrorDescriber"/>.
    /// </summary>
    public interface IIdentityErrorDescriberFactory
    {
        IdentityErrorDescriber Get(IContainerResolver resolver);
    }
}