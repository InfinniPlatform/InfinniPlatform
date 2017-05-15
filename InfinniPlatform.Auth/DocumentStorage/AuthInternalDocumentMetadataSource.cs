using System.Collections.Generic;
using InfinniPlatform.DocumentStorage.Metadata;

namespace InfinniPlatform.Auth.DocumentStorage
{
    internal class AuthInternalDocumentMetadataSource : IDocumentMetadataSource
    {
        public IEnumerable<DocumentMetadata> GetDocumentsMetadata()
        {
            var userStore = new DocumentMetadata
                            {
                                Type = "UserStore",
                                Indexes = new[]
                                          {
                                              new DocumentIndex
                                              {
                                                  Key = new Dictionary<string, DocumentIndexKeyType>
                                                        {
                                                            { "UserName", DocumentIndexKeyType.Asc }
                                                        }
                                              },
                                              new DocumentIndex
                                              {
                                                  Key = new Dictionary<string, DocumentIndexKeyType>
                                                        {
                                                            { "Email", DocumentIndexKeyType.Asc }
                                                        }
                                              },
                                              new DocumentIndex
                                              {
                                                  Key = new Dictionary<string, DocumentIndexKeyType>
                                                        {
                                                            { "PhoneNumber", DocumentIndexKeyType.Asc }
                                                        }
                                              },
                                              new DocumentIndex
                                              {
                                                  Key = new Dictionary<string, DocumentIndexKeyType>
                                                        {
                                                            { "Logins.ProviderKey", DocumentIndexKeyType.Asc }
                                                        }
                                              }
                                          }
                            };

            return new[] { userStore };
        }
    }
}