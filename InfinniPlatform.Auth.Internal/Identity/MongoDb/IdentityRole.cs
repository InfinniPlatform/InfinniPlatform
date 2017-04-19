using InfinniPlatform.DocumentStorage.Abstractions;

namespace InfinniPlatform.Auth.Identity.MongoDb
{
    public class IdentityRole : Document
    {
        public IdentityRole(string roleName)
        {
            Name = roleName;
        }

        public string Id
        {
            get { return _id?.ToString(); }
            set { _id = value; }
        }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}