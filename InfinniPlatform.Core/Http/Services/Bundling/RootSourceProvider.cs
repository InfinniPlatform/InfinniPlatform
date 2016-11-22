using System.IO;
using System.Reflection;

using Nancy;

namespace InfinniPlatform.Core.Http.Services.Bundling
{
    public class RootSourceProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
            var assembly = Assembly.GetEntryAssembly();

            return assembly != null ?
                Path.GetDirectoryName(assembly.Location) :
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}