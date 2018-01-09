using System.Security.Claims;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Application user claim representation.
    /// </summary>
    public class AppUserClaim
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AppUserClaim" />.
        /// </summary>
        public AppUserClaim()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AppUserClaim" />.
        /// </summary>
        public AppUserClaim(Claim claim)
        {
            Type = claim.Type;
            Value = claim.Value;
        }

        /// <summary>
        /// Claim type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Claim value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Create new instance of <see cref="Claim"/> from this <see cref="AppUserClaim"/> instance.
        /// </summary>
        /// <returns></returns>
        public Claim ToSecurityClaim()
        {
            return new Claim(Type, Value);
        }
    }
}