using InfinniPlatform.DocumentStorage.QueryFactories;
using InfinniPlatform.DocumentStorage.QuerySyntax;
using InfinniPlatform.Http;
using InfinniPlatform.IoC;

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

            builder.RegisterType<DocumentHttpService>()
                   .AsSelf()
                   .InstancePerDependency();

            builder.RegisterGeneric(typeof(DocumentHttpService<>))
                   .As(typeof(DocumentHttpService<>))
                   .InstancePerDependency();

            builder.RegisterType<DocumentHttpServiceFactory>()
                   .As<IDocumentHttpServiceFactory>()
                   .SingleInstance();

            builder.RegisterType<HttpServiceWrapperFactory>()
                   .As<IHttpServiceWrapperFactory>()
                   .SingleInstance();

            builder.RegisterType<DocumentHttpServiceSource>()
                   .As<IHttpServiceSource>()
                   .SingleInstance();
        }
    }
}