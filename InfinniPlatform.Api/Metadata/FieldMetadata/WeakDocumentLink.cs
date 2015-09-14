namespace InfinniPlatform.Api.Metadata.FieldMetadata
{
    /// <summary>
    ///     Объект слабой ссылки на документ (независимая ссылка)
    /// </summary>
    public sealed class WeakDocumentLink
    {
        private readonly string _configId;
        private readonly dynamic _documentContainer;
        private readonly string _documentId;
        private readonly string _documentLinkId;
        private readonly string _propertyName;

        public WeakDocumentLink(string propertyName, string configId, string documentId, dynamic documentContainer,
            string documentLinkId)
        {
            _propertyName = propertyName;
            _configId = configId;
            _documentId = documentId;
            _documentContainer = documentContainer;
            _documentLinkId = documentLinkId;
        }

        public string ConfigId
        {
            get { return _configId; }
        }

        public string DocumentId
        {
            get { return _documentId; }
        }

        public dynamic DocumentContainer
        {
            get { return _documentContainer; }
        }

        public string DocumentLinkId
        {
            get { return _documentLinkId; }
        }

        public void Resolve(dynamic document)
        {
            DocumentContainer[_propertyName] = document;
        }
    }
}