namespace InfinniPlatform.MetadataDesigner.Views.ProcessTemplates
{
	public interface IProcessBuilder
	{
		void BuildProcess(string id, string name, string caption, int workflowType);

	    /// <summary>
	    ///   Добавить переход из состояния в состояние
	    /// </summary>
	    /// <param name="transitionName">Наименование перехода</param>
	    /// <param name="stateFrom">Начальное состояние</param>
	    /// <param name="validationPointError">Точка валидации ошибок</param>
	    /// <param name="validationPointWarning">Точка валидации предупреждений</param>
	    /// <param name="actionPoint">Точка действия</param>
	    /// <param name="successPoint">Точка успешного выполнения действия</param>
	    /// <param name="registerPoint">Точка проведения в регистры</param>
	    /// <param name="failPoint">Точка обработки ошибки действия</param>
	    /// <param name="deletePoint">Точка обработки удаления</param>
	    /// <param name="validationRuleWarning">Простая валидация предупреждений</param>
	    /// <param name="validationRuleError">Простая валидация ошибок</param>
	    /// <param name="deletingDocumentValidationRuleError">Валидация, которая запускается перед удалением документа</param>
	    /// <param name="defaultValuesSchema">Схема предзаполнения</param>
	    /// <param name="credentialsType">Тип предоставляемых credentials</param>
	    /// <param name="credentialsPoint">Точка получения пользовательских credentials</param>
	    void AddTransition(
            string transitionName, 
            string stateFrom, 
            object validationPointError, 
            object validationPointWarning, 
            object actionPoint, 
            object successPoint,
            object registerPoint, 
            object failPoint,
            object deletePoint, 
            string validationRuleWarning, 
            string validationRuleError,
            object deletingDocumentValidationRuleError,
            string defaultValuesSchema,
			string credentialsType,
			object credentialsPoint	
			);

		void DeleteTransition(string transitionName);
		void EditTransition(dynamic transition);
	}
}
