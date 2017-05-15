using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// ������� ��� ��������� ���������� ����� ��� ������� <see cref="IPasswordHasher{TUser}"/>.
    /// </summary>
    public interface IPasswordHasherFactory
    {
        IPasswordHasher<TUser> Get<TUser>(IContainerResolver resolver) where TUser : AppUser;
    }
}