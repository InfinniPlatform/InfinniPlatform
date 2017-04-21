using System;
using System.Collections.Generic;

using InfinniPlatform.DocumentStorage;
using InfinniPlatform.DocumentStorage.Metadata;
using InfinniPlatform.Scheduler.Common;

namespace InfinniPlatform.Scheduler.Storage
{
    /// <summary>
    /// Предоставляет метаданные документов планировщика заданий.
    /// </summary>
    internal class SchedulerDocumentMetadataSource : IDocumentMetadataSource
    {
        public SchedulerDocumentMetadataSource(QuartzSchedulerOptions options)
        {
            _options = options;
        }


        private readonly QuartzSchedulerOptions _options;


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

            if (_options.ExpireHistoryAfter != null && _options.ExpireHistoryAfter > 0)
            {
                yield return new DocumentMetadata
                             {
                                 Type = DocumentStorageExtensions.GetDefaultDocumentTypeName<JobInstance>(),
                                 Indexes = new[]
                                           {
                                               new DocumentIndex
                                               {
                                                   ExpireAfter = TimeSpan.FromSeconds(_options.ExpireHistoryAfter.Value),
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