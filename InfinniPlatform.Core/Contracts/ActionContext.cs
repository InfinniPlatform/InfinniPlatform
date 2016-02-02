using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Core.Contracts
{
    public sealed class ActionContext : IActionContext
    {
        public ActionContext()
        {
            IsValid = true;
        }

        public string DocumentType { get; set; }

        public dynamic Item { get; set; }

        public bool IsValid { get; set; }

        public dynamic Result { get; set; }

        public dynamic ValidationMessage { get; set; }
    }
}