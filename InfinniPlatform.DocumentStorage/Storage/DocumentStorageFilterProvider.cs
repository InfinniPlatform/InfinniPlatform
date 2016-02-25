using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal sealed class DocumentStorageFilterProvider : IDocumentStorageFilterProvider
    {
        public DocumentStorageFilterProvider(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }


        private readonly ITenantProvider _tenantProvider;


        public Func<IDocumentFilterBuilder, object> AddSystemFilter(Func<IDocumentFilterBuilder, object> filter)
        {
            var tenants = GetAvailableTenants();

            return f =>
                   {
                       var appFilter = filter?.Invoke(f);
                       var systemFilter = f.And(f.In("_header._tenant", tenants), f.Exists("_header._deleted", false));

                       return (appFilter != null) ? f.And(appFilter, systemFilter) : systemFilter;
                   };
        }

        public Expression<Func<TDocument, bool>> AddSystemFilter<TDocument>(Expression<Func<TDocument, bool>> filter) where TDocument : Document
        {
            var tenants = new[] { _tenantProvider.GetTenantId(), DocumentStorageHelpers.AnonymousUser };

            Expression<Func<TDocument, bool>> systemFilter = i => tenants.Contains(i._header._tenant) && i._header._deleted == null;

            return (filter != null) ? LambdaExpressionComposer.Compose(filter, systemFilter, Expression.AndAlso) : systemFilter;
        }


        private string[] GetAvailableTenants()
        {
            return new[] { _tenantProvider.GetTenantId(), DocumentStorageHelpers.AnonymousUser };
        }


        /// <summary>
        /// Предоставляет интерфейс для объединения выражений.
        /// </summary>
        private class LambdaExpressionComposer : ExpressionVisitor
        {
            private LambdaExpressionComposer(Dictionary<ParameterExpression, ParameterExpression> secondToFirstParameterMap)
            {
                _secondToFirstParameterMap = secondToFirstParameterMap ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }


            private readonly Dictionary<ParameterExpression, ParameterExpression> _secondToFirstParameterMap;


            protected override Expression VisitParameter(ParameterExpression secondParameter)
            {
                ParameterExpression firstParameter;

                if (_secondToFirstParameterMap.TryGetValue(secondParameter, out firstParameter))
                {
                    secondParameter = firstParameter;
                }

                return base.VisitParameter(secondParameter);
            }


            /// <summary>
            /// Объединяет выражения с использованием указанной функции.
            /// </summary>
            public static Expression<T> Compose<T>(Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> composeFunc)
            {
                // Таблица соответствий параметров первого и второго выражений
                var secondToFirstMap = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

                // Переименование параметров второго выражения
                var newSecondBody = new LambdaExpressionComposer(secondToFirstMap).Visit(second.Body);

                // Объединение выражений заданной операцией
                return Expression.Lambda<T>(composeFunc(first.Body, newSecondBody), first.Parameters);
            }
        }
    }
}