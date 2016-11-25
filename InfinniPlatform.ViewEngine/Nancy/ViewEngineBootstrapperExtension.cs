using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using InfinniPlatform.Sdk.ViewEngine;
using InfinniPlatform.ViewEngine.Settings;

using Nancy.Conventions;
using Nancy.ViewEngines;

namespace InfinniPlatform.ViewEngine.Nancy
{
    public class ViewEngineBootstrapperExtension : IViewEngineBootstrapperExtension
    {
        private const string RazorViewFileExtension = ".cshtml";

        public ViewEngineBootstrapperExtension(ViewEngineSettings viewEngineSettings)
        {
            _viewEngineSettings = viewEngineSettings;
        }

        private readonly ViewEngineSettings _viewEngineSettings;

        public Type ViewLocatorType => typeof(AggregateViewLocationProvider);

        public void ViewLocatorsRegistration(dynamic conventions)
        {
            RegisterRazorViews((NancyConventions)conventions);
            RegisterEmbeddedRazorViews();
        }

        private void RegisterRazorViews(NancyConventions conventions)
        {
            conventions.ViewLocationConventions.Add((viewName, model, context) => $"{_viewEngineSettings.RazorViewsPath.ToWebPath()}/{viewName}");
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