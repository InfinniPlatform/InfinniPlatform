using System.Collections.Generic;
using System.IO;
using System.Linq;

using dotless.Core;
using dotless.Core.configuration;

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
            BundleLess();
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

        private void BundleLess()
        {
            //var files = Directory.GetFiles(@"C:\Projects\InfinniPlatform\InfinniPlatform.Server\UI\bower_components\infinni-ui-v2\app\styles\main.less");

            var dotlessConfiguration = new DotlessConfiguration
                                       {
                                           MinifyOutput = true,
                                       };

            var lessEngine = new EngineFactory(dotlessConfiguration).GetEngine();
            lessEngine.CurrentDirectory = @"C:\Projects\InfinniPlatform\InfinniPlatform.Server\UI\bower_components\infinni-ui-v2\app\styles";

            //            foreach (var f in files)
            //            {
            var lessText = File.ReadAllText(@"C:\Projects\InfinniPlatform\InfinniPlatform.Server\UI\bower_components\infinni-ui-v2\app\styles\main.less");

            //var s = Less.Parse(lessText);

            var transformToCss = lessEngine.TransformToCss(lessText, "try.less");

            //            }

            //Bundle.Css().AddString();
        }
    }
}