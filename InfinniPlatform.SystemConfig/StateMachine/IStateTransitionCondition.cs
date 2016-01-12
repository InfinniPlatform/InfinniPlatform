namespace InfinniPlatform.SystemConfig.StateMachine
{
    public interface IStateTransitionCondition
    {
        bool CanApplyFor(object state);
    }
}