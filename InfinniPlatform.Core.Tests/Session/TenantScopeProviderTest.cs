using System.Threading.Tasks;

using InfinniPlatform.Core.Session;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Session
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    internal class TenantScopeProviderTest
    {
        [Test]
        public void GetTenantScope_Returns_Null_When_Root_Scope()
        {
            // Given
            var scopeProvider = new TenantScopeProvider();

            // When
            var rootScope = scopeProvider.GetTenantScope();

            // Then
            Assert.IsNull(rootScope);
        }

        [Test]
        public async Task GetTenantScope_Returns_Current_Scope()
        {
            // Given

            const string scope1 = "scope1";
            const string scope2 = "scope2";
            const string scope3 = "scope3";

            var scopeProvider = new TenantScopeProvider();

            // When

            string actualScope11, actualScope12, actualScope13;
            string actualScope21, actualScope22;
            string actualScope31, actualScope32;

            using (scopeProvider.BeginTenantScope(scope1))
            {
                actualScope11 = scopeProvider.GetTenantScope()?.TenantId;

                await FakeMethodAsync();

                using (scopeProvider.BeginTenantScope(scope2))
                {
                    actualScope21 = scopeProvider.GetTenantScope()?.TenantId;
                    await FakeMethodAsync();
                    actualScope22 = scopeProvider.GetTenantScope()?.TenantId;
                }

                actualScope12 = scopeProvider.GetTenantScope()?.TenantId;

                await FakeMethodAsync();

                using (scopeProvider.BeginTenantScope(scope3))
                {
                    actualScope31 = scopeProvider.GetTenantScope()?.TenantId;
                    await FakeMethodAsync();
                    actualScope32 = scopeProvider.GetTenantScope()?.TenantId;
                }

                actualScope13 = scopeProvider.GetTenantScope()?.TenantId;

                await FakeMethodAsync();
            }

            var actualRoot = scopeProvider.GetTenantScope()?.TenantId;


            // Then

            Assert.IsNull(actualRoot);

            Assert.AreEqual(scope1, actualScope11);
            Assert.AreEqual(scope1, actualScope12);
            Assert.AreEqual(scope1, actualScope13);

            Assert.AreEqual(scope2, actualScope21);
            Assert.AreEqual(scope2, actualScope22);

            Assert.AreEqual(scope3, actualScope31);
            Assert.AreEqual(scope3, actualScope32);
        }


        private static async Task FakeMethodAsync()
        {
            await Task.Delay(0);
        }
    }
}