using InfinniPlatform.Core.RestApi.CommonApi.RouteTraces;

namespace InfinniPlatform.Core.SelfDocumentation
{
    public static class DocumentationKeeperBuilder
    {
        private static readonly RouteTraceSaveQueryLog InnerTracer = new RouteTraceSaveQueryLog();

        public static RouteTraceSaveQueryLog Tracer
        {
            get { return InnerTracer; }
        }

        public static DocumentationKeeper Build(
            string helpPath,
            string assemblyPath,
            IDocumentationFormatter documentationFormatter)
        {
            return null;
        }
    }
}