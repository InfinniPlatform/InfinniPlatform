using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Validation;

namespace InfinniPlatform.SystemConfig.StateMachine
{
    public sealed class StateTransitionConfig : IStateTransitionConfig
    {
        private readonly List<Func<Action<dynamic>>> _stateUpdaters = new List<Func<Action<dynamic>>>();
        private Func<Action<dynamic>> _deleteAction;
        private Func<Action<dynamic>> _failAction;
        private Func<IValidationOperator> _stateMoveValidatorError;
        private Func<Action<dynamic>> _successAction;

        public IStateTransitionConfig WithAction(Func<Action<dynamic>> stateUpdater)
        {
            _stateUpdaters.Add(stateUpdater);
            return this;
        }

        public IStateTransitionConfig WithValidationError(Func<IValidationOperator> stateMoveValidator)
        {
            _stateMoveValidatorError = stateMoveValidator;
            return this;
        }

        public IStateTransitionConfig OnSuccess(Func<Action<dynamic>> successAction)
        {
            _successAction = successAction;
            return this;
        }

        public IStateTransitionConfig OnFail(Func<Action<dynamic>> failAction)
        {
            _failAction = failAction;
            return this;
        }

        public IStateTransitionConfig OnDelete(Func<Action<dynamic>> deleteAction)
        {
            _deleteAction = deleteAction;
            return this;
        }

        public StateTransition BuildStateTransition()
        {
            return new StateTransition(
                _stateUpdaters.Select(u => u.Invoke()).ToList(),
                _stateMoveValidatorError?.Invoke(),
                _successAction?.Invoke(),
                _failAction?.Invoke(),
                _deleteAction?.Invoke());
        }
    }
}