using System.IO;
using Microsoft.AspNetCore.StaticFiles;

namespace InfinniPlatform.Http
{
    /// <inheritdoc />
    public class MimeTypeResolver : IMimeTypeResolver
    {
        private const string DefaultContentType = "application/octet-stream";
        private readonly FileExtensionContentTypeProvider _contentTypeProvider;

        /// <inheritdoc />
        public MimeTypeResolver()
        {
            _contentTypeProvider = new FileExtensionContentTypeProvider();
        }

        /// <inheritdoc />
        public string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            return _contentTypeProvider.TryGetContentType(extension, out var contentType)
                       ? contentType
                       : DefaultContentType;
        }
    }
}