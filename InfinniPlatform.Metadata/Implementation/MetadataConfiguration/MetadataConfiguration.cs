using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Logging;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Scripts;
using InfinniPlatform.Sdk.Environment.Worklow;

namespace InfinniPlatform.Metadata.Implementation.MetadataConfiguration
{
    /// <summary>
    ///     Метаданные конфигурации
    /// </summary>
    public class MetadataConfiguration : IMetadataConfiguration
    {
        private readonly IList<MetadataContainer> _containers = new List<MetadataContainer>();

        /// <summary>
        ///     Является ли конфигурация встроенной в код C#
        /// </summary>
        private readonly bool _isEmbeddedConfiguration;

        private readonly List<dynamic> _menuList = new List<dynamic>();
        private readonly List<dynamic> _registers = new List<dynamic>();
        private readonly IScriptConfiguration _scriptConfiguration;
        private readonly IServiceRegistrationContainer _serviceRegistrationContainer;
        private readonly IServiceTemplateConfiguration _serviceTemplateConfiguration;

        public MetadataConfiguration(
            IScriptConfiguration scriptConfiguration,
            IServiceRegistrationContainer serviceRegistrationContainer,
            IServiceTemplateConfiguration serviceTemplateConfiguration,
            bool isEmbeddedConfiguration)
        {
            _scriptConfiguration = scriptConfiguration;

            _serviceRegistrationContainer = serviceRegistrationContainer;

            _serviceTemplateConfiguration = serviceTemplateConfiguration;

            _isEmbeddedConfiguration = isEmbeddedConfiguration;
        }

		private MetadataContainer RegisterContainer(string containerId)
		{
			var metadata = new MetadataContainer(containerId);
			metadata.ContainerId = containerId;
			_containers.Add(metadata);
			return metadata;
		}

		private MetadataContainer GetMetadataContainer(string containerId)
		{
			return _containers.FirstOrDefault(c => c.ContainerId.ToLowerInvariant() == containerId.ToLowerInvariant()) ?? RegisterContainer(containerId);
		}


        /// <summary>
        ///     Идентификатор конфигурации
        /// </summary>
        public string ConfigurationId { get; set; }

        /// <summary>
        ///     Конфигурация прикладных скриптов
        /// </summary>
        public IScriptConfiguration ScriptConfiguration
        {
            get { return _scriptConfiguration; }
        }

        /// <summary>
        ///     Контейнер регистрации сервисов конфигурации
        /// </summary>
        public IServiceRegistrationContainer ServiceRegistrationContainer
        {
            get { return _serviceRegistrationContainer; }
        }

        /// <summary>
        ///     Контейнер шаблонов обработчиков для модуля
        /// </summary>
        public IServiceTemplateConfiguration ServiceTemplateConfiguration
        {
            get { return _serviceTemplateConfiguration; }
        }

        /// <summary>
        ///     Актуальная версия конфигурации метаданных
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        ///     Признак того, что конфигурация является встроенной в код C# (не хранится в JSON)
        ///     Например, конфигурация "Справочники НСИ" является встроенной в код.
        ///     Это важно для некоторых операций, в частности операций доступа к метаданным конфигурации
        /// </summary>
        public bool IsEmbeddedConfiguration
        {
            get { return _isEmbeddedConfiguration; }
        }

        /// <summary>
        ///     Список индексов метаданных
        /// </summary>
        public IEnumerable<string> Containers
        {
            get { return _containers.Select(c => c.ContainerId).ToList(); }
        }

        /// <summary>
        ///     Выполнить указанный поток работы для указанных метаданных
        /// </summary>
        /// <param name="containerId">Метаданные контейнера</param>
        /// <param name="workflowId">Идентификатор потока</param>
        /// <param name="target">Объект, над которым выполняется переход</param>
        /// <param name="state">Состояние, в которое выполняется перевод</param>
        /// <returns>Результат выполнения потока</returns>
        public dynamic MoveWorkflow(string containerId, string workflowId, dynamic target, object state = null)
        {
            return GetMetadataContainer(containerId).MoveWorkflow(workflowId, target, state);
        }

        /// <summary>
        ///     Зарегистрировать поток выполнения
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="workflowId">Идентификатор потока работы</param>
        /// <param name="actionConfiguration">Конфигурация выполняемых действий</param>
        public void RegisterWorkflow(string containerId, string workflowId,
            Action<IStateWorkflowStartingPointConfig> actionConfiguration)
        {
            GetMetadataContainer(containerId).RegisterWorkflow(workflowId, actionConfiguration);
        }

        /// <summary>
        ///     Зарегистрировать список меню
        /// </summary>
        /// <param name="menuList">список меню</param>
        public void RegisterMenu(IEnumerable<dynamic> menuList)
        {
            _menuList.AddRange(menuList);
        }

        public void RegisterDocument(string containerId)
        {
            RegisterContainer(containerId);
        }

        public dynamic GetMenu(Func<dynamic, bool> viewSelector)
        {
            return _menuList.FirstOrDefault(f => viewSelector(f));
        }

        /// <summary>
        ///     Возвращает тип документа для индекса
        ///     По умолчанию - пустая строка, в этом случае для
        ///     документа не создается собственный тип и найти его можно
        ///     только используя поиск по всему индексу
        /// </summary>
        /// <param name="containerId">Идентификатор объекта метаданных</param>
        /// <returns>Тип для данных индекса</returns>
        public string GetMetadataIndexType(string containerId)
        {
            return GetMetadataContainer(containerId).MetadataIndexType;
        }

        /// <summary>
        ///     Установить тип документа для индекса
        ///     По умолчанию - пустая строка, в этом случае для
        ///     документа не создается собственный тип и найти его можно
        ///     только используя поиск по всему индексу
        /// </summary>
        /// <param name="containerId">Идентификатор объекта метаданных</param>
        /// <param name="indexType">Наименование создаваемого индекса</param>
        public void SetMetadataIndexType(string containerId, string indexType)
        {
            GetMetadataContainer(containerId).MetadataIndexType = indexType;
        }

        /// <summary>
        ///     Установить схему данных для указанного документа
        /// </summary>
        /// <param name="containerId">Идентификатор объекта метаданных</param>
        /// <param name="schema">Модель данных документа</param>
        public void SetSchemaVersion(string containerId, dynamic schema)
        {
            GetMetadataContainer(containerId).Schema = schema;
        }

        /// <summary>
        ///     Получить схему данных для указанного документа
        /// </summary>
        /// <param name="containerId">Идентификатор объекта метаданных</param>
        /// <returns>Модель данных</returns>
        public dynamic GetSchemaVersion(string containerId)
        {
            return GetMetadataContainer(containerId).Schema;
        }

        /// <summary>
        ///     Зарегистрировать объект метаданных бизнес-процесса
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="process"></param>
        public void RegisterProcess(string containerId, dynamic process)
        {
            GetMetadataContainer(containerId).RegisterProcess(process);

            LoadProcess(containerId, process);
        }

		private void LoadProcess(string containerId, dynamic processFull)
		{

			try
			{
				Action<IStateWorkflowStartingPointConfig> workflowConfig = null;
				if (processFull.Transitions == null || processFull.Transitions.Count == 0)
				{
					Logger.Log.Error("No one transition defined for process: {0}:{1}", processFull.Name, containerId);
					return;
				}

				if (processFull.Type == null)
				{
					processFull.Type = WorkflowTypes.WithoutState;
				}

				if ((WorkflowTypes)processFull.Type == WorkflowTypes.WithoutState)
				{

					var transition = processFull.Transitions[0];
					Action<IStateTransitionConfig> configTransition = ws =>
					{
						if (transition.ValidationPointError != null)
						{
							ws.WithValidationError(() => ScriptConfiguration.GetValidator(transition.ValidationPointError.ScenarioId));
						}
						if (transition.ValidationPointWarning != null)
						{
							ws.WithValidationWarning(() => ScriptConfiguration.GetValidator(transition.ValidationPointWarning.ScenarioId));
						}
                        if (transition.DeletingDocumentValidationPoint != null)
                        {
                            ws.WithValidationError(() => ScriptConfiguration.GetValidator(transition.DeletingDocumentValidationPoint.ScenarioId));
                        }
						//TODO: Необходимо переработать механизм подключения валидаций для бизнес-процессов без состояния
						//if (transition.ValidationRuleError != null)
						//{
						//	ws.WithValidationError(() => ValidationExtensions.CreateValidatorFromConfigValidator(transition.ValidationRuleError.ValidationOperator));
						//}
						//if (transition.ValidationRuleWarning != null)
						//{
						//	ws.WithValidationWarning(() => ValidationExtensions.CreateValidatorFromConfigValidator(transition.ValidationRuleWarning.ValidationOperator));
						//}
						if (transition.ActionPoint != null)
						{
							ws.WithAction(() => ScriptConfiguration.GetAction(transition.ActionPoint.ScenarioId));
						}
						if (transition.SuccessPoint != null)
						{
							ws.OnSuccess(() => ScriptConfiguration.GetAction(transition.SuccessPoint.ScenarioId));
						}
						if (transition.FailPoint != null)
						{
							ws.OnFail(() => ScriptConfiguration.GetAction(transition.FailPoint.ScenarioId));
						}
						if (transition.DeletePoint != null)
						{
							ws.OnDelete(() => ScriptConfiguration.GetAction(transition.DeletePoint.ScenarioId));
						}
						if (transition.AuthorizationPoint != null)
						{
							ws.WithSimpleAuthorization(
								() => ScriptConfiguration.GetAction(transition.AuthorizationPoint.ScenarioId));
						}
						if (transition.ComplexAuthorizationPoint != null)
						{
							ws.WithComplexAuthorization(
								() => ScriptConfiguration.GetAction(transition.ComplexAuthorizationPoint.ScenarioId));
						}
						if (transition.CredentialsType == AuthorizationStorageExtensions.CustomCredentials)
						{
							ws.OnCredentials(() => ScriptConfiguration.GetAction(transition.CredentialsPoint.ScenarioId));
						}
						if (transition.CredentialsType == AuthorizationStorageExtensions.AnonimousUserCredentials)
						{
							ws.OnCredentials(() => ScriptConfiguration.GetAction("SetAnonimousCredentials"));
						}
					};


					workflowConfig = wf => wf.FlowWithoutState(wc => wc.Move(configTransition));
				}
				if ((WorkflowTypes)processFull.Type == WorkflowTypes.WithState)
				{
					if (processFull.Transitions.Count == 0)
					{
						Logger.Log.Error("No transition found for process: {0}", processFull.Name);
					}
					var initialStateFrom = processFull.Transitions[0].StateFrom != null ? processFull.Transitions[0].StateFrom.Name.ToString() : null;

					dynamic process1 = processFull;
					Action<IStateWorkflowConfig> configWorkFlow = wc =>
					{
						foreach (var transition in DynamicWrapperExtensions.ToEnumerable(process1.Transitions))
						{

							dynamic transition1 = transition;
							Action<IStateTransitionConfig> configTransition = ws =>
							{
								if (transition1.ValidationPointError != null)
								{
									ws.WithValidationError(() => ScriptConfiguration.GetValidator(transition1.ValidationPointError.ScenarioId));
								}
								if (transition1.ValidationPointWarning != null)
								{
									ws.WithValidationWarning(() => ScriptConfiguration.GetValidator(transition1.ValidationPointWarning.ScenarioId));
								}
                                if (transition1.DeletingDocumentValidationPoint != null)
                                {
                                    ws.WithValidationError(() => ScriptConfiguration.GetValidator(transition1.DeletingDocumentValidationPoint.ScenarioId));
                                }

								//TODO: Необходимо переработать механизм подключения валидаций для кастомных бизнес-процессов
								//if (transition1.ValidationRuleError != null)
								//{
								//	ws.WithValidationError(() => ValidationExtensions.CreateValidatorFromConfigValidator(transition1.ValidationRuleError.ValidationOperator));
								//}
								//if (transition1.ValidationRuleWarning != null)
								//{
								//	ws.WithValidationWarning(() => ValidationExtensions.CreateValidatorFromConfigValidator(transition1.ValidationRuleWarning.ValidationOperator));
								//}

								if (transition1.ActionPoint != null)
								{
									ws.WithAction(() => ScriptConfiguration.GetAction(transition1.ActionPoint.ScenarioId));
								}
								if (transition1.SuccessPoint != null)
								{
									ws.OnSuccess(() => ScriptConfiguration.GetAction(transition1.SuccessPoint.ScenarioId));
								}
								if (transition1.FailPoint != null)
								{
									ws.OnFail(() => ScriptConfiguration.GetAction(transition1.FailPoint.ScenarioId));
								}
								if (transition1.DeletePoint != null)
								{
									ws.OnDelete(() => ScriptConfiguration.GetAction(transition1.DeletePoint.ScenarioId));
								}
								if (transition1.AuthorizationPoint != null)
								{
									ws.WithSimpleAuthorization(
										() => ScriptConfiguration.GetAction(transition1.AuthorizationPoint.ScenarioId));
								}
								if (transition1.ComplexAuthorizationPoint != null)
								{
									ws.WithComplexAuthorization(
										() => ScriptConfiguration.GetAction(transition1.ComplexAuthorizationPoint.ScenarioId));
								}
								if (transition1.CredentialsType == AuthorizationStorageExtensions.CustomCredentials)
								{
									ws.OnCredentials(() => ScriptConfiguration.GetAction(transition.CredentialsPoint.ScenarioId));
								}
								if (transition1.CredentialsType == AuthorizationStorageExtensions.AnonimousUserCredentials)
								{
									ws.OnCredentials(() => ScriptConfiguration.GetAction("SetAnonimousCredentials"));
								}
							};
							wc.Move(configTransition);
						}
					};
					workflowConfig = wf => wf.ForState(initialStateFrom, configWorkFlow);
				}

				RegisterWorkflow(containerId, processFull.Name, workflowConfig);

				Logger.Log.Info("Config:{0}, Document: {1}, Process: {2} registered", ConfigurationId, containerId, processFull.Name);
			}
			catch (Exception e)
			{
				Logger.Log.Error("Config:{0}, Document: {1}, Process: {2}. REGISTRATION ERROR: {3}", ConfigurationId, containerId, processFull.Name, e.Message);
			}
		}

		/// <summary>
	    ///   Регистрация метаданных сервиса
	    /// </summary>
	    /// <param name="containerId"></param>
	    /// <param name="service"></param>
	    public void RegisterService(string containerId, dynamic service)
	    {
	        GetMetadataContainer(containerId).RegisterService(service);
	    }

        /// <summary>
        ///     Регистрация метаданных сценария
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="scenario"></param>
        public void RegisterScenario(string containerId, dynamic scenario)
        {
            GetMetadataContainer(containerId).RegisterScenario(scenario);
        }

        /// <summary>
        ///     Регистрация генератора представлений
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="generator">Метаданные генератора</param>
        public void RegisterGenerator(string containerId, dynamic generator)
        {
            GetMetadataContainer(containerId).RegisterGenerator(generator);
        }

        /// <summary>
        ///     Регистрация метаданных представления
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="view">Метаданные генератора</param>
        public void RegisterView(string containerId, dynamic view)
        {
            GetMetadataContainer(containerId).RegisterView(view);
        }

        /// <summary>
        ///     Регистрация метаданных печатного представления.
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных.</param>
        /// <param name="printView">Метаданные печатного представления.</param>
        public void RegisterPrintView(string containerId, dynamic printView)
        {
            GetMetadataContainer(containerId).RegisterPrintView(printView);
        }

        /// <summary>
        ///     Регистрация метаданных предупреждений валидации
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="warning">Метаданные генератора</param>
        public void RegisterValidationWarning(string containerId, dynamic warning)
        {
            GetMetadataContainer(containerId).RegisterValidationWarning(warning);
        }

        /// <summary>
        ///     Регистрация метаданных ошибок валидации
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="error">Метаданные генератора</param>
        public void RegisterValidationError(string containerId, dynamic error)
        {
            GetMetadataContainer(containerId).RegisterValidationError(error);
        }

        /// <summary>
        ///     Регистрация метаданных статусов
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="status">Метаданные генератора</param>
        public void RegisterStatus(string containerId, dynamic status)
        {
            GetMetadataContainer(containerId).RegisterStatus(status);
        }

        /// <summary>
        ///     Регистрация регистра (регистр представляет собой особый тип документа, хранящий
        ///     сведения, которые зачастую являются функциональным отражением состояния объектов)
        /// </summary>
        public void RegisterRegister(dynamic register)
        {
            _registers.Add(register);
        }

        /// <summary>
        ///     Удалить объект метаданных бизнес-процесса
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="processName"></param>
        public void UnregisterProcess(string containerId, string processName)
        {
            GetMetadataContainer(containerId).DeleteProcess(processName);
        }

        /// <summary>
        ///     Удалить метаданные сервиса
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="serviceName"></param>
        public void UnregisterService(string containerId, string serviceName)
        {
            GetMetadataContainer(containerId).DeleteService(serviceName);
        }

        /// <summary>
        ///     Удалить метаданные сценария
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="scenarioName"></param>
        public void UnregisterScenario(string containerId, string scenarioName)
        {
            GetMetadataContainer(containerId).DeleteScenario(scenarioName);
        }

        /// <summary>
        ///     Удалить генератор представлений
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="generatorName">Метаданные генератора</param>
        public void UnregisterGenerator(string containerId, string generatorName)
        {
            GetMetadataContainer(containerId).DeleteGenerator(generatorName);
        }

        /// <summary>
        ///     Удалить метаданные представления
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="viewName">Метаданные генератора</param>
        public void UnregisterView(string containerId, string viewName)
        {
            GetMetadataContainer(containerId).DeleteView(viewName);
        }

        /// <summary>
        ///     Удалить метаданные печатного представления.
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных.</param>
        /// <param name="printViewName">Метаданные печатного представления.</param>
        public void UnregisterPrintView(string containerId, string printViewName)
        {
            GetMetadataContainer(containerId).DeletePrintView(printViewName);
        }

        /// <summary>
        ///     Удалить метаданные предупреждений валидации
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="warningName">Метаданные генератора</param>
        public void UnregisterValidationWarning(string containerId, string warningName)
        {
            GetMetadataContainer(containerId).DeleteValidationWarning(warningName);
        }

        /// <summary>
        ///     Удалить метаданные ошибок валидации
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="errorName">Метаданные генератора</param>
        public void UnregisterValidationError(string containerId, string errorName)
        {
            GetMetadataContainer(containerId).DeleteValidationError(errorName);
        }

        /// <summary>
        ///     Удалить метаданные статусов
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="statusName">Метаданные генератора</param>
        public void UnregisterStatus(string containerId, string statusName)
        {
            GetMetadataContainer(containerId).DeleteStatus(statusName);
        }

        /// <summary>
        ///     Удалить регистр (регистр представляет собой особый тип документа, хранящий
        ///     сведения, которые зачастую являются функциональным отражением состояния объектов)
        /// </summary>
        public void UnregisterRegister(string registerName)
        {
            var existingRegister =
                _registers.FirstOrDefault(v => v.Name.ToLowerInvariant() == registerName.ToLowerInvariant());
            if (existingRegister != null)
            {
                _registers.Remove(existingRegister);
            }
        }

        public dynamic GetProcess(string containerId, string processName)
        {
            return GetMetadataContainer(containerId).GetProcess(processName);
        }

        public dynamic GetProcess(string containerId, Func<dynamic, bool> processSelector)
        {
            return GetMetadataContainer(containerId).GetProcess(processSelector);
        }

        public dynamic GetService(string containerId, string serviceName)
        {
            return GetMetadataContainer(containerId).GetService(serviceName);
        }

        public dynamic GetScenario(string containerId, string scenarioName)
        {
            return GetMetadataContainer(containerId).GetScenario(scenarioName);
        }

        public dynamic GetValidationError(string containerId, string errorName)
        {
            return GetMetadataContainer(containerId).GetValidationError(errorName);
        }

        public dynamic GetValidationError(string containerId, Func<dynamic, bool> validationErrorSelector)
        {
            return GetMetadataContainer(containerId).GetValidationError(validationErrorSelector);
        }

        public dynamic GetValidationWarning(string containerId, string warningName)
        {
            return GetMetadataContainer(containerId).GetValidationWarning(warningName);
        }

        public dynamic GetValidationWarning(string containerId, Func<dynamic, bool> validationWarningSelector)
        {
            return GetMetadataContainer(containerId).GetValidationWarning(validationWarningSelector);
        }

        public dynamic GetStatus(string containerId, string statusName)
        {
            return GetMetadataContainer(containerId).GetStatus(statusName);
        }

        public dynamic GetScenario(string containerId, Func<dynamic, bool> scenarioSelector)
        {
            return GetMetadataContainer(containerId).GetScenario(scenarioSelector);
        }

        public dynamic GetService(string containerId, Func<dynamic, bool> serviceSelector)
        {
            return GetMetadataContainer(containerId).GetService(serviceSelector);
        }

        public dynamic GetGenerator(string containerId, Func<dynamic, bool> generatorSelector)
        {
            return GetMetadataContainer(containerId).GetGenerator(generatorSelector);
        }

        public dynamic GetView(string containerId, Func<dynamic, bool> viewSelector)
        {
            return GetMetadataContainer(containerId).GetView(viewSelector);
        }

        public dynamic GetPrintView(string containerId, Func<dynamic, bool> selector)
        {
            return GetMetadataContainer(containerId).GetPrintView(selector);
        }

        /// <summary>
        ///     Возвращает регистр по имени (регистр представляет собой особый тип документа, хранящий
        ///     сведения, которые зачастую являются функциональным отражением состояния объектов)
        /// </summary>
        public dynamic GetRegister(string registerName)
        {
            return _registers.FirstOrDefault(sc => sc.Name == registerName);
        }

        public IEnumerable<dynamic> GetRegisterList()
        {
            return _registers.ToList();
        }

        public IEnumerable<dynamic> GetViews(string containerId)
        {
            return GetMetadataContainer(containerId).GetViews();
        }

        public IEnumerable<dynamic> GetPrintViews(string containerId)
        {
            return GetMetadataContainer(containerId).GetPrintViews();
        }

        public IEnumerable<dynamic> GetGenerators(string containerId)
        {
            return GetMetadataContainer(containerId).GetGenerators();
        }

        public IEnumerable<dynamic> GetScenarios(string containerId)
        {
            return GetMetadataContainer(containerId).GetScenarios();
        }

        public IEnumerable<dynamic> GetProcesses(string containerId)
        {
            return GetMetadataContainer(containerId).GetProcesses();
        }

        public IEnumerable<dynamic> GetServices(string containerId)
        {
            return GetMetadataContainer(containerId).GetServices();
        }

        public IEnumerable<dynamic> GetValidationErrors(string containerId)
        {
            return GetMetadataContainer(containerId).GetValidationErrors();
        }

        public IEnumerable<dynamic> GetValidationWarnings(string containerId)
        {
            return GetMetadataContainer(containerId).GetValidationWarnings();
        }

        /// <summary>
        ///     Получить список меню для конфигурации
        /// </summary>
        /// <returns>Список меню</returns>
        public IEnumerable<dynamic> GetMenuList()
        {
            return _menuList;
        }

        //For example: metadataId = "metadata", instanceName = "Save", extensionPointTypeName = "FilterEvents"
        /// <summary>
        ///     Получить идентификатор точки расширения для указанных метаданных
        /// </summary>
        /// <param name="metadataId">Идентификатор метаданных</param>
        /// <param name="actionInstanceName">Идентификатор экземпляра обработчика действия</param>
        /// <param name="extensionPointTypeName">Идентификатор типа точки расширения</param>
        /// <returns>Значение идентификатора точки расширения логики</returns>
        public string GetExtensionPointValue(string metadataId, string actionInstanceName, string extensionPointTypeName)
        {
            if (string.IsNullOrEmpty(actionInstanceName))
            {
                return null;
            }

            var actionHandlerInstance = ServiceRegistrationContainer.GetRegistrationByInstanceName(metadataId,
                actionInstanceName);

            if (actionHandlerInstance != null)
            {
                var extensionPoint = actionHandlerInstance.GetExtensionPoint(extensionPointTypeName);
                return extensionPoint != null ? extensionPoint.StateMachineReference : null;
            }

            throw new ArgumentException(
                string.Format("Metadata extension point for point: {0} and container: {1} not registered",
                    extensionPointTypeName, metadataId));
        }

        /// <summary>
        ///     Получить для указанного контейнера допустимый тип поиска
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <returns>Возможности поиска по контейнеру</returns>
        public SearchAbilityType GetSearchAbilityType(string containerId)
        {
            return GetMetadataContainer(containerId).SearchAbility;
        }

        /// <summary>
        ///     Установить доступный тип поиска для провайдера
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера</param>
        /// <param name="searchAbility">Возможности поиска по контейнеру</param>
        public void SetSearchAbilityType(string containerId, SearchAbilityType searchAbility)
        {
            GetMetadataContainer(containerId).UpdateSearchAbilityType(searchAbility);
        }

		public void UnregisterDocument(string documentName)
		{
			var container = GetMetadataContainer(documentName);
			_containers.Remove(container);
		}
        
    }
}