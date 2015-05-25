using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Api;
using Newtonsoft.Json;
using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
    [Ignore("Тесты SDK не выполняют запуск сервера InfinniPlatform. Необходимо существование уже запущенного сервера на localhost : 9900")]
    [TestFixture]
    public class AuthApiTest
    {
        private const string InfinniSessionPort = "9900";
        private const string InfinniSessionServer = "localhost";
        private const string InfinniSessionVersion = "1";
        private InfinniAuthApi _api;
        private InfinniSignInApi _signInApi;

        [TestFixtureSetUp]
        public void SetupApi()
        {
            _api = new InfinniAuthApi(InfinniSessionServer, InfinniSessionPort, InfinniSessionVersion);
            _signInApi = new InfinniSignInApi(InfinniSessionServer, InfinniSessionPort, InfinniSessionVersion);
        }


        [Test]
        public void ShouldAddAndDeleteUser()
        {
            _signInApi.SignInInternal("Admin", "Admin", false);

            _api.CookieContainer = _signInApi.CookieContainer;

            var user =  "NewUser_" + Guid.NewGuid();

            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(_api.AddUser(user, user).Content.ToString());

            Assert.AreEqual(true, result.IsValid);

            dynamic userObject = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetUser(user).ToString());

            result = JsonConvert.DeserializeObject<ExpandoObject>(_api.DeleteUser(userObject).Content.ToString());

            Assert.AreEqual(true, result.IsValid);
        }
    }
}
