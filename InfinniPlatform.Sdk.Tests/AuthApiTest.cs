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
    [TestFixture]
	[Category(TestCategories.IntegrationTest)]
	[Ignore]
	public class AuthApiTest
    {
        private const string InfinniSessionPort = "9900";
        private const string InfinniSessionServer = "localhost";
        private const string Route = "1";

        private InfinniAuthApi _api;
        private InfinniSignInApi _signInApi;

        [TestFixtureSetUp]
        public void SetupApi()
        {
            _api = new InfinniAuthApi(InfinniSessionServer, InfinniSessionPort,Route);
            _signInApi = new InfinniSignInApi(InfinniSessionServer, InfinniSessionPort,Route);
            _signInApi.SignInInternal("Admin", "Admin", false);
            _api.CookieContainer = _signInApi.CookieContainer;
        }


        [Test]
        public void ShouldAddAndDeleteUser()
        {
            var user =  "NewUser_" + Guid.NewGuid();

            dynamic result = _api.AddUser(user, user);

            Assert.AreEqual(true, result.IsValid);

            dynamic userObject = _api.GetUser(user);

            Assert.IsNotNull(userObject);

            result = _api.DeleteUser(user);

            Assert.AreEqual(true, result.IsValid);

            userObject = _api.GetUser(user);

            Assert.AreEqual(userObject.IsValid, false);
            Assert.True(((string) userObject.ValidationMessage).Contains("not found"));
        }
        
        [Test]
        public void ShouldGetAcl()
        {
            dynamic aclItems =  _api.GetAclList(AclType.User, null, 0, 1000, null);

            Assert.True(aclItems.Count > 0);
        }

        [Test]
        public void ShouldGrantAcl()
        {
            var user = "NewUser_" + Guid.NewGuid();

            _api.AddUser(user, user);

            dynamic accessResult = _api.GrantAccess(user, "Gameshop", "UserProfile", "GetDocument");

            Assert.AreEqual(accessResult.IsValid,true);
        }

        [Test]
        public void ShouldDenyAcl()
        {
            var user = "NewUser_" + Guid.NewGuid();

            _api.AddUser(user, user);

            dynamic accessResult = _api.DenyAccess(user, "Gameshop", "UserProfile", "GetDocument");

            Assert.AreEqual(accessResult.IsValid,true);
        }

        [Test]
        public void ShouldAddAndRemoveUserClaim()
        {
            var user = "NewUser_" + Guid.NewGuid();

            string claimType = "OrganizationId";

            _api.AddUser(user, user);

            dynamic accessResult =
                _api.AddUserClaim(user,claimType,
                    "TestOrganization");

            Assert.AreEqual(accessResult.IsValid,true);

            dynamic userClaim = _api.GetUserClaim(user,claimType);

            Assert.AreEqual(userClaim.ClaimValue, "TestOrganization");

            accessResult =  _api.DeleteUserClaim(user, "OrganizationId");
            Assert.AreEqual(accessResult.IsValid, true);

            userClaim = _api.GetUserClaim(user, claimType);

            Assert.IsNull(userClaim.ClaimValue);
            
        }

        [Test]
        public void ShouldAddAndRemoveRole()
        {
            string roleName = "TestRole" + Guid.NewGuid();

            dynamic accessResult = _api.AddRole(roleName);
            Assert.AreEqual(accessResult.IsValid, true);

            accessResult = _api.DeleteRole(roleName);
            Assert.AreEqual(accessResult.IsValid, true);
        }


        [Test]
        public void ShouldAddAndRemoveUserRole()
        {
            var user = "NewUser_" + Guid.NewGuid();

            _api.AddUser(user, user);

            string roleName = "TestRole" + Guid.NewGuid();

            _api.AddRole(roleName);

            dynamic accessResult = _api.AddUserRole(user, roleName);
            Assert.AreEqual(accessResult.IsValid, true);

            accessResult = _api.DeleteUserRole(user, roleName);
            Assert.AreEqual(accessResult.IsValid, true);
        }

        [Test]
        public void ShouldGetUsers()
        {
            var user = "NewUser_" + Guid.NewGuid();

            _api.AddUser(user, user);

            IEnumerable<dynamic> users = _api.GetAclList(AclType.User, f => f.AddCriteria(cr => cr.Property("UserName").IsEquals(user)), 0, 1);

            Assert.AreEqual(users.Count(),1);

            Assert.AreEqual(users.First().UserName.ToString(), user);
        }


        [Test]
        public void ShouldGetRoles()
        {
            var role = "NewRole_" + Guid.NewGuid();

            _api.AddRole(role);

            IEnumerable<dynamic> roles = _api.GetAclList(AclType.Role, f => f.AddCriteria(cr => cr.Property("Name").IsEquals(role)), 0, 1);

            Assert.AreEqual(roles.Count(), 1);

            Assert.AreEqual(roles.First().Name.ToString(), role);
        }
    }
}
