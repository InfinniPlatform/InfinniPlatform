using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

using InfinniPlatform.DocumentStorage.Services.QuerySyntax;
using InfinniPlatform.Sdk.Documents.Services;

namespace InfinniPlatform.DocumentStorage.Services.QueryFactories
{
    /// <summary>
    /// Выполняет синтаксический разбор для получения правила фильтрации документов <see cref="DocumentGetQuery{TDocument}.Filter"/>.
    /// </summary>
    internal sealed class ExpressionFilterQuerySyntaxVisitor : ExpressionBaseQuerySyntaxVisitor
    {
        private static readonly Dictionary<string, Func<ExpressionFilterQuerySyntaxVisitor, InvocationQuerySyntaxNode, Expression>> KnownFunctions
            = new Dictionary<string, Func<ExpressionFilterQuerySyntaxVisitor, InvocationQuerySyntaxNode, Expression>>(StringComparer.OrdinalIgnoreCase)
              {
                { QuerySyntaxHelper.NotMethodName, NotInvocation },
                { QuerySyntaxHelper.OrMethodName, OrInvocation },
                { QuerySyntaxHelper.AndMethodName, AndInvocation },
                { QuerySyntaxHelper.ExistsMethodName, ExistsInvocation },
                { QuerySyntaxHelper.InMethodName, InInvocation },
                { QuerySyntaxHelper.NotInMethodName, NotInInvocation },
                { QuerySyntaxHelper.EqMethodName, EqInvocation },
                { QuerySyntaxHelper.NotEqMethodName, NotEqInvocation },
                { QuerySyntaxHelper.GtMethodName, GtInvocation },
                { QuerySyntaxHelper.GteMethodName, GteInvocation },
                { QuerySyntaxHelper.LtMethodName, LtInvocation },
                { QuerySyntaxHelper.LteMethodName, LteInvocation },
                { QuerySyntaxHelper.RegexMethodName, RegexInvocation },
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
                { QuerySyntaxHelper.DateMethodName, DateInvocation }
              };


        private ExpressionFilterQuerySyntaxVisitor(Type type) : base(type)
        {
        }


        public static Expression CreateFilterExpression(Type type, InvocationQuerySyntaxNode node)
        {
            var visitor = new ExpressionFilterQuerySyntaxVisitor(type);
            var filterBody = visitor.Visit(node);
            return Expression.Lambda(filterBody, visitor.Parameter);
        }


        // Overrides

        public override Expression VisitInvocationExpression(InvocationQuerySyntaxNode node)
        {
            Func<ExpressionFilterQuerySyntaxVisitor, InvocationQuerySyntaxNode, Expression> factory;

            if (!KnownFunctions.TryGetValue(node.Name, out factory))
            {
                return base.VisitInvocationExpression(node);
            }

            return factory(this, node);
        }


        // KnownFunctions

        private static Expression NotInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // not(condition) --> !condition
            var condition = visitor.Visit(node.Arguments[0]);
            return Expression.Not(condition);
        }

        private static Expression OrInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // or(condition1, condition2, condition3, ...) --> (((condition1 || condition2) || condition3) || ...)

            Expression result = null;

            var conditions = node.Arguments.Select(visitor.Visit);

            foreach (var condition in conditions)
            {
                result = (result != null)
                    ? Expression.OrElse(result, condition)
                    : condition;
            }

            return result;
        }

        private static Expression AndInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // and(condition1, condition2, condition3, ...) --> (((condition1 && condition2) && condition3) && ...)

            Expression result = null;

            var conditions = node.Arguments.Select(visitor.Visit);

            foreach (var condition in conditions)
            {
                result = (result != null)
                    ? Expression.AndAlso(result, condition)
                    : condition;
            }

            return result;
        }

        private static Expression ExistsInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // exists(property, true) --> i.property != null
            // exists(property, false) --> i.property == null
            var property = visitor.Visit(node.Arguments[0]);
            var exists = (node.Arguments.Count < 2 || node.Arguments[1].AsBooleanLiteral());
            var nullValue = Expression.Constant(null);
            return exists ? Expression.NotEqual(property, nullValue) : Expression.Equal(property, nullValue);
        }

        private static Expression InInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // in(property, value1, value2, value3, ...) --> Enumerable.Contains(new[] { value1, value2, value3, ... }, i.property)
            var property = visitor.Visit(node.Arguments[0]);
            var values = QuerySyntaxHelper.NewArray(property.Type, node.Arguments.Skip(1).Select(visitor.Visit));
            return QuerySyntaxHelper.InvokeContains(property.Type, values, property);
        }

        private static Expression NotInInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // notIn(property, value1, value2, value3, ...) --> !Enumerable.Contains(new[] { value1, value2, value3, ... }, i.property)
            return Expression.Not(InInvocation(visitor, node));
        }

        private static Expression EqInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // eq(property, value) --> i.property == value
            var property = visitor.Visit(node.Arguments[0]);
            var value = visitor.Visit(node.Arguments[1]);
            return QuerySyntaxHelper.Compare(property, value, Expression.Equal);
        }

        private static Expression NotEqInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // notEq(property, value) --> i.property != value
            var property = visitor.Visit(node.Arguments[0]);
            var value = visitor.Visit(node.Arguments[1]);
            return QuerySyntaxHelper.Compare(property, value, Expression.NotEqual);
        }

        private static Expression GtInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // gt(property, value) --> i.property > value
            var property = visitor.Visit(node.Arguments[0]);
            var value = visitor.Visit(node.Arguments[1]);
            return QuerySyntaxHelper.Compare(property, value, Expression.GreaterThan);
        }

        private static Expression GteInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // gte(property, value) --> i.property >= value
            var property = visitor.Visit(node.Arguments[0]);
            var value = visitor.Visit(node.Arguments[1]);
            return QuerySyntaxHelper.Compare(property, value, Expression.GreaterThanOrEqual);
        }

        private static Expression LtInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // lt(property, value) --> i.property < value
            var property = visitor.Visit(node.Arguments[0]);
            var value = visitor.Visit(node.Arguments[1]);
            return QuerySyntaxHelper.Compare(property, value, Expression.LessThan);
        }

        private static Expression LteInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // lte(property, value) --> i.property <= value
            var property = visitor.Visit(node.Arguments[0]);
            var value = visitor.Visit(node.Arguments[1]);
            return QuerySyntaxHelper.Compare(property, value, Expression.LessThanOrEqual);
        }

        private static Expression RegexInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // regex(property, pattern, 'option1', 'option2', 'option3', ...) --> Regex.IsMatch(i.property, pattern, option1 | option2 | option3 | ...)
            var property = visitor.Visit(node.Arguments[0]);
            var pattern = visitor.Visit(node.Arguments[1]);
            var options = Expression.Constant(node.Arguments.Skip(2).Aggregate(RegexOptions.None, (r, n) => r | n.AsEnumIdentifier<RegexOptions>()));
            return QuerySyntaxHelper.InvokeRegex(property, pattern, options);
        }

        private static Expression MatchInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // match(arrayProperty, filter) --> Enumerable.Any(i.arrayProperty, ii => filter(ii))
            var arrayProperty = visitor.Visit(node.Arguments[0]);
            var itemType = QuerySyntaxHelper.GetCollectionItemType(arrayProperty.Type);
            var itemFilter = CreateFilterExpression(itemType, (InvocationQuerySyntaxNode)node.Arguments[1]);
            return QuerySyntaxHelper.InvokeAny(itemType, arrayProperty, itemFilter);
        }

        private static Expression AllInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // all(arrayProperty, item1, item2, item3, ...) --> Enumerable.All(new[] { item1, item2, item3, ... }, ii => Enumerable.Contains(i.arrayProperty, ii))
            return CreateFilterForArrayProperty(visitor, node, QuerySyntaxHelper.InvokeAll);
        }

        private static Expression AnyInInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // anyIn(arrayProperty, item1, item2, item3, ...) --> Enumerable.Any(new[] { item1, item2, item3, ... }, ii => Enumerable.Contains(i.arrayProperty, ii))
            return CreateFilterForArrayProperty(visitor, node, QuerySyntaxHelper.InvokeAny);
        }

        private static Expression AnyNotInInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // anyNotIn(arrayProperty, item1, item2, item3, ...) --> !Enumerable.Any(new[] { item1, item2, item3, ... }, ii => Enumerable.Contains(i.arrayProperty, ii))
            return Expression.Not(AnyInInvocation(visitor, node));
        }

        private static Expression AnyEqInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // anyEq(arrayProperty, item) --> Enumerable.Any(i.arrayProperty, ii => ii == item)
            return CreateAnyFilterForArrayProperty(visitor, node, Expression.Equal);
        }

        private static Expression AnyNotEqInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // anyNotEq(arrayProperty, item) --> !Enumerable.Any(i.arrayProperty, ii => ii == item)
            return Expression.Not(AnyEqInvocation(visitor, node));
        }

        private static Expression AnyGtInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // anyGt(arrayProperty, item) --> Enumerable.Any(i.arrayProperty, ii => ii > item)
            return CreateAnyFilterForArrayProperty(visitor, node, Expression.GreaterThan);
        }

        private static Expression AnyGteInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // anyGte(arrayProperty, item) --> Enumerable.Any(i.arrayProperty, ii => ii >= item)
            return CreateAnyFilterForArrayProperty(visitor, node, Expression.GreaterThanOrEqual);
        }

        private static Expression AnyLtInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // anyLt(arrayProperty, item) --> Enumerable.Any(i.arrayProperty, ii => ii < item)
            return CreateAnyFilterForArrayProperty(visitor, node, Expression.LessThan);
        }

        private static Expression AnyLteInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // anyLte(arrayProperty, item) --> Enumerable.Any(i.arrayProperty, ii => ii <= item)
            return CreateAnyFilterForArrayProperty(visitor, node, Expression.LessThanOrEqual);
        }

        private static Expression SizeEqInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // sizeEq(arrayProperty, value) --> Enumerable.Count(i.arrayProperty) == value
            return CreateSizeFilterForArrayProperty(visitor, node, Expression.Equal);
        }

        private static Expression SizeGtInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // sizeGt(arrayProperty, value) --> Enumerable.Count(i.arrayProperty) > value
            return CreateSizeFilterForArrayProperty(visitor, node, Expression.GreaterThan);
        }

        private static Expression SizeGteInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // sizeGte(arrayProperty, value) --> Enumerable.Count(i.arrayProperty) >= value
            return CreateSizeFilterForArrayProperty(visitor, node, Expression.GreaterThanOrEqual);
        }

        private static Expression SizeLtInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // sizeLt(arrayProperty, value) --> Enumerable.Count(i.arrayProperty) < value
            return CreateSizeFilterForArrayProperty(visitor, node, Expression.LessThan);
        }

        private static Expression SizeLteInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // sizeLte(arrayProperty, value) --> Enumerable.Count(i.arrayProperty) <= value
            return CreateSizeFilterForArrayProperty(visitor, node, Expression.LessThanOrEqual);
        }

        private static Expression DateInvocation(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node)
        {
            // date('value') --> DateTime.Parse('value')
            var dateTimeString = node.Arguments[0].AsStringLiteral();
            var dateTime = DateTime.Parse(dateTimeString);
            return Expression.Constant(dateTime);
        }


        // Helpers

        private static Expression CreateFilterForArrayProperty(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node, Func<Type, Expression, Expression, Expression> filter)
        {
            // filter(arrayProperty, item1, item2, item3, ...) --> filter(new[] { item1, item2, item3, ... }, ii => Enumerable.Contains(i.arrayProperty, ii))
            var arrayProperty = visitor.Visit(node.Arguments[0]);
            var itemType = QuerySyntaxHelper.GetCollectionItemType(arrayProperty.Type);
            var items = QuerySyntaxHelper.NewArray(itemType, node.Arguments.Skip(1).Select(visitor.Visit));
            var itemParameter = Expression.Parameter(itemType);
            var itemFilter = QuerySyntaxHelper.InvokeContains(itemType, arrayProperty, itemParameter);
            return filter(itemType, items, Expression.Lambda(itemFilter, itemParameter));
        }

        private static Expression CreateAnyFilterForArrayProperty(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node, Func<Expression, Expression, Expression> filter)
        {
            // filter(arrayProperty, item) --> Enumerable.Any(i.arrayProperty, ii => filter(ii, item))
            var arrayProperty = visitor.Visit(node.Arguments[0]);
            var item = visitor.Visit(node.Arguments[1]);
            var itemType = QuerySyntaxHelper.GetCollectionItemType(arrayProperty.Type);
            var itemParameter = Expression.Parameter(itemType);
            return QuerySyntaxHelper.InvokeAny(itemType, arrayProperty, Expression.Lambda(QuerySyntaxHelper.Compare(itemParameter, item, filter), itemParameter));
        }

        private static Expression CreateSizeFilterForArrayProperty(ExpressionFilterQuerySyntaxVisitor visitor, InvocationQuerySyntaxNode node, Func<Expression, Expression, Expression> filter)
        {
            // filter(arrayProperty, value) --> filter(Enumerable.Count(i.arrayProperty), value)
            var arrayProperty = visitor.Visit(node.Arguments[0]);
            var value = visitor.Visit(node.Arguments[1]);
            var itemType = QuerySyntaxHelper.GetCollectionItemType(arrayProperty.Type);
            return QuerySyntaxHelper.Compare(QuerySyntaxHelper.InvokeCount(itemType, arrayProperty), value, filter);
        }
    }
}