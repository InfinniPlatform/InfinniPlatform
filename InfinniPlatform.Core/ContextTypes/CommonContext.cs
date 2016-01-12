using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Core.ContextTypes
{
    public abstract class CommonContext : ICommonContext
    {
        protected CommonContext()
        {
            IsValid = true;
        }

        public string Configuration { get; set; }

        public string Metadata { get; set; }

        public string Action { get; set; }

        public bool IsValid { get; set; }

        public bool IsInternalServerError { get; set; }

        public dynamic ValidationMessage { get; set; }

        public dynamic Result { get; set; }
    }
}