using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.Api.Validation.CommonValidators;
using InfinniPlatform.SearchOptions;


namespace InfinniPlatform.Json.IndexQueryLanguage
{
	public class QueryCriteriaInterpreter
	{
		private readonly Dictionary<CriteriaType, Func<object,IPropertyValidationOperator>> _criteriaInterpreter = 
			new Dictionary<CriteriaType, Func<object, IPropertyValidationOperator>>(); 

		public QueryCriteriaInterpreter()
		{
			_criteriaInterpreter.Add(CriteriaType.IsEquals, value => new IsEqualsValidator(new TargetSelectorDefault(), value));
			_criteriaInterpreter.Add(CriteriaType.IsNotEquals, value => new IsNotValidator(new IsEqualsValidator(new TargetSelectorDefault(),value )));
			_criteriaInterpreter.Add(CriteriaType.IsEmpty, value => new IsNullOrEmptyValidator(new TargetSelectorDefault()));
			_criteriaInterpreter.Add(CriteriaType.IsNotEmpty, value => new IsNotValidator(new IsNullOrEmptyValidator(new TargetSelectorDefault())));
			_criteriaInterpreter.Add(CriteriaType.IsContains, value => new IsContainsValidator(new TargetSelectorDefault(), value != null ? value.ToString() : string.Empty));
			_criteriaInterpreter.Add(CriteriaType.IsNotContains, value => new IsNotValidator(new IsContainsValidator(new TargetSelectorDefault(), value != null ? value.ToString() : string.Empty)));
			_criteriaInterpreter.Add(CriteriaType.IsStartsWith, value => new IsStartsWithValidator(new TargetSelectorDefault(), value != null ? value.ToString() : string.Empty));
			_criteriaInterpreter.Add(CriteriaType.IsEndsWith, value => new IsEndsWithValidator(new TargetSelectorDefault(), value != null ? value.ToString() : string.Empty));
			_criteriaInterpreter.Add(CriteriaType.IsNotStartsWith, value => new IsNotValidator(new IsStartsWithValidator(new TargetSelectorDefault(), value != null ? value.ToString() : string.Empty)));
			_criteriaInterpreter.Add(CriteriaType.IsNotEndsWith, value => new IsNotValidator(new IsEndsWithValidator(new TargetSelectorDefault(), value != null ? value.ToString() : string.Empty)));

			_criteriaInterpreter.Add(CriteriaType.IsMoreThan, value => new IsMoreThanValidator(new TargetSelectorDefault(), value));
			_criteriaInterpreter.Add(CriteriaType.IsLessThan, value => new IsLessThanValidator(new TargetSelectorDefault(), value));
			_criteriaInterpreter.Add(CriteriaType.IsMoreThanOrEquals, value => new IsMoreThanOrEqualValidator(new TargetSelectorDefault(), value));
			_criteriaInterpreter.Add(CriteriaType.IsLessThanOrEquals, value => new IsLessThanOrEqualValidator(new TargetSelectorDefault(), value));

		}


		public IPropertyValidationOperator BuildOperator(Criteria criteria)
		{
			if (_criteriaInterpreter.ContainsKey(criteria.CriteriaType))
			{
				return _criteriaInterpreter[criteria.CriteriaType].Invoke(criteria.Value);
			}
			throw new ArgumentException("Указанный оператор языка запросов не поддерживается");
		} 

	}
}