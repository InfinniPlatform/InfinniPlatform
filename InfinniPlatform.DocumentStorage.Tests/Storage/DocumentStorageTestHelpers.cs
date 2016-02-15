using System.Security.Claims;

using InfinniPlatform.Core.Transactions;
using InfinniPlatform.DocumentStorage.Storage;
using InfinniPlatform.DocumentStorage.Tests.MongoDB;
using InfinniPlatform.Sdk.Security;

using Moq;

namespace InfinniPlatform.DocumentStorage.Tests.Storage
{
    internal static class DocumentStorageTestHelpers
    {
        public const string FakeTenant = "FakeTenant";
        public const string FakeUserId = "FakeUserId";
        public const string FakeUserName = "FakeUserName";


        public static DocumentStorageImpl GetEmptyStorage(string documentType)
        {
            var tenantProvider = new Mock<ITenantProvider>();
            tenantProvider.Setup(i => i.GetTenantId()).Returns(FakeTenant);

            var userIdentityProvider = new Mock<IUserIdentityProvider>();
            userIdentityProvider.Setup(i => i.GetUserIdentity()).Returns(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, FakeUserId), new Claim(ClaimTypes.Name, FakeUserName) }, "TestAuth"));

            return new DocumentStorageImpl(
                documentType,
                d => MongoTestHelpers.GetEmptyStorageProvider(documentType),
                new DocumentStorageIdProvider(),
                new DocumentStorageHeaderProvider(tenantProvider.Object, userIdentityProvider.Object),
                new DocumentStorageFilterProvider(tenantProvider.Object),
                new DocumentStorageInterceptorProvider(null));
        }
    }
}