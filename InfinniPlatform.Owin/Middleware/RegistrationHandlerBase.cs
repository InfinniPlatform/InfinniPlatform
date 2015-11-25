using System;
using System.Linq;
using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    ///     Базовый обработчик роутинга
    /// </summary>
    public sealed class RegistrationHandlerBase : IHandlerRegistration
    {
        private readonly Func<IOwinContext, IRequestHandlerResult> _handler;
        private readonly string _method;
        private readonly string _pathString;

        public RegistrationHandlerBase(string method, PathString pathString, Func<IOwinContext, IRequestHandlerResult> handler)
        {
            _method = method;
            _pathString = NormalizePath(pathString);
            _handler = handler;
        }

        public Priority Priority
        {
            get { return Priority.Standard; }
        }

        public string Method
        {
            get { return _method; }
        }

        public bool CanProcessRequest(IOwinContext context, string requestPath)
        {
            return string.Equals(_pathString, requestPath, StringComparison.OrdinalIgnoreCase);
        }

        public IRequestHandlerResult Execute(IOwinContext context)
        {
            return _handler.Invoke(context);
        }

        private static string NormalizePath(PathString path)
        {
            if (path.HasValue)
            {
                return path.Value.Split(new[] {'?'}, StringSplitOptions.RemoveEmptyEntries).First().TrimEnd('/');
            }

            return string.Empty;
        }
    }
}