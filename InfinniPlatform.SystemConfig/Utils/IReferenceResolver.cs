using System.Collections.Generic;

namespace InfinniPlatform.SystemConfig.Utils
{
    public interface IReferenceResolver
    {
        void ResolveReferences(string configId, string documentId, dynamic documents, IEnumerable<dynamic> ignoreResolve);
    }
}