namespace InfinniPlatform.Core.Metadata
{
    public interface IStateTransitionApprover
    {
        void ApproveState(dynamic target);
    }
}