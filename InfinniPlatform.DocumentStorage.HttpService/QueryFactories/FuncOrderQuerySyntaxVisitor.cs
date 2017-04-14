using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.DocumentStorage.HttpService.QuerySyntax;

namespace InfinniPlatform.DocumentStorage.HttpService.QueryFactories
{
    /// <summary>
    /// Выполняет синтаксический разбор для получения правила сортировки документов <see cref="DocumentGetQuery.Order"/>.
    /// </summary>
    public class FuncOrderQuerySyntaxVisitor : FuncBaseQuerySyntaxVisitor<IEnumerable<KeyValuePair<string, DocumentSortOrder>>>
    {
        private static readonly Dictionary<string, Func<FuncOrderQuerySyntaxVisitor, InvocationQuerySyntaxNode, IEnumerable<KeyValuePair<string, DocumentSortOrder>>>> KnownFunctions
            = new Dictionary<string, Func<FuncOrderQuerySyntaxVisitor, InvocationQuerySyntaxNode, IEnumerable<KeyValuePair<string, DocumentSortOrder>>>>(StringComparer.OrdinalIgnoreCase)
              {
                { QuerySyntaxHelper.AscMethodName, AscInvocation },
                { QuerySyntaxHelper.DescMethodName, DescInvocation },
                { QuerySyntaxHelper.TextScoreMethodName, TextScoreInvocation }
              };


        public static IEnumerable<KeyValuePair<string, DocumentSortOrder>> CreateOrderExpression(InvocationQuerySyntaxNode node)
        {
            var visitor = new FuncOrderQuerySyntaxVisitor();
            var orderProperties = visitor.Visit(node);
            return orderProperties;
        }


        // Overrides

        public override IEnumerable<KeyValuePair<string, DocumentSortOrder>> VisitInvocationExpression(InvocationQuerySyntaxNode node)
        {
            Func<FuncOrderQuerySyntaxVisitor, InvocationQuerySyntaxNode, IEnumerable<KeyValuePair<string, DocumentSortOrder>>> factory;

            if (!KnownFunctions.TryGetValue(node.Name, out factory))
            {
                return base.VisitInvocationExpression(node);
            }

            return factory(this, node);
        }


        // KnownFunctions

        private static IEnumerable<KeyValuePair<string, DocumentSortOrder>> AscInvocation(FuncOrderQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // asc(property1, property2, property3, ...) --> new[] { { property1, DocumentSortOrder.Asc }, { property2, DocumentSortOrder.Asc }, { property3, DocumentSortOrder.Asc }, ... }
            return node.Arguments.Select(i => new KeyValuePair<string, DocumentSortOrder>(i.AsIdentifierName(), DocumentSortOrder.Asc));
        }

        private static IEnumerable<KeyValuePair<string, DocumentSortOrder>> DescInvocation(FuncOrderQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // desc(property1, property2, property3, ...) --> new[] { { property1, DocumentSortOrder.Desc }, { property2, DocumentSortOrder.Desc }, { property3, DocumentSortOrder.Desc }, ... }
            return node.Arguments.Select(i => new KeyValuePair<string, DocumentSortOrder>(i.AsIdentifierName(), DocumentSortOrder.Desc));
        }

        private static IEnumerable<KeyValuePair<string, DocumentSortOrder>> TextScoreInvocation(FuncOrderQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // textScore(property) --> p.IncludeTextScore(property)
            var property = (node.Arguments.Count > 0) ? node.Arguments[0].AsIdentifierName() : QuerySyntaxHelper.TextScoreMethodName;
            yield return new KeyValuePair<string, DocumentSortOrder>(property, DocumentSortOrder.TextScore);
        }
    }
}