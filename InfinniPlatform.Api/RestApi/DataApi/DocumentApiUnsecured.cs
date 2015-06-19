namespace InfinniPlatform.Api.RestApi.DataApi
{
    public sealed class DocumentApiUnsecured : DocumentApi
    {
        public DocumentApiUnsecured(string version) : base(version, false)
        {
        }
    }
}