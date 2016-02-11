using System;

using InfinniPlatform.DocumentStorage.MongoDB;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.DocumentStorage.IoC
{
    internal sealed class DocumentStorageContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterFactory(GetMongoConnectionSettings)
                   .As<MongoConnectionSettings>()
                   .SingleInstance();

            builder.RegisterFactory(GetMongoConnection)
                   .As<MongoConnection>()
                   .SingleInstance();

            builder.RegisterType<MongoDocumentStorageManager>()
                   .As<IDocumentStorageManager>()
                   .SingleInstance();

            builder.RegisterType<MongoDocumentStorageProvider>()
                   .As<IDocumentStorageProvider>()
                   .InstancePerDependency();

            builder.RegisterGeneric(typeof(MongoDocumentStorageProvider<>))
                   .As(typeof(IDocumentStorageProvider<>))
                   .InstancePerDependency();
        }


        private static MongoConnectionSettings GetMongoConnectionSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<MongoConnectionSettings>(MongoConnectionSettings.SectionName);
        }

        private static MongoConnection GetMongoConnection(IContainerResolver resolver)
        {
            return new MongoConnection(resolver.Resolve<IAppEnvironment>().Name, resolver.Resolve<MongoConnectionSettings>());
        }
    }
}