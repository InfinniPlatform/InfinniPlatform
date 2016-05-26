using System;
using System.Collections.Generic;

using FakeNamespace.DontChange;

using InfinniPlatform.DocumentStorage.MongoDB.Conventions;

namespace InfinniPlatform.DocumentStorage.Tests.Storage
{
    public class DocumentKnownTypeSource : IDocumentKnownTypeSource
    {
        public IEnumerable<Type> KnownTypes => new List<Type> { typeof(C1), typeof(C2) };
    }
}