using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.Services.QuerySyntax;
using InfinniPlatform.Sdk.Documents.Services;

namespace InfinniPlatform.DocumentStorage.Services.QueryBuilders
{
    /// <summary>
    /// Предоставляет интерфейс для построения запросов к сервису документов.
    /// </summary>
    /// <typeparam name="TDocument">Тип документа.</typeparam>
    public sealed class DocumentQueryBuilder<TDocument> : DocumentQueryBuilderBase, IDocumentQueryBuilder<TDocument>
    {
        public DocumentQueryBuilder(IQuerySyntaxTreeParser syntaxTreeParser) : base(syntaxTreeParser)
        {
        }


        public DocumentGetQuery<TDocument> BuildGetQuery(IDictionary<string, object> query)
        {
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


        private Expression<Func<TDocument, bool>> BuildFilter(IDictionary<string, object> query)
        {
            var filterMethod = ParseFilter(query);

            if (filterMethod != null)
            {
                return (Expression<Func<TDocument, bool>>)ExpressionFilterQuerySyntaxVisitor.CreateFilterExpression(typeof(TDocument), filterMethod);
            }

            return null;
        }

        private Expression<Func<TDocument, object>> BuildSelect(IDictionary<string, object> query)
        {
            var selectMethods = ParseSelect(query);

            var selectMethod = selectMethods?.FirstOrDefault();

            if (selectMethod != null)
            {
                return (Expression<Func<TDocument, object>>)ExpressionSelectQuerySyntaxVisitor.CreateSelectExpression(typeof(TDocument), selectMethod);
            }

            return null;
        }

        private IDictionary<Expression<Func<TDocument, object>>, DocumentSortOrder> BuildOrder(IDictionary<string, object> query)
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