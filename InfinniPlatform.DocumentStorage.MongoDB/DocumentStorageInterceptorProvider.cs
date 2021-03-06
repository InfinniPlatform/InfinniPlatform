﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using InfinniPlatform.DocumentStorage.Interceptors;

namespace InfinniPlatform.DocumentStorage
{
    internal class DocumentStorageInterceptorProvider : IDocumentStorageInterceptorProvider
    {
        public DocumentStorageInterceptorProvider(IEnumerable<IDocumentStorageInterceptor> writeHandlers)
        {
            _writeHandlers = new Dictionary<string, IDocumentStorageInterceptor>();
            _genericWriteHandlers = new Dictionary<string, IDocumentStorageInterceptor>();

            if (writeHandlers != null)
            {
                foreach (var writeHandler in writeHandlers)
                {
                    _writeHandlers[writeHandler.DocumentType] = writeHandler;

                    var clrDocumentTypes
                        = writeHandler.GetType()
                                      .GetTypeInfo()
                                      .GetInterfaces()
                                      .Where(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IDocumentStorageInterceptor<>))
                                      .Select(i => i.GetTypeInfo().GetGenericArguments()[0]);

                    foreach (var clrDocumentType in clrDocumentTypes)
                    {
                        var genericWriteHandlerKey = GetGenericWriteHandlerKey(clrDocumentType, writeHandler.DocumentType);

                        _genericWriteHandlers[genericWriteHandlerKey] = writeHandler;
                    }
                }
            }
        }


        private readonly Dictionary<string, IDocumentStorageInterceptor> _writeHandlers;
        private readonly Dictionary<string, IDocumentStorageInterceptor> _genericWriteHandlers;


        public IDocumentStorageInterceptor GetInterceptor(string documentType)
        {
            IDocumentStorageInterceptor interceptor;

            if (_writeHandlers.TryGetValue(documentType, out interceptor))
            {
                return interceptor;
            }

            return null;
        }

        public IDocumentStorageInterceptor<TDocument> GetInterceptor<TDocument>(string documentType) where TDocument : Document
        {
            IDocumentStorageInterceptor interceptor;

            var genericWriteHandlerKey = GetGenericWriteHandlerKey(typeof(TDocument), documentType);

            if (_genericWriteHandlers.TryGetValue(genericWriteHandlerKey, out interceptor))
            {
                return (IDocumentStorageInterceptor<TDocument>)interceptor;
            }

            return null;
        }


        private static string GetGenericWriteHandlerKey(Type clrDocumentType, string documentType)
        {
            return $"{clrDocumentType.FullName};{documentType}";
        }
    }
}