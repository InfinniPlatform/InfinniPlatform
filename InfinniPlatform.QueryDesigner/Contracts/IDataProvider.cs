using System.Collections.Generic;

using InfinniPlatform.Core.Schema;

namespace InfinniPlatform.QueryDesigner.Contracts
{
    public interface IDataProvider
    {
        IEnumerable<dynamic> GetConfigurationList();
        IEnumerable<object> GetDocuments(string configurationId);
        dynamic GetDocumentSchema(string configuration, string document);

        IEnumerable<SchemaObject> GetPropertyPaths(string configuration, string document, string alias,
            PathResolveType pathResovleType);
    }
}