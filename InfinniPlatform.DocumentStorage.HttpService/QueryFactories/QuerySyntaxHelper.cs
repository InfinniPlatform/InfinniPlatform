using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

using InfinniPlatform.Core.Dynamic;
using InfinniPlatform.DocumentStorage.HttpService.QuerySyntax;

namespace InfinniPlatform.DocumentStorage.HttpService.QueryFactories
{
    internal static class QuerySyntaxHelper
    {
        // Parameters
        public const string SearchParameterName = "search";
        public const string FilterParameterName = "filter";
        public const string SelectParameterName = "select";
        public const string OrderParameterName = "order";
        public const string CountParameterName = "count";
        public const string SkipParameterName = "skip";
        public const string TakeParameterName = "take";

        // Filter
        public const string NotMethodName = "not";
        public const string OrMethodName = "or";
        public const string AndMethodName = "and";
        public const string ExistsMethodName = "exists";
        public const string TypeMethodName = "type";
        public const string InMethodName = "in";
        public const string NotInMethodName = "notIn";
        public const string EqMethodName = "eq";
        public const string NotEqMethodName = "notEq";
        public const string GtMethodName = "gt";
        public const string GteMethodName = "gte";
        public const string LtMethodName = "lt";
        public const string LteMethodName = "lte";
        public const string RegexMethodName = "regex";
        public const string StartsWithMethodName = "startsWith";
        public const string EndsWithMethodName = "endsWith";
        public const string ContainsMethodName = "contains";
        public const string MatchMethodName = "match";
        public const string AllMethodName = "all";
        public const string AnyInMethodName = "anyIn";
        public const string AnyNotInMethodName = "anyNotIn";
        public const string AnyEqMethodName = "anyEq";
        public const string AnyNotEqMethodName = "anyNotEq";
        public const string AnyGtMethodName = "anyGt";
        public const string AnyGteMethodName = "anyGte";
        public const string AnyLtMethodName = "anyLt";
        public const string AnyLteMethodName = "anyLte";
        public const string SizeEqMethodName = "sizeEq";
        public const string SizeGtMethodName = "sizeGt";
        public const string SizeGteMethodName = "sizeGte";
        public const string SizeLtMethodName = "sizeLt";
        public const string SizeLteMethodName = "sizeLte";
        public const string TextMethodName = "text";
        public const string DateMethodName = "date";

        // Select
        public const string IncludeMethodName = "include";
        public const string ExcludeMethodName = "exclude";
        public const string TextScoreMethodName = "textScore";

        // Order
        public const string AscMethodName = "asc";
        public const string DescMethodName = "desc";


        static QuerySyntaxHelper()
        {
            Expression<Func<IEnumerable<object>, bool>> allExpression = items => items.All(i => true);
            Expression<Func<IEnumerable<object>, bool>> anyExpression = items => items.Any(i => true);
            Expression<Func<IEnumerable<object>, bool>> containsExpression = items => items.Contains(null);
            Expression<Func<IEnumerable<object>, int>> countExpression = items => items.Count();
            Expression<Func<string, string, RegexOptions, bool>> regexExpression = (input, pattern, options) => Regex.IsMatch(input, pattern, options);
            Expression<Func<DynamicWrapper>> newProjectionExpression = () => new DynamicWrapper();
            Expression<Action<DynamicWrapper, string, object>> addProjectionPropertyExpression = (instance, property, value) => instance.Add(property, value);

            AllMethod = ((MethodCallExpression)allExpression.Body).Method.GetGenericMethodDefinition();
            AnyMethod = ((MethodCallExpression)anyExpression.Body).Method.GetGenericMethodDefinition();
            ContainsMethod = ((MethodCallExpression)containsExpression.Body).Method.GetGenericMethodDefinition();
            CountMethod = ((MethodCallExpression)countExpression.Body).Method.GetGenericMethodDefinition();
            RegexMethod = ((MethodCallExpression)regexExpression.Body).Method;
            NewProjectionExpression = (NewExpression)newProjectionExpression.Body;
            AddProjectionPropertyMethod = ((MethodCallExpression)addProjectionPropertyExpression.Body).Method;
        }


        private static readonly MethodInfo AllMethod;
        private static readonly MethodInfo AnyMethod;
        private static readonly MethodInfo ContainsMethod;
        private static readonly MethodInfo CountMethod;
        private static readonly MethodInfo RegexMethod;
        private static readonly NewExpression NewProjectionExpression;
        private static readonly MethodInfo AddProjectionPropertyMethod;


        public static Expression Compare(Expression property, Expression value, Func<Expression, Expression, Expression> compare)
        {
            return compare(property, Convert(value, property.Type));
        }

        public static Expression NewArray(Type itemType, IEnumerable<Expression> items)
        {
            return Expression.NewArrayInit(itemType, items.Select(i => Convert(i, itemType)));
        }

        public static Expression Convert(Expression value, Type type)
        {
            return (value.Type != type) ? Expression.Convert(value, type) : value;
        }


        public static Expression InvokeAll(Type itemType, Expression items, Expression filter)
        {
            return Expression.Call(null, AllMethod.MakeGenericMethod(itemType), items, filter);
        }

        public static Expression InvokeAny(Type itemType, Expression items, Expression filter)
        {
            return Expression.Call(null, AnyMethod.MakeGenericMethod(itemType), items, filter);
        }

        public static Expression InvokeContains(Type itemType, Expression items, Expression value)
        {
            return Expression.Call(null, ContainsMethod.MakeGenericMethod(itemType), items, value);
        }

        public static Expression InvokeCount(Type itemType, Expression items)
        {
            return Expression.Call(null, CountMethod.MakeGenericMethod(itemType), items);
        }

        public static Expression InvokeRegex(Expression input, Expression pattern, Expression options)
        {
            return Expression.Call(null, RegexMethod, new[] { input, pattern, options });
        }

        public static Expression Projection(IEnumerable<Expression> properties)
        {
            var initList = new List<ElementInit>();

            foreach (var property in properties.OfType<MemberExpression>())
            {
                initList.Add(Expression.ElementInit(AddProjectionPropertyMethod, Expression.Constant(property.Member.Name), Expression.TypeAs(property, typeof(object))));
            }

            return Expression.Convert(Expression.ListInit(NewProjectionExpression, initList), typeof(object));
        }


        public static string AsIdentifierName(this IQuerySyntaxNode node)
        {
            return ((IdentifierNameQuerySyntaxNode)node).Identifier;
        }

        public static TEnum AsEnumIdentifier<TEnum>(this IQuerySyntaxNode node) where TEnum : struct
        {
            TEnum result;
            return Enum.TryParse(node.AsIdentifierName(), true, out result) ? result : default(TEnum);
        }

        public static string AsStringLiteral(this IQuerySyntaxNode node)
        {
            return (string)((LiteralQuerySyntaxNode)node).Value;
        }

        public static bool AsBooleanLiteral(this IQuerySyntaxNode node)
        {
            return (bool)((LiteralQuerySyntaxNode)node).Value;
        }


        public static Type GetCollectionItemType(Type collectionType)
        {
            var elementType = collectionType.GetElementType();

            if (elementType != null)
            {
                return elementType;
            }

            var genericArguments = collectionType.GetTypeInfo().GetGenericArguments();

            if (genericArguments.Length > 0)
            {
                return genericArguments[0];
            }

            return null;
        }
    }
}