using System;

namespace InfinniPlatform.SystemConfig.StateMachine
{
    public interface IStateWorkflowConfig
    {
        /// <summary>
        ///     Перевести объект с использованием указанных параметров
        /// </summary>
        /// <returns></returns>
        IStateWorkflowConfig Move(Action<IStateTransitionConfig> stateTransitionConfig = null);
    }

    //public static class StateWorkflowConfigExtensions
    //{
    //	public static IStateWorkflowConfig MoveWithoutState(this IStateWorkflowConfig stateWorkflowConfig,
    //													   Action<IStateTransitionConfig> stateTransitionConfig = null)
    //	{
    //		return stateWorkflowConfig.Move(() => new StateTransitionApproverNone(), () => new StateTransitionInvalidNone(), stateTransitionConfig);
    //	}

    //}
}