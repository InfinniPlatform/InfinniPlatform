using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.DocumentStorage.Properties;
using InfinniPlatform.DocumentStorage.QuerySyntax;
using InfinniPlatform.Dynamic;
using InfinniPlatform.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace InfinniPlatform.DocumentStorage.QueryFactories
{
    public class DocumentQueryFactory : DocumentQueryFactoryBase, IDocumentQueryFactory
    {
        public DocumentQueryFactory(IQuerySyntaxTreeParser syntaxTreeParser, IJsonObjectSerializer objectSerializer) : base(syntaxTreeParser, objectSerializer)
        {
        }


        public DocumentGetQuery CreateGetQuery(HttpRequest request, RouteData routeData, string documentIdKey = DocumentHttpServiceConstants.DocumentIdKey)
        {
            return new DocumentGetQuery
            {
                Search = ParseSearch(request),
                Filter = BuildFilter(request, routeData, documentIdKey),
                Select = BuildSelect(request),
                Order = BuildOrder(request),
                Count = ParseCount(request),
                Skip = ParseSkip(request),
                Take = ParseTake(request)
            };
        }

        public DocumentPostQuery CreatePostQuery(HttpRequest request, RouteData routeData, string documentFormKey = DocumentHttpServiceConstants.DocumentFormKey)
        {
            var document = ReadRequestForm<DynamicDocument>(request, documentFormKey);

            if (document != null)
            {
                return new DocumentPostQuery
                {
                    Document = document,
                    AspFiles = request.Form.Files
                };
            }

            throw new InvalidOperationException(Resources.MethodNotAllowed);
        }

        public DocumentDeleteQuery CreateDeleteQuery(HttpRequest request, RouteData routeData, string documentIdKey = DocumentHttpServiceConstants.DocumentIdKey)
        {
            var filter = BuildFilter(request, routeData, documentIdKey);

            if (filter != null)
            {
                return new DocumentDeleteQuery
                {
                    Filter = filter
                };
            }

            throw new InvalidOperationException(Resources.MethodNotAllowed);
        }

        private Func<IDocumentFilterBuilder, object> BuildFilter(HttpRequest request, RouteData routeData, string documentIdKey)
        {
            var filterMethod = ParseFilter(request, routeData, documentIdKey);

            if (filterMethod != null)
            {
                return FuncFilterQuerySyntaxVisitor.CreateFilterExpression(filterMethod);
            }

            return null;
        }

        private Action<IDocumentProjectionBuilder> BuildSelect(HttpRequest request)
        {
            var selectMethods = ParseSelect(request);

            var selectMethod = selectMethods?.FirstOrDefault();

            if (selectMethod != null)
            {
                return FuncSelectQuerySyntaxVisitor.CreateSelectExpression(selectMethod);
            }

            return null;
        }

        private IDictionary<string, DocumentSortOrder> BuildOrder(HttpRequest request)
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