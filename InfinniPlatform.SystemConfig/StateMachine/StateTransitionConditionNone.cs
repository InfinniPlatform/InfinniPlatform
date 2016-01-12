namespace InfinniPlatform.SystemConfig.StateMachine
{
    public sealed class StateTransitionConditionNone : IStateTransitionCondition
    {
        public bool CanApplyFor(object state)
        {
            return state == null;
        }
    }
}