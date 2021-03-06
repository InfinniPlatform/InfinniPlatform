﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.Properties;
using InfinniPlatform.DocumentStorage.QuerySyntax;
using InfinniPlatform.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace InfinniPlatform.DocumentStorage.QueryFactories
{
    /// <inheritdoc cref="IDocumentQueryFactory" />
    /// <typeparam name="TDocument">Document type.</typeparam>
    public class DocumentQueryFactory<TDocument> : DocumentQueryFactoryBase, IDocumentQueryFactory<TDocument>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DocumentQueryFactory{TDocument}" />.
        /// </summary>
        /// <param name="syntaxTreeParser">Syntax analyzer for query.</param>
        /// <param name="objectSerializer">JSON objects serializer.</param>
        public DocumentQueryFactory(IQuerySyntaxTreeParser syntaxTreeParser, 
                                    IJsonObjectSerializer objectSerializer) 
            : base(syntaxTreeParser, objectSerializer)
        {
        }


        /// <inheritdoc />
        public DocumentGetQuery<TDocument> CreateGetQuery(HttpRequest request, RouteData routeData, string documentIdKey = DocumentHttpServiceConstants.DocumentIdKey)
        {
            return new DocumentGetQuery<TDocument>
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

        /// <inheritdoc />
        public DocumentPostQuery<TDocument> CreatePostQuery(HttpRequest request, RouteData routeData, string documentFormKey = DocumentHttpServiceConstants.DocumentFormKey)
        {
            var document = ReadRequestForm<TDocument>(request, documentFormKey);

            if (document != null)
            {
                return new DocumentPostQuery<TDocument>
                {
                    Document = document,
                    Files = IsFromForm(request) ? request.Form.Files : null
                };
            }

            throw new InvalidOperationException(Resources.MethodNotAllowed);
        }

        /// <inheritdoc />
        public DocumentDeleteQuery<TDocument> CreateDeleteQuery(HttpRequest request, RouteData routeData, string documentIdKey = DocumentHttpServiceConstants.DocumentIdKey)
        {
            var filter = BuildFilter(request, routeData, documentIdKey);

            if (filter != null)
            {
                return new DocumentDeleteQuery<TDocument>
                {
                    Filter = filter
                };
            }

            throw new InvalidOperationException(Resources.MethodNotAllowed);
        }


        private Expression<Func<TDocument, bool>> BuildFilter(HttpRequest request, RouteData routeData, string documentIdKey)
        {
            var filterMethod = ParseFilter(request, routeData, documentIdKey);

            if (filterMethod != null)
            {
                return (Expression<Func<TDocument, bool>>)ExpressionFilterQuerySyntaxVisitor.CreateFilterExpression(typeof(TDocument), filterMethod);
            }

            return null;
        }

        private Expression<Func<TDocument, object>> BuildSelect(HttpRequest request)
        {
            var selectMethods = ParseSelect(request);

            var selectMethod = selectMethods?.FirstOrDefault();

            if (selectMethod != null)
            {
                return (Expression<Func<TDocument, object>>)ExpressionSelectQuerySyntaxVisitor.CreateSelectExpression(typeof(TDocument), selectMethod);
            }

            return null;
        }

        private IDictionary<Expression<Func<TDocument, object>>, DocumentSortOrder> BuildOrder(HttpRequest request)
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