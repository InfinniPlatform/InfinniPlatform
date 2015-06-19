using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Api.Tests.Extensions;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
	[TestFixture]
	[Category(TestCategories.AcceptanceTest)]
	public sealed class AuthorizationBehavior
	{
		private IDisposable _server;
		private string _configurationId = "testconfigauthorization";
		private string _documentId = "testauthdocument";

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_server = TestApi.StartServer(c => c
				.SetHostingConfig(HostingConfig.Default)
				);

			TestApi.InitClientRouting(HostingConfig.Default);

		}

		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			_server.Dispose();
		}

		[Test]
		public void ShouldSignInternal()
		{
            AuthorizationExtensionsTest.ClearAuthConfig();

			new AuthApi().AddUser("TestUser", "Password1");

			new SignInApi().SignInInternal("TestUser", "Password1", false);
			new SignInApi().SignOutInternal();

		}

		[Test]
		public void ShouldChangePassword()
		{
            AuthorizationExtensionsTest.ClearAuthConfig();

			new AuthApi().AddUser("TestUser", "Password1");

			new SignInApi().SignInInternal("TestUser", "Password1", false);

			new SignInApi().ChangePassword("TestUser","Password1", "Password2");

			new SignInApi().SignOutInternal();

			new SignInApi().SignInInternal("TestUser", "Password2", false);

		}

		[Test]
		public void ShouldDenyAccessForUser()
		{
			CreateTestConfig();

			//логинимся под админом
			new SignInApi().SignInInternal("Admin", "Admin", false);

			//password: "Password1"
			new AuthApi().AddUser("TestUser", "Password1");

			new SignInApi().SignOutInternal();

            //получаем доступ к объекту, для которого не установлены права
		    var id = Guid.NewGuid().ToString();
		    dynamic testDocument = new DynamicWrapper();
		    testDocument.Id = id;
		    testDocument.TestProperty = "1";

		    dynamic result = new DocumentApi(null).SetDocument(_configurationId, _documentId, testDocument);
            Assert.AreEqual(result.IsValid, true);
                       
            //залогиниваемся под обычным пользователем
            new SignInApi().SignInInternal("TestUser","Password1",false);

            //успешно обновляем документ
		    result = new DocumentApi(null).SetDocument(_configurationId, _documentId, testDocument);
            Assert.AreEqual(result.IsValid, true);

			new SignInApi().SignOutInternal();

			//логинимся под админом
			new SignInApi().SignInInternal("Admin", "Admin", false);

            //отбираем доступ пользователя к документу
            new AuthApi().DenyAccess("TestUser",_configurationId, _documentId);

			new SignInApi().SignOutInternal();

			//залогиниваемся под обычным пользователем
			new SignInApi().SignInInternal("TestUser", "Password1", false);

            //проверяем отсутствие доступа к документу при сохранении
			try
			{
				result = new DocumentApi(null).SetDocument(_configurationId, _documentId, testDocument);
			}
			catch (Exception e)
			{
				Assert.True(e.Message.Contains("access denied"));
			}

            //проверяем отсутствие доступа к документам на чтение
		    try
		    {
		        new DocumentApi(null).GetDocument(_configurationId, _documentId, null, 0, 1);
		    }
		    catch (Exception e)
		    {
		        result = e.Message.ToDynamic();
		    }

            Assert.AreEqual(result.IsValid, false);
            Assert.AreEqual(result.ValidationMessage.ValidationErrors.IsValid, false);
            Assert.AreEqual(result.ValidationMessage.ValidationErrors.Message[0], string.Format(
                    Resources.AccessDeniedToAction,
                    "TestUser", _configurationId, _documentId, "getdocument"));		                        
		}

        [Test]
        public void ShouldDenyAccessForAction()
        {
            CreateTestConfig();

			//логинимся под админом
			new SignInApi().SignInInternal("Admin", "Admin", false);

            //password: "Password1"
			new AuthApi().AddUser("TestUser", "Password1");

			new SignInApi().SignOutInternal();

            var id = Guid.NewGuid().ToString();
            dynamic testDocument = new DynamicWrapper();
            testDocument.Id = id;
            testDocument.TestProperty = "1";

            //залогиниваемся под простым пользователем
            new SignInApi().SignInInternal("TestUser", "Password1", false);

            //записываем документ
            var result = new DocumentApi(null).SetDocument(_configurationId, _documentId, testDocument);

            Assert.AreEqual(result.IsValid, true);

			new SignInApi().SignOutInternal();

			new SignInApi().SignInInternal("Admin", "Admin", false);

            //отбираем доступ пользователя к документу на запись, на чтение не меняем (по умолчанию - если нет явного запрета - разрешено)
            new AuthApi().DenyAccess("TestUser", _configurationId, _documentId, "setdocument");

			new SignInApi().SignOutInternal();

			//залогиниваемся под простым пользователем
			new SignInApi().SignInInternal("TestUser", "Password1", false);

	        try
	        {
		        //проверяем отсутствие доступа к этому же документу при попытке сохранения
		        new DocumentApi(null).SetDocument(_configurationId, _documentId, testDocument);
	        }
			catch (Exception e)
			{
				Assert.True(e.Message.Contains("access denied"));
			}

            //проверяем отсутствие доступа к другому документу при попытке сохранения
            var id1 = Guid.NewGuid().ToString();
            dynamic testDocument1 = new DynamicWrapper();
            testDocument1.Id = id1;
            testDocument1.TestProperty = "2";
	        try
	        {
		        new DocumentApi(null).SetDocument(_configurationId, _documentId, testDocument1);
	        }
	        catch (Exception e)
	        {
		        Assert.True(e.Message.Contains("access denied"));
	        }


	        //проверяем доступность на чтение

            IEnumerable<dynamic> docs = new DocumentApi(null).GetDocument(_configurationId, _documentId, null, 0, 1000);
            Assert.True(docs.Any());

			new SignInApi().SignOutInternal();
        }

        [Test]
        public void ShouldExcludeDocumentsWhichDenied()
        {
            CreateTestConfig();

			//залогиниваемся под админом, чтобы добавить пользователя
			new SignInApi().SignInInternal("Admin", "Admin", false);

            //password: "Password1"
			new AuthApi().AddUser("TestUser", "Password1");

			new SignInApi().SignOutInternal();

            //залогиниваемся
            new SignInApi().SignInInternal("TestUser", "Password1", false);

            var id = Guid.NewGuid().ToString();
            dynamic testDocument = new DynamicWrapper();
            testDocument.Id = id;
            testDocument.TestProperty = "1";
            
            var result = new DocumentApi(null).SetDocument(_configurationId, _documentId, testDocument);
            Assert.AreEqual(result.IsValid,true);

            var id1 = Guid.NewGuid().ToString();
            dynamic testDocument1 = new DynamicWrapper();
            testDocument1.Id = id1;
            testDocument1.TestProperty = "2";

            result = new DocumentApi(null).SetDocument(_configurationId, _documentId, testDocument1);
            Assert.AreEqual(result.IsValid,true);

			new SignInApi().SignOutInternal();

			new SignInApi().SignInInternal("Admin", "Admin", false);

            //отбираем доступ на просмотр конкретного экземпляра документа
            new AuthApi().DenyAccess("TestUser", _configurationId, _documentId, "getdocument", id);

			new SignInApi().SignOutInternal();

            //проверяем, что при запросе возвращает только один документ
			new SignInApi().SignInInternal("TestUser", "Password1", false);

            IEnumerable<dynamic> docs = new DocumentApi(null).GetDocument(_configurationId, _documentId, null, 0, 10);

            Assert.AreEqual(1, docs.Count());
            Assert.AreEqual(docs.First().Id, id1);

			new SignInApi().SignOutInternal();
        }

        [Test]
        public void ShouldGrantAccessToDocumentInstanceAndDenyAccessToDocumentType()
        {
            CreateTestConfig();

			//залогиниваемся под админом, чтобы добавить пользователя
			new SignInApi().SignInInternal("Admin", "Admin", false);

            //password: "Password1"
			new AuthApi().AddUser("TestUser", "Password1");


            var id = Guid.NewGuid().ToString();
            dynamic testDocument = new DynamicWrapper();
            testDocument.Id = id;
            testDocument.TestProperty = "1";

            //добавляем доступ на редактирование конкретного экземпляра документа
            new AuthApi().GrantAccess("TestUser", _configurationId, _documentId, "setdocument", id);

            //отбираем доступ пользователя к документу
            new AuthApi().DenyAccess("TestUser", _configurationId, _documentId);

			new SignInApi().SignOutInternal();

			//залогиниваемся под обычным пользователем
			new SignInApi().SignInInternal("TestUser", "Password1", false);

            //проверяем наличие доступа к конкретному экземпляру
            var result = new DocumentApi(null).SetDocument(_configurationId, _documentId, testDocument);

            Assert.AreEqual(result.IsValid, true);

            //проверяем отсутствие доступа ко все документам
            try
            {
                new DocumentApi(null).GetDocument(_configurationId, _documentId, null, 0, 10);
            }
            catch (Exception e)
            {
                result = e.Message.ToDynamic();
            }

            Assert.AreEqual(result.IsValid, false);
            Assert.AreEqual(result.ValidationMessage.ValidationErrors.IsValid, false);
            Assert.AreEqual(result.ValidationMessage.ValidationErrors.Message[0], string.Format(
                    Resources.AccessDeniedToAction,
                    "TestUser", _configurationId, _documentId, "getdocument"));		   

			new SignInApi().SignOutInternal();
        }

        [Test]
        public void ShouldDenyAccessForDocumentInstance()
        {
            CreateTestConfig();

			//логинимся под админом
			new SignInApi().SignInInternal("Admin", "Admin", false);

            //password: "Password1"
			new AuthApi().AddUser("TestUser", "Password1");

            var id = Guid.NewGuid().ToString();
            dynamic testDocument = new DynamicWrapper();
            testDocument.Id = id;
            testDocument.TestProperty = "1";
          
            //отбираем доступ пользователя к документу
            new AuthApi().DenyAccess("TestUser", _configurationId, _documentId, "setdocument",id);

			new SignInApi().SignOutInternal();

			//залогиниваемся под пользователем
			new SignInApi().SignInInternal("TestUser", "Password1", false);


            //проверяем наличие доступа к другому документу
            var id1 = Guid.NewGuid().ToString();
            dynamic testDocument1 = new DynamicWrapper();
            testDocument1.Id = id1;
            testDocument1.TestProperty = "2";
            
            var result = new DocumentApi(null).SetDocument(_configurationId, _documentId, testDocument1);
            Assert.AreEqual(result.IsValid, true);

            //проверяем отсутствие доступа к конкретному экземпляру
	        try
	        {
		        new DocumentApi(null).SetDocument(_configurationId, _documentId, testDocument);
	        }
	        catch (Exception e)
	        {
		        Assert.True(e.Message.Contains("access denied"));
	        }
        }

        [Test]
        public void ShouldDenyAccessAllUsers()
        {
            CreateTestConfig();

            //password: "Password1"
			new AuthApi().AddUser("TestUser", "Password1");

            //залогиниваемся под админом
            new SignInApi().SignInInternal("Admin", "Admin", false);


            var id = Guid.NewGuid().ToString();
            dynamic testDocument = new DynamicWrapper();
            testDocument.Id = id;
            testDocument.TestProperty = "1";

            //отбираем доступ у всех пользователей
            new AuthApi().DenyAccessAll(_configurationId, _documentId);

			new SignInApi().SignOutInternal();

			//залогиниваемся под простым пользователем
			new SignInApi().SignInInternal("TestUser", "Password1", false);

            //проверяем отсутствие доступа у пользователя
	        try
	        {
		        new DocumentApi(null).SetDocument(_configurationId, _documentId, testDocument);
	        }
	        catch (Exception e)
	        {
		        Assert.True(e.Message.Contains("access denied"));
	        }
        }

        [Test]
        public void ShouldCheckAccessPerformanceLessThan50ms()
        {
            CreateTestConfig();

            for (int i = 0; i < 100; i++)
            {
                var id = Guid.NewGuid().ToString();
                dynamic testDocument = new DynamicWrapper();
                testDocument.Id = id;
                testDocument.TestProperty = i.ToString();

                new DocumentApi(null).SetDocument(_configurationId, _documentId, testDocument);
            }

            new DocumentApi(null).GetDocument(_configurationId, _documentId, null, 0, 1000);

            var watch = Stopwatch.StartNew();

            new DocumentApi(null).GetDocument(_configurationId, _documentId, null, 0, 1000);

            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);

            Assert.True(watch.ElapsedMilliseconds < 50);
        }

        [Test]
        public void ShouldApplyRoleRestrictions()
        {
            CreateTestConfig();

            //password: "Password1"
			new AuthApi().AddUser("TestUser", "Password1");

            new AuthApi().AddRole("TestRole","TestRole","TestRole");

            new AuthApi().AddUserToRole("TestUser", "TestRole");
		
            //залогиниваемся под администратором

			new SignInApi().SignInInternal("Admin", "Admin", false);

			//запрещаем роли доступ к тестовому документу
			new AuthApi().DenyAccessRole("TestRole", _configurationId, _documentId);

			//разлогиниваемся из-под админа
			new SignInApi().SignOutInternal();
			
			//логинимся под обычным пользователем без прав
			new SignInApi().SignInInternal("TestUser", "Password1", false);
           
            var id = Guid.NewGuid().ToString();
            dynamic testDocument = new DynamicWrapper();
            testDocument.Id = id;
            testDocument.TestProperty = "1";

            //проверяем отсутствие доступа у пользователя
	        try
	        {
		        var result = new DocumentApi(null).SetDocument(_configurationId, _documentId, testDocument);
	        }
	        catch (Exception e)
	        {
				Assert.True(e.Message.Contains("access denied"));   
	        }

        }

		[Test]
		public void ShouldDenyAccessToAdministrateDocument()
		{
			CreateTestConfig();

			//залогиниваемся под администратором
			new SignInApi().SignInInternal("Admin", "Admin", false);

			//password: "Password1"
			new AuthApi().AddUser("TestUser", "Password1");

			//отбираем доступ пользователя к документу
			new AuthApi().DenyAccess("TestUser", _configurationId, _documentId);
			
			new SignInApi().SignOutInternal();

			//залогиниваемся
			new SignInApi().SignInInternal("TestUser", "Password1", false);


			//пытаемся получить документы
			try
			{
				new AuthApi().GrantAccess("TestUser", _configurationId, _documentId);
			}
			catch (Exception e)
			{
				Assert.True(e.Message.Contains("access denied"));
			}


			//разлогиниваемся, чтобы добавить прав
			new SignInApi().SignOutInternal();

			//залогиниваемся под администратором
			new SignInApi().SignInInternal("Admin", "Admin", false);

			//возвращаем права
			dynamic result = new AuthApi().GrantAccess("TestUser", _configurationId, _documentId);

			Assert.AreNotEqual(result.IsValid,false);

			new SignInApi().SignOutInternal();

			//регистрируемся под тем же пользователем
			new SignInApi().SignInInternal("TestUser","Password1",false);

			var id = Guid.NewGuid().ToString();
			dynamic testDocument = new DynamicWrapper();
			testDocument.Id = id;
			testDocument.TestProperty = "1";

			//проверяем наличие доступа у пользователя
			result = new DocumentApi(null).SetDocument(_configurationId, _documentId, testDocument);
			Assert.AreEqual(result.IsValid, true);

		}

		private void CreateTestConfig()
		{
            AuthorizationExtensionsTest.ClearAuthConfig();
			new IndexApi().RebuildIndex(_configurationId,_documentId);

            var managerConfig = ManagerFactoryConfiguration.BuildConfigurationManager(null);
            dynamic config = managerConfig.CreateItem(_configurationId);
            managerConfig.DeleteItem(config);
            managerConfig.MergeItem(config);

            var managerFactoryDocument = new ManagerFactoryConfiguration(null, _configurationId);
            var documentManager = managerFactoryDocument.BuildDocumentManager();
            dynamic doc = documentManager.CreateItem(_documentId);
            documentManager.MergeItem(doc);
            
            RestQueryApi.QueryPostNotify(null, _configurationId);
            new UpdateApi(null).UpdateStore(_configurationId);


		}

	}
}
