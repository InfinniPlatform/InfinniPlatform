﻿using System.Collections.Generic;

namespace InfinniPlatform.Sdk.ContextComponents
{
    public interface IReferenceResolver
    {
        void ResolveReferences(string configId, string documentId, dynamic documents, IEnumerable<dynamic> ignoreResolve);
    }
}