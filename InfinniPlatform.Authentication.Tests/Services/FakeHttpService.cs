using System.Threading.Tasks;

using InfinniPlatform.Sdk.Security;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Authentication.Tests.Services
{
    internal sealed class FakeHttpService : IHttpService
    {
        public FakeHttpService(IApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        private readonly IApplicationUserManager _userManager;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/Fake";
            builder.Get["/SomeGet"] = request => Task.FromResult<object>((request.User != null) ? request.User.Name : null);
            builder.Post["/SomePost"] = request => Task.FromResult<object>((request.User != null) ? request.User.Name : null);
            builder.Post["/CreateUser"] = request => { _userManager.CreateUser(request.Form.UserName, request.Form.Password); return Task.FromResult<object>(null); };
            builder.Post["/FindUser"] = request => { _userManager.FindUserByName(request.Form.UserName); return Task.FromResult<object>(null); };
            builder.Post["/FindUserAsync"] = async request => await _userManager.FindUserByNameAsync(request.Form.UserName);
        }
    }
}