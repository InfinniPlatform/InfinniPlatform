using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Http;
using InfinniPlatform.Core.Serialization;
using InfinniPlatform.DocumentStorage.QuerySyntax;

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
        protected TDocument ReadRequestForm<TDocument>(IHttpRequest request, string documentFormKey)
        {
            if (request.Headers.ContentType.StartsWith(HttpConstants.MultipartFormDataContentType, StringComparison.OrdinalIgnoreCase)
                || request.Headers.ContentType.StartsWith(HttpConstants.FormUrlencodedContentType, StringComparison.OrdinalIgnoreCase))
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
            else if (request.Headers.ContentType.StartsWith(HttpConstants.JsonContentType, StringComparison.OrdinalIgnoreCase))
            {
                if (request.Form != null)
                {
                    return _objectSerializer.ConvertFromDynamic<TDocument>((object)request.Form);
                }
            }
            else if (request.Content != null)
            {
                return _objectSerializer.Deserialize<TDocument>(request.Content);
            }

            return default(TDocument);
        }


        /// <summary>
        /// Возвращает строку полнотекстового поиска.
        /// </summary>
        protected static string ParseSearch(IHttpRequest request)
        {
            var parameter = GetQueryParameter(request, QuerySyntaxHelper.SearchParameterName);

            return parameter?.Trim();
        }

        /// <summary>
        /// Возвращает функцию фильтрации документов.
        /// </summary>
        protected InvocationQuerySyntaxNode ParseFilter(IHttpRequest request, string documentIdKey)
        {
            InvocationQuerySyntaxNode filter = null;

            // Если определен фильтр по идентификатору документа
            var documentId = GetRequestParameter(request, documentIdKey);

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
        protected IEnumerable<InvocationQuerySyntaxNode> ParseSelect(IHttpRequest request)
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
        protected IEnumerable<InvocationQuerySyntaxNode> ParseOrder(IHttpRequest request)
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
        protected static bool ParseCount(IHttpRequest request)
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
        protected static int ParseSkip(IHttpRequest request)
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
        protected static int ParseTake(IHttpRequest request)
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


        private static string GetQueryParameter(IHttpRequest request, string name)
        {
            dynamic query = request.Query;

            var valueAsString = (string)query[name];

            if (!string.IsNullOrWhiteSpace(valueAsString))
            {
                return valueAsString;
            }

            return null;
        }

        protected static string GetRequestParameter(IHttpRequest request, string parameterName)
        {
            string value = null;

            if (request.Parameters != null)
            {
                value = (string)request.Parameters[parameterName];
            }

            if (string.IsNullOrWhiteSpace(value) && request.Query != null)
            {
                value = (string)request.Query[parameterName];
            }

            return value;
        }
    }
}