using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.ContentManager.Bundling;

namespace InfinniPlatform.Server.Hosting
{
    public class ServerContentSource : IContentSource
    {
        public IEnumerable<string> Css()
        {
            const string root = "~/content/www/compiled/platform/css";

            return new[]
                   {
                       $"{root}/main.css",
                       $"{root}/vendor.css"
                   };
        }

        public IEnumerable<string> Less()
        {
            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> Js()
        {
            const string root = "~/content/www";

            return new[]
                   {
                       $"{root}/config.js",
                       $"{root}/compiled/platform/vendor.js",
                       $"{root}/compiled/platform/templates.js",
                       $"{root}/compiled/platform/platform.js",
                       $"{root}/compiled/js/templates.js",
                       $"{root}/compiled/js/app.js",
                       $"{root}/js/main.js",
                       $"{root}/js/homeView.js",
                       $"{root}/js/installView.js",
                       $"{root}/js/configView.js",
                       $"{root}/js/environmentView.js",
                       $"{root}/js/taskView.js",
                       $"{root}/js/agentView.js"
                   };
        }

        public IEnumerable<string> Assets()
        {
            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> Json()
        {
            return Enumerable.Empty<string>();
        }
    }
}