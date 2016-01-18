using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Runtime;

namespace InfinniPlatform.Core.Metadata
{
    /// <summary>
    /// Конфигурация метаданных модуля
    /// </summary>
    public interface IMetadataConfiguration
    {
        /// <summary>
        /// Идентификатор конфигурации
        /// </summary>
        string ConfigurationId { get; set; }

        /// <summary>
        /// Конфигурация прикладных скриптов
        /// </summary>
        IScriptConfiguration ScriptConfiguration { get; }

        /// <summary>
        /// Список индексов метаданных
        /// </summary>
        IEnumerable<string> Documents { get; }

        /// <summary>
        /// Признак того, что конфигурация является встроенной в код C# (не хранится в JSON)
        /// Например, конфигурация "Справочники НСИ" является встроенной в код.
        /// Это важно для некоторых операций, в частности операций доступа к метаданным конфигурации
        /// </summary>
        bool IsEmbeddedConfiguration { get; }

        /// <summary>
        /// Получить для указанного контейнера допустимый тип поиска
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <returns>Возможности поиска по контейнеру</returns>
        SearchAbilityType GetSearchAbilityType(string documentId);

        /// <summary>
        /// Установить доступный тип поиска для провайдера
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера</param>
        /// <param name="searchAbility">Возможности поиска по контейнеру</param>
        void SetSearchAbilityType(string documentId, SearchAbilityType searchAbility);

        /// <summary>
        /// Зарегистрировать список меню
        /// </summary>
        /// <param name="menuList">список меню</param>
        void RegisterMenu(IEnumerable<dynamic> menuList);

        /// <summary>
        /// Зарегистрирвоать документ
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        void RegisterDocument(string documentId);

        /// <summary>
        /// Получить список меню для конфигурации
        /// </summary>
        /// <returns>Список меню</returns>
        IEnumerable<dynamic> GetMenuList();

        //получить меню
        dynamic GetMenu(Func<dynamic, bool> viewSelector);

        /// <summary>
        /// Возвращает тип документа для индекса
        /// </summary>
        /// <param name="documentId">Идентификатор объекта метаданных</param>
        /// <returns>Тип для данных индекса</returns>
        string GetMetadataIndexType(string documentId);

        /// <summary>
        /// Установить тип документа для индекса
        /// </summary>
        /// <param name="documentId">Идентификатор объекта метаданных</param>
        /// <param name="indexType">Тип в индексе</param>
        void SetMetadataIndexType(string documentId, string indexType);

        /// <summary>
        /// Установить схему данных для указанного документа
        /// </summary>
        /// <param name="documentId">Идентификатор объекта метаданных</param>
        /// <param name="schema">Модель данных документа</param>
        void SetSchemaVersion(string documentId, dynamic schema);

        /// <summary>
        /// Получить схему данных для указанного документа
        /// </summary>
        /// <param name="documentId">Идентификатор объекта метаданных</param>
        /// <returns>Модель данных</returns>
        dynamic GetSchemaVersion(string documentId);

        /// <summary>
        /// Зарегистрировать объект метаданных бизнес-процесса
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="process"></param>
        void RegisterProcess(string documentId, dynamic process);

        /// <summary>
        /// Регистрация метаданных сервиса
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="service"></param>
        void RegisterService(string documentId, dynamic service);

        /// <summary>
        /// Регистрация метаданных сценария
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="scenario"></param>
        void RegisterScenario(string documentId, dynamic scenario);

        /// <summary>
        /// Регистрация генератора представлений
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="generator">Метаданные генератора</param>
        void RegisterGenerator(string documentId, dynamic generator);

        /// <summary>
        /// Регистрация метаданных представления
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="view">Метаданные генератора</param>
        void RegisterView(string documentId, dynamic view);

        /// <summary>
        /// Регистрация метаданных печатного представления.
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных.</param>
        /// <param name="printView">Метаданные печатного представления.</param>
        void RegisterPrintView(string documentId, dynamic printView);

        /// <summary>
        /// Регистрация метаданных предупреждений валидации
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="warning">Метаданные генератора</param>
        void RegisterValidationWarning(string documentId, dynamic warning);

        /// <summary>
        /// Регистрация метаданных ошибок валидации
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="error">Метаданные генератора</param>
        void RegisterValidationError(string documentId, dynamic error);

        /// <summary>
        /// Регистрация метаданных статусов
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="status">Метаданные генератора</param>
        void RegisterStatus(string documentId, dynamic status);

        /// <summary>
        /// Регистрация регистра (регистр представляет собой особый тип документа, хранящий
        /// сведения, которые зачастую являются функциональным отражением состояния объектов)
        /// </summary>
        void RegisterRegister(dynamic register);

        /// <summary>
        /// Удалить объект метаданных бизнес-процесса
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="processName"></param>
        void UnregisterProcess(string documentId, string processName);

        /// <summary>
        /// Удалить метаданные сервиса
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="serviceName"></param>
        void UnregisterService(string documentId, string serviceName);

        /// <summary>
        /// Удалить метаданные сценария
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="scenarioName"></param>
        void UnregisterScenario(string documentId, string scenarioName);

        /// <summary>
        /// Удалить генератор представлений
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="generatorName">Метаданные генератора</param>
        void UnregisterGenerator(string documentId, string generatorName);

        /// <summary>
        /// Удалить метаданные представления
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="viewName">Метаданные генератора</param>
        void UnregisterView(string documentId, string viewName);

        /// <summary>
        /// Удалить метаданные печатного представления.
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных.</param>
        /// <param name="printViewName">Метаданные печатного представления.</param>
        void UnregisterPrintView(string documentId, string printViewName);

        /// <summary>
        /// Удалить метаданные предупреждений валидации
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="warningName">Метаданные генератора</param>
        void UnregisterValidationWarning(string documentId, string warningName);

        /// <summary>
        /// Удалить метаданные ошибок валидации
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="errorName">Метаданные генератора</param>
        void UnregisterValidationError(string documentId, string errorName);

        /// <summary>
        /// Удалить метаданные статусов
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="statusName">Метаданные генератора</param>
        void UnregisterStatus(string documentId, string statusName);

        /// <summary>
        /// Удалить регистр (регистр представляет собой особый тип документа, хранящий
        /// сведения, которые зачастую являются функциональным отражением состояния объектов)
        /// </summary>
        void UnregisterRegister(string registerName);

        dynamic GetProcess(string documentId, string processName);
        dynamic GetService(string documentId, string serviceName);
        dynamic GetScenario(string documentId, string scenarioName);
        dynamic GetValidationError(string documentId, string errorName);
        dynamic GetValidationWarning(string documentId, string warningName);
        dynamic GetStatus(string documentId, string statusName);
        dynamic GetGenerator(string documentId, Func<dynamic, bool> generatorSelector);
        dynamic GetView(string documentId, Func<dynamic, bool> viewSelector);
        dynamic GetPrintView(string documentId, Func<dynamic, bool> selector);
        dynamic GetScenario(string documentId, Func<object, bool> scenarioSelector);
        dynamic GetService(string documentId, Func<object, bool> serviceSelector);
        dynamic GetProcess(string documentId, Func<object, bool> processSelector);
        dynamic GetValidationError(string documentId, Func<object, bool> validationErrorSelector);
        dynamic GetValidationWarning(string documentId, Func<object, bool> validationWarningSelector);
        IEnumerable<dynamic> GetRegisterList();

        /// <summary>
        /// Возвращает регистр по имени (регистр представляет собой особый тип документа, хранящий
        /// сведения, которые зачастую являются функциональным отражением состояния объектов)
        /// </summary>
        dynamic GetRegister(string registerName);

        IEnumerable<dynamic> GetViews(string documentId);
        IEnumerable<dynamic> GetPrintViews(string documentId);
        IEnumerable<dynamic> GetGenerators(string documentId);
        IEnumerable<dynamic> GetScenarios(string documentId);
        IEnumerable<dynamic> GetProcesses(string documentId);
        IEnumerable<dynamic> GetServices(string documentId);
        IEnumerable<dynamic> GetValidationErrors(string documentId);
        IEnumerable<dynamic> GetValidationWarnings(string documentId);
    }
}