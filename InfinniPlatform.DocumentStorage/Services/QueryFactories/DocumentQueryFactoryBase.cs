using System;
using System.Collections.Generic;

using InfinniPlatform.DocumentStorage.Properties;
using InfinniPlatform.DocumentStorage.Services.QuerySyntax;
using InfinniPlatform.Sdk.Documents.Services;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.DocumentStorage.Services.QueryFactories
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


        public DocumentDeleteQuery CreateDeleteQuery(IHttpRequest request, string documentIdKey)
        {
            var documentId = ReadRequestParameter(request, documentIdKey);

            if (!string.IsNullOrEmpty(documentId))
            {
                return new DocumentDeleteQuery
                {
                    DocumentId = documentId
                };
            }

            throw new InvalidOperationException(Resources.MethodNotAllowed);
        }


        /// <summary>
        /// Возвращает условие запроса.
        /// </summary>
        protected object ReadRequestQuery(IHttpRequest request)
        {
            return request.Query;
        }

        /// <summary>
        /// Возвращает параметр запроса.
        /// </summary>
        protected string ReadRequestParameter(IHttpRequest request, string parameterName)
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

        /// <summary>
        /// Возвращает форму запроса.
        /// </summary>
        protected TDocument ReadRequestForm<TDocument>(IHttpRequest request, string documentFormKey)
        {
            if (request.Headers.ContentType.StartsWith(HttpConstants.MultipartFormDataContentType)
                || request.Headers.ContentType.StartsWith(HttpConstants.FormUrlencodedContentType))
            {
                if (request.Form != null)
                {
                    var documentString = request.Form[documentFormKey] as string;

                    if (!string.IsNullOrWhiteSpace(documentString))
                    {
                        return _objectSerializer.Deserialize<TDocument>(documentString);
                    }
                }
            }
            else if (request.Form != null)
            {
                return _objectSerializer.ConvertFromDynamic<TDocument>((object)request.Form);
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
        protected string ParseSearch(object query)
        {
            var parameter = GetQueryParameter(query, QuerySyntaxHelper.SearchParameterName);

            return parameter?.Trim();
        }

        /// <summary>
        /// Возвращает функцию фильтрации документов.
        /// </summary>
        protected InvocationQuerySyntaxNode ParseFilter(object query)
        {
            var parameter = GetQueryParameter(query, QuerySyntaxHelper.FilterParameterName);

            if (parameter != null)
            {
                var syntaxNodes = _syntaxTreeParser.Parse(parameter);

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

            return null;
        }

        /// <summary>
        /// Возвращает набор функций выборки документов.
        /// </summary>
        protected IEnumerable<InvocationQuerySyntaxNode> ParseSelect(object query)
        {
            var parameter = GetQueryParameter(query, QuerySyntaxHelper.SelectParameterName);

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
        protected IEnumerable<InvocationQuerySyntaxNode> ParseOrder(object query)
        {
            var parameter = GetQueryParameter(query, QuerySyntaxHelper.OrderParameterName);

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
        protected bool ParseCount(object query)
        {
            var parameter = GetQueryParameter(query, QuerySyntaxHelper.CountParameterName);

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
        protected int ParseSkip(object query)
        {
            var parameter = GetQueryParameter(query, QuerySyntaxHelper.SkipParameterName);

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
        protected int ParseTake(object query)
        {
            var parameter = GetQueryParameter(query, QuerySyntaxHelper.TakeParameterName);

            if (parameter != null)
            {
                int value;

                if (int.TryParse(parameter, out value) && value >= 0)
                {
                    return Math.Min(value, 1000);
                }
            }

            return 10;
        }


        private static string GetQueryParameter(object query, string name)
        {
            dynamic queryAsDynamic = query;

            var valueAsString = (string)queryAsDynamic[name];

            if (!string.IsNullOrWhiteSpace(valueAsString))
            {
                return valueAsString;
            }

            return null;
        }
    }
}