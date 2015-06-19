namespace InfinniPlatform.Api.Worklow
{
    public interface IStateTransitionCondition
    {
        bool CanApplyFor(object state);
    }
}