using InfinniPlatform.Auth.HttpService.Controllers;
using Newtonsoft.Json;

namespace InfinniPlatform.Auth.HttpService.Models
{
    /// <summary>
    /// Model for methods of <see cref="AuthInternalController{TUser}"/>.
    /// </summary>
    public class SignInModel
    {
        /// <summary>
        /// User key for authentication (e.g. id, username, email).
        /// </summary>
        public string UserKey { get; set; }
        
        /// <summary>
        /// User password.
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// Cookie persistence flag.
        /// </summary>
        public bool Remember { get; set; }

        [JsonProperty("Id")]
        private string Id { set => UserKey = value; }

        [JsonProperty("UserName")]
        private string UserName { set => UserKey = value; }

        [JsonProperty("Email")]
        private string Email { set => UserKey = value; }

        [JsonProperty("PhoneNumber")]
        private string PhoneNumber { set => UserKey = value; }
    }
}