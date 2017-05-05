using System.Security.Claims;

namespace InfinniPlatform.Auth.Identity
{
    public class AppUserClaim
    {
        public AppUserClaim()
        {
        }

        public AppUserClaim(Claim claim)
        {
            Type = claim.Type;
            Value = claim.Value;
        }

        public string Type { get; set; }

        public string Value { get; set; }

        public Claim ToSecurityClaim()
        {
            return new Claim(Type, Value);
        }
    }
}