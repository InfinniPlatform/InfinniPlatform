using System;
using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage.MongoDB.Conventions
{
    public interface IDocumentKnownTypeSource
    {
        IEnumerable<Type> KnownTypes { get; }
    }
}