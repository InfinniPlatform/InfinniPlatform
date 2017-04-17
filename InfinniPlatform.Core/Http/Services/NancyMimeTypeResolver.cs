using InfinniPlatform.Core.Abstractions.Http;

using Nancy;

namespace InfinniPlatform.Core.Http.Services
{
    internal class NancyMimeTypeResolver : IMimeTypeResolver
    {
        public string GetMimeType(string fileName)
        {
            return MimeTypes.GetMimeType(fileName);
        }
    }
}