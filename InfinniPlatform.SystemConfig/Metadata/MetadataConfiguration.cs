using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Runtime;

namespace InfinniPlatform.SystemConfig.Metadata
{
    /// <summary>
    /// Метаданные конфигурации
    /// </summary>
    public class MetadataConfiguration : IMetadataConfiguration
    {
        public MetadataConfiguration(IScriptConfiguration scriptConfiguration, bool isEmbeddedConfiguration)
        {
            ScriptConfiguration = scriptConfiguration;

            _isEmbeddedConfiguration = isEmbeddedConfiguration;
        }

        private readonly Dictionary<string, MetadataContainer> _documents = new Dictionary<string, MetadataContainer>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Является ли конфигурация встроенной в код C#
        /// </summary>
        private readonly bool _isEmbeddedConfiguration;

        private readonly List<dynamic> _menuList = new List<dynamic>();
        private readonly List<dynamic> _registers = new List<dynamic>();

        /// <summary>
        /// Идентификатор конфигурации
        /// </summary>
        public string ConfigurationId { get; set; }

        /// <summary>
        /// Конфигурация прикладных скриптов
        /// </summary>
        public IScriptConfiguration ScriptConfiguration { get; }

        /// <summary>
        /// Признак того, что конфигурация является встроенной в код C# (не хранится в JSON)
        /// Например, конфигурация "Справочники НСИ" является встроенной в код.
        /// Это важно для некоторых операций, в частности операций доступа к метаданным конфигурации
        /// </summary>
        public bool IsEmbeddedConfiguration
        {
            get { return _isEmbeddedConfiguration; }
        }

        /// <summary>
        /// Список индексов метаданных
        /// </summary>
        public IEnumerable<string> Documents
        {
            get { return _documents.Keys; }
        }

        /// <summary>
        /// Зарегистрировать список меню
        /// </summary>
        /// <param name="menuList">список меню</param>
        public void RegisterMenu(IEnumerable<dynamic> menuList)
        {
            _menuList.AddRange(menuList);
        }

        public void RegisterDocument(string documentId)
        {
            RegisterContainer(documentId);
        }

        public dynamic GetMenu(Func<dynamic, bool> viewSelector)
        {
            return _menuList.FirstOrDefault(f => viewSelector(f));
        }

        /// <summary>
        /// Возвращает тип документа для индекса
        /// По умолчанию - пустая строка, в этом случае для
        /// документа не создается собственный тип и найти его можно
        /// только используя поиск по всему индексу
        /// </summary>
        /// <param name="documentId">Идентификатор объекта метаданных</param>
        /// <returns>Тип для данных индекса</returns>
        public string GetMetadataIndexType(string documentId)
        {
            var document = GetMetadataContainer(documentId);
            return (document != null) ? document.MetadataIndexType : null;
        }

        /// <summary>
        /// Установить тип документа для индекса
        /// По умолчанию - пустая строка, в этом случае для
        /// документа не создается собственный тип и найти его можно
        /// только используя поиск по всему индексу
        /// </summary>
        /// <param name="documentId">Идентификатор объекта метаданных</param>
        /// <param name="indexType">Наименование создаваемого индекса</param>
        public void SetMetadataIndexType(string documentId, string indexType)
        {
            GetMetadataContainer(documentId).MetadataIndexType = indexType;
        }

        /// <summary>
        /// Установить схему данных для указанного документа
        /// </summary>
        /// <param name="documentId">Идентификатор объекта метаданных</param>
        /// <param name="schema">Модель данных документа</param>
        public void SetSchemaVersion(string documentId, dynamic schema)
        {
            GetMetadataContainer(documentId).Schema = schema;
        }

        /// <summary>
        /// Получить схему данных для указанного документа
        /// </summary>
        /// <param name="documentId">Идентификатор объекта метаданных</param>
        /// <returns>Модель данных</returns>
        public dynamic GetSchemaVersion(string documentId)
        {
            return GetMetadataContainer(documentId).Schema;
        }

        /// <summary>
        /// Зарегистрировать объект метаданных бизнес-процесса
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="process"></param>
        public void RegisterProcess(string documentId, dynamic process)
        {
            GetMetadataContainer(documentId).RegisterProcess(process);
        }

        /// <summary>
        /// Регистрация метаданных сервиса
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="service"></param>
        public void RegisterService(string documentId, dynamic service)
        {
            GetMetadataContainer(documentId).RegisterService(service);
        }

        /// <summary>
        /// Регистрация метаданных сценария
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="scenario"></param>
        public void RegisterScenario(string documentId, dynamic scenario)
        {
            GetMetadataContainer(documentId).RegisterScenario(scenario);
        }

        /// <summary>
        /// Регистрация генератора представлений
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="generator">Метаданные генератора</param>
        public void RegisterGenerator(string documentId, dynamic generator)
        {
            GetMetadataContainer(documentId).RegisterGenerator(generator);
        }

        /// <summary>
        /// Регистрация метаданных представления
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="view">Метаданные генератора</param>
        public void RegisterView(string documentId, dynamic view)
        {
            GetMetadataContainer(documentId).RegisterView(view);
        }

        /// <summary>
        /// Регистрация метаданных печатного представления.
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных.</param>
        /// <param name="printView">Метаданные печатного представления.</param>
        public void RegisterPrintView(string documentId, dynamic printView)
        {
            GetMetadataContainer(documentId).RegisterPrintView(printView);
        }

        /// <summary>
        /// Регистрация метаданных предупреждений валидации
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="warning">Метаданные генератора</param>
        public void RegisterValidationWarning(string documentId, dynamic warning)
        {
            GetMetadataContainer(documentId).RegisterValidationWarning(warning);
        }

        /// <summary>
        /// Регистрация метаданных ошибок валидации
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="error">Метаданные генератора</param>
        public void RegisterValidationError(string documentId, dynamic error)
        {
            GetMetadataContainer(documentId).RegisterValidationError(error);
        }

        /// <summary>
        /// Регистрация метаданных статусов
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="status">Метаданные генератора</param>
        public void RegisterStatus(string documentId, dynamic status)
        {
            GetMetadataContainer(documentId).RegisterStatus(status);
        }

        /// <summary>
        /// Регистрация регистра (регистр представляет собой особый тип документа, хранящий
        /// сведения, которые зачастую являются функциональным отражением состояния объектов)
        /// </summary>
        public void RegisterRegister(dynamic register)
        {
            _registers.Add(register);
        }

        /// <summary>
        /// Удалить объект метаданных бизнес-процесса
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="processName"></param>
        public void UnregisterProcess(string documentId, string processName)
        {
            GetMetadataContainer(documentId).DeleteProcess(processName);
        }

        /// <summary>
        /// Удалить метаданные сервиса
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="serviceName"></param>
        public void UnregisterService(string documentId, string serviceName)
        {
            GetMetadataContainer(documentId).DeleteService(serviceName);
        }

        /// <summary>
        /// Удалить метаданные сценария
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="scenarioName"></param>
        public void UnregisterScenario(string documentId, string scenarioName)
        {
            GetMetadataContainer(documentId).DeleteScenario(scenarioName);
        }

        /// <summary>
        /// Удалить генератор представлений
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="generatorName">Метаданные генератора</param>
        public void UnregisterGenerator(string documentId, string generatorName)
        {
            GetMetadataContainer(documentId).DeleteGenerator(generatorName);
        }

        /// <summary>
        /// Удалить метаданные представления
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="viewName">Метаданные генератора</param>
        public void UnregisterView(string documentId, string viewName)
        {
            GetMetadataContainer(documentId).DeleteView(viewName);
        }

        /// <summary>
        /// Удалить метаданные печатного представления.
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных.</param>
        /// <param name="printViewName">Метаданные печатного представления.</param>
        public void UnregisterPrintView(string documentId, string printViewName)
        {
            GetMetadataContainer(documentId).DeletePrintView(printViewName);
        }

        /// <summary>
        /// Удалить метаданные предупреждений валидации
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="warningName">Метаданные генератора</param>
        public void UnregisterValidationWarning(string documentId, string warningName)
        {
            GetMetadataContainer(documentId).DeleteValidationWarning(warningName);
        }

        /// <summary>
        /// Удалить метаданные ошибок валидации
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="errorName">Метаданные генератора</param>
        public void UnregisterValidationError(string documentId, string errorName)
        {
            GetMetadataContainer(documentId).DeleteValidationError(errorName);
        }

        /// <summary>
        /// Удалить метаданные статусов
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <param name="statusName">Метаданные генератора</param>
        public void UnregisterStatus(string documentId, string statusName)
        {
            GetMetadataContainer(documentId).DeleteStatus(statusName);
        }

        /// <summary>
        /// Удалить регистр (регистр представляет собой особый тип документа, хранящий
        /// сведения, которые зачастую являются функциональным отражением состояния объектов)
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

        public dynamic GetProcess(string documentId, string processName)
        {
            return GetMetadataContainer(documentId).GetProcess(processName);
        }

        public dynamic GetProcess(string documentId, Func<dynamic, bool> processSelector)
        {
            return GetMetadataContainer(documentId).GetProcess(processSelector);
        }

        public dynamic GetService(string documentId, string serviceName)
        {
            return GetMetadataContainer(documentId).GetService(serviceName);
        }

        public dynamic GetScenario(string documentId, string scenarioName)
        {
            return GetMetadataContainer(documentId).GetScenario(scenarioName);
        }

        public dynamic GetValidationError(string documentId, string errorName)
        {
            return GetMetadataContainer(documentId).GetValidationError(errorName);
        }

        public dynamic GetValidationError(string documentId, Func<dynamic, bool> validationErrorSelector)
        {
            return GetMetadataContainer(documentId).GetValidationError(validationErrorSelector);
        }

        public dynamic GetValidationWarning(string documentId, string warningName)
        {
            return GetMetadataContainer(documentId).GetValidationWarning(warningName);
        }

        public dynamic GetValidationWarning(string documentId, Func<dynamic, bool> validationWarningSelector)
        {
            return GetMetadataContainer(documentId).GetValidationWarning(validationWarningSelector);
        }

        public dynamic GetStatus(string documentId, string statusName)
        {
            return GetMetadataContainer(documentId).GetStatus(statusName);
        }

        public dynamic GetScenario(string documentId, Func<dynamic, bool> scenarioSelector)
        {
            return GetMetadataContainer(documentId).GetScenario(scenarioSelector);
        }

        public dynamic GetService(string documentId, Func<dynamic, bool> serviceSelector)
        {
            return GetMetadataContainer(documentId).GetService(serviceSelector);
        }

        public dynamic GetGenerator(string documentId, Func<dynamic, bool> generatorSelector)
        {
            return GetMetadataContainer(documentId).GetGenerator(generatorSelector);
        }

        public dynamic GetView(string documentId, Func<dynamic, bool> viewSelector)
        {
            return GetMetadataContainer(documentId).GetView(viewSelector);
        }

        public dynamic GetPrintView(string documentId, Func<dynamic, bool> selector)
        {
            return GetMetadataContainer(documentId).GetPrintView(selector);
        }

        /// <summary>
        /// Возвращает регистр по имени (регистр представляет собой особый тип документа, хранящий
        /// сведения, которые зачастую являются функциональным отражением состояния объектов)
        /// </summary>
        public dynamic GetRegister(string registerName)
        {
            return _registers.FirstOrDefault(sc => sc.Name == registerName);
        }

        public IEnumerable<dynamic> GetRegisterList()
        {
            return _registers.ToList();
        }

        public IEnumerable<dynamic> GetViews(string documentId)
        {
            return GetMetadataContainer(documentId).GetViews();
        }

        public IEnumerable<dynamic> GetPrintViews(string documentId)
        {
            return GetMetadataContainer(documentId).GetPrintViews();
        }

        public IEnumerable<dynamic> GetGenerators(string documentId)
        {
            return GetMetadataContainer(documentId).GetGenerators();
        }

        public IEnumerable<dynamic> GetScenarios(string documentId)
        {
            return GetMetadataContainer(documentId).GetScenarios();
        }

        public IEnumerable<dynamic> GetProcesses(string documentId)
        {
            return GetMetadataContainer(documentId).GetProcesses();
        }

        public IEnumerable<dynamic> GetServices(string documentId)
        {
            return GetMetadataContainer(documentId).GetServices();
        }

        public IEnumerable<dynamic> GetValidationErrors(string documentId)
        {
            return GetMetadataContainer(documentId).GetValidationErrors();
        }

        public IEnumerable<dynamic> GetValidationWarnings(string documentId)
        {
            return GetMetadataContainer(documentId).GetValidationWarnings();
        }

        /// <summary>
        /// Получить список меню для конфигурации
        /// </summary>
        /// <returns>Список меню</returns>
        public IEnumerable<dynamic> GetMenuList()
        {
            return _menuList;
        }

        /// <summary>
        /// Получить для указанного контейнера допустимый тип поиска
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера метаданных</param>
        /// <returns>Возможности поиска по контейнеру</returns>
        public SearchAbilityType GetSearchAbilityType(string documentId)
        {
            return GetMetadataContainer(documentId).SearchAbility;
        }

        /// <summary>
        /// Установить доступный тип поиска для провайдера
        /// </summary>
        /// <param name="documentId">Идентификатор контейнера</param>
        /// <param name="searchAbility">Возможности поиска по контейнеру</param>
        public void SetSearchAbilityType(string documentId, SearchAbilityType searchAbility)
        {
            GetMetadataContainer(documentId).UpdateSearchAbilityType(searchAbility);
        }

        private MetadataContainer RegisterContainer(string documentId)
        {
            MetadataContainer metadataContainer;

            if (!_documents.TryGetValue(documentId, out metadataContainer))
            {
                metadataContainer = new MetadataContainer(documentId) { ContainerId = documentId };

                _documents[documentId] = metadataContainer;
            }

            return metadataContainer;
        }

        private MetadataContainer GetMetadataContainer(string documentId)
        {
            return RegisterContainer(documentId);
        }

        public void UnregisterDocument(string documentName)
        {
            _documents.Remove(documentName);
        }
    }
}