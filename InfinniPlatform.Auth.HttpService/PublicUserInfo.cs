using System.Collections.Generic;
using InfinniPlatform.Auth.Identity;

namespace InfinniPlatform.Auth.HttpService
{
    /// <summary>
    /// ���������� � ������������, ��������� ����� <see cref="AuthInternalHttpService{TUser}" />.
    /// </summary>
    internal class PublicUserInfo
    {
        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="userName">��� ������������.</param>
        /// <param name="displayName">������������ ��� ������������.</param>
        /// <param name="description">��������.</param>
        /// <param name="roles">���� ������������.</param>
        /// <param name="logins">������� ������ ������������.</param>
        /// <param name="claims">����������� ������������.</param>
        public PublicUserInfo(string userName,
                              string displayName,
                              string description,
                              IEnumerable<string> roles,
                              IEnumerable<AppUserLogin> logins,
                              List<AppUserClaim> claims)
        {
            UserName = userName;
            DisplayName = displayName;
            Description = description;
            Roles = roles;
            Logins = logins;
            Claims = claims;
        }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<AppUserLogin> Logins { get; set; }

        public List<AppUserClaim> Claims { get; set; }
    }
}