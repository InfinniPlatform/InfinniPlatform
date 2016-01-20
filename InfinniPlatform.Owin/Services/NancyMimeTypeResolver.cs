using InfinniPlatform.Sdk.Services;

using Nancy;

namespace InfinniPlatform.Owin.Services
{
    internal sealed class NancyMimeTypeResolver : IMimeTypeResolver
    {
        public string GetMimeType(string fileName)
        {
            return MimeTypes.GetMimeType(fileName);
        }
    }
}