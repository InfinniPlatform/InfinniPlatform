using System;

using InfinniPlatform.Api.Validation;

namespace InfinniPlatform.Metadata.StateMachine.ValidationUnits.ValidationUnitBuilders
{
	public sealed class ValidationOperator : IValidationOperator
	{
		private readonly Action<dynamic> _actionToExecute;

		public ValidationOperator(Action<dynamic> action)
		{
			_actionToExecute = action;
		}

		public bool Validate(dynamic validationObject, ValidationResult validationResult, string parentProperty)
		{
			_actionToExecute(validationObject);

			return validationObject.IsValid;
		}
	}
}