using System.Security.Claims;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Identity;
using InfinniPlatform.Http;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.ServiceHost
{
    public class AuthTestHttpService : IHttpService
    {
        private readonly UserManager<AppCustomUser> _userManager;

        public AuthTestHttpService(IUserManagerFactory userManagerFactory)
        {
            _userManager = userManagerFactory.GetUserManager<AppCustomUser>();
        }

        public void Load(IHttpServiceBuilder builder)
        {
            // UserManager

            builder.Post["/UserManager/Create"] = CreateUser;
            builder.Get["/UserManager/FindById"] = FindById;
            builder.Get["/UserManager/FindByEmail"] = FindByEmail;
            builder.Get["/UserManager/FindByName"] = FindByName;
            builder.Get["/UserManager/Delete"] = Delete;
            builder.Get["/UserManager/AddClaim"] = AddClaim;
        }

        private async Task<object> CreateUser(IHttpRequest httpRequest)
        {
            string email = httpRequest.Form.Email;
            string password = httpRequest.Form.Password;

            var appUser = new AppCustomUser
                          {
                              Age = 100,
                              UserName = email,
                              Email = email,
                              Id = "59142e50061ec276e470823f"
                          };
            var identityResult = await _userManager.CreateAsync(appUser, password);

            return identityResult;
        }

        private async Task<object> FindById(IHttpRequest httpRequest)
        {
            string id = httpRequest.Query.Id;

            var appUser = await _userManager.FindByIdAsync(id);

            return appUser;
        }

        private async Task<object> FindByEmail(IHttpRequest httpRequest)
        {
            string email = httpRequest.Query.Email;

            var appUser = await _userManager.FindByEmailAsync(email);

            return appUser;
        }


        private async Task<object> FindByName(IHttpRequest httpRequest)
        {
            string name = httpRequest.Query.UserName;

            var appUser = await _userManager.FindByNameAsync(name);

            return appUser;
        }

        private async Task<object> Delete(IHttpRequest httpRequest)
        {
            string name = httpRequest.Query.UserName;

            var appUser = await _userManager.FindByNameAsync(name);
            var identityResult = await _userManager.DeleteAsync(appUser);

            return identityResult;
        }

        private async Task<object> AddClaim(IHttpRequest httpRequest)
        {
            string name = httpRequest.Query.UserName;
            string claimType = httpRequest.Query.ClaimType;
            string claimValue = httpRequest.Query.ClaimValue;

            var appUser = await _userManager.FindByNameAsync(name);
            var identityResult = await _userManager.AddClaimAsync(appUser, new Claim(claimType, claimValue));

            return identityResult;
        }
    }
}