using System;
using InfinniPlatform.Api.Actions;
using InfinniPlatform.Api.ContextTypes;

namespace InfinniPlatform.Metadata.StateMachine.ActionUnits.ActionOperatorBuilders
{
	public sealed class ActionOperatorScript : IActionOperator
	{
		private Action<IEventContext> _actionToExecute;

		public ActionOperatorScript(Action<IEventContext> action)
		{
			_actionToExecute = action;
		}

		public void Action(IEventContext target)
		{
			if (_actionToExecute != null)
			{
				_actionToExecute.Invoke(target);
			}
		}
	}


}