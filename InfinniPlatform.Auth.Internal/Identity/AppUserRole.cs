using InfinniPlatform.DocumentStorage;

namespace InfinniPlatform.Auth.Identity
{
    public class AppUserRole : Document
    {
        public AppUserRole(string roleName)
        {
            Name = roleName;
        }

        public string Id
        {
            get => _id?.ToString();
            set => _id = value;
        }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}