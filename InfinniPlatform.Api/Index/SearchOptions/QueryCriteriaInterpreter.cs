using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.Api.Validation.BooleanValidators;
using InfinniPlatform.Api.Validation.ObjectValidators;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Index.SearchOptions
{
    /// <summary>
    ///     Интерпретатор критериев запроса
    /// </summary>
    public sealed class QueryCriteriaInterpreter
    {
        private readonly Dictionary<CriteriaType, Func<object, IValidationOperator>> _criteriaInterpreter =
            new Dictionary<CriteriaType, Func<object, IValidationOperator>>();

        public QueryCriteriaInterpreter()
        {
            _criteriaInterpreter.Add(CriteriaType.IsEquals, value => new EqualValidator {Value = value});
            _criteriaInterpreter.Add(CriteriaType.IsNotEquals,
                value => new NotValidator {Operator = new EqualValidator {Value = value}});

            _criteriaInterpreter.Add(CriteriaType.IsEmpty, value => new NullOrEmptyValidator());
            _criteriaInterpreter.Add(CriteriaType.IsNotEmpty,
                value => new NotValidator {Operator = new NullOrEmptyValidator()});

            _criteriaInterpreter.Add(CriteriaType.IsContains,
                value => new ContainsValidator {Value = ToStringOrEmpty(value)});
            _criteriaInterpreter.Add(CriteriaType.IsNotContains,
                value => new NotValidator {Operator = new ContainsValidator {Value = ToStringOrEmpty(value)}});

            _criteriaInterpreter.Add(CriteriaType.IsStartsWith,
                value => new StartsWithValidator {Value = ToStringOrEmpty(value)});
            _criteriaInterpreter.Add(CriteriaType.IsNotStartsWith,
                value => new NotValidator {Operator = new StartsWithValidator {Value = ToStringOrEmpty(value)}});

            _criteriaInterpreter.Add(CriteriaType.IsEndsWith,
                value => new EndsWithValidator {Value = ToStringOrEmpty(value)});
            _criteriaInterpreter.Add(CriteriaType.IsNotEndsWith,
                value => new NotValidator {Operator = new EndsWithValidator {Value = ToStringOrEmpty(value)}});

            _criteriaInterpreter.Add(CriteriaType.IsMoreThan, value => new MoreThanValidator {Value = value});
            _criteriaInterpreter.Add(CriteriaType.IsMoreThanOrEquals,
                value => new MoreThanOrEqualValidator {Value = value});

            _criteriaInterpreter.Add(CriteriaType.IsLessThan, value => new LessThanValidator {Value = value});
            _criteriaInterpreter.Add(CriteriaType.IsLessThanOrEquals,
                value => new LessThanOrEqualValidator {Value = value});

            _criteriaInterpreter.Add(CriteriaType.IsIdIn,
                value => new InValidator {Items = (value as IEnumerable).Cast<string>()});
        }

        private static string ToStringOrEmpty(object value)
        {
            return (value != null) ? value.ToString() : string.Empty;
        }

        public IValidationOperator BuildOperator(Criteria criteria)
        {
            if (_criteriaInterpreter.ContainsKey(criteria.CriteriaType))
            {
                return _criteriaInterpreter[criteria.CriteriaType].Invoke(criteria.Value);
            }

            throw new ArgumentException("Указанный оператор языка запросов не поддерживается");
        }

        public IEnumerable<dynamic> ApplyFilter(IEnumerable<dynamic> filter, IEnumerable<dynamic> result)
        {
            var criteriaList = filter.Select(f => new Criteria
            {
                CriteriaType = (CriteriaType) f.CriteriaType,
                Property = f.Property,
                Value = f.Value
            }).Select(c => new
            {
                Operator = BuildOperator(c),
                Criteria = c
            }).ToList();

            return
                result.Where(
                    r => criteriaList.All(c => c.Operator.Validate(ObjectHelper.GetProperty(r, c.Criteria.Property))))
                    .ToList();
        }
    }
}