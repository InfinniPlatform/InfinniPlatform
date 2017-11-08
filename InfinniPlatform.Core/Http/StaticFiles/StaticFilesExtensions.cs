using System.IO;

using InfinniPlatform.Extensions;
using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Http.StaticFiles
{
    public static class StaticFilesExtensions
    {
        /// <summary>
        /// Configure serving static files using mapping in configuration file.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="configuration">Configuration.</param>
        /// <param name="resolver"></param>
        public static void UseStaticFilesMapping(this IApplicationBuilder app, IConfiguration configuration, IContainerResolver resolver)
        {
            var appOptions = configuration.GetSection(AppOptions.SectionName).Get<AppOptions>();
            var logger = resolver.ResolveOptional<ILogger<IApplicationBuilder>>();

            foreach (var mapping in appOptions.StaticFilesMapping)
            {
                var requestedPath = mapping.Key;

                if (requestedPath.Equals("/"))
                {
                    requestedPath = string.Empty;
                }

                var contentPath = Path.Combine(Directory.GetCurrentDirectory(), mapping.Value.ToWebPath());

                if (Directory.Exists(contentPath))
                {
                    app.UseStaticFiles(new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(contentPath),
                        RequestPath = requestedPath
                    });
                }
                else
                {
                    logger?.LogInformation($"Directory {contentPath} does not exist.");
                }
            }
        }
    }
}