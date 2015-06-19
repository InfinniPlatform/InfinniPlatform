using InfinniPlatform.Api.Context;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.Api.ContextTypes.ContextImpl
{
    public sealed class UrlEncodedDataContext : IUrlEncodedDataContext
    {
        public IGlobalContext Context { get; set; }
        public dynamic ValidationMessage { get; set; }
        public bool IsValid { get; set; }
        public bool IsInternalServerError { get; set; }
        public string Version { get; set; }
        public string Configuration { get; set; }
        public string Metadata { get; set; }
        public string Action { get; set; }
        public string UserName { get; set; }
        public dynamic Result { get; set; }
        public dynamic FormData { get; set; }
    }
}