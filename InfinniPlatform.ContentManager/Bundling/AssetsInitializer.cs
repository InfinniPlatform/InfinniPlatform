using InfinniPlatform.Sdk.Hosting;

using SquishIt.Framework;

namespace InfinniPlatform.ContentManager.Bundling
{
    public class AssetsInitializer : AppEventHandler
    {
        public AssetsInitializer() : base(1)
        {
            Bundle.ConfigureDefaults()
                  .UsePathTranslator(new NancyPathTranslator(new RootSourceProvider()));

            //TODO Move to config
            Bundle.Css()
                  .Add("~/content/www/compiled/platform/css/main.css")
                  .Add("~/content/www/compiled/platform/css/vendor.css")
                  .AsNamed("style", "~/content/www/compiled/bundle.css");

            //TODO Move to config
            Bundle.JavaScript()
                  .Add("~/content/www/config.js")
                  .Add("~/content/www/compiled/platform/vendor.js")
                  .Add("~/content/www/compiled/platform/templates.js")
                  .Add("~/content/www/compiled/platform/platform.js")
                  .Add("~/content/www/compiled/js/templates.js")
                  .Add("~/content/www/compiled/js/app.js")
                  .Add("~/content/www/js/main.js")
                  .Add("~/content/www/js/homeView.js")
                  .Add("~/content/www/js/installView.js")
                  .Add("~/content/www/js/configView.js")
                  .Add("~/content/www/js/environmentView.js")
                  .Add("~/content/www/js/taskView.js")
                  .Add("~/content/www/js/agentView.js")
                  .AsNamed("script", "~/content/www/compiled/bundle.js");
        }
    }
}