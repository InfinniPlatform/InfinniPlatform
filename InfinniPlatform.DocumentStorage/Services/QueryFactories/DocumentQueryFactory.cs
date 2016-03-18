using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.DocumentStorage.Properties;
using InfinniPlatform.DocumentStorage.Services.QuerySyntax;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Documents.Services;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.DocumentStorage.Services.QueryFactories
{
    public sealed class DocumentQueryFactory : DocumentQueryFactoryBase, IDocumentQueryFactory
    {
        public DocumentQueryFactory(IQuerySyntaxTreeParser syntaxTreeParser, IJsonObjectSerializer objectSerializer) : base(syntaxTreeParser, objectSerializer)
        {
        }


        public DocumentGetQuery CreateGetQuery(IHttpRequest request)
        {
            var query = ReadRequestQuery(request);

            var result = new DocumentGetQuery();

            if (query != null)
            {
                result.Search = ParseSearch(query);
                result.Filter = BuildFilter(query);
                result.Select = BuildSelect(query);
                result.Order = BuildOrder(query);
                result.Count = ParseCount(query);
                result.Skip = ParseSkip(query);
                result.Take = ParseTake(query);
            }

            return result;
        }

        public DocumentPostQuery CreatePostQuery(IHttpRequest request, string documentFormKey)
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


        private Func<IDocumentFilterBuilder, object> BuildFilter(object query)
        {
            var filterMethod = ParseFilter(query);

            if (filterMethod != null)
            {
                return FuncFilterQuerySyntaxVisitor.CreateFilterExpression(filterMethod);
            }

            return null;
        }

        private Action<IDocumentProjectionBuilder> BuildSelect(object query)
        {
            var selectMethods = ParseSelect(query);

            var selectMethod = selectMethods?.FirstOrDefault();

            if (selectMethod != null)
            {
                return FuncSelectQuerySyntaxVisitor.CreateSelectExpression(selectMethod);
            }

            return null;
        }

        private IDictionary<string, DocumentSortOrder> BuildOrder(object query)
        {
            var orderMethods = ParseOrder(query);

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