using System;

using InfinniPlatform.Core.Validation;

namespace InfinniPlatform.SystemConfig.StateMachine
{
    public interface IStateTransitionConfig
    {
        IStateTransitionConfig WithAction(Func<Action<dynamic>> stateUpdater);
        IStateTransitionConfig WithValidationError(Func<IValidationOperator> stateMoveValidator);
        IStateTransitionConfig WithValidationWarning(Func<IValidationOperator> stateMoveValidator);
        IStateTransitionConfig OnSuccess(Func<Action<dynamic>> successAction);
        IStateTransitionConfig OnFail(Func<Action<dynamic>> failAction);
        IStateTransitionConfig OnDelete(Func<Action<dynamic>> deleteAction);
        IStateTransitionConfig WithSimpleAuthorization(Func<Action<dynamic>> authorizeAction);
        IStateTransitionConfig WithComplexAuthorization(Func<Action<dynamic>> complexAuthorizeAction);
        IStateTransitionConfig OnCredentials(Func<Action<dynamic>> credentialsAction);
    }
}