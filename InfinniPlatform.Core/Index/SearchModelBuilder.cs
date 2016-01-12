using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Core.Index
{
    public class SearchModelBuilder
    {
        private readonly IFilterBuilder _filterFactory;
        private readonly IList<Action<SearchModel>> _modelActions = new List<Action<SearchModel>>();

        public SearchModelBuilder(IFilterBuilder filterFactory)
        {
            _filterFactory = filterFactory;
        }

        public SearchModelBuilder Filter(string searchCriteriaField, object criteriaValue)
        {
            return Filter(searchCriteriaField, criteriaValue, CriteriaType.IsEquals);
        }

        public SearchModelBuilder Filter(string searchCriteriaField, object criteriaValue, CriteriaType criteriaType)
        {
            var propertyName = searchCriteriaField;
            var propertyValue = criteriaValue;

            var criteria = _filterFactory.Get(propertyName, propertyValue, criteriaType);

            _modelActions.Add(model => model.AddFilter(criteria));
            return this;
        }

        public SearchModelBuilder OrderBy(string orderExpression, SortOrder orderValue)
        {
            var orderPropertyName = orderExpression;

            _modelActions.Add(model => model.AddSort(orderPropertyName, orderValue));
            return this;
        }

        public SearchModelBuilder FromPage(int value)
        {
            _modelActions.Add(model => model.SetFromPage(value));
            return this;
        }

        public SearchModelBuilder PageSize(int pageSize)
        {
            _modelActions.Add(model => model.SetPageSize(pageSize));
            return this;
        }

        public SearchModelBuilder Skip(int skip)
        {
            _modelActions.Add(model => model.SetSkip(skip));
            return this;
        }

        public SearchModel BuildQuery()
        {
            var searchModel = new SearchModel();
            foreach (var modelAction in _modelActions)
            {
                modelAction.Invoke(searchModel);
            }
            return searchModel;
        }
    }
}