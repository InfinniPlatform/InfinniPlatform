using InfinniPlatform.Auth.HttpService.Controllers;
using Newtonsoft.Json;

namespace InfinniPlatform.Auth.HttpService.Models
{
    /// <summary>
    /// Model for methods of <see cref="AuthInternalController{TUser}"/>.
    /// </summary>
    public class SignInModel
    {
        public string UserKey { get; set; }
        public string Password { get; set; }
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