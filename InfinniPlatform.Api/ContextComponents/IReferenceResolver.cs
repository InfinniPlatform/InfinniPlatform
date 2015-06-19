using System.Collections.Generic;

namespace InfinniPlatform.Api.ContextComponents
{
    public interface IReferenceResolver
    {
        void ResolveReferences(string version, string configId, string documentId, dynamic documents,
            IEnumerable<dynamic> ignoreResolve);
    }
}