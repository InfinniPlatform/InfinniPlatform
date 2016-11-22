using System;
using System.IO;

using Nancy;

using SquishIt.Framework;

namespace InfinniPlatform.Core.Http.Services.Bundling
{
    public class NancyPathTranslator : IPathTranslator
    {
        public NancyPathTranslator(IRootPathProvider rootPathProvider)
        {
            _rootPathProvider = rootPathProvider;
        }

        private readonly IRootPathProvider _rootPathProvider;

        public string ResolveAppRelativePathToFileSystem(string file)
        {
            // Remove query string
            if (file.IndexOf('?') != -1)
            {
                file = file.Substring(0, file.IndexOf('?'));
            }

            var output = _rootPathProvider.GetRootPath().TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + file.TrimStart('~').TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            return output;
        }

        public string ResolveFileSystemPathToAppRelative(string file)
        {
            var root = new Uri(_rootPathProvider.GetRootPath());
            return root.MakeRelativeUri(new Uri(file, UriKind.RelativeOrAbsolute)).ToString();
        }

        public string BuildAbsolutePath(string siteRelativePath)
        {
            throw new NotImplementedException();
        }
    }
}