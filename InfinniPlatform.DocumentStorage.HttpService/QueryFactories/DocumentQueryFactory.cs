using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.DocumentStorage.Abstractions;
using InfinniPlatform.DocumentStorage.HttpService.Properties;
using InfinniPlatform.DocumentStorage.HttpService.QuerySyntax;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.DocumentStorage.HttpService.QueryFactories
{
    public class DocumentQueryFactory : DocumentQueryFactoryBase, IDocumentQueryFactory
    {
        public DocumentQueryFactory(IQuerySyntaxTreeParser syntaxTreeParser, IJsonObjectSerializer objectSerializer) : base(syntaxTreeParser, objectSerializer)
        {
        }


        public DocumentGetQuery CreateGetQuery(IHttpRequest request, string documentIdKey = DocumentHttpServiceConstants.DocumentIdKey)
        {
            return new DocumentGetQuery
            {
                Search = ParseSearch(request),
                Filter = BuildFilter(request, documentIdKey),
                Select = BuildSelect(request),
                Order = BuildOrder(request),
                Count = ParseCount(request),
                Skip = ParseSkip(request),
                Take = ParseTake(request)
            };
        }

        public DocumentPostQuery CreatePostQuery(IHttpRequest request, string documentFormKey = DocumentHttpServiceConstants.DocumentFormKey)
        {
            var document = ReadRequestForm<DynamicWrapper>(request, documentFormKey);

            if (document != null)
            {
                return new DocumentPostQuery
                {
                    Document = document,
                    Files = request.Files
                };
            }

            throw new InvalidOperationException(Resources.MethodNotAllowed);
        }

        public DocumentDeleteQuery CreateDeleteQuery(IHttpRequest request, string documentIdKey = DocumentHttpServiceConstants.DocumentIdKey)
        {
            var filter = BuildFilter(request, documentIdKey);

            if (filter != null)
            {
                return new DocumentDeleteQuery
                {
                    Filter = filter
                };
            }

            throw new InvalidOperationException(Resources.MethodNotAllowed);
        }


        private Func<IDocumentFilterBuilder, object> BuildFilter(IHttpRequest request, string documentIdKey)
        {
            var filterMethod = ParseFilter(request, documentIdKey);

            if (filterMethod != null)
            {
                return FuncFilterQuerySyntaxVisitor.CreateFilterExpression(filterMethod);
            }

            return null;
        }

        private Action<IDocumentProjectionBuilder> BuildSelect(IHttpRequest request)
        {
            var selectMethods = ParseSelect(request);

            var selectMethod = selectMethods?.FirstOrDefault();

            if (selectMethod != null)
            {
                return FuncSelectQuerySyntaxVisitor.CreateSelectExpression(selectMethod);
            }

            return null;
        }

        private IDictionary<string, DocumentSortOrder> BuildOrder(IHttpRequest request)
        {
            var orderMethods = ParseOrder(request);

            if (orderMethods != null)
            {
                var order = new Dictionary<string, DocumentSortOrder>();

                foreach (var orderMethod in orderMethods)
                {
                    var orderProperties = FuncOrderQuerySyntaxVisitor.CreateOrderExpression(orderMethod);

                    if (orderProperties != null)
                    {
                        foreach (var orderProperty in orderProperties)
                        {
                            order.Add(orderProperty.Key, orderProperty.Value);
                        }
                    }
                }

                return order;
            }

            return null;
        }
    }
}