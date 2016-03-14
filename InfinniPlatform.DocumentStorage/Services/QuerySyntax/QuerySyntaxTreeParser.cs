using System;
using System.Collections.Generic;

using InfinniPlatform.DocumentStorage.Properties;

namespace InfinniPlatform.DocumentStorage.Services.QuerySyntax
{
    /// <summary>
    /// Синтаксический анализатора строки запроса.
    /// </summary>
    /// <remarks>
    /// Данная реализация позволяет делать синтаксический разбор выражений, которые представляют собой
    /// комбинацию следующих элементов: метод, литерал, идентификатор. Метод - функция, которая может
    /// принимать любое количество аргументов. В качестве аргументов метода могут выступать литералы
    /// и идентификаторы. Литералы - константы - нулевой указатель (<c>null</c>), логические значения
    /// (<c>true</c> или <c>false</c>), целые и дробные числа, строки. Идентификаторы - переменные,
    /// значения которых определено во внешнем контексте. В контексте использования данного класса
    /// в роли идентификаторов выступают свойства документов (с той целью, чтобы отличать их от
    /// строковых литералов).
    /// </remarks>
    public sealed class QuerySyntaxTreeParser : IQuerySyntaxTreeParser
    {
        /// <summary>
        /// Осуществляет синтаксический анализ строки запроса.
        /// </summary>
        /// <param name="query">Строка запроса.</param>
        /// <exception cref="NotSupportedException"></exception>
        public IList<IQuerySyntaxNode> Parse(string query)
        {
            var expressions = new List<IQuerySyntaxNode>();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var tokenReader = new QueryTokenReader(query);
                var callStack = new Stack<IQuerySyntaxNode>();

                for (QueryToken token, prevToken = null; tokenReader.CanRead(); prevToken = token)
                {
                    token = tokenReader.ReadNext();

                    switch (token.Kind)
                    {
                        case QueryTokenKind.Identifier:
                            if (prevToken == null
                                || prevToken.Kind == QueryTokenKind.OpenBracket
                                || prevToken.Kind == QueryTokenKind.ArgumentSeparator)
                            {
                                callStack.Push(new IdentifierNameQuerySyntaxNode((string)token.Value));
                                continue;
                            }
                            break;

                        case QueryTokenKind.OpenBracket:
                            if (prevToken != null
                                && prevToken.Kind == QueryTokenKind.Identifier)
                            {
                                callStack.Pop();
                                callStack.Push(new InvocationQuerySyntaxNode((string)prevToken.Value));
                                continue;
                            }
                            break;

                        case QueryTokenKind.Null:
                        case QueryTokenKind.Boolean:
                        case QueryTokenKind.Integer:
                        case QueryTokenKind.Float:
                        case QueryTokenKind.String:
                            if (prevToken == null
                                || prevToken.Kind == QueryTokenKind.OpenBracket
                                || prevToken.Kind == QueryTokenKind.ArgumentSeparator)
                            {
                                callStack.Push(new LiteralQuerySyntaxNode(token.Value));
                                continue;
                            }
                            break;

                        case QueryTokenKind.ArgumentSeparator:
                            if (prevToken != null
                                && prevToken.Kind != QueryTokenKind.OpenBracket
                                && prevToken.Kind != QueryTokenKind.ArgumentSeparator)
                            {
                                if (callStack.Count > 0)
                                {
                                    var argument = callStack.Pop();

                                    if (callStack.Count > 0)
                                    {
                                        var method = callStack.Peek() as InvocationQuerySyntaxNode;

                                        if (method != null)
                                        {
                                            method.Arguments.Add(argument);
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        expressions.Add(argument);
                                        continue;
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            break;

                        case QueryTokenKind.CloseBracket:
                            if (prevToken != null
                                && prevToken.Kind != QueryTokenKind.ArgumentSeparator)
                            {
                                if (callStack.Count > 0)
                                {
                                    var argument = callStack.Pop();

                                    if (callStack.Count > 0)
                                    {
                                        var method = callStack.Peek() as InvocationQuerySyntaxNode;

                                        if (method != null)
                                        {
                                            method.Arguments.Add(argument);
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        expressions.Add(argument);
                                        continue;
                                    }
                                }
                            }
                            break;

                        default:
                            throw new NotSupportedException(string.Format(Resources.TokenIsNotSupported, token.Value));
                    }

                    throw new InvalidOperationException(string.Format(Resources.IllegalToken, token.Value));
                }

                if (callStack.Count == 1)
                {
                    expressions.Add(callStack.Pop());
                }
                else if (callStack.Count != 0)
                {
                    throw new InvalidOperationException(Resources.IncompleteExpression);
                }
            }

            return expressions;
        }
    }
}