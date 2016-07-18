using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Core.Extensions;
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
            var documentsPath = Path.Combine(_metadataSettings.DocumentsPath).ToFileSystemPath();

            if (!Directory.Exists(documentsPath))
            {
                return Enumerable.Empty<DocumentMetadata>();
            }

            var strings = Directory.GetFiles(documentsPath);

            var documentsMetadata = strings.Select(file =>
                                                   {
                                                       var bytes = File.ReadAllBytes(file);

                                                       var documentMetadata = JsonObjectSerializer.Default.Deserialize<DocumentMetadata>(bytes);

                                                       documentMetadata.Type = Path.GetFileNameWithoutExtension(file);

                                                       return documentMetadata;
                                                   });

            return documentsMetadata;
        }
    }
}