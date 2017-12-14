using InfinniPlatform.DocumentStorage;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Application user role representation.
    /// </summary>
    public class AppUserRole : Document
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AppUserRole" />.
        /// </summary>
        public AppUserRole(string roleName)
        {
            Name = roleName;
        }

        /// <summary>
        /// Role id.
        /// </summary>
        public string Id
        {
            get => _id?.ToString();
            set => _id = value;
        }

        /// <summary>
        /// Role name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Role name normalized for search.
        /// </summary>
        public string NormalizedName { get; set; }

        /// <summary>
        /// Return string representation of user.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}