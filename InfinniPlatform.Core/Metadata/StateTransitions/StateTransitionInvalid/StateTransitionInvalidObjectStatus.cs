
namespace InfinniPlatform.Metadata.StateTransitions.StateTransitionInvalid
{
	public sealed class StateTransitionInvalidObjectStatus : IStateTransitionInvalid
	{
	    public void ApplyInvalidState(dynamic target)
	    {
		    target.Status = "Invalid";
	    }
	}
}