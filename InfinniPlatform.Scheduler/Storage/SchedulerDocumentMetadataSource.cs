using System.Collections.Generic;

using InfinniPlatform.Scheduler.Common;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Metadata.Documents;

namespace InfinniPlatform.Scheduler.Storage
{
    /// <summary>
    /// Предоставляет метаданные документов планировщика заданий.
    /// </summary>
    internal class SchedulerDocumentMetadataSource : IDocumentMetadataSource
    {
        public IEnumerable<DocumentMetadata> GetDocumentsMetadata()
        {
            return new[]
                   {
                       new DocumentMetadata
                       {
                           Type = DocumentStorageExtensions.GetDefaultDocumentTypeName<JobInfo>(),
                           Indexes = new[]
                                     {
                                         new DocumentIndex
                                         {
                                             Key = new Dictionary<string, DocumentIndexKeyType>
                                                   {
                                                       { "Name", DocumentIndexKeyType.Asc }
                                                   }
                                         },
                                         new DocumentIndex
                                         {
                                             Key = new Dictionary<string, DocumentIndexKeyType>
                                                   {
                                                       { "Group", DocumentIndexKeyType.Asc }
                                                   }
                                         },
                                         new DocumentIndex
                                         {
                                             Key = new Dictionary<string, DocumentIndexKeyType>
                                                   {
                                                       { "State", DocumentIndexKeyType.Asc }
                                                   }
                                         }
                                     }
                       }
                   };
        }
    }
}