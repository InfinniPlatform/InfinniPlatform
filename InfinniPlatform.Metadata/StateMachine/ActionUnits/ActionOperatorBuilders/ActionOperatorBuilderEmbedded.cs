using System;
using InfinniPlatform.Metadata.Properties;
using InfinniPlatform.Sdk.Environment.Actions;

namespace InfinniPlatform.Metadata.StateMachine.ActionUnits.ActionOperatorBuilders
{
    public sealed class ActionOperatorBuilderEmbedded : IActionOperatorBuilder
    {
        private readonly Type _actionUnitType;

        public ActionOperatorBuilderEmbedded(Type actionUnitType)
        {
            _actionUnitType = actionUnitType;
        }

        public IActionOperator BuildActionOperator()
        {
            var actionInstance = Activator.CreateInstance(_actionUnitType);

            var methodInfo = _actionUnitType.GetMethod("Action");
            if (methodInfo != null)
            {
                return new ActionOperator(_actionUnitType.Name,
                    context => methodInfo.Invoke(actionInstance, new object[] {context}));
            }

            throw new ArgumentException(Resources.ActionMethodNotSpecified);
        }
    }
}