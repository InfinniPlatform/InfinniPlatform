using System.Threading.Tasks;

using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Security;

namespace InfinniPlatform.Auth.Internal.Tests.Services
{
    internal class FakeHttpService : IHttpService
    {
        public FakeHttpService(IAppUserManager userManager)
        {
            _userManager = userManager;
        }


        private readonly IAppUserManager _userManager;


        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/Fake";

            builder.Get["/SomeGet"] = request => Task.FromResult<object>(request.User?.Name);

            builder.Post["/SomePost"] = request => Task.FromResult<object>(request.User?.Name);

            builder.Post["/CreateUser"] = request =>
                                          {
                                              _userManager.CreateUser(request.Form.UserName, request.Form.Password);
                                              return Task.FromResult<object>(null);
                                          };

            builder.Post["/FindUser"] = request =>
                                        {
                                            _userManager.FindUserByName(request.Form.UserName);
                                            return Task.FromResult<object>(null);
                                        };

            builder.Post["/FindUserAsync"] = async request => await _userManager.FindUserByNameAsync(request.Form.UserName);
        }
    }
}