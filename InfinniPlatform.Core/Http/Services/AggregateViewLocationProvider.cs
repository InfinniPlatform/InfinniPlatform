using System.Collections.Generic;
using System.Linq;

using Nancy;
using Nancy.ViewEngines;

namespace InfinniPlatform.Core.Http.Services
{
    public class AggregateViewLocationProvider : IViewLocationProvider
    {
        //TODO: Should it be customizable?
        private static readonly string[] RazorViewFileExtensions = { "cshtml" };

        public AggregateViewLocationProvider()
        {
            _fileSystemViewLocationProvider = new FileSystemViewLocationProvider(new DefaultRootPathProvider());
            _resourceViewLocationProvider = new ResourceViewLocationProvider();
        }

        private readonly FileSystemViewLocationProvider _fileSystemViewLocationProvider;
        private readonly ResourceViewLocationProvider _resourceViewLocationProvider;

        public IEnumerable<ViewLocationResult> GetLocatedViews(IEnumerable<string> supportedViewExtensions)
        {
            var fs = _fileSystemViewLocationProvider.GetLocatedViews(RazorViewFileExtensions);
            var res = _resourceViewLocationProvider.GetLocatedViews(RazorViewFileExtensions);

            return fs.Concat(res);
        }

        public IEnumerable<ViewLocationResult> GetLocatedViews(IEnumerable<string> supportedViewExtensions, string location, string viewName)
        {
            var fs = _fileSystemViewLocationProvider.GetLocatedViews(RazorViewFileExtensions, location, viewName);
            var res = _resourceViewLocationProvider.GetLocatedViews(RazorViewFileExtensions, location, viewName);

            return fs.Concat(res);
        }
    }
}