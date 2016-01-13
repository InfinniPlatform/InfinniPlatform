using System;
using System.Collections;
using System.Collections.Generic;

using InfinniPlatform.Core.Properties;
using InfinniPlatform.Core.Validation;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.StateMachine
{
    public sealed class StateTransition
    {
        public StateTransition(IEnumerable<Action<dynamic>> stateUpdaters,
                               IValidationOperator stateMoveValidatorError,
                               Action<dynamic> successAction = null,
                               Action<dynamic> failAction = null,
                               Action<dynamic> deleteAction = null)
        {
            _actions = new List<Action<dynamic>>(stateUpdaters);

            _stateMoveValidatorError = stateMoveValidatorError;

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


        private readonly List<Action<dynamic>> _actions;
        private readonly Action<dynamic> _failAction;
        private readonly IValidationOperator _stateMoveValidatorError;


        private static bool ProcessCriticalOperation(Action targetAction, Action<Exception> catchAction)
        {
            try
            {
                targetAction();
                return true;
            }
            catch (Exception e)
            {
                catchAction(e);
            }

            return false;
        }


        public bool ApplyTransition(dynamic target)
        {
            target.IsValid = true;

            // предварительно предзаполняем экземпляр результата валидации
            dynamic validationMessage = new DynamicWrapper();
            validationMessage.ValidationErrors = new DynamicWrapper();
            validationMessage.ValidationErrors.IsValid = true;
            validationMessage.ValidationErrors.Message = string.Empty;

            var itemsErrors = new List<dynamic>();
            var isValidError = true;

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
                                                              target.ValidationMessage.GetType() != typeof(string))
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

            target.ValidationMessage = validationMessage;
            target.ValidationMessage.ValidationErrors.IsValid = isValidError;
            target.ValidationMessage.ValidationErrors.Message = itemsErrors;
            target.IsValid = isValidError;

            // при успешном прохождении валидации выполняем действия
            if (target.IsValid)
            {
                Action<dynamic> currentAction = null;

                return ProcessCriticalOperation(
                    () =>
                    {
                        foreach (var stateUpdater in _actions)
                        {
                            currentAction = stateUpdater;
                            stateUpdater(target);
                        }
                    },
                    e => FillValidationMessage(string.IsNullOrEmpty(e.Message) ? e.ToString() : e.Message, target));
            }

            if (_failAction != null)
            {
                _failAction(target);
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