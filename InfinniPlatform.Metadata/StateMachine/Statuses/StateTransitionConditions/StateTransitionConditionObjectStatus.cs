using InfinniPlatform.Sdk.Environment.Worklow;

namespace InfinniPlatform.Metadata.StateMachine.Statuses.StateTransitionConditions
{
    public sealed class StateTransitionConditionObjectStatus : IStateTransitionCondition
    {
        private readonly object _status;

        public StateTransitionConditionObjectStatus(object status)
        {
            _status = status;
        }

        public object Status
        {
            get { return _status; }
        }

        public bool CanApplyFor(object state)
        {
            return state != null && state.Equals(Status);
        }
    }
}