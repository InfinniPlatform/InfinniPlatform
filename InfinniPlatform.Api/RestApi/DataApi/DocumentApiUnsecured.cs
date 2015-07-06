namespace InfinniPlatform.Api.RestApi.DataApi
{
    public sealed class DocumentApiUnsecured : DocumentApi
    {
        public DocumentApiUnsecured() : base(false)
        {
        }
    }
}