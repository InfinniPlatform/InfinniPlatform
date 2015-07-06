using System.Collections.Generic;

namespace InfinniPlatform.Sdk.ContextComponents
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
        IEnumerable<dynamic> Versions { get; } 
        void UpdateUserRoles();
        void UpdateUsers();
        void UpdateAcl();
        void UpdateRoles();
        void UpdateVersions();

        string GetClaim(string claimType, string userName);
        void UpdateClaim(string userName, string claimType, string claimValue);
    }
}