namespace InfinniPlatform.Metadata.StateTransitions.StateTransitionApprovers
{
	public sealed class StateTransitionApproverObjectStatus : IStateTransitionApprover
	{
        private readonly string _stateTo;

        public StateTransitionApproverObjectStatus(string stateTo)
		{
		    _stateTo = stateTo;
		}

	    public void ApproveState(dynamic target)
	    {
		    target.Status = _stateTo;
			if (target.Item != null)
			{
				target.Item.Status = _stateTo;
			}
	    }
	}
}