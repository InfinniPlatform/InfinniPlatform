using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Hosting;

using SquishIt.Framework;

namespace InfinniPlatform.ContentManager.Bundling
{
    public class AssetsInitializer : AppEventHandler
    {
        public AssetsInitializer(IEnumerable<IContentSource> sources) : base(1)
        {
            _sources = sources.ToArray();
        }

        private readonly IContentSource[] _sources;

        public override void OnAfterStart()
        {
            Bundle.ConfigureDefaults()
                  .UsePathTranslator(new NancyPathTranslator(new RootSourceProvider()));

            BundleCss();
            BundleJavaScript();
        }


        private void BundleCss()
        {
            var bundle = Bundle.Css();

            foreach (var contentSource in _sources)
            {
                foreach (var cssFile in contentSource.Css())
                {
                    bundle.Add(cssFile);
                }
            }

            bundle.AsNamed("style", "~/content/www/compiled/bundle.css");
        }

        private void BundleJavaScript()
        {
            var bundle = Bundle.JavaScript();

            foreach (var contentSource in _sources)
            {
                foreach (var jsFile in contentSource.Js())
                {
                    bundle.Add(jsFile);
                }
            }

            bundle.AsNamed("script", "~/content/www/compiled/bundle.js");
        }
    }
}