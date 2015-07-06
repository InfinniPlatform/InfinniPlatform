namespace InfinniPlatform.Sdk.Environment.Worklow
{
    public interface IStateTransitionCondition
    {
        bool CanApplyFor(object state);
    }
}