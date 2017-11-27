using InfinniPlatform.DocumentStorage.QueryFactories;
using InfinniPlatform.DocumentStorage.QuerySyntax;
using InfinniPlatform.IoC;

using Microsoft.AspNetCore.Mvc;

namespace InfinniPlatform.DocumentStorage.IoC
{
    public class DocumentStorageHttpServiceContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<QuerySyntaxTreeParser>()
                   .As<IQuerySyntaxTreeParser>()
                   .SingleInstance();

            builder.RegisterType<DocumentQueryFactory>()
                   .As<IDocumentQueryFactory>()
                   .SingleInstance();

            builder.RegisterGeneric(typeof(DocumentQueryFactory<>))
                   .As(typeof(IDocumentQueryFactory<>))
                   .SingleInstance();

            builder.RegisterType<DocumentsController>()
                   .As<Controller>()
                   .AsSelf()
                   .InstancePerDependency();

            builder.RegisterType<DocumentRequestExecutor>()
                   .AsSelf()
                   .InstancePerDependency();

            builder.RegisterGeneric(typeof(DocumentRequestExecutor<>))
                   .As(typeof(DocumentRequestExecutor<>))
                   .InstancePerDependency();

            builder.RegisterType<DocumentRequestExecutorProvider>()
                   .As<IDocumentRequestExecutorProvider>()
                   .SingleInstance();
        }
    }
}