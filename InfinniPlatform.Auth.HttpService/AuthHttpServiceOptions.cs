namespace InfinniPlatform.Auth.HttpService
{
    /// <summary>
    /// Authentication HTTP-service configuration options.
    /// </summary>
    public class AuthHttpServiceOptions : IOptions
    {
        /// <inheritdoc />
        public string SectionName => "authHttpService";
        
        /// <summary>
        /// Default callback URL on successful authentication.
        /// </summary>
        public const string DefaultSuccessUrl = "/";
        
        /// <summary>
        /// Default callback URL on failed authentication.
        /// </summary>
        public const string DefaultFailureUrl = "/";

        /// <summary>
        /// Default <see cref="AuthHttpServiceOptions"/> instance.
        /// </summary>
        public static readonly AuthHttpServiceOptions Default = new AuthHttpServiceOptions();


        /// <summary>
        /// Initialize a new instance of <see cref="AuthHttpServiceOptions"/> class.
        /// </summary>
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