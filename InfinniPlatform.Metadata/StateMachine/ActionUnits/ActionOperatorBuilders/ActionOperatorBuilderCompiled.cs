using System;
using InfinniPlatform.Api.Actions;

namespace InfinniPlatform.Metadata.StateMachine.ActionUnits.ActionOperatorBuilders
{
    public sealed class ActionOperatorBuilderCompiled : IActionOperatorBuilder
    {
        private readonly Type _actionUnitType;

        public ActionOperatorBuilderCompiled(Type actionUnitType)
        {
            _actionUnitType = actionUnitType;
        }

        private IActionOperator _actionInstance;

        public IActionOperator BuildActionOperator()
        {
            if (_actionInstance != null)
            {
                return _actionInstance;
            }

            var actionInstance = Activator.CreateInstance(_actionUnitType);
            if (!(actionInstance is IActionOperator))
            {
                throw new ArgumentException("registered type is not ActionUnit");
            }
            _actionInstance = (IActionOperator)actionInstance;
            return _actionInstance;
        }
    }
}
