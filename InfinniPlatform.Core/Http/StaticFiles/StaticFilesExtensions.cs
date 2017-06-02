using System.IO;

using InfinniPlatform.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace InfinniPlatform.Http.StaticFiles
{
    public static class StaticFilesExtensions
    {
        /// <summary>
        /// Configure serving static files using mapping in configuration file.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="configuration">Configuration.</param>
        public static void UseStaticFilesMapping(this IApplicationBuilder app, IConfigurationRoot configuration)
        {
            var appOptions = configuration.GetSection(AppOptions.SectionName).Get<AppOptions>();

            foreach (var mapping in appOptions.StaticFilesMapping)
            {
                var requestedPath = mapping.Key;

                if (requestedPath.Equals("/"))
                {
                    requestedPath = string.Empty;
                }

                var contentPath = Path.Combine(Directory.GetCurrentDirectory(), mapping.Value.ToWebPath());

                app.UseStaticFiles(new StaticFileOptions
                                   {
                                       FileProvider = new PhysicalFileProvider(contentPath),
                                       RequestPath = requestedPath
                                   });
            }
        }
    }
}