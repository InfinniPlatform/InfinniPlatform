using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.Services.QuerySyntax;
using InfinniPlatform.Sdk.Documents.Services;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.DocumentStorage.Services.QueryFactory
{
    /// <summary>
    /// Предоставляет интерфейс для создания запросов к сервису документов.
    /// </summary>
    /// <typeparam name="TDocument">Тип документа.</typeparam>
    public sealed class DocumentQueryFactory<TDocument> : DocumentQueryFactoryBase, IDocumentQueryFactory<TDocument>
    {
        public DocumentQueryFactory(IQuerySyntaxTreeParser syntaxTreeParser, IJsonObjectSerializer objectSerializer) : base(syntaxTreeParser, objectSerializer)
        {
        }


        public DocumentGetQuery<TDocument> CreateGetQuery(IHttpRequest request)
        {
            var query = ReadRequestQuery(request);

            if (query != null)
            {
                var result = new DocumentGetQuery<TDocument>
                {
                    Search = ParseSearch(query),
                    Filter = BuildFilter(query),
                    Select = BuildSelect(query),
                    Order = BuildOrder(query),
                    Count = ParseCount(query),
                    Skip = ParseSkip(query),
                    Take = ParseTake(query)
                };

                return result;
            }

            return null;
        }

        public DocumentPostQuery<TDocument> CreatePostQuery(IHttpRequest request, string documentFormKey)
        {
            var document = ReadRequestForm<TDocument>(request, documentFormKey);

            if (document != null)
            {
                return new DocumentPostQuery<TDocument>
                {
                    Document = document,
                    Files = request.Files
                };
            }

            return null;
        }

        public DocumentDeleteQuery CreateDeleteQuery(IHttpRequest request, string documentIdKey)
        {
            var documentId = ReadRequestParameter(request, documentIdKey);

            return new DocumentDeleteQuery
            {
                DocumentId = documentId
            };
        }


        private Expression<Func<TDocument, bool>> BuildFilter(object query)
        {
            var filterMethod = ParseFilter(query);

            if (filterMethod != null)
            {
                return (Expression<Func<TDocument, bool>>)ExpressionFilterQuerySyntaxVisitor.CreateFilterExpression(typeof(TDocument), filterMethod);
            }

            return null;
        }

        private Expression<Func<TDocument, object>> BuildSelect(object query)
        {
            var selectMethods = ParseSelect(query);

            var selectMethod = selectMethods?.FirstOrDefault();

            if (selectMethod != null)
            {
                return (Expression<Func<TDocument, object>>)ExpressionSelectQuerySyntaxVisitor.CreateSelectExpression(typeof(TDocument), selectMethod);
            }

            return null;
        }

        private IDictionary<Expression<Func<TDocument, object>>, DocumentSortOrder> BuildOrder(object query)
        {
            var orderMethods = ParseOrder(query);

            if (orderMethods != null)
            {
                var order = new Dictionary<Expression<Func<TDocument, object>>, DocumentSortOrder>();

                foreach (var orderMethod in orderMethods)
                {
                    var orderProperties = ExpressionOrderQuerySyntaxVisitor.CreateOrderExpression(typeof(TDocument), orderMethod);

                    if (orderProperties != null)
                    {
                        foreach (var orderProperty in orderProperties)
                        {
                            order.Add((Expression<Func<TDocument, object>>)orderProperty.Key, orderProperty.Value);
                        }
                    }
                }

                return order;
            }

            return null;
        }
    }
}