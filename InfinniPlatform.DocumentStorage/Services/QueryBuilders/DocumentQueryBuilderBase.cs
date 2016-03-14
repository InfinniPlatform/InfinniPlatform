using System.Collections.Generic;

using InfinniPlatform.DocumentStorage.Services.QuerySyntax;

namespace InfinniPlatform.DocumentStorage.Services.QueryBuilders
{
    public abstract class DocumentQueryBuilderBase
    {
        protected DocumentQueryBuilderBase(IQuerySyntaxTreeParser syntaxTreeParser)
        {
            _syntaxTreeParser = syntaxTreeParser;
        }


        private readonly IQuerySyntaxTreeParser _syntaxTreeParser;


        /// <summary>
        /// Возвращает строку полнотекстового поиска.
        /// </summary>
        protected string ParseSearch(IDictionary<string, object> query)
        {
            var parameter = GetQueryParameter(query, QuerySyntaxHelper.SearchParameterName);

            return parameter?.Trim();
        }

        /// <summary>
        /// Возвращает функцию фильтрации документов.
        /// </summary>
        protected InvocationQuerySyntaxNode ParseFilter(IDictionary<string, object> query)
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
        protected IEnumerable<InvocationQuerySyntaxNode> ParseSelect(IDictionary<string, object> query)
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
        protected IEnumerable<InvocationQuerySyntaxNode> ParseOrder(IDictionary<string, object> query)
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
        protected bool ParseCount(IDictionary<string, object> query)
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
        protected int ParseSkip(IDictionary<string, object> query)
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
        protected int? ParseTake(IDictionary<string, object> query)
        {
            var parameter = GetQueryParameter(query, QuerySyntaxHelper.TakeParameterName);

            if (parameter != null)
            {
                int value;

                if (int.TryParse(parameter, out value) && value >= 0)
                {
                    return value;
                }
            }

            return null;
        }

        private static string GetQueryParameter(IDictionary<string, object> query, string name)
        {
            object value;

            if (query.TryGetValue(name, out value))
            {
                var valueAsString = value as string;

                if (!string.IsNullOrWhiteSpace(valueAsString))
                {
                    return valueAsString;
                }
            }

            return null;
        }
    }
}