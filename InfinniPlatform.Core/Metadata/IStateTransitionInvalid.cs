namespace InfinniPlatform.Core.Metadata
{
    public interface IStateTransitionInvalid
    {
        void ApplyInvalidState(dynamic target);
    }
}