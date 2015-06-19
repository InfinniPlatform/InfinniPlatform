using InfinniPlatform.Api.ContextTypes;

namespace InfinniPlatform.Metadata
{
    public class ExtensionPointType
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public ContextTypeKind ContextTypeKind { get; set; }
    }
}