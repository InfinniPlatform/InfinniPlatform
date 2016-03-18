using System;
using System.Security.Claims;

using InfinniPlatform.Core.Transactions;
using InfinniPlatform.DocumentStorage.MongoDB;
using InfinniPlatform.DocumentStorage.Storage;
using InfinniPlatform.DocumentStorage.Tests.MongoDB;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Metadata.Documents;
using InfinniPlatform.Sdk.Security;

using Moq;

namespace InfinniPlatform.DocumentStorage.Tests.Storage
{
    internal static class DocumentStorageTestHelpers
    {
        public const string FakeTenant = "FakeTenant";
        public const string FakeUserId = "FakeUserId";
        public const string FakeUserName = "FakeUserName";


        public static DocumentStorageImpl GetStorage(string documentType = null)
        {
            var tenantProvider = GetTenantProvider();
            var userIdentityProvider = GetUserIdentityProvider();

            return new DocumentStorageImpl(
                documentType,
                GetStorageProviderFactory(d => MongoTestHelpers.GetStorageProvider(documentType)),
                new DocumentStorageIdProvider(new MongoDocumentIdGenerator()),
                new DocumentStorageHeaderProvider(tenantProvider, userIdentityProvider),
                new DocumentStorageFilterProvider(tenantProvider),
                new DocumentStorageInterceptorProvider(null));
        }

        public static DocumentStorageImpl<TDocument> GetStorage<TDocument>(string documentType = null) where TDocument : Document
        {
            var tenantProvider = GetTenantProvider();
            var userIdentityProvider = GetUserIdentityProvider();

            return new DocumentStorageImpl<TDocument>(
                GetStorageProviderFactory(d => MongoTestHelpers.GetStorageProvider<TDocument>(documentType)),
                new DocumentStorageIdProvider(new MongoDocumentIdGenerator()),
                new DocumentStorageHeaderProvider(tenantProvider, userIdentityProvider),
                new DocumentStorageFilterProvider(tenantProvider),
                new DocumentStorageInterceptorProvider(null), documentType);
        }


        public static DocumentStorageImpl GetEmptyStorage(string documentType, params DocumentIndex[] indexes)
        {
            var tenantProvider = GetTenantProvider();
            var userIdentityProvider = GetUserIdentityProvider();

            return new DocumentStorageImpl(
                documentType,
                GetStorageProviderFactory(d => MongoTestHelpers.GetEmptyStorageProvider(documentType, indexes)),
                new DocumentStorageIdProvider(new MongoDocumentIdGenerator()),
                new DocumentStorageHeaderProvider(tenantProvider, userIdentityProvider),
                new DocumentStorageFilterProvider(tenantProvider),
                new DocumentStorageInterceptorProvider(null));
        }

        public static DocumentStorageImpl<TDocument> GetEmptyStorage<TDocument>(string documentType, params DocumentIndex[] indexes) where TDocument : Document
        {
            var tenantProvider = GetTenantProvider();
            var userIdentityProvider = GetUserIdentityProvider();

            return new DocumentStorageImpl<TDocument>(
                GetStorageProviderFactory(d => MongoTestHelpers.GetEmptyStorageProvider<TDocument>(d, indexes)),
                new DocumentStorageIdProvider(new MongoDocumentIdGenerator()),
                new DocumentStorageHeaderProvider(tenantProvider, userIdentityProvider),
                new DocumentStorageFilterProvider(tenantProvider),
                new DocumentStorageInterceptorProvider(null), documentType);
        }


        private static IDocumentStorageProviderFactory GetStorageProviderFactory(Func<string, IDocumentStorageProvider> factory)
        {
            var storageProviderFactory = new Mock<IDocumentStorageProviderFactory>();
            storageProviderFactory.Setup(i => i.GetStorageProvider(It.IsAny<string>())).Returns(factory);
            return storageProviderFactory.Object;
        }

        private static IDocumentStorageProviderFactory GetStorageProviderFactory<TDocument>(Func<string, IDocumentStorageProvider<TDocument>> factory)
        {
            var storageProviderFactory = new Mock<IDocumentStorageProviderFactory>();
            storageProviderFactory.Setup(i => i.GetStorageProvider<TDocument>(It.IsAny<string>())).Returns(factory);
            return storageProviderFactory.Object;
        }

        private static ITenantProvider GetTenantProvider()
        {
            var tenantProvider = new Mock<ITenantProvider>();
            tenantProvider.Setup(i => i.GetTenantId()).Returns(FakeTenant);
            return tenantProvider.Object;
        }

        private static IUserIdentityProvider GetUserIdentityProvider()
        {
            var userIdentityProvider = new Mock<IUserIdentityProvider>();
            userIdentityProvider.Setup(i => i.GetUserIdentity()).Returns(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, FakeUserId), new Claim(ClaimTypes.Name, FakeUserName) }, "TestAuth"));
            return userIdentityProvider.Object;
        }
    }
}