using InfinniPlatform.Api.DocumentStatus;

namespace InfinniPlatform.Metadata.Statuses
{
    public static class DocumentStateProviderFactory
    {
        public static IDocumentStateProvider GetInstance(IStatusFactory statusFactory)
        {
            return new DocumentStateProviderBase(statusFactory);
        }
    }
}
