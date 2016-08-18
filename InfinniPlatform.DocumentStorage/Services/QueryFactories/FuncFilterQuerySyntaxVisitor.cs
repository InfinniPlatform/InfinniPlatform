using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using InfinniPlatform.DocumentStorage.Services.QuerySyntax;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Documents.Services;
using InfinniPlatform.Sdk.Metadata.Documents;

namespace InfinniPlatform.DocumentStorage.Services.QueryFactories
{
    /// <summary>
    /// Выполняет синтаксический разбор для получения правила фильтрации документов <see cref="DocumentGetQuery.Filter"/>.
    /// </summary>
    internal sealed class FuncFilterQuerySyntaxVisitor : FuncBaseQuerySyntaxVisitor<Func<IDocumentFilterBuilder, object>>
    {
        private static readonly Dictionary<string, Func<FuncFilterQuerySyntaxVisitor, InvocationQuerySyntaxNode, Func<IDocumentFilterBuilder, object>>> KnownFunctions
            = new Dictionary<string, Func<FuncFilterQuerySyntaxVisitor, InvocationQuerySyntaxNode, Func<IDocumentFilterBuilder, object>>>(StringComparer.OrdinalIgnoreCase)
              {
                { QuerySyntaxHelper.NotMethodName, NotInvocation },
                { QuerySyntaxHelper.OrMethodName, OrInvocation },
                { QuerySyntaxHelper.AndMethodName, AndInvocation },
                { QuerySyntaxHelper.ExistsMethodName, ExistsInvocation },
                { QuerySyntaxHelper.TypeMethodName, TypeInvocation },
                { QuerySyntaxHelper.InMethodName, InInvocation },
                { QuerySyntaxHelper.NotInMethodName, NotInInvocation },
                { QuerySyntaxHelper.EqMethodName, EqInvocation },
                { QuerySyntaxHelper.NotEqMethodName, NotEqInvocation },
                { QuerySyntaxHelper.GtMethodName, GtInvocation },
                { QuerySyntaxHelper.GteMethodName, GteInvocation },
                { QuerySyntaxHelper.LtMethodName, LtInvocation },
                { QuerySyntaxHelper.LteMethodName, LteInvocation },
                { QuerySyntaxHelper.RegexMethodName, RegexInvocation },
                { QuerySyntaxHelper.StartsWithMethodName, StartsWithInvocation },
                { QuerySyntaxHelper.EndsWithMethodName, EndsWithInvocation },
                { QuerySyntaxHelper.ContainsMethodName, ContainsInvocation },
                { QuerySyntaxHelper.MatchMethodName, MatchInvocation },
                { QuerySyntaxHelper.AllMethodName, AllInvocation },
                { QuerySyntaxHelper.AnyInMethodName, AnyInInvocation },
                { QuerySyntaxHelper.AnyNotInMethodName, AnyNotInInvocation },
                { QuerySyntaxHelper.AnyEqMethodName, AnyEqInvocation },
                { QuerySyntaxHelper.AnyNotEqMethodName, AnyNotEqInvocation },
                { QuerySyntaxHelper.AnyGtMethodName, AnyGtInvocation },
                { QuerySyntaxHelper.AnyGteMethodName, AnyGteInvocation },
                { QuerySyntaxHelper.AnyLtMethodName, AnyLtInvocation },
                { QuerySyntaxHelper.AnyLteMethodName, AnyLteInvocation },
                { QuerySyntaxHelper.SizeEqMethodName, SizeEqInvocation },
                { QuerySyntaxHelper.SizeGtMethodName, SizeGtInvocation },
                { QuerySyntaxHelper.SizeGteMethodName, SizeGteInvocation },
                { QuerySyntaxHelper.SizeLtMethodName, SizeLtInvocation },
                { QuerySyntaxHelper.SizeLteMethodName, SizeLteInvocation },
                { QuerySyntaxHelper.TextMethodName, TextInvocation },
                { QuerySyntaxHelper.DateMethodName, DateInvocation }
              };


        public static Func<IDocumentFilterBuilder, object> CreateFilterExpression(InvocationQuerySyntaxNode node)
        {
            var visitor = new FuncFilterQuerySyntaxVisitor();
            var filterBody = visitor.Visit(node);
            return filterBody;
        }


        // Overrides

        public override Func<IDocumentFilterBuilder, object> VisitInvocationExpression(InvocationQuerySyntaxNode node)
        {
            Func<FuncFilterQuerySyntaxVisitor, InvocationQuerySyntaxNode, Func<IDocumentFilterBuilder, object>> factory;

            if (!KnownFunctions.TryGetValue(node.Name, out factory))
            {
                return base.VisitInvocationExpression(node);
            }

            return factory(this, node);
        }

        public override Func<IDocumentFilterBuilder, object> VisitIdentifierName(IdentifierNameQuerySyntaxNode node)
        {
            return f => node.Identifier;
        }

        public override Func<IDocumentFilterBuilder, object> VisitLiteral(LiteralQuerySyntaxNode node)
        {
            return f => node.Value;
        }


        // KnownFunctions

        private static Func<IDocumentFilterBuilder, object> NotInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // not(condition) --> f.Not(condition)
            var condition = visitor.Visit(node.Arguments[0]);
            return f => f.Not(condition(f));
        }

        private static Func<IDocumentFilterBuilder, object> OrInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // or(condition1, condition2, condition3, ...) --> f.Or(condition1, condition2, condition3, ...)
            var conditions = node.Arguments.Select(visitor.Visit);
            return f => f.Or(conditions.Select(i => i(f)));
        }

        private static Func<IDocumentFilterBuilder, object> AndInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // and(condition1, condition2, condition3, ...) --> f.And(condition1, condition2, condition3, ...)
            var conditions = node.Arguments.Select(visitor.Visit);
            return f => f.And(conditions.Select(i => i(f)));
        }

        private static Func<IDocumentFilterBuilder, object> ExistsInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // exists(property) --> f.Exists("property", true)
            // exists(property, true) --> f.Exists("property", true)
            // exists(property, false) --> f.Exists("property", false)
            var property = node.Arguments[0].AsIdentifierName();
            var exists = (node.Arguments.Count < 2 || node.Arguments[1].AsBooleanLiteral());
            return f => f.Exists(property, exists);
        }

        private static Func<IDocumentFilterBuilder, object> TypeInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // type(property, value) --> f.Type("property", value)
            var property = node.Arguments[0].AsIdentifierName();
            var value = node.Arguments[1].AsEnumIdentifier<DataType>();
            return f => f.Type(property, value);
        }

        private static Func<IDocumentFilterBuilder, object> InInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // in(property, value1, value2, value3, ...) --> f.In("property", new[] { value1, value2, value3, ... })
            var property = node.Arguments[0].AsIdentifierName();
            var values = node.Arguments.Skip(1).Select(visitor.Visit);
            return f => f.In(property, values.Select(i => i(f)));
        }

        private static Func<IDocumentFilterBuilder, object> NotInInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // notIn(property, value1, value2, value3, ...) --> f.NotIn("property", new[] { value1, value2, value3, ... })
            var property = node.Arguments[0].AsIdentifierName();
            var values = node.Arguments.Skip(1).Select(visitor.Visit);
            return f => f.NotIn(property, values.Select(i => i(f)));
        }

        private static Func<IDocumentFilterBuilder, object> EqInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // eq(property, value) --> f.Eq("property", value)
            var property = node.Arguments[0].AsIdentifierName();
            var value = visitor.Visit(node.Arguments[1]);
            return f => f.Eq(property, value(f));
        }

        private static Func<IDocumentFilterBuilder, object> NotEqInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // notEq(property, value) --> f.NotEq("property", value)
            var property = node.Arguments[0].AsIdentifierName();
            var value = visitor.Visit(node.Arguments[1]);
            return f => f.NotEq(property, value(f));
        }

        private static Func<IDocumentFilterBuilder, object> GtInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // gt(property, value) --> f.Gt("property", value)
            var property = node.Arguments[0].AsIdentifierName();
            var value = visitor.Visit(node.Arguments[1]);
            return f => f.Gt(property, value(f));
        }

        private static Func<IDocumentFilterBuilder, object> GteInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // gte(property, value) --> f.Gte("property", value)
            var property = node.Arguments[0].AsIdentifierName();
            var value = visitor.Visit(node.Arguments[1]);
            return f => f.Gte(property, value(f));
        }

        private static Func<IDocumentFilterBuilder, object> LtInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // lt(property, value) --> f.Lt("property", value)
            var property = node.Arguments[0].AsIdentifierName();
            var value = visitor.Visit(node.Arguments[1]);
            return f => f.Lt(property, value(f));
        }

        private static Func<IDocumentFilterBuilder, object> LteInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // lte(property, value) --> f.Lte("property", value)
            var property = node.Arguments[0].AsIdentifierName();
            var value = visitor.Visit(node.Arguments[1]);
            return f => f.Lte(property, value(f));
        }

        private static Func<IDocumentFilterBuilder, object> RegexInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // regex(property, pattern, 'option1', 'option2', 'option3', ...) --> f.Regex("property", new Regex("pattern", option1 | option2 | option3 | ...))
            var property = node.Arguments[0].AsIdentifierName();
            var pattern = node.Arguments[1].AsStringLiteral();
            var options = node.Arguments.Skip(2).Aggregate(RegexOptions.None, (r, n) => r | n.AsEnumIdentifier<RegexOptions>());
            var regex = new Regex(pattern, options);
            return f => f.Regex(property, regex);
        }

        private static Func<IDocumentFilterBuilder, object> StartsWithInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // startsWith(property, value, ignoreCase) --> f.StartsWith("property", "value", ignoreCase)
            var property = node.Arguments[0].AsIdentifierName();
            var value = node.Arguments[1].AsStringLiteral();
            var ignoreCase = (node.Arguments.Count <= 2 || node.Arguments[2].AsBooleanLiteral());
            return f => f.StartsWith(property, value, ignoreCase);
        }

        private static Func<IDocumentFilterBuilder, object> EndsWithInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // endsWith(property, value, ignoreCase) --> f.EndsWith("property", "value", ignoreCase)
            var property = node.Arguments[0].AsIdentifierName();
            var value = node.Arguments[1].AsStringLiteral();
            var ignoreCase = (node.Arguments.Count <= 2 || node.Arguments[2].AsBooleanLiteral());
            return f => f.EndsWith(property, value, ignoreCase);
        }

        private static Func<IDocumentFilterBuilder, object> ContainsInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // contains(property, value, ignoreCase) --> f.Contains("property", "value", ignoreCase)
            var property = node.Arguments[0].AsIdentifierName();
            var value = node.Arguments[1].AsStringLiteral();
            var ignoreCase = (node.Arguments.Count <= 2 || node.Arguments[2].AsBooleanLiteral());
            return f => f.Contains(property, value, ignoreCase);
        }

        private static Func<IDocumentFilterBuilder, object> MatchInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // match(arrayProperty, filter) --> f.Match("arrayProperty", filter(f))
            var arrayProperty = node.Arguments[0].AsIdentifierName();
            var filter = visitor.Visit(node.Arguments[1]);
            return f => f.Match(arrayProperty, filter(f));
        }

        private static Func<IDocumentFilterBuilder, object> AllInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // all(arrayProperty, item1, item2, item3, ...) --> f.All("arrayProperty", new[] { item1, item2, item3, ... })
            return CreateFilterForArrayProperty(visitor, node, (f, p, i) => f.All(p, i));
        }

        private static Func<IDocumentFilterBuilder, object> AnyInInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // anyIn(arrayProperty, item1, item2, item3, ...) --> f.AnyIn("arrayProperty", new[] { item1, item2, item3, ... })
            return CreateFilterForArrayProperty(visitor, node, (f, p, i) => f.AnyIn(p, i));
        }

        private static Func<IDocumentFilterBuilder, object> AnyNotInInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // anyNotIn(arrayProperty, item1, item2, item3, ...) --> f.AnyNotIn("arrayProperty", new[] { item1, item2, item3, ... })
            return CreateFilterForArrayProperty(visitor, node, (f, p, i) => f.AnyNotIn(p, i));
        }

        private static Func<IDocumentFilterBuilder, object> AnyEqInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // anyEq(arrayProperty, item) --> f.AnyEq("arrayProperty", item)
            return CreateAnyFilterForArrayProperty(visitor, node, (f, p, i) => f.AnyEq(p, i));
        }

        private static Func<IDocumentFilterBuilder, object> AnyNotEqInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // anyNotEq(arrayProperty, item) --> f.AnyNotEq("arrayProperty", item)
            return CreateAnyFilterForArrayProperty(visitor, node, (f, p, i) => f.AnyNotEq(p, i));
        }

        private static Func<IDocumentFilterBuilder, object> AnyGtInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // anyGt(arrayProperty, item) --> f.AnyGt("arrayProperty", item)
            return CreateAnyFilterForArrayProperty(visitor, node, (f, p, i) => f.AnyGt(p, i));
        }

        private static Func<IDocumentFilterBuilder, object> AnyGteInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // anyGte(arrayProperty, item) --> f.AnyGte("arrayProperty", item)
            return CreateAnyFilterForArrayProperty(visitor, node, (f, p, i) => f.AnyGte(p, i));
        }

        private static Func<IDocumentFilterBuilder, object> AnyLtInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // anyLt(arrayProperty, item) --> f.AnyLt("arrayProperty", item)
            return CreateAnyFilterForArrayProperty(visitor, node, (f, p, i) => f.AnyLt(p, i));
        }

        private static Func<IDocumentFilterBuilder, object> AnyLteInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // anyLte(arrayProperty, item) --> f.AnyLte("arrayProperty", item)
            return CreateAnyFilterForArrayProperty(visitor, node, (f, p, i) => f.AnyLte(p, i));
        }

        private static Func<IDocumentFilterBuilder, object> SizeEqInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // sizeEq(arrayProperty, value) --> f.SizeEq("arrayProperty", value)
            return CreateSizeFilterForArrayProperty(visitor, node, (f, p, i) => f.SizeEq(p, i));
        }

        private static Func<IDocumentFilterBuilder, object> SizeGtInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // sizeGt(arrayProperty, value) --> f.SizeGt("arrayProperty", value)
            return CreateSizeFilterForArrayProperty(visitor, node, (f, p, i) => f.SizeGt(p, i));
        }

        private static Func<IDocumentFilterBuilder, object> SizeGteInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // sizeGte(arrayProperty, value) --> f.SizeGte("arrayProperty", value)
            return CreateSizeFilterForArrayProperty(visitor, node, (f, p, i) => f.SizeGte(p, i));
        }

        private static Func<IDocumentFilterBuilder, object> SizeLtInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // sizeLt(arrayProperty, value) --> f.SizeLt("arrayProperty", value)
            return CreateSizeFilterForArrayProperty(visitor, node, (f, p, i) => f.SizeLt(p, i));
        }

        private static Func<IDocumentFilterBuilder, object> SizeLteInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // sizeLte(arrayProperty, item) --> f.SizeLte("arrayProperty", value)
            return CreateSizeFilterForArrayProperty(visitor, node, (f, p, i) => f.SizeLte(p, i));
        }

        private static Func<IDocumentFilterBuilder, object> TextInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // text('search', 'language', caseSensitive, diacriticSensitive) --> f.Text("search", "language", caseSensitive, diacriticSensitive)
            var search = node.Arguments[0].AsStringLiteral();
            var language = (node.Arguments.Count > 1) ? node.Arguments[1].AsStringLiteral() : null;
            var caseSensitive = (node.Arguments.Count > 2) && node.Arguments[2].AsBooleanLiteral();
            var diacriticSensitive = (node.Arguments.Count > 3) && node.Arguments[3].AsBooleanLiteral();
            return f => f.Text(search, language, caseSensitive, diacriticSensitive);
        }

        private static Func<IDocumentFilterBuilder, object> DateInvocation(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // date('value') --> DateTime.Parse('value')
            var dateTimeString = node.Arguments[0].AsStringLiteral();
            var dateTime = DateTime.Parse(dateTimeString);
            return f => dateTime;
        }


        // Helpers

        private static Func<IDocumentFilterBuilder, object> CreateFilterForArrayProperty(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node, Func<IDocumentFilterBuilder, string, IEnumerable<object>, object> filter)
        {
            // filter(arrayProperty, item1, item2, item3, ...) --> filter("arrayProperty", new[] { item1, item2, item3, ... })
            var arrayProperty = node.Arguments[0].AsIdentifierName();
            var items = node.Arguments.Skip(1).Select(visitor.Visit);
            return f => filter(f, arrayProperty, items.Select(i => i(f)));
        }

        private static Func<IDocumentFilterBuilder, object> CreateAnyFilterForArrayProperty(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node, Func<IDocumentFilterBuilder, string, object, object> filter)
        {
            // filter(arrayProperty, item) --> filter("arrayProperty", item)
            var arrayProperty = node.Arguments[0].AsIdentifierName();
            var item = visitor.Visit(node.Arguments[1]);
            return f => filter(f, arrayProperty, item(f));
        }

        private static Func<IDocumentFilterBuilder, object> CreateSizeFilterForArrayProperty(FuncFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node, Func<IDocumentFilterBuilder, string, int, object> filter)
        {
            // filter(arrayProperty, value) --> filter("arrayProperty", value)
            var arrayProperty = node.Arguments[0].AsIdentifierName();
            var value = visitor.Visit(node.Arguments[1]);
            return f => filter(f, arrayProperty, (int)value(f));
        }
    }
}