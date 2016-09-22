using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Nancy;
using Nancy.Helpers;
using Nancy.Responses;

namespace InfinniPlatform.Owin.Services
{
    public static class EmbeddedStaticContentConventionBuilder
    {
        private const string PathSeparator = "/";
        private static readonly ConcurrentDictionary<string, Func<Response>> ResponseFactoryCache;

        static EmbeddedStaticContentConventionBuilder()
        {
            ResponseFactoryCache = new ConcurrentDictionary<string, Func<Response>>();
        }

        /// <summary>
        /// Добавляе соглашение по путям к файлам встроенных ресурсов.
        /// </summary>
        /// <param name="conventions">Соглашения по путям к статическому контенту.</param>
        /// <param name="requestedPath">The path that should be matched with the request.</param>
        /// <param name="assembly">Сборка, содержащая ресурсы.</param>
        /// <param name="contentPath">Локальный путь до файлов.</param>
        /// <param name="allowedExtensions">Список разрешенных разрешений.</param>
        public static void AddDirectory(this IList<Func<NancyContext, string, Response>> conventions, string requestedPath, Assembly assembly, string contentPath = null, params string[] allowedExtensions)
        {
            conventions.Add(AddDirectory(requestedPath, assembly, contentPath, allowedExtensions));
        }

        private static Func<NancyContext, string, Response> AddDirectory(string requestedPath, Assembly assembly, string contentPath, params string[] allowedExtensions)
        {
            if (!requestedPath.StartsWith(PathSeparator))
            {
                requestedPath = string.Concat(PathSeparator, requestedPath);
            }

            return (ctx, root) =>
                   {
                       var path = HttpUtility.UrlDecode(ctx.Request.Path);

                       var fileName = Path.GetFileName(path);

                       if (string.IsNullOrEmpty(fileName))
                       {
                           return null;
                       }

                       var pathWithoutFilename = GetPathWithoutFilename(fileName, path);

                       if (!pathWithoutFilename.StartsWith(requestedPath, StringComparison.OrdinalIgnoreCase))
                       {
                           return null;
                       }

                       contentPath = GetContentPath(requestedPath, contentPath);

                       var responseFactory = ResponseFactoryCache.GetOrAdd(path, BuildContentDelegate(requestedPath, assembly, allowedExtensions));

                       return responseFactory.Invoke();
                   };
        }

        private static Func<string, Func<Response>> BuildContentDelegate(string requestedPath, Assembly assembly, string[] allowedExtensions)
        {
            return requestPath =>
                   {
                       var extension = Path.GetExtension(requestPath);

                       if ((allowedExtensions.Length != 0) && !allowedExtensions.Any(e => string.Equals(e, extension, StringComparison.OrdinalIgnoreCase)))
                       {
                           return () => null;
                       }

                       var assemblyName = assembly.GetName().Name;

                       var internalPath = requestPath.Replace(requestedPath, string.Empty).Replace('/', '.').Substring(1);

                       if (!assembly.GetManifestResourceNames().Any(x => string.Equals(x, assemblyName + "." + internalPath, StringComparison.OrdinalIgnoreCase)))
                       {
                           return () => null;
                       }

                       return () => new EmbeddedFileResponse(assembly, assemblyName, internalPath);
                   };
        }

        private static string GetContentPath(string requestedPath, string contentPath)
        {
            contentPath = contentPath ?? requestedPath;

            if (!contentPath.StartsWith(PathSeparator))
            {
                contentPath = string.Concat(PathSeparator, contentPath);
            }

            return contentPath;
        }

        private static string GetPathWithoutFilename(string fileName, string path)
        {
            var pathWithoutFileName = path.Replace(fileName, string.Empty);

            return pathWithoutFileName.Equals(PathSeparator)
                       ? pathWithoutFileName
                       : pathWithoutFileName.TrimEnd('/');
        }
    }
}