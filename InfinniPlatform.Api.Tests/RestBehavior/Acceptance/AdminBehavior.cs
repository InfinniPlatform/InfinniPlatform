﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.SystemConfig.UserStorage;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
	[TestFixture]
	[Category(TestCategories.AcceptanceTest)]
	public sealed class AdminBehavior
	{
		private IDisposable _server;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_server = TestApi.StartServer(c => c
				.SetHostingConfig(HostingConfig.Default)
				.InstallFromJson("Authorization.zip", new string[0]));

			TestApi.InitClientRouting(HostingConfig.Default);

		}

		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			_server.Dispose();
		}

		[Test]
		public void ShouldGrantAdminAccess()
		{

			ClearAuthConfig();
			new SignInApi(null).SignInInternal("Admin", "Admin", false);

			//password: "Password1"
            new AuthApi(null).AddUser("TestUser", "Password1");
            new AuthApi(null).AddUser("TestUser1", "Password1");

			//добавляем пользователю доступ к настройке прав
			new AdminApi().GrantAdminAcl("TestUser");


            new SignInApi(null).SignOutInternal();

			//залогиниваемся
            new SignInApi(null).SignInInternal("TestUser", "Password1", false);


            dynamic result = new AuthApi(null).DenyAccess("TestUser1", "Test");

			Assert.AreNotEqual(result.IsValid, false);

			//запрещаем всем административный доступ
			new AdminApi().SetDefaultAcl();

			//проверяем, что доступ по-прежнему присутствует
            result = new AuthApi(null).DenyAccessAll("Test");

			Assert.AreNotEqual(result.IsValid,false);

			//разлогиниваемся
            new SignInApi(null).SignOutInternal();

			//пробуем установить права

			try
			{
				new AdminApi().SetDefaultAcl();
			}
			catch (Exception e)
			{
				Assert.True(e.Message.Contains("access denied"));
			}

			//логинимся под другим пользователем, у которого нет прав
            new SignInApi(null).SignInInternal("TestUser1", "Password1", false);
			try
			{
				new AdminApi().SetDefaultAcl();
			}
			catch (Exception e)
			{
				Assert.True(e.Message.Contains("access denied"));
			}			
		}

		private static void ClearAuthConfig()
		{
            new SignInApi(null).SignInInternal("Admin", "Admin", false);

            var aclApi = new AuthApi(null);

			var userRoles = aclApi.GetUserRoles();
			foreach (var userRole in userRoles)
			{
				if (userRole.RoleName != AuthorizationStorageExtensions.AdminRole &&
					userRole.RoleName != AuthorizationStorageExtensions.Default)
				{
					aclApi.RemoveUserRole(userRole.UserName, userRole.RoleName);
				}
			}

			var users = aclApi.GetUsers();
			foreach (var user in users)
			{
				if (user.UserName != AuthorizationStorageExtensions.AdminUser &&
					user.UserName != AuthorizationStorageExtensions.AnonimousUser &&
					user.UserName != AuthorizationStorageExtensions.Default)
				{
					aclApi.RemoveUser(user.UserName);
				}
			}

			var roles = aclApi.GetRoles();
			foreach (var role in roles)
			{
				if (role.Name != AuthorizationStorageExtensions.AdminRole &&
					role.Name != AuthorizationStorageExtensions.Default)
				{
					aclApi.RemoveRole(role.Name);
				}
			}

			var acl = aclApi.GetAcl();
			foreach (var a in acl)
			{
				if (a.UserName != AuthorizationStorageExtensions.AdminRole &&
					a.UserName != AuthorizationStorageExtensions.AdminUser &&
					a.UserName != AuthorizationStorageExtensions.AnonimousUser)
				{
					aclApi.RemoveAcl(a.Id);
				}
			}

            new SignInApi(null).SignOutInternal();

		}


	}
}
