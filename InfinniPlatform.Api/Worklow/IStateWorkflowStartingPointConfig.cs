using System;

namespace InfinniPlatform.Api.Worklow
{
	/// <summary>
	///   Конфигурация потока выполнения
	/// </summary>
	public interface IStateWorkflowStartingPointConfig
	{
		/// <summary>
		///   Инициализировать поток для указанного состояния
		/// </summary>
		/// <param name="status">Состояние</param>
		/// <param name="workflowConfigInitializer">Инициализатор</param>
		/// <returns>Конфигурация</returns>
		IStateWorkflowStartingPointConfig ForState(object status, Action<IStateWorkflowConfig> workflowConfigInitializer = null);

		/// <summary>
		///   Зарегистрировать поток без осуществления перехода в какое-либо состояние
		/// </summary>
		/// <param name="workflowConfigInitializer">Инициализатор потока</param>
		/// <returns></returns>
		IStateWorkflowStartingPointConfig FlowWithoutState(Action<IStateWorkflowConfig> workflowConfigInitializer = null);

		/// <summary>
		///   Выполнить перевод объекта из указанного состояния
		/// </summary>
		/// <param name="target">Объект, над которым осуществляется переход</param>
		/// <param name="state">Состояние объекта, из которого выполняется переход</param>
		/// <returns>результат перехода</returns>
		dynamic MoveWorkflow(dynamic target, object state);
	}
}
