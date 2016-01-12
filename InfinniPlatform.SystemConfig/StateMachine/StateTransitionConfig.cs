using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Properties;
using InfinniPlatform.Core.Validation;

namespace InfinniPlatform.SystemConfig.StateMachine
{
    public sealed class StateTransitionConfig : IStateTransitionConfig
    {
        private readonly List<Func<Action<dynamic>>> _stateUpdaters = new List<Func<Action<dynamic>>>();
        private Func<Action<dynamic>> _authorizeAction;
        private Func<Action<dynamic>> _complexAuthorizeAction;
        private Func<Action<dynamic>> _credentialsAction;
        private Func<Action<dynamic>> _deleteAction;
        private Func<Action<dynamic>> _failAction;
        private Func<IValidationOperator> _stateMoveValidatorError;
        private Func<IValidationOperator> _stateMoveValidatorSimpleError;
        private Func<IValidationOperator> _stateMoveValidatorSimpleWarning;
        private Func<IValidationOperator> _stateMoveValidatorWarning;
        private Func<Action<dynamic>> _successAction;

        public IStateTransitionConfig WithAction(Func<Action<dynamic>> stateUpdater)
        {
            if (stateUpdater == null)
            {
                throw new ArgumentException(Resources.StateUpdaterShouldNotBeEmpty);
            }
            _stateUpdaters.Add(stateUpdater);
            return this;
        }

        public IStateTransitionConfig WithValidationError(Func<IValidationOperator> stateMoveValidator)
        {
            if (stateMoveValidator == null)
            {
                throw new ArgumentException(Resources.StateValidatorShouldNotBeEmpty);
            }
            _stateMoveValidatorError = stateMoveValidator;
            return this;
        }

        public IStateTransitionConfig WithValidationWarning(Func<IValidationOperator> stateMoveValidator)
        {
            if (stateMoveValidator == null)
            {
                throw new ArgumentException(Resources.StateValidatorShouldNotBeEmpty);
            }
            _stateMoveValidatorWarning = stateMoveValidator;
            return this;
        }

        public IStateTransitionConfig WithSimpleAuthorization(Func<Action<dynamic>> authorizeAction)
        {
            if (authorizeAction == null)
            {
                throw new ArgumentException(Resources.AuthorizationPointShouldNotBeEmpty);
            }
            _authorizeAction = authorizeAction;
            return this;
        }

        public IStateTransitionConfig WithComplexAuthorization(Func<Action<dynamic>> complexAuthorizeAction)
        {
            if (complexAuthorizeAction == null)
            {
                throw new ArgumentException(Resources.ComplexAuthorizationShouldNotBeEmpty);
            }
            _complexAuthorizeAction = complexAuthorizeAction;
            return this;
        }

        public IStateTransitionConfig OnSuccess(Func<Action<dynamic>> successAction)
        {
            if (successAction == null)
            {
                throw new ArgumentException(Resources.SuccessActionShouldNotBeEmpty);
            }

            _successAction = successAction;
            return this;
        }

        public IStateTransitionConfig OnFail(Func<Action<dynamic>> failAction)
        {
            if (failAction == null)
            {
                throw new ArgumentException(Resources.FailActionShouldNotBeEmpty);
            }

            _failAction = failAction;
            return this;
        }

        public IStateTransitionConfig OnDelete(Func<Action<dynamic>> deleteAction)
        {
            if (deleteAction == null)
            {
                throw new ArgumentException(Resources.DeleteActionShouldNotBeEmpty);
            }

            _deleteAction = deleteAction;
            return this;
        }

        public IStateTransitionConfig OnCredentials(Func<Action<dynamic>> credentialsAction)
        {
            if (credentialsAction == null)
            {
                throw new ArgumentException(Resources.CredentialsActionShouldNotBeEmpty);
            }

            _credentialsAction = credentialsAction;
            return this;
        }

        internal StateTransition BuildStateTransition()
        {
            return new StateTransition(
                _stateUpdaters.Select(u => u.Invoke()).ToList(),
                _stateMoveValidatorError != null ? _stateMoveValidatorError() : null,
                _stateMoveValidatorWarning != null ? _stateMoveValidatorWarning() : null,
                _stateMoveValidatorSimpleError != null ? _stateMoveValidatorSimpleError() : null,
                _stateMoveValidatorSimpleWarning != null ? _stateMoveValidatorSimpleWarning() : null,
                _successAction != null ? _successAction() : null,
                _failAction != null ? _failAction() : null,
                _deleteAction != null ? _deleteAction() : null,
                _authorizeAction != null ? _authorizeAction() : null,
                _complexAuthorizeAction != null ? _complexAuthorizeAction() : null,
                _credentialsAction != null ? _credentialsAction() : null);
        }
    }
}