using System;

using InfinniPlatform.Sdk.Environment.Actions;

namespace InfinniPlatform.SystemConfig.Metadata.StateMachine.ActionUnits
{
    public sealed class ActionUnit
    {
        private readonly IActionOperatorBuilder _actionUnitBuilder;
        private readonly string _unitId;

        public ActionUnit(string unitId, IActionOperatorBuilder actionUnitBuilder)
        {
            if (string.IsNullOrEmpty(unitId))
            {
                throw new ArgumentException("validation unit identifier should not be empty");
            }

            if (actionUnitBuilder == null)
            {
                throw new ArgumentException("validation unit builder should be specified");
            }

            _unitId = unitId;
            _actionUnitBuilder = actionUnitBuilder;
        }

        public IActionOperator ActionOperator
        {
            get { return _actionUnitBuilder.BuildActionOperator(); }
        }

        public string UnitId
        {
            get { return _unitId; }
        }
    }
}