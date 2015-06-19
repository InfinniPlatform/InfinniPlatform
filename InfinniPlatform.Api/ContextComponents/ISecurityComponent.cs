using System.Collections.Generic;

namespace InfinniPlatform.Api.ContextComponents
{
    /// <summary>
    ///     Компонент безопасности глобального контекста
    /// </summary>
    public interface ISecurityComponent
    {
        IEnumerable<dynamic> Acl { get; }
        IEnumerable<dynamic> Users { get; }
        IEnumerable<dynamic> Roles { get; }
        IEnumerable<dynamic> UserRoles { get; }
        void UpdateUserRoles();
        void UpdateUsers();
        void UpdateAcl();
        void UpdateRoles();
        string GetClaim(string claimType, string userName);
        void UpdateClaim(string userName, string claimType, string claimValue);
    }
}