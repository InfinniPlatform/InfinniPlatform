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
    //[Ignore("Тесты SDK не выполняют запуск сервера InfinniPlatform. Необходимо существование уже запущенного сервера на localhost : 9900")]
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
            _signInApi.SignInInternal("Admin", "Admin", false);
            _api.CookieContainer = _signInApi.CookieContainer;

        }


        [Test]
        public void ShouldAddAndDeleteUser()
        {
            var user =  "NewUser_" + Guid.NewGuid();

            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(_api.AddUser(user, user).ToString());

            Assert.AreEqual(true, result.IsValid);

            dynamic userObject = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetUser(user).ToString());

            Assert.IsNotNull(userObject);

            result = JsonConvert.DeserializeObject<ExpandoObject>(_api.DeleteUser(user).ToString());

            Assert.AreEqual(true, result.IsValid);

            userObject = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetUser(user).ToString());

            Assert.AreEqual(userObject.IsValid, false);
            Assert.True(((string) userObject.ValidationMessage).Contains("not found"));
        }

        [Test]
        public void ShouldGrantAcl()
        {
            var user = "NewUser_" + Guid.NewGuid();

            _api.AddUser(user, user);

            dynamic accessResult = JsonConvert.DeserializeObject<ExpandoObject>(_api.GrantAccess(user, "Gameshop", "UserProfile", "GetDocument").ToString());

            Assert.AreEqual(accessResult.IsValid,true);
        }

        [Test]
        public void ShouldDenyAcl()
        {
            var user = "NewUser_" + Guid.NewGuid();

            _api.AddUser(user, user);

            dynamic accessResult = JsonConvert.DeserializeObject<ExpandoObject>(_api.DenyAccess(user, "Gameshop", "UserProfile", "GetDocument").ToString());

            Assert.AreEqual(accessResult.IsValid,true);
        }

        [Test]
        public void ShouldAddAndRemoveUserClaim()
        {
            var user = "NewUser_" + Guid.NewGuid();

            string claimType = "OrganizationId";

            _api.AddUser(user, user);

            dynamic accessResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_api.AddUserClaim(user,claimType,
                    "TestOrganization").ToString());

            Assert.AreEqual(accessResult.IsValid,true);

            dynamic userClaim = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetUserClaim(user,claimType).ToString());

            Assert.AreEqual(userClaim.ClaimValue, "TestOrganization");

            accessResult =  JsonConvert.DeserializeObject<ExpandoObject>(_api.DeleteUserClaim(user, "OrganizationId").ToString());
            Assert.AreEqual(accessResult.IsValid, true);

            userClaim = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetUserClaim(user, claimType).ToString());

            Assert.False(((IDictionary<string,object>) userClaim).ContainsKey("ClaimValue"));
        }

        [Test]
        public void ShouldAddAndRemoveRole()
        {
            string roleName = "TestRole" + Guid.NewGuid();

            dynamic accessResult = JsonConvert.DeserializeObject<ExpandoObject>(_api.AddRole(roleName).ToString());
            Assert.AreEqual(accessResult.IsValid, true);

            accessResult = JsonConvert.DeserializeObject<ExpandoObject>(_api.DeleteRole(roleName).ToString());
            Assert.AreEqual(accessResult.IsValid, true);
        }


        [Test]
        public void ShouldAddAndRemoveUserRole()
        {
            var user = "NewUser_" + Guid.NewGuid();

            _api.AddUser(user, user);

            string roleName = "TestRole" + Guid.NewGuid();

            _api.AddRole(roleName);

            dynamic accessResult = JsonConvert.DeserializeObject<ExpandoObject>(_api.AddUserRole(user, roleName).ToString());
            Assert.AreEqual(accessResult.IsValid, true);

            accessResult = JsonConvert.DeserializeObject<ExpandoObject>(_api.DeleteUserRole(user, roleName).ToString());
            Assert.AreEqual(accessResult.IsValid, true);
        }

    }
}
