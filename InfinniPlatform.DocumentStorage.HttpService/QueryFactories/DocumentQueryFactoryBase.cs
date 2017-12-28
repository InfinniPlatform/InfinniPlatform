using System;
using System.Collections.Generic;

using InfinniPlatform.DocumentStorage.QuerySyntax;
using InfinniPlatform.Http;
using InfinniPlatform.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace InfinniPlatform.DocumentStorage.QueryFactories
{
    /// <summary>
    /// Base class for <see cref="IDocumentQueryFactory"/>.
    /// </summary>
    public abstract class DocumentQueryFactoryBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DocumentQueryFactoryBase" />.
        /// </summary>
        /// <param name="syntaxTreeParser">Syntax analyzer for query.</param>
        /// <param name="objectSerializer">JSON objects serializer.</param>
        protected DocumentQueryFactoryBase(IQuerySyntaxTreeParser syntaxTreeParser, IJsonObjectSerializer objectSerializer)
        {
            _syntaxTreeParser = syntaxTreeParser;
            _objectSerializer = objectSerializer;
        }


        private readonly IQuerySyntaxTreeParser _syntaxTreeParser;
        private readonly IJsonObjectSerializer _objectSerializer;


        /// <summary>
        /// Returns request form content.
        /// </summary>
        protected TDocument ReadRequestForm<TDocument>(HttpRequest request, string documentFormKey)
        {
            if (IsFromForm(request))
            {
                if (request.Form != null)
                {
                    var documentString = (string)request.Form[documentFormKey];

                    if (!string.IsNullOrWhiteSpace(documentString))
                    {
                        return _objectSerializer.Deserialize<TDocument>(documentString);
                    }
                }
            }
            else if (request.Body != null)
            {
                return _objectSerializer.Deserialize<TDocument>(request.Body);
            }

            return default(TDocument);
        }

        /// <summary>
        /// Flag indicating if request payload contains in form data. 
        /// </summary>
        /// <param name="request"><see cref="HttpRequest"/> instance.</param>
        protected static bool IsFromForm(HttpRequest request)
        {
            return (request.ContentType.StartsWith(ContentType.MultipartFormDataContentType, StringComparison.OrdinalIgnoreCase)
                   || request.ContentType.StartsWith(ContentType.FormUrlencodedContentType, StringComparison.OrdinalIgnoreCase)) &&
                   request.Form != null;
        }


        /// <summary>
        /// Returns full-text search string from request parameters.
        /// </summary>
        protected static string ParseSearch(HttpRequest request)
        {
            var parameter = GetQueryParameter(request, QuerySyntaxHelper.SearchParameterName);

            return parameter?.Trim();
        }

        /// <summary>
        /// Returns documents filter func.
        /// </summary>
        protected InvocationQuerySyntaxNode ParseFilter(HttpRequest request, RouteData routeData, string documentIdKey)
        {
            InvocationQuerySyntaxNode filter = null;

            // Если определен фильтр по идентификатору документа
            var documentId = GetRequestParameter(request, routeData, documentIdKey);

            if (!string.IsNullOrEmpty(documentId))
            {
                filter = new InvocationQuerySyntaxNode(QuerySyntaxHelper.EqMethodName);
                filter.Arguments.Add(new IdentifierNameQuerySyntaxNode("_id"));
                filter.Arguments.Add(new LiteralQuerySyntaxNode(documentId));
            }
            // Если определено условие фильтрации
            else
            {
                var documentQuery = GetQueryParameter(request, QuerySyntaxHelper.FilterParameterName);

                if (documentQuery != null)
                {
                    var syntaxNodes = _syntaxTreeParser.Parse(documentQuery);

                    if (syntaxNodes != null && syntaxNodes.Count > 0)
                    {
                        if (syntaxNodes.Count == 1)
                        {
                            return syntaxNodes[0] as InvocationQuerySyntaxNode;
                        }

                        // По умолчанию все условия объединяются методом and()
                        var and = new InvocationQuerySyntaxNode(QuerySyntaxHelper.AndMethodName);
                        and.Arguments.AddRange(syntaxNodes);
                        return and;
                    }
                }
            }

            return filter;
        }

        /// <summary>
        /// Returns set of document search funcs.
        /// </summary>
        protected IEnumerable<InvocationQuerySyntaxNode> ParseSelect(HttpRequest request)
        {
            var parameter = GetQueryParameter(request, QuerySyntaxHelper.SelectParameterName);

            if (parameter != null)
            {
                var syntaxNodes = _syntaxTreeParser.Parse(parameter);

                if (syntaxNodes != null && syntaxNodes.Count > 0)
                {
                    // По умолчанию перечисление полей объединяется методом include()
                    var include = new InvocationQuerySyntaxNode(QuerySyntaxHelper.IncludeMethodName);

                    foreach (var node in syntaxNodes)
                    {
                        if (node is InvocationQuerySyntaxNode invocationNode)
                        {
                            yield return invocationNode;
                        }
                        else
                        {
                            include.Arguments.Add(node);
                        }
                    }

                    if (include.Arguments.Count > 0)
                    {
                        yield return include;
                    }
                }
            }
        }

        /// <summary>
        /// Returns set of document sort funcs.
        /// </summary>
        protected IEnumerable<InvocationQuerySyntaxNode> ParseOrder(HttpRequest request)
        {
            var parameter = GetQueryParameter(request, QuerySyntaxHelper.OrderParameterName);

            if (parameter != null)
            {
                var syntaxNodes = _syntaxTreeParser.Parse(parameter);

                if (syntaxNodes != null && syntaxNodes.Count > 0)
                {
                    foreach (var node in syntaxNodes)
                    {
                        if (node is InvocationQuerySyntaxNode invocationNode)
                        {
                            yield return invocationNode;
                        }
                        else
                        {
                            // По умолчанию перечисление полей объединяется методом asc()
                            var asc = new InvocationQuerySyntaxNode(QuerySyntaxHelper.AscMethodName);
                            asc.Arguments.Add(node);
                            yield return asc;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns flag indicating if documents count is needed.
        /// </summary>
        protected static bool ParseCount(HttpRequest request)
        {
            var parameter = GetQueryParameter(request, QuerySyntaxHelper.CountParameterName);

            if (parameter != null)
            {
                return bool.TryParse(parameter, out var value) && value;
            }

            return false;
        }

        /// <summary>
        /// Returns number of documents to skip.
        /// </summary>
        protected static int ParseSkip(HttpRequest request)
        {
            var parameter = GetQueryParameter(request, QuerySyntaxHelper.SkipParameterName);

            if (parameter != null)
            {
                if (int.TryParse(parameter, out var value) && value >= 0)
                {
                    return value;
                }
            }

            return 0;
        }

        /// <summary>
        /// Returns max number of returning documents.
        /// </summary>
        protected static int ParseTake(HttpRequest request)
        {
            var parameter = GetQueryParameter(request, QuerySyntaxHelper.TakeParameterName);

            if (parameter != null)
            {
                if (int.TryParse(parameter, out var value) && value > 0)
                {
                    return Math.Min(value, 1000);
                }
            }

            return 10;
        }


        private static string GetQueryParameter(HttpRequest request, string name)
        {
            var query = request.Query;

            var valueAsString = (string)query[name];

            if (!string.IsNullOrWhiteSpace(valueAsString))
            {
                return valueAsString;
            }

            return null;
        }

        private static string GetRequestParameter(HttpRequest request, RouteData routeData, string parameterName)
        {
            string value = null;

            if (routeData?.Values != null)
            {
                value = (string)routeData.Values[parameterName];
            }

            if (string.IsNullOrWhiteSpace(value) && request.Query != null)
            {
                value = request.Query[parameterName];
            }

            return value;
        }
    }
}