using System;
using System.Collections.Generic;

using InfinniPlatform.Scheduler.Common;
using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Metadata.Documents;

namespace InfinniPlatform.Scheduler.Storage
{
    /// <summary>
    /// Предоставляет метаданные документов планировщика заданий.
    /// </summary>
    internal class SchedulerDocumentMetadataSource : IDocumentMetadataSource
    {
        public SchedulerDocumentMetadataSource(SchedulerSettings settings)
        {
            _settings = settings;
        }


        private readonly SchedulerSettings _settings;


        public IEnumerable<DocumentMetadata> GetDocumentsMetadata()
        {
            yield return new DocumentMetadata
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
                         };

            if (_settings.ExpireHistoryAfter != null && _settings.ExpireHistoryAfter > 0)
            {
                yield return new DocumentMetadata
                             {
                                 Type = DocumentStorageExtensions.GetDefaultDocumentTypeName<JobInstance>(),
                                 Indexes = new[]
                                           {
                                               new DocumentIndex
                                               {
                                                   ExpireAfter = TimeSpan.FromSeconds(_settings.ExpireHistoryAfter.Value),
                                                   Key = new Dictionary<string, DocumentIndexKeyType>
                                                         {
                                                             { "_header._created", DocumentIndexKeyType.Ttl }
                                                         }
                                               }
                                           }
                             };
            }
        }
    }
}