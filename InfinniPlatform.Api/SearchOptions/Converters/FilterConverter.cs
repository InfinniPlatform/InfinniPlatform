using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Api.SearchOptions.Converters
{
    /// <summary>
    ///   Конвертор фильтра в виде строки в список объектов
    /// </summary>
    public sealed class FilterConverter
    {
        private readonly Dictionary<string,CriteriaType> _criteriaTypes = new Dictionary<string, CriteriaType>()
        {
            {"isequals",CriteriaType.IsEquals},
            {"isnotequals",CriteriaType.IsNotEquals},
            {"ismorethan",CriteriaType.IsMoreThan},
            {"islessthan",CriteriaType.IsLessThan},
            {"ismorethanorequals",CriteriaType.IsMoreThanOrEquals},
            {"islessthanorequals",CriteriaType.IsLessThanOrEquals},
            {"iscontains",CriteriaType.IsContains},
            {"isnotcontains",CriteriaType.IsNotContains},
            {"isempty",CriteriaType.IsEmpty},
            {"isnotempty",CriteriaType.IsNotEmpty},
            {"isstartswith",CriteriaType.IsStartsWith},
            {"isnotstartswith",CriteriaType.IsNotStartsWith},
            {"isendswith",CriteriaType.IsEndsWith},
            {"isnotendswith",CriteriaType.IsNotEndsWith},
            {"fulltextsearch",CriteriaType.FullTextSearch},
            {"valueset",CriteriaType.IsIn},
            {"isidin",CriteriaType.IsIdIn}
        };

        private readonly Dictionary<string, Func<string, object>> _criteriaProcessing = new Dictionary
            <string, Func<string, object>>()
        {
            {
                "isidin",
                (criteriaValue) => criteriaValue.Split(new string[] {"[,]"}, StringSplitOptions.RemoveEmptyEntries)
            }
        };

        private dynamic ConstructCriteria(string criteria)
        {
            //Price IsEquals 200

            //propertyName = Price
            var propertyName = criteria.Substring(0, criteria.IndexOf(" ", StringComparison.Ordinal));
            //criteria = IsEquals 200
            criteria = criteria.Substring(criteria.IndexOf(" ", StringComparison.Ordinal)).Trim();
            //op = IsEquals
            var opIndex = criteria.IndexOf(" ", StringComparison.Ordinal);
            
            object valueProcessed = null;
            var value = string.Empty;
            dynamic criteriaDynamic = new DynamicWrapper();
            criteriaDynamic.Property = propertyName;
            
            string op = string.Empty;
            if (opIndex == -1)
            {
                op = criteria.Substring(0).ToLowerInvariant();
            }
            else
            {
                op = criteria.Substring(0, opIndex).ToLowerInvariant();

                //criteria = 200
                var valueIndex = criteria.IndexOf(" ", StringComparison.Ordinal) + 1;

                
                if (valueIndex != -1)
                {
                    value = criteria.Substring(valueIndex);
                }
               
                if (value != string.Empty && _criteriaProcessing.ContainsKey(op))
                {
                    valueProcessed = _criteriaProcessing[op].Invoke(value);
                }
            }

            if (!_criteriaTypes.ContainsKey(op))
            {
                throw new ArgumentException(string.Format("Can't find criteria type for operator: {0}", op));
            }
            criteriaDynamic.CriteriaType = _criteriaTypes[op];
            criteriaDynamic.Value = valueProcessed ??  (value == "null" ? null : value);
            criteriaDynamic.Property = propertyName;

            return criteriaDynamic;
        }

        /// <summary>
        ///   Получить набор критериев фильтрации из строки
        /// </summary>
        /// <param name="filter">Строка фильтра</param>
        /// <returns>Список критериев</returns>
        public IEnumerable<dynamic> Convert(string filter)
        {
            var criteriaList = filter.Split(new string[] {" and "}, StringSplitOptions.RemoveEmptyEntries);

            return criteriaList.Select(c => ConstructCriteria(c.Trim())).ToList();
        }


        public IEnumerable<dynamic> ConvertToInternal(Action<Sdk.FilterBuilder> filter)
        {

            var filterBuilder = new Sdk.FilterBuilder();
            if (filter != null)
            {
                filter.Invoke(filterBuilder);
                var filterCriteriaStrings = filterBuilder.GetFilter();
                return filterCriteriaStrings.Select(c => ConstructCriteria(c.Trim())).ToList();
            }
            return new List<dynamic>();

        } 


    }
}
