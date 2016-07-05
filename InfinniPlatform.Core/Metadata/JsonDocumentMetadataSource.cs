using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.DocumentStorage.Hosting;
using InfinniPlatform.Sdk.Metadata.Documents;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Core.Metadata
{
    public class JsonDocumentMetadataSource : IDocumentMetadataSource
    {
        public JsonDocumentMetadataSource(MetadataSettings metadataSettings)
        {
            _metadataSettings = metadataSettings;
        }

        private readonly MetadataSettings _metadataSettings;

        public IEnumerable<DocumentMetadata> GetDocumentsMetadata()
        {
            var strings = Directory.GetFiles(Path.Combine(_metadataSettings.ContentDirectory, "metadata\\Documents"));

            var documentsMetadata = strings.Select(s =>
                                                   {
                                                       var bytes = File.ReadAllBytes(s);

                                                       var documentMetadata = JsonObjectSerializer.Default.Deserialize<DocumentMetadata>(bytes);

                                                       documentMetadata.Type = Path.GetFileNameWithoutExtension(s);

                                                       return documentMetadata;
                                                   });

            return documentsMetadata;
        }
    }
}