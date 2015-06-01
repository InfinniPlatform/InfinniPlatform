using System;
using System.Linq;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.Security;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.SystemConfig.UserStorage;
using NUnit.Framework;

namespace InfinniPlatform.SystemConfig.Tests.Authorization
{
	[TestFixture]
	[Category(TestCategories.AcceptanceTest)]
	public sealed class ApplicationUserStorePersistentBehavior
	{

		private IDisposable _server;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));

			TestApi.InitClientRouting(TestSettings.DefaultHostingConfig);

			IndexApi.RebuildIndex(AuthorizationStorageExtensions.AuthorizationConfigId,AuthorizationStorageExtensions.UserStore);
			IndexApi.RebuildIndex(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.RoleStore);
			IndexApi.RebuildIndex(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.UserRoleStore);
			IndexApi.RebuildIndex(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.ClaimStore);
		}

		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			_server.Dispose();
		}

		[Test]
		public void ShouldCrudUserInStorage()
		{
			var storage = new ApplicationUserStorePersistentStorage();

			var applicationUser = new ApplicationUser();
			applicationUser.UserName = "TestUser";
			
			//should create user
			storage.CreateUser(applicationUser);

			ApplicationUser user = storage.FindUserByName(applicationUser.UserName);

			Assert.IsNotNull(user);
			Assert.AreEqual(user.UserName,applicationUser.UserName);
			Assert.IsNotNull(user.Logins);
			Assert.IsNotNull(user.Roles);
			Assert.IsNotNull(user.Claims);

			//should update user
			string changedUserName = "ChangedUserName";
			user.UserName = changedUserName;
			storage.UpdateUser(user);

			var updatedUser = storage.FindUserByName(user.UserName);

			Assert.IsNotNull(updatedUser);
			Assert.AreEqual(updatedUser.UserName, user.UserName);


			//should add role
			storage.AddRole("TestRole","TestRole","TestDescription");

			//should add user to role
			storage.AddUserToRole(user,"TestRole");

			var userWithRole = storage.FindUserByName(user.UserName);

			Assert.IsNotNull(userWithRole);
			Assert.AreEqual(userWithRole.Roles.Count(),1);
			Assert.AreEqual(userWithRole.Roles.First().DisplayName,"TestRole");

			//should throws if user with specified role exists
			Assert.Throws<ArgumentException>(() => storage.RemoveRole("TestRole"));

			//should remove user from role
			storage.RemoveUserFromRole(userWithRole, "TestRole");

			userWithRole = storage.FindUserByName(userWithRole.UserName);

			Assert.AreEqual(userWithRole.Roles.Count(), 0);

			//should remove role
			storage.RemoveRole("TestRole");

			//should not add user to unexisting role
			var appUserWithoutRole = new ApplicationUser();
			appUserWithoutRole.Id = userWithRole.Id;
			appUserWithoutRole.UserName = "TestUserWithoutRole";

			Assert.Throws<ArgumentException>(() => storage.AddUserToRole(appUserWithoutRole, "TestRole"));

			//should add user login
			var userLogin = new ApplicationUserLogin()
			{
				Provider = "test",
				ProviderKey = "test"
			};

			storage.AddUserLogin(userWithRole, userLogin);

			//should find user by login

			var userByLogin = storage.FindUserByLogin(userLogin);

			Assert.IsNotNull(userByLogin);
			Assert.AreEqual(userByLogin.UserName, changedUserName);

			//should remove user login

			storage.RemoveUserLogin(userByLogin, userLogin);
			
			var userNotFoundByLogin = storage.FindUserByLogin(userLogin);

			Assert.IsNull(userNotFoundByLogin);
			

			//should add claim

			var claimType = "TestClaim";
			storage.AddClaimType(claimType);

			//should add user claim
			storage.AddUserClaim(userByLogin,claimType,"1");

			var userWithClaim = storage.FindUserByName(userByLogin.UserName);
			
			Assert.AreEqual(userWithClaim.Claims.Count(),1);

			//should fail to remove claim
			Assert.Throws<ArgumentException>(() => storage.RemoveClaimType(claimType));

			//should delete user claim
			storage.RemoveUserClaim(userWithClaim, claimType);

			var userWithoutClaim = storage.FindUserByName(userWithClaim.UserName);
			Assert.AreEqual(userWithoutClaim.Claims.Count(),0);

			//should success delete claim type
			storage.RemoveClaimType(claimType);
			

		}
	}
}
