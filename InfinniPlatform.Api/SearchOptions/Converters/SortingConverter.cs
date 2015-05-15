using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Dynamic;

namespace InfinniPlatform.Api.SearchOptions.Converters
{
    /// <summary>
    ///   Конвертер критериев сортировки
    /// </summary>
    public sealed class SortingConverter
    {
        private Dictionary<string,SortOrder> _sortOrders = new Dictionary<string, SortOrder>()
        {
            {"ascending",SortOrder.Ascending},
            {"descending",SortOrder.Descending}
        }; 

        private dynamic ConstructCriteria(string criteria)
        {
            //Price ascending

            //propertyName = Price
            var propertyName = criteria.Substring(0, criteria.IndexOf(" ", StringComparison.Ordinal));
            //sorting = ascending
            var sorting = criteria.Substring(criteria.IndexOf(" ", StringComparison.Ordinal)).Trim().ToLowerInvariant();

            dynamic criteriaDynamic = new DynamicWrapper();
            criteriaDynamic.Property = propertyName;

            if (!_sortOrders.ContainsKey(sorting))
            {
                throw new ArgumentException(string.Format("Can't find sort order for operator: {0}", sorting));
            }

            criteriaDynamic.SortOrder = _sortOrders[sorting];
            criteriaDynamic.PropertyName = propertyName;

            return criteriaDynamic;
        }

        /// <summary>
        ///   Получить список объекто критериев сортировки из строки
        /// </summary>
        /// <param name="sorting"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> Convert(string sorting)
        {
            var criteriaList = sorting.Split(new string[] { " and " }, StringSplitOptions.RemoveEmptyEntries);
            return criteriaList.Select(c => ConstructCriteria(c.Trim())).ToList();
        } 
    }
}
