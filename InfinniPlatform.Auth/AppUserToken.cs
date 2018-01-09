namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Application user login representation.
    /// </summary>
    public class AppUserToken
    {
        /// <summary>
        /// Login provider name.
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// Token name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Token value.
        /// </summary>
        public string Value { get; set; }
    }
}