using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage
{
    public interface IDocumentControllerProcessorProvider
    {
        Dictionary<string, IDocumentControllerProcessor> ProcessorsCache { get; }
    }
}