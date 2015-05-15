using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.Metadata.StateMachine.ValidationUnits;

namespace InfinniPlatform.Metadata.Implementation.MetadataConfiguration
{
    /// <summary>
    ///   Сериализуемая конфигурация модулей валидации. 
    ///   
    /// </summary>
	public sealed class ValidationConfiguration : IValidationConfiguration
	{
        private readonly IList<ValidationUnit> _validationUnits = new List<ValidationUnit>();

	    public IList<ValidationUnit> ValidationUnits
	    {
	        get { return _validationUnits; }
	    }

	    /// <summary>
	    ///  Зарегистрировать модуль валидации
	    /// </summary>
	    /// <param name="unitIdentifier">Идентификатор метаданных валидатора</param>
	    /// <param name="validationUnitBuilder">Конструктор валидации</param>
	    public void RegisterValidationUnitEmbedded(string unitIdentifier, IValidationUnitBuilder validationUnitBuilder)
		{
            ValidationUnits.Add(new ValidationUnit(unitIdentifier, validationUnitBuilder));						
		}

	    /// <summary>
	    ///   Получить оператор валидации по указанному идентификатору
	    /// </summary>
	    /// <param name="unitIdentifier">Идентификатор валидации</param>
	    /// <returns>Оператор валидации</returns>
	    public IValidationOperator GetValidationUnit(string unitIdentifier)
        {
            return ValidationUnits
                .Where(v => v.UnitId.ToLowerInvariant() == unitIdentifier.ToLowerInvariant())
                .Select(v => v.ValidationOperator)                
                .FirstOrDefault();
        }

	}
}