using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity
{
    /// <summary>
    /// ������� ��� ��������� ����������� <see cref="UserManager{TUser}" />.
    /// </summary>
    public interface IUserManagerFactory
    {
        UserManager<TUser> GetUserManager<TUser>() where TUser : AppUser;
    }
}