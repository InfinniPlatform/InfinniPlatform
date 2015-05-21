using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Api;
using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{

    [Ignore("Тесты SDK не выполняют запуск сервера InfinniPlatform. Необходимо существование уже запущенного сервера на localhost : 9900")]
    [TestFixture]
    public sealed class SignInApiTest
    {
        private const string InfinniSessionPort = "9900";
        private const string InfinniSessionServer = "localhost";
        private const string InfinniSessionVersion = "1";
        private InfinniSignInApi _signInApi;

        [TestFixtureSetUp]
        public void SetupApi()
        {
            _signInApi = new InfinniSignInApi(InfinniSessionServer, InfinniSessionPort, InfinniSessionVersion);
        }

        [Test]
        public void ShouldSignInUser()
        {
            dynamic signResult = _signInApi.SignInInternal("Admin", "Admin", true);
            Assert.AreEqual(Convert.ToBoolean(signResult.IsValid), true);
        }

        [Test]
        public void ShouldSignOutUser()
        {
            dynamic signResult = _signInApi.SignOut();
            Assert.AreEqual(Convert.ToBoolean(signResult.IsValid), true);            
        }

        [Test]
        public void ShouldCantChangePassword()        
        {
            try
            {
                dynamic changePasswordResult = _signInApi.ChangePassword("Admin", "Admin", "Admin1");
            }
            catch (Exception e)
            {
                //не можем изменить пароль пользователя суперадмина
                Assert.True(e.Message.Contains("User not found"));    
            }
            
        }
    }
}
