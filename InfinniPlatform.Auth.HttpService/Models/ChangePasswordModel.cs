using InfinniPlatform.Auth.HttpService.Controllers;

namespace InfinniPlatform.Auth.HttpService.Models
{
    /// <summary>
    /// Model for <see cref="AuthManagementController{TUser}.ChangePassword"/>.
    /// </summary>
    public class ChangePasswordModel
    {
        /// <summary>
        /// User's old password. 
        /// </summary>
        public string OldPassword { get; set; }
        
        /// <summary>
        /// User's new password.
        /// </summary>
        public string NewPassword { get; set; }
    }
}