using System;
using InfinniPlatform.Api.Actions;
using InfinniPlatform.Api.Validation;

namespace InfinniPlatform.Api.Worklow
{
	public interface IStateTransitionConfig
	{
		IStateTransitionConfig WithAction(Func<IActionOperator> stateUpdater);
		IStateTransitionConfig WithValidationError(Func<IValidationOperator> stateMoveValidator);
		IStateTransitionConfig WithValidationWarning(Func<IValidationOperator> stateMoveValidator);
		IStateTransitionConfig OnSuccess(Func<IActionOperator> successAction);
		IStateTransitionConfig OnFail(Func<IActionOperator> failAction);
        IStateTransitionConfig OnDelete(Func<IActionOperator> deleteAction);
		IStateTransitionConfig WithSimpleAuthorization(Func<IActionOperator> authorizeAction);
		IStateTransitionConfig WithComplexAuthorization(Func<IActionOperator> complexAuthorizeAction);
		IStateTransitionConfig WithValidationErrorSimple(Func<IValidationOperator> stateMoveValidator);
		IStateTransitionConfig WithValidationWarningSimple(Func<IValidationOperator> stateMoveValidator);
		IStateTransitionConfig OnCredentials(Func<IActionOperator> credentialsAction);
	}
}
