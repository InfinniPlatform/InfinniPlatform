using System;
using System.Collections.Generic;

using InfinniPlatform.DocumentStorage.QuerySyntax;
using InfinniPlatform.Http;
using InfinniPlatform.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace InfinniPlatform.DocumentStorage.QueryFactories
{
    public abstract class DocumentQueryFactoryBase
    {
        protected DocumentQueryFactoryBase(IQuerySyntaxTreeParser syntaxTreeParser, IJsonObjectSerializer objectSerializer)
        {
            _syntaxTreeParser = syntaxTreeParser;
            _objectSerializer = objectSerializer;
        }


        private readonly IQuerySyntaxTreeParser _syntaxTreeParser;
        private readonly IJsonObjectSerializer _objectSerializer;


        /// <summary>
        /// Возвращает форму запроса.
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

        protected static bool IsFromForm(HttpRequest request)
        {
            return (request.ContentType.StartsWith(ContentType.MultipartFormDataContentType, StringComparison.OrdinalIgnoreCase)
                   || request.ContentType.StartsWith(ContentType.FormUrlencodedContentType, StringComparison.OrdinalIgnoreCase)) &&
                   request.Form != null;
        }


        /// <summary>
        /// Возвращает строку полнотекстового поиска.
        /// </summary>
        protected static string ParseSearch(HttpRequest request)
        {
            var parameter = GetQueryParameter(request, QuerySyntaxHelper.SearchParameterName);

            return parameter?.Trim();
        }

        /// <summary>
        /// Возвращает функцию фильтрации документов.
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
        /// Возвращает набор функций выборки документов.
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
                        var invocationNode = node as InvocationQuerySyntaxNode;

                        if (invocationNode != null)
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
        /// Возвращает набор функций сортировки документов.
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
                        var invocationNode = node as InvocationQuerySyntaxNode;

                        if (invocationNode != null)
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
        /// Возвращает признак необходимости подсчета количества документов.
        /// </summary>
        protected static bool ParseCount(HttpRequest request)
        {
            var parameter = GetQueryParameter(request, QuerySyntaxHelper.CountParameterName);

            if (parameter != null)
            {
                bool value;
                return bool.TryParse(parameter, out value) && value;
            }

            return false;
        }

        /// <summary>
        /// Возвращает количество документов, которое нужно пропустить.
        /// </summary>
        protected static int ParseSkip(HttpRequest request)
        {
            var parameter = GetQueryParameter(request, QuerySyntaxHelper.SkipParameterName);

            if (parameter != null)
            {
                int value;

                if (int.TryParse(parameter, out value) && value >= 0)
                {
                    return value;
                }
            }

            return 0;
        }

        /// <summary>
        /// Возвращает максимальное количество документов, которое нужно выбрать.
        /// </summary>
        protected static int ParseTake(HttpRequest request)
        {
            var parameter = GetQueryParameter(request, QuerySyntaxHelper.TakeParameterName);

            if (parameter != null)
            {
                int value;

                if (int.TryParse(parameter, out value) && value > 0)
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

        protected static string GetRequestParameter(HttpRequest request, RouteData routeData, string parameterName)
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