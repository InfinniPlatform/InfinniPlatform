using System;
using System.Collections.Generic;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Scripts;
using InfinniPlatform.Sdk.Environment.Worklow;

namespace InfinniPlatform.Sdk.Environment.Metadata
{
    /// <summary>
    ///     Конфигурация метаданных модуля
    /// </summary>
    public interface IMetadataConfiguration : IModule
    {
        /// <summary>
        ///     Идентификатор конфигурации
        /// </summary>
        string ConfigurationId { get; set; }

        /// <summary>
        ///     Конфигурация прикладных скриптов
        /// </summary>
        IScriptConfiguration ScriptConfiguration { get; }

        /// <summary>
        ///     Список индексов метаданных
        /// </summary>
        IEnumerable<string> Containers { get; }

        /// <summary>
        ///     Признак того, что конфигурация является встроенной в код C# (не хранится в JSON)
        ///     Например, конфигурация "Справочники НСИ" является встроенной в код.
        ///     Это важно для некоторых операций, в частности операций доступа к метаданным конфигурации
        /// </summary>
        bool IsEmbeddedConfiguration { get; }

        /// <summary>
        ///     Выполнить указанный поток работы для указанных метаданных
        /// </summary>
        /// <param name="containerId">Метаданные контейнера</param>
        /// <param name="workflowId">Идентификатор потока</param>
        /// <param name="target">Объект, над которым выполняется переход</param>
        /// <param name="state">Состояние, в которое выполняется перевод</param>
        /// <returns>Результат выполнения потока</returns>
        dynamic MoveWorkflow(string containerId, string workflowId, dynamic target, object state = null);

        /// <summary>
        ///     Получить идентификатор точки расширения для указанных метаданных
        /// </summary>
        /// <param name="metadataId">Идентификатор метаданных</param>
        /// <param name="actionInstanceName">Идентификатор экземпляра обработчика действия</param>
        /// <param name="extensionPointTypeName">Идентификатор типа точки расширения</param>
        /// <returns>Значение идентификатора точки расширения логики</returns>
        string GetExtensionPointValue(string metadataId, string actionInstanceName, string extensionPointTypeName);

        /// <summary>
        ///     Получить для указанного контейнера допустимый тип поиска
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <returns>Возможности поиска по контейнеру</returns>
        SearchAbilityType GetSearchAbilityType(string containerId);

        /// <summary>
        ///     Установить доступный тип поиска для провайдера
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера</param>
        /// <param name="searchAbility">Возможности поиска по контейнеру</param>
        void SetSearchAbilityType(string containerId, SearchAbilityType searchAbility);

        /// <summary>
        ///     Зарегистрировать поток выполнения
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="workflowId">Идентификатор потока работы</param>
        /// <param name="actionConfiguration">Конфигурация выполняемых действий</param>
        void RegisterWorkflow(string containerId, string workflowId,
            Action<IStateWorkflowStartingPointConfig> actionConfiguration);

        /// <summary>
        ///     Зарегистрировать список меню
        /// </summary>
        /// <param name="menuList">список меню</param>
        void RegisterMenu(IEnumerable<dynamic> menuList);

        /// <summary>
        ///     Зарегистрирвоать документ
        /// </summary>
        /// <param name="containerId">Идентификатор документа</param>
        void RegisterDocument(string containerId);

        /// <summary>
        ///     Получить список меню для конфигурации
        /// </summary>
        /// <returns>Список меню</returns>
        IEnumerable<dynamic> GetMenuList();

        //получить меню
        dynamic GetMenu(Func<dynamic, bool> viewSelector);

        /// <summary>
        ///     Возвращает тип документа для индекса
        /// </summary>
        /// <param name="containerId">Идентификатор объекта метаданных</param>
        /// <returns>Тип для данных индекса</returns>
        string GetMetadataIndexType(string containerId);

        /// <summary>
        ///     Установить тип документа для индекса
        /// </summary>
        /// <param name="containerId">Идентификатор объекта метаданных</param>
        /// <param name="indexType">Тип в индексе</param>
        void SetMetadataIndexType(string containerId, string indexType);

        /// <summary>
        ///     Установить схему данных для указанного документа
        /// </summary>
        /// <param name="containerId">Идентификатор объекта метаданных</param>
        /// <param name="schema">Модель данных документа</param>
        void SetSchemaVersion(string containerId, dynamic schema);

        /// <summary>
        ///     Получить схему данных для указанного документа
        /// </summary>
        /// <param name="containerId">Идентификатор объекта метаданных</param>
        /// <returns>Модель данных</returns>
        dynamic GetSchemaVersion(string containerId);

        /// <summary>
        ///     Зарегистрировать объект метаданных бизнес-процесса
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="process"></param>
        void RegisterProcess(string containerId, dynamic process);

        /// <summary>
        ///     Регистрация метаданных сервиса
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="service"></param>
        void RegisterService(string containerId, dynamic service);

        /// <summary>
        ///     Регистрация метаданных сценария
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="scenario"></param>
        void RegisterScenario(string containerId, dynamic scenario);

        /// <summary>
        ///     Регистрация генератора представлений
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="generator">Метаданные генератора</param>
        void RegisterGenerator(string containerId, dynamic generator);

        /// <summary>
        ///     Регистрация метаданных представления
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="view">Метаданные генератора</param>
        void RegisterView(string containerId, dynamic view);

        /// <summary>
        ///     Регистрация метаданных печатного представления.
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных.</param>
        /// <param name="printView">Метаданные печатного представления.</param>
        void RegisterPrintView(string containerId, dynamic printView);

        /// <summary>
        ///     Регистрация метаданных предупреждений валидации
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="warning">Метаданные генератора</param>
        void RegisterValidationWarning(string containerId, dynamic warning);

        /// <summary>
        ///     Регистрация метаданных ошибок валидации
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="error">Метаданные генератора</param>
        void RegisterValidationError(string containerId, dynamic error);

        /// <summary>
        ///     Регистрация метаданных статусов
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="status">Метаданные генератора</param>
        void RegisterStatus(string containerId, dynamic status);

        /// <summary>
        ///     Регистрация регистра (регистр представляет собой особый тип документа, хранящий
        ///     сведения, которые зачастую являются функциональным отражением состояния объектов)
        /// </summary>
        void RegisterRegister(dynamic register);

        /// <summary>
        ///     Удалить объект метаданных бизнес-процесса
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="processName"></param>
        void UnregisterProcess(string containerId, string processName);

        /// <summary>
        ///     Удалить метаданные сервиса
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="serviceName"></param>
        void UnregisterService(string containerId, string serviceName);

        /// <summary>
        ///     Удалить метаданные сценария
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="scenarioName"></param>
        void UnregisterScenario(string containerId, string scenarioName);

        /// <summary>
        ///     Удалить генератор представлений
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="generatorName">Метаданные генератора</param>
        void UnregisterGenerator(string containerId, string generatorName);

        /// <summary>
        ///     Удалить метаданные представления
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="viewName">Метаданные генератора</param>
        void UnregisterView(string containerId, string viewName);

        /// <summary>
        ///     Удалить метаданные печатного представления.
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных.</param>
        /// <param name="printViewName">Метаданные печатного представления.</param>
        void UnregisterPrintView(string containerId, string printViewName);

        /// <summary>
        ///     Удалить метаданные предупреждений валидации
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="warningName">Метаданные генератора</param>
        void UnregisterValidationWarning(string containerId, string warningName);

        /// <summary>
        ///     Удалить метаданные ошибок валидации
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="errorName">Метаданные генератора</param>
        void UnregisterValidationError(string containerId, string errorName);

        /// <summary>
        ///     Удалить метаданные статусов
        /// </summary>
        /// <param name="containerId">Идентификатор контейнера метаданных</param>
        /// <param name="statusName">Метаданные генератора</param>
        void UnregisterStatus(string containerId, string statusName);

        /// <summary>
        ///     Удалить регистр (регистр представляет собой особый тип документа, хранящий
        ///     сведения, которые зачастую являются функциональным отражением состояния объектов)
        /// </summary>
        void UnregisterRegister(string registerName);

        dynamic GetProcess(string containerId, string processName);
        dynamic GetService(string containerId, string serviceName);
        dynamic GetScenario(string containerId, string scenarioName);
        dynamic GetValidationError(string containerId, string errorName);
        dynamic GetValidationWarning(string containerId, string warningName);
        dynamic GetStatus(string containerId, string statusName);
        dynamic GetGenerator(string documentId, Func<dynamic, bool> generatorSelector);
        dynamic GetView(string containerId, Func<dynamic, bool> viewSelector);
        dynamic GetPrintView(string containerId, Func<dynamic, bool> selector);
        dynamic GetScenario(string containerId, Func<object, bool> scenarioSelector);
        dynamic GetService(string containerId, Func<object, bool> serviceSelector);
        dynamic GetProcess(string containerId, Func<object, bool> processSelector);
        dynamic GetValidationError(string containerId, Func<object, bool> validationErrorSelector);
        dynamic GetValidationWarning(string containerId, Func<object, bool> validationWarningSelector);
        IEnumerable<dynamic> GetRegisterList();

        /// <summary>
        ///     Возвращает регистр по имени (регистр представляет собой особый тип документа, хранящий
        ///     сведения, которые зачастую являются функциональным отражением состояния объектов)
        /// </summary>
        dynamic GetRegister(string registerName);

        IEnumerable<dynamic> GetViews(string containerId);
        IEnumerable<dynamic> GetPrintViews(string containerId);
        IEnumerable<dynamic> GetGenerators(string containerId);
        IEnumerable<dynamic> GetScenarios(string containerId);
        IEnumerable<dynamic> GetProcesses(string containerId);
        IEnumerable<dynamic> GetServices(string containerId);
        IEnumerable<dynamic> GetValidationErrors(string containerId);
        IEnumerable<dynamic> GetValidationWarnings(string containerId);
    }
}