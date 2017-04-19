namespace InfinniPlatform.Auth.Identity.MongoDb
{
    public class IdentityUserToken
    {
        public string LoginProvider { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}