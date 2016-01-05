using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;

using InfinniPlatform.Core.Properties;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Actions;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.SystemConfig.Metadata.StateMachine.Statuses
{
	public sealed class StateTransition
	{
		private readonly IList<IActionOperator> _actions;
		private readonly IActionOperator _failAction;
		private readonly IValidationOperator _stateMoveValidatorError;
		private readonly IValidationOperator _stateMoveValidatorWarning;
		private readonly IValidationOperator _stateMoveValidatorErrorSimple;
		private readonly IValidationOperator _stateMoveValidatorWarningSimple;
		private readonly IActionOperator _authorizeAction;
		private readonly IActionOperator _complexAuthorizeAction;
		private readonly IActionOperator _credentialsAction;

		public StateTransition(IEnumerable<IActionOperator> stateUpdaters,
			IValidationOperator stateMoveValidatorError,
			IValidationOperator stateMoveValidatorWarning,
			IValidationOperator stateMoveValidatorErrorSimple,
			IValidationOperator stateMoveValidatorWarningSimple,
			IActionOperator successAction = null,
			IActionOperator failAction = null,
			IActionOperator deleteAction = null,
			IActionOperator authorizeAction = null,
			IActionOperator complexAuthorizeAction = null,
			IActionOperator credentialsAction = null
			)
		{
			_actions = new List<IActionOperator>(stateUpdaters);
			_authorizeAction = authorizeAction;
			_complexAuthorizeAction = complexAuthorizeAction;
			_credentialsAction = credentialsAction;
			_stateMoveValidatorError = stateMoveValidatorError;
			_stateMoveValidatorWarning = stateMoveValidatorWarning;
			_stateMoveValidatorWarningSimple = stateMoveValidatorWarningSimple;
			_stateMoveValidatorErrorSimple = stateMoveValidatorErrorSimple;

			if (successAction != null)
			{
				_actions.Add(successAction);
			}

			if (deleteAction != null)
			{
				_actions.Add(deleteAction);
			}

			_failAction = failAction;
		}


		/// <summary>
		///   Выполнить действие в защищенном режиме
		/// </summary>
		/// <returns>Признак успешного выполнения операции</returns>
		private static bool ProcessCriticalOperation(Action targetAction, Action<Exception> catchAction)
		{
			ExceptionDispatchInfo exceptionDispatchInfo;

			try
			{
				targetAction();
				return true;
			}
			catch (Exception e)
			{
				exceptionDispatchInfo = ExceptionDispatchInfo.Capture(e);
			}

			try
			{
				// Capture an exception and re-throw it without changing the stack-trace
				exceptionDispatchInfo.Throw();
			}
			catch (Exception restoredException)
			{
				// Пропускаем все неинформативные исключения типа TargetInvocationException
				while (true)
				{
					if (restoredException is TargetInvocationException)
					{
						restoredException = restoredException.InnerException;
						continue;
					}

					break;
				}

				catchAction(restoredException);
			}

			return false;
		}

		/// <summary>
		///   Выполняем переход в указанное состояние.
		///   Перед выполнением перехода выполняем проверку, если она указана
		///   Если проверка не пройдена, документ не переводится в нужное состояние,
		///   а со списком ошибок возвращается клиенту класса
		/// </summary>
		/// <param name="target"></param>
		internal bool ApplyTransition(dynamic target)
		{
			target.IsValid = true;

			//предварительно предзаполняем экземпляр результата валидации
			dynamic validationMessage = new DynamicWrapper();
			validationMessage.ValidationWarnings = new DynamicWrapper();
			validationMessage.ValidationWarnings.IsValid = true;
			validationMessage.ValidationWarnings.Message = string.Empty;

			validationMessage.ValidationErrors = new DynamicWrapper();
			validationMessage.ValidationErrors.IsValid = true;
			validationMessage.ValidationErrors.Message = string.Empty;

			var itemsWarnings = new List<dynamic>();
			var itemsErrors = new List<dynamic>();
			var isValidWarning = true;
			var isValidError = true;
			var ignoreWarnings = ObjectHelper.GetProperty(target, "Item") != null && target.Item.IgnoreWarnings == true;

			//вызов установленного обработчика credentials пользователя
			if (_credentialsAction != null)
			{
				if(!ProcessCriticalOperation(() =>
					                             {
						                             _credentialsAction.Action(target);

													  if (!target.IsValid)
													  {
														  itemsErrors.Add(target.ValidationMessage);
														  isValidError = target.IsValid;													      
													  }
					                             }, e => FillValidationMessage(string.Format("Credentials action failed: {0}", e),target)))
				{
					return false;
				}
			}

			//проверка простой авторизации доступа
			if (_authorizeAction != null)
			{
				if (!ProcessCriticalOperation(() =>
												  {
													  _authorizeAction.Action(target);

													  if (!target.IsValid)
													  {
														  itemsErrors.Add(target.ValidationMessage);
														  isValidError = target.IsValid;													      
													  }

												  }, e => FillValidationMessage(string.Format("Authorization action failed: {0}", e), target)))
				{
					return false;
				}
			}

			//проверка комплексной авторизации доступа
			if (_complexAuthorizeAction != null)
			{
				if (!ProcessCriticalOperation(() =>
				{
					_complexAuthorizeAction.Action(target);

					if (!target.IsValid)
					{
						itemsErrors.Add(target.ValidationMessage);
						isValidError = target.IsValid;
					}

				}, e => FillValidationMessage(string.Format("Complex authorization action failed: {0}", e), target)))
				{
					return false;
				}
			}

            //если успешно прошли авторизацию, выполняем валидацию
		    if (target.IsValid)
		    {


		        // проверка простой клиентской валидации предупреждений
		        if (_stateMoveValidatorWarningSimple != null && !ignoreWarnings)
		        {
		            if (!ProcessCriticalOperation(() =>
		                {
		                    var validationResult = new ValidationResult();

		                    _stateMoveValidatorWarningSimple.Validate(target, validationResult);

		                    itemsWarnings.AddRange(validationResult.Items);
		                    isValidWarning = validationResult.IsValid;

		                },
		                                          e =>
		                                          FillValidationMessage(
		                                              string.Format(Resources.WarningsValidationFailedWithException, e),
		                                              target)))
		            {
		                return false;
		            }
		        }

		        //проверка простой клиентской валидации ошибок
		        if (_stateMoveValidatorErrorSimple != null)
		        {
		            if (!ProcessCriticalOperation(() =>
		                {
		                    var validationResult = new ValidationResult();
		                    _stateMoveValidatorErrorSimple.Validate(target, validationResult);

		                    itemsErrors.AddRange(validationResult.Items);
		                    isValidError = validationResult.IsValid;

		                }, e => FillValidationMessage(string.Format(Resources.ErrorsValidationFailedWithException, e), target)))
		            {
		                return false;
		            }
		        }

		        //проверка комплексной серверной валидации предупреждений
                if (_stateMoveValidatorWarning != null && !ignoreWarnings)
		        {
		            if (!ProcessCriticalOperation(() =>
		                {
		                    var validationResult = new ValidationResult();

		                    _stateMoveValidatorWarning.Validate(target, validationResult);

                            //TODO: необходим рефакторинг для приведения к единой схеме валидации с использованием validationResult
		                    if (target.ValidationMessage != null && target.ValidationMessage.ToString() != string.Empty)
		                    {
		                        if (target.ValidationMessage is IEnumerable &&
		                            target.ValidationMessage.GetType() != typeof (string))
		                        {
		                            itemsWarnings.AddRange(target.ValidationMessage);
		                        }
		                        else
		                        {
		                            itemsWarnings.Add(target.ValidationMessage);
		                        }
		                    }
		                    isValidWarning = isValidWarning && target.IsValid;
		                },
		                                          e =>
		                                          FillValidationMessage(
		                                              string.Format(Resources.ServerWarningsValidationFailedWithException, e),
		                                              target)))
		            {
		                return false;
		            }
		        }

		        //проверка комплексной валидации ошибок
		        if (_stateMoveValidatorError != null)
		        {
		            if (!ProcessCriticalOperation(() =>
		                {
		                    var validationResult = new ValidationResult();
		                    _stateMoveValidatorError.Validate(target, validationResult);

		                    if (target.ValidationMessage != null && target.ValidationMessage.ToString() != string.Empty)
		                    {
		                        if (target.ValidationMessage is IEnumerable &&
		                            target.ValidationMessage.GetType() != typeof (string))
		                        {
		                            itemsErrors.AddRange(target.ValidationMessage);
		                        }
		                        else
		                        {
		                            itemsErrors.Add(target.ValidationMessage);
		                        }
		                    }
		                    isValidError = isValidError && target.IsValid;

		                },
		                                          e =>
		                                          FillValidationMessage(
		                                              string.Format("Server errors validation failed with exception: {0}", e),
		                                              target)))
		            {
		                return false;
		            }
		        }
		    }

		    target.ValidationMessage = validationMessage;
			target.ValidationMessage.ValidationErrors.IsValid = isValidError;
			target.ValidationMessage.ValidationErrors.Message = itemsErrors;
			target.ValidationMessage.ValidationWarnings.Message = itemsWarnings;
			target.ValidationMessage.ValidationWarnings.IsValid = isValidWarning;
			target.IsValid = isValidError && isValidWarning;

			// при успешном прохождении валидации выполняем действия
			if (target.IsValid)
			{
				IActionOperator currentAction = null;

				return ProcessCriticalOperation(
					() =>
					{
						foreach (var stateUpdater in _actions)
						{
							currentAction = stateUpdater;
							stateUpdater.Action(target);
						}
					},
					e => FillValidationMessage(string.IsNullOrEmpty(e.Message) ? e.ToString() : e.Message, target));
			}

			if (_failAction != null)
			{
				_failAction.Action(target);
			}

			return false;
		}

		private static void FillValidationMessage(string message, dynamic target)
		{
			target.ValidationMessage = new DynamicWrapper();
			// MC-3582 fix  target.IsInternalServerError = true;
			target.ValidationMessage.ValidationErrors = new DynamicWrapper();
			target.ValidationMessage.ValidationErrors.Message = message;
			target.ValidationMessage.ValidationErrors.IsValid = false;
			target.IsValid = false;
		}
	}
}