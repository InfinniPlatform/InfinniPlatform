using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using InfinniPlatform.Core.Http;
using InfinniPlatform.Core.Http.Services;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.ViewEngine.Settings;

using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Nancy.ViewEngines;

namespace InfinniPlatform.ViewEngine
{
    public class NancyBootstrapper : HttpServiceNancyBootstrapper
    {
        private const string RazorViewFileExtension = ".cshtml";

        public NancyBootstrapper(INancyModuleCatalog nancyModuleCatalog,
                                 StaticContentSettings staticContentSettings,
                                 ILog log,
                                 ViewEngineSettings viewEngineSettings) : base(nancyModuleCatalog, staticContentSettings, log)
        {
            _viewEngineSettings = viewEngineSettings;
        }

        private readonly ViewEngineSettings _viewEngineSettings;

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            RegisterRazorViews();
            RegisterEmbeddedRazorViews();
        }

        private void RegisterRazorViews()
        {
            Conventions.ViewLocationConventions.Add((viewName, model, context) => $"{_viewEngineSettings.RazorViewsPath.ToWebPath()}/{viewName}");
        }
        
        private void RegisterEmbeddedRazorViews()
        {
            foreach (var mapping in _viewEngineSettings.EmbeddedRazorViewsAssemblies)
            {
                var razorViews = Assembly.Load(mapping)
                                         .GetManifestResourceNames()
                                         .Where(s => s.EndsWith(RazorViewFileExtension));

                var rootNamespaces = new List<string>();

                foreach (var razorView in razorViews)
                {
                    var pathWithoutExtention = razorView.Replace(RazorViewFileExtension, string.Empty);

                    // Assuming views names does not contains '.'
                    var fileName = pathWithoutExtention.Split('.').Last();

                    rootNamespaces.Add(pathWithoutExtention.Replace($".{fileName}", string.Empty));
                }

                foreach (var path in rootNamespaces)
                {
                    ResourceViewLocationProvider.RootNamespaces.Add(Assembly.Load(mapping), path);
                }
            }
        }
    }
}