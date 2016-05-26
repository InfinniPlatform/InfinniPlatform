using InfinniPlatform.DocumentStorage.MongoDB.Conventions;
using InfinniPlatform.DocumentStorage.Tests.Storage;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.DocumentStorage.Tests.IoC
{
    internal sealed class DocumentStorageTestsContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<DocumentKnownTypeSource>()
                   .As<IDocumentKnownTypeSource>()
                   .SingleInstance();
        }
    }
}