using System.Security.Claims;

using InfinniPlatform.Core.Transactions;
using InfinniPlatform.DocumentStorage.MongoDB;
using InfinniPlatform.DocumentStorage.Storage;
using InfinniPlatform.DocumentStorage.Tests.MongoDB;
using InfinniPlatform.Sdk.Documents;
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
                new DocumentStorageIdProvider(new MongoDocumentIdGenerator()),
                new DocumentStorageHeaderProvider(tenantProvider.Object, userIdentityProvider.Object),
                new DocumentStorageFilterProvider(tenantProvider.Object),
                new DocumentStorageInterceptorProvider(null));
        }

        public static DocumentStorageImpl<TDocument> GetEmptyStorage<TDocument>(string documentType) where TDocument : Document
        {
            var tenantProvider = new Mock<ITenantProvider>();
            tenantProvider.Setup(i => i.GetTenantId()).Returns(FakeTenant);

            var userIdentityProvider = new Mock<IUserIdentityProvider>();
            userIdentityProvider.Setup(i => i.GetUserIdentity()).Returns(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, FakeUserId), new Claim(ClaimTypes.Name, FakeUserName) }, "TestAuth"));

            return new DocumentStorageImpl<TDocument>(
                documentType,
                d => MongoTestHelpers.GetEmptyStorageProvider<TDocument>(documentType),
                new DocumentStorageIdProvider(new MongoDocumentIdGenerator()),
                new DocumentStorageHeaderProvider(tenantProvider.Object, userIdentityProvider.Object),
                new DocumentStorageFilterProvider(tenantProvider.Object),
                new DocumentStorageInterceptorProvider(null));
        }
    }
}