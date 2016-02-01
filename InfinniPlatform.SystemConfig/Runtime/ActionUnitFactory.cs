using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.SystemConfig.Properties;

namespace InfinniPlatform.SystemConfig.Runtime
{
    /// <summary>
    /// Фабрика прикладных скриптов.
    /// </summary>
    internal sealed class ActionUnitFactory
    {
        private const string ActionUnitPrefix = "ActionUnit";
        private const string ValidationUnitPrefix = "ValidationUnit";


        public ActionUnitFactory(IContainerResolver containerResolver)
        {
            _actionUnits = new Lazy<Dictionary<string, Action<IActionContext>>>(() => FindActionUnits(containerResolver));
        }


        private readonly Lazy<Dictionary<string, Action<IActionContext>>> _actionUnits;


        private static Dictionary<string, Action<IActionContext>> FindActionUnits(IContainerResolver containerResolver)
        {
            var result = new Dictionary<string, Action<IActionContext>>(StringComparer.OrdinalIgnoreCase);

            foreach (var type in containerResolver.Services)
            {
                if (type.IsClass && !type.IsAbstract && !type.IsGenericType
                    && (type.Name.IndexOf(ActionUnitPrefix, StringComparison.OrdinalIgnoreCase) >= 0
                        || type.Name.IndexOf(ValidationUnitPrefix, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    // TODO: Конфигурация хранит не уникальное имя типа, нужно хранить type.FullName

                    result[type.Name] = context =>
                                        {
                                            dynamic actionUnit = containerResolver.Resolve(type);

                                            actionUnit.Action(context);
                                        };
                }
            }

            return result;
        }


        /// <summary>
        /// Создает прикладной скрипт.
        /// </summary>
        /// <param name="actionUnitType">Имя типа прикладного скрипта.</param>
        public Action<IActionContext> CreateActionUnit(string actionUnitType)
        {
            Action<IActionContext> actionUnit;

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