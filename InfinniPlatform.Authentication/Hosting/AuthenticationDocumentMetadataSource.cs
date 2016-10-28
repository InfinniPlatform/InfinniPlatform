using System.Collections.Generic;

using InfinniPlatform.Sdk.Documents.Metadata;

namespace InfinniPlatform.Authentication.Hosting
{
    public class AuthenticationDocumentMetadataSource : IDocumentMetadataSource
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