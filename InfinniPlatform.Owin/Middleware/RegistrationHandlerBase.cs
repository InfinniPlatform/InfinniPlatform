using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    ///Базовый обработчик роутинга
    /// </summary>
    public sealed class RegistrationHandlerBase : IHandlerRegistration
    {
        private readonly string _method;
        private readonly PathString _pathString;
        private readonly Func<IOwinContext, IRequestHandlerResult> _handler;

        public RegistrationHandlerBase(string method, PathString pathString, Func<IOwinContext, IRequestHandlerResult> handler)
        {
            _method = method;
            _pathString = pathString;
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
            return NormalizePath(_pathString).ToLowerInvariant() == requestPath.ToLowerInvariant();
        }

        public IRequestHandlerResult Execute(IOwinContext context)
        {
            return _handler.Invoke(context);
        }

        private static string NormalizePath(PathString path)
        {
            if (path.HasValue)
            {
                return path.Value.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries).First().TrimEnd('/').ToLower();
            }
            return string.Empty;
        }
    }
}
