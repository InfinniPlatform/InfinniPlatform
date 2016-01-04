using InfinniPlatform.Sdk.Environment.Worklow;

namespace InfinniPlatform.SystemConfig.Metadata.StateMachine.Statuses.StateTransitionConditions
{
    public sealed class StateTransitionConditionNone : IStateTransitionCondition
    {
        public bool CanApplyFor(object state)
        {
            return state == null;
        }
    }
}