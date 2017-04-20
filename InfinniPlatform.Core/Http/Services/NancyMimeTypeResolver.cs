using Nancy;

namespace InfinniPlatform.Http.Services
{
    internal class NancyMimeTypeResolver : IMimeTypeResolver
    {
        public string GetMimeType(string fileName)
        {
            return MimeTypes.GetMimeType(fileName);
        }
    }
}