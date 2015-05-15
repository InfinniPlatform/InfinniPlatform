using System;
using InfinniPlatform.Api.Validation;

namespace InfinniPlatform.Metadata.StateMachine.ValidationUnits
{

    /// <summary>
    ///   Модуль валидации. Регистрирует в общем списке билдеры валидации.
    ///   Цель ValidationUnit - предоставить возможность регистрации модулей валидации
    ///   без указания способа их создания.
    ///   Способ инстанцирования модулей валидации определяет реализация IValidationUnitBuilder
    ///   Этот интерфейс позволяет осуществлять инстанцирование как объявленных в Design-time,
    ///   так и скриптовых модулей валидации.
    /// </summary>
    public class ValidationUnit
    {
        private readonly string _unitId;
        private readonly IValidationUnitBuilder _validationUnitBuilder;

        public ValidationUnit(string unitId, IValidationUnitBuilder validationUnitBuilder)
        {
            if (string.IsNullOrEmpty(unitId))
            {
                throw new ArgumentException("validation unit identifier should not be empty");
            }

            if (validationUnitBuilder == null)
            {
                throw  new ArgumentException("validation unit builder should be specified");
            }
 
            _unitId = unitId;
            _validationUnitBuilder = validationUnitBuilder;
        }

        public IValidationOperator ValidationOperator
        {
            get { return _validationUnitBuilder.BuildValidationUnit(); }
        }

        public string UnitId
        {
            get { return _unitId; }
        }
    }
}