using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Core.ContextTypes
{
    public sealed class UrlEncodedDataContext : CommonContext, IUrlEncodedDataContext
    {
        public dynamic FormData { get; set; }
    }
}