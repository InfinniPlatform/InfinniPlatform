using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Properties;
using InfinniPlatform.Sdk.Environment.Actions;
using InfinniPlatform.Sdk.Environment.Validations;
using InfinniPlatform.Sdk.Environment.Worklow;

namespace InfinniPlatform.SystemConfig.StateMachine.Statuses
{
	public sealed class StateTransitionConfig : IStateTransitionConfig
	{
		private readonly List<Func<IActionOperator>> _stateUpdaters = new List<Func<IActionOperator>>();
		private Func<IValidationOperator> _stateMoveValidatorError;
		private Func<IValidationOperator> _stateMoveValidatorWarning;
		private Func<IValidationOperator> _stateMoveValidatorSimpleWarning;
		private Func<IValidationOperator> _stateMoveValidatorSimpleError;
        private Func<IActionOperator> _successAction;
		private Func<IActionOperator> _failAction;
        private Func<IActionOperator> _deleteAction;
		private Func<IActionOperator> _authorizeAction;
		private Func<IActionOperator> _complexAuthorizeAction;
		private Func<IActionOperator> _credentialsAction;


		public IStateTransitionConfig WithAction(Func<IActionOperator> stateUpdater)
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

	    public IStateTransitionConfig WithDeletingDocumentValidationError(Func<IValidationOperator> stateMoveValidator)
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


		public IStateTransitionConfig WithSimpleAuthorization(Func<IActionOperator> authorizeAction)
		{
			if (authorizeAction == null)
			{
				throw new ArgumentException(Resources.AuthorizationPointShouldNotBeEmpty);
			}
			_authorizeAction = authorizeAction;
			return this;
		}

		public IStateTransitionConfig WithComplexAuthorization(Func<IActionOperator> complexAuthorizeAction)
		{
			if (complexAuthorizeAction == null)
			{
				throw new ArgumentException(Resources.ComplexAuthorizationShouldNotBeEmpty);
			}
			_complexAuthorizeAction = complexAuthorizeAction;
			return this;
		}

		public IStateTransitionConfig WithValidationErrorSimple(Func<IValidationOperator> stateMoveValidator)
		{
			if (stateMoveValidator == null)
			{
				throw new ArgumentException(Resources.StateValidatorShouldNotBeEmpty);
			}
			_stateMoveValidatorSimpleError = stateMoveValidator;
			return this;
		}

		public IStateTransitionConfig WithValidationWarningSimple(Func<IValidationOperator> stateMoveValidator)
		{
			if (stateMoveValidator == null)
			{
				throw new ArgumentException(Resources.StateValidatorShouldNotBeEmpty);
			}
			_stateMoveValidatorSimpleWarning = stateMoveValidator;
			return this;
		}



		public IStateTransitionConfig OnSuccess(Func<IActionOperator> successAction)
		{
			if (successAction == null)
			{
				throw new ArgumentException(Resources.SuccessActionShouldNotBeEmpty);
			}

			_successAction = successAction;
			return this;
		}


		public IStateTransitionConfig OnFail(Func<IActionOperator> failAction)
		{
			if (failAction == null)
			{
				throw new ArgumentException(Resources.FailActionShouldNotBeEmpty);
			}

			_failAction = failAction;
			return this;
		}

        public IStateTransitionConfig OnDelete(Func<IActionOperator> deleteAction)
        {
            if (deleteAction == null)
            {
                throw new ArgumentException(Resources.DeleteActionShouldNotBeEmpty);
            }

            _deleteAction = deleteAction;
            return this;
        }

		public IStateTransitionConfig OnCredentials(Func<IActionOperator> credentialsAction)
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