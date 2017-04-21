using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.DocumentStorage.QuerySyntax;

namespace InfinniPlatform.DocumentStorage.QueryFactories
{
    /// <summary>
    /// Выполняет синтаксический разбор для получения правила отображения документа <see cref="DocumentGetQuery{TDocument}.Select"/>.
    /// </summary>
    public class FuncSelectQuerySyntaxVisitor : FuncBaseQuerySyntaxVisitor<Action<IDocumentProjectionBuilder>>
    {
        private static readonly Dictionary<string, Func<FuncSelectQuerySyntaxVisitor, InvocationQuerySyntaxNode, Action<IDocumentProjectionBuilder>>> KnownFunctions
            = new Dictionary<string, Func<FuncSelectQuerySyntaxVisitor, InvocationQuerySyntaxNode, Action<IDocumentProjectionBuilder>>>(StringComparer.OrdinalIgnoreCase)
              {
                  { QuerySyntaxHelper.IncludeMethodName, IncludeInvocation },
                  { QuerySyntaxHelper.ExcludeMethodName, ExcludeInvocation },
                  { QuerySyntaxHelper.TextScoreMethodName, TextScoreInvocation }
              };


        public static Action<IDocumentProjectionBuilder> CreateSelectExpression(InvocationQuerySyntaxNode node)
        {
            var visitor = new FuncSelectQuerySyntaxVisitor();
            var selectBody = visitor.Visit(node);
            return selectBody;
        }


        // Overrides

        public override Action<IDocumentProjectionBuilder> VisitInvocationExpression(InvocationQuerySyntaxNode node)
        {
            Func<FuncSelectQuerySyntaxVisitor, InvocationQuerySyntaxNode, Action<IDocumentProjectionBuilder>> factory;

            if (!KnownFunctions.TryGetValue(node.Name, out factory))
            {
                return base.VisitInvocationExpression(node);
            }

            return factory(this, node);
        }


        // KnownFunctions

        private static Action<IDocumentProjectionBuilder> IncludeInvocation(FuncSelectQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // include(property1, property2, property3, ...) --> p.Include(property1).Include(property2).Include(property3). ...
            var properties = node.Arguments.Select(i => i.AsIdentifierName()).ToArray();
            return p => properties.Aggregate(p, (current, property) => current.Include(property));
        }

        private static Action<IDocumentProjectionBuilder> ExcludeInvocation(FuncSelectQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // exclude(property1, property2, property3, ...) --> p.Exclude(property1).Exclude(property2).Exclude(property3). ...
            var properties = node.Arguments.Select(i => i.AsIdentifierName()).ToArray();
            return p => properties.Aggregate(p, (current, property) => current.Exclude(property));
        }

        private static Action<IDocumentProjectionBuilder> TextScoreInvocation(FuncSelectQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // textScore(property) --> p.IncludeTextScore(property)
            var property = (node.Arguments.Count > 0) ? node.Arguments[0].AsIdentifierName() : QuerySyntaxHelper.TextScoreMethodName;
            return p => p.IncludeTextScore(property);
        }
    }
}