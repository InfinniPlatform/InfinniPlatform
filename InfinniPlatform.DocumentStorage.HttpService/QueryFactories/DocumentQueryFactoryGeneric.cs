using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.Core.Abstractions.Http;
using InfinniPlatform.Core.Abstractions.Serialization;
using InfinniPlatform.DocumentStorage.HttpService.Properties;
using InfinniPlatform.DocumentStorage.HttpService.QuerySyntax;

namespace InfinniPlatform.DocumentStorage.HttpService.QueryFactories
{
    /// <summary>
    /// Предоставляет интерфейс для создания запросов к сервису документов.
    /// </summary>
    /// <typeparam name="TDocument">Тип документа.</typeparam>
    public class DocumentQueryFactory<TDocument> : DocumentQueryFactoryBase, IDocumentQueryFactory<TDocument>
    {
        public DocumentQueryFactory(IQuerySyntaxTreeParser syntaxTreeParser, IJsonObjectSerializer objectSerializer) : base(syntaxTreeParser, objectSerializer)
        {
        }


        public DocumentGetQuery<TDocument> CreateGetQuery(IHttpRequest request, string documentIdKey = DocumentHttpServiceConstants.DocumentIdKey)
        {
            return new DocumentGetQuery<TDocument>
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

        public DocumentPostQuery<TDocument> CreatePostQuery(IHttpRequest request, string documentFormKey = DocumentHttpServiceConstants.DocumentFormKey)
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

            throw new InvalidOperationException(Resources.MethodNotAllowed);
        }

        public DocumentDeleteQuery<TDocument> CreateDeleteQuery(IHttpRequest request, string documentIdKey = DocumentHttpServiceConstants.DocumentIdKey)
        {
            var filter = BuildFilter(request, documentIdKey);

            if (filter != null)
            {
                return new DocumentDeleteQuery<TDocument>
                {
                    Filter = filter
                };
            }

            throw new InvalidOperationException(Resources.MethodNotAllowed);
        }


        private Expression<Func<TDocument, bool>> BuildFilter(IHttpRequest request, string documentIdKey)
        {
            var filterMethod = ParseFilter(request, documentIdKey);

            if (filterMethod != null)
            {
                return (Expression<Func<TDocument, bool>>)ExpressionFilterQuerySyntaxVisitor.CreateFilterExpression(typeof(TDocument), filterMethod);
            }

            return null;
        }

        private Expression<Func<TDocument, object>> BuildSelect(IHttpRequest request)
        {
            var selectMethods = ParseSelect(request);

            var selectMethod = selectMethods?.FirstOrDefault();

            if (selectMethod != null)
            {
                return (Expression<Func<TDocument, object>>)ExpressionSelectQuerySyntaxVisitor.CreateSelectExpression(typeof(TDocument), selectMethod);
            }

            return null;
        }

        private IDictionary<Expression<Func<TDocument, object>>, DocumentSortOrder> BuildOrder(IHttpRequest request)
        {
            var orderMethods = ParseOrder(request);

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