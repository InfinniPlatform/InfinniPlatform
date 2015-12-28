using System;
using System.Collections.Generic;
using System.Reflection;

using InfinniPlatform.Runtime.Properties;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Runtime.Implementation
{
    /// <summary>
    /// Фабрика прикладных скриптов.
    /// </summary>
    internal sealed class ActionUnitFactory
    {
        private const string ActionUnitMethod = "Action";
        private const string ActionUnitPrefix = "ActionUnit";
        private const string ValidationUnitPrefix = "ValidationUnit";


        public ActionUnitFactory(IContainerResolver containerResolver)
        {
            _actionUnits = new Lazy<Dictionary<string, Action<object>>>(() => FindActionUnits(containerResolver));
        }


        private readonly Lazy<Dictionary<string, Action<object>>> _actionUnits;


        private static Dictionary<string, Action<object>> FindActionUnits(IContainerResolver containerResolver)
        {
            var result = new Dictionary<string, Action<object>>(StringComparer.OrdinalIgnoreCase);

            foreach (var type in containerResolver.Services)
            {
                if (type.IsClass && !type.IsAbstract && !type.IsGenericType
                    && (type.Name.IndexOf(ActionUnitPrefix, StringComparison.OrdinalIgnoreCase) >= 0
                        || type.Name.IndexOf(ValidationUnitPrefix, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    // TODO: Пока невозможно обойтись без Reflection из-за разнообразия контекстов

                    var methodInfo = type.GetMethod(ActionUnitMethod);

                    if (methodInfo != null)
                    {
                        // TODO: Конфигурация хранит не уникальное имя типа, нужно хранить type.FullName

                        result[type.Name] = context =>
                                            {
                                                var actionUnit = containerResolver.Resolve(type);

                                                try
                                                {
                                                    methodInfo.Invoke(actionUnit, new[] { context });
                                                }
                                                catch (TargetInvocationException e)
                                                {
                                                    throw e.InnerException ?? e;
                                                }
                                            };
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Создает прикладной скрипт.
        /// </summary>
        /// <param name="actionUnitType">Имя типа прикладного скрипта.</param>
        public Action<object> CreateActionUnit(string actionUnitType)
        {
            Action<object> actionUnit;

            if (!_actionUnits.Value.TryGetValue(actionUnitType, out actionUnit))
            {
                var error = new ArgumentOutOfRangeException(nameof(actionUnitType), Resources.ActionUnitIsNotRegistered);
                error.Data.Add("ActionUnitType", actionUnitType);
                throw error;
            }

            return actionUnit;
        }
    }
}