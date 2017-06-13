namespace InfinniPlatform.Auth.HttpService
{
    /// <summary>
    /// Authentication HTTP-service options.
    /// </summary>
    public class AuthHttpServiceOptions
    {
        public const string SectionName = "authHttpService";

        public const string DefaultSuccessUrl = "/";

        public const string DefaultFailureUrl = "/";

        public static readonly AuthHttpServiceOptions Default = new AuthHttpServiceOptions();


        public AuthHttpServiceOptions()
        {
            SuccessUrl = DefaultSuccessUrl;
            FailureUrl = DefaultFailureUrl;
        }


        /// <summary>
        /// Redirect URL for successful external authentication.
        /// </summary>
        public string SuccessUrl { get; set; }

        /// <summary>
        /// Redirect URL for failed external authentication.
        /// </summary>
        public string FailureUrl { get; set; }
    }
}