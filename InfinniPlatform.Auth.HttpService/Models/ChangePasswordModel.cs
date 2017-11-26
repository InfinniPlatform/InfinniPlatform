using InfinniPlatform.Auth.HttpService.Controllers;

namespace InfinniPlatform.Auth.HttpService.Models
{
    /// <summary>
    /// Model for <see cref="AuthManagementController{TUser}.ChangePassword"/>.
    /// </summary>
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}