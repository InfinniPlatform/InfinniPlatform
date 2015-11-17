using System.Collections.Generic;

using InfinniPlatform.Sdk.Properties;

namespace InfinniPlatform.Sdk.Api
{
    public class InfinniMetadataApi : BaseApi
    {
        public InfinniMetadataApi(string server, string port, string route) : base(server, port, route)
        {
        }

        /// <summary>
        /// Добавить метаданные указанного решения
        /// </summary>
        /// <returns>Результат добавления метаданных решения</returns>
        public dynamic InsertSolution(dynamic solutionMetadata)
        {
            var response = RequestExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlMetadataSolution(), solutionMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertSolutionMetadata, response));
        }

        /// <summary>
        /// Обновить метаданные указанного решения
        /// </summary>
        /// <returns>Результат обновления метаданных решения</returns>
        public dynamic UpdateSolution(dynamic solutionMetadata)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataSolution(), solutionMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateSolutionMetadata, response));
        }

        /// <summary>
        /// Создать метаданные нового решения
        /// </summary>
        public dynamic CreateSolution()
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataSolution());

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateSolutionMetadata, response));
        }

        /// <summary>
        /// Удалить метаданные указанного решения
        /// </summary>
        /// <returns>Результат удаления метаданных решения</returns>
        public dynamic DeleteSolution(string version, string name)
        {
            var response = RequestExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlMetadataSolutionById(version, name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteSolutionMetadata, response));
        }

        /// <summary>
        /// Получить метаданные решения по указанному идентификатору для указанной версии
        /// </summary>
        /// <param name="version">Версия приложения</param>
        /// <param name="name">Наименование приложения</param>
        /// <returns>Метаданные решения</returns>
        public dynamic GetSolution(string version, string name)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataSolutionById(version, name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertSolutionMetadata, response));
        }

        /// <summary>
        /// Получить список метаданных решений
        /// </summary>
        /// <returns>Список метаданных решений</returns>
        public dynamic GetSolutionItems()
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataSolution());

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertSolutionMetadata, response));
        }

        /// <summary>
        /// Добавить метаданные указанной конфигурации
        /// </summary>
        /// <returns>Результат добавления метаданных конфигурации</returns>
        public dynamic InsertConfig(dynamic configMetadata)
        {
            var response = RequestExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlMetadataConfig(), configMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertConfigMetadata, response));
        }

        /// <summary>
        /// Обновить метаданные указанной конфигурации
        /// </summary>
        /// <returns>Результат обновления метаданных конфигурации</returns>
        public dynamic UpdateConfig(dynamic configMetadata)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataConfig(), configMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateConfigMetadata, response));
        }

        /// <summary>
        /// Создать метаданные новой конфигурации
        /// </summary>
        public dynamic CreateConfig()
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataConfig());

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateConfigMetadata, response));
        }

        /// <summary>
        /// Удалить метаданные указанной конфигурации
        /// </summary>
        /// <returns>Результат удаления метаданных конфигурации</returns>
        public dynamic DeleteConfig(string version, string name)
        {
            var response = RequestExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlMetadataConfigById(version, name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteConfigMetadata, response));
        }

        /// <summary>
        /// Получить метаданные конфигурации по указанному идентификатору для указанной версии
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="name">Наименование конфигурации</param>
        /// <returns>Метаданные конфигурации</returns>
        public dynamic GetConfig(string version, string name)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataConfigById(version, name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetConfigMetadata, response));
        }

        /// <summary>
        /// Получить метаданные всех конфигураций
        /// </summary>
        /// <returns>Список метаданных конфигураций</returns>
        public dynamic GetConfigList()
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataConfig());

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetConfigMetadata, response));
        }

        /// <summary>
        /// Добавить метаданные указанного меню
        /// </summary>
        /// <returns>Результат добавления метаданных указанного меню</returns>
        public dynamic InsertMenu(dynamic menuMetadata, string version, string configuration)
        {
            var response = RequestExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Menu"), menuMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertMenuMetadata, response));
        }

        /// <summary>
        /// Обновить метаданные указанного меню
        /// </summary>
        /// <returns>Результат обновления метаданных меню</returns>
        public dynamic UpdateMenu(dynamic menuMetadata, string version, string configuration)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Menu"), menuMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateMenuMetadata, response));
        }

        /// <summary>
        /// Создать метаданные меню
        /// </summary>
        public dynamic CreateMenu(string version, string configuration)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Menu"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateMenuMetadata, response));
        }

        /// <summary>
        /// Удалить метаданные указанного меню
        /// </summary>
        /// <returns>Результат удаления метаданных меню</returns>
        public dynamic DeleteMenu(string version, string configuration, string name)
        {
            var response = RequestExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlMetadataElementById(version, configuration, "Menu", name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteMenuMetadata, response));
        }

        /// <summary>
        /// Получить метаданные меню по указанному идентификатору
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="name">Наименование меню</param>
        /// <returns>Метаданные меню</returns>
        public dynamic GetMenu(string version, string configuration, string name)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataElementById(version, configuration, "Menu", name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetMenuMetadata, response));
        }

        /// <summary>
        /// Получить метаданные всех меню для указанной конфигурации
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <returns>Метаданные меню</returns>
        public dynamic GetMenuList(string version, string configuration)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Menu"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetMenuMetadata, response));
        }

        /// <summary>
        /// Добавить метаданные указанной сборки
        /// </summary>
        /// <returns>Результат добавления метаданных указанной сборки</returns>
        public dynamic InsertAssembly(dynamic assemblyMetadata, string version, string configuration)
        {
            var response = RequestExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Assembly"), assemblyMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertAssemblyMetadata, response));
        }

        /// <summary>
        /// Обновить метаданные указанной сборки
        /// </summary>
        /// <returns>Результат обновления метаданных сборок</returns>
        public dynamic UpdateAssembly(dynamic assemblyMetadata, string version, string configuration)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Assembly"), assemblyMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateAssemblyMetadata, response));
        }

        /// <summary>
        /// Создать метаданные сборки
        /// </summary>
        public dynamic CreateAssembly(string version, string configuration)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Assembly"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateAssemblyMetadata, response));
        }

        /// <summary>
        /// Удалить метаданные указанной сборки
        /// </summary>
        /// <returns>Результат удаления метаданных сборки</returns>
        public dynamic DeleteAssembly(string version, string configuration, string name)
        {
            var response = RequestExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlMetadataElementById(version, configuration, "Assembly", name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteAssemblyMetadata, response));
        }

        /// <summary>
        /// Получить метаданные сборки по указанному идентификатору
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="name">Наименование сборки</param>
        /// <returns>Метаданные сборки</returns>
        public dynamic GetAssembly(string version, string configuration, string name)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataElementById(version, configuration, "Assembly", name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetAssemblyMetadata, response));
        }

        /// <summary>
        /// Получить список метаданных сборок
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="configuration">Наименование конфигурации</param>
        /// <returns>Список сборок прикладной конфигурации</returns>
        public IEnumerable<dynamic> GetAssemblyList(string version, string configuration)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Assembly"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetAssemblyMetadata, response));
        }

        /// <summary>
        /// Добавить метаданные указанного регистра
        /// </summary>
        /// <returns>Результат добавления метаданных указанного регистра</returns>
        public dynamic InsertRegister(dynamic registerMetadata, string version, string configuration)
        {
            var response = RequestExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Register"), registerMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertRegisterMetadata, response));
        }

        /// <summary>
        /// Обновить метаданные указанного регистра
        /// </summary>
        /// <returns>Результат обновления метаданных регистра</returns>
        public dynamic UpdateRegister(dynamic registerMetadata, string version, string configuration)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Register"), registerMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateRegisterMetadata, response));
        }

        /// <summary>
        /// Создать метаданные регистра
        /// </summary>
        public dynamic CreateRegister(string version, string configuration)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Register"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateRegisterMetadata, response));
        }

        /// <summary>
        /// Удалить метаданные указанного регистра
        /// </summary>
        /// <returns>Результат удаления метаданных регистра</returns>
        public dynamic DeleteRegister(string version, string configuration, string name)
        {
            var response = RequestExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlMetadataElementById(version, configuration, "Register", name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteRegisterMetadata, response));
        }

        /// <summary>
        /// Получить метаданные регистра по указанному идентификатору
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="name">Наименование регистра</param>
        /// <returns>Метаданные регистра</returns>
        public dynamic GetRegister(string version, string configuration, string name)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataElementById(version, configuration, "Register", name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetRegisterMetadata, response));
        }

        /// <summary>
        /// Получить метаданные регистров
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <returns>Метаданные регистров</returns>
        public dynamic GetRegisterList(string version, string configuration)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Register"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetRegisterMetadata, response));
        }

        /// <summary>
        /// Добавить метаданные указанного документа
        /// </summary>
        /// <returns>Результат добавления метаданных указанного документа</returns>
        public dynamic InsertDocument(dynamic documentMetadata, string version, string configuration)
        {
            var response = RequestExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Document"), documentMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertDocumentMetadata, response));
        }

        /// <summary>
        /// Обновить метаданные указанного документа
        /// </summary>
        /// <returns>Результат обновления метаданных документа</returns>
        public dynamic UpdateDocument(dynamic documentMetadata, string version, string configuration)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Document"), documentMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateDocumentMetadata, response));
        }

        /// <summary>
        /// Создать метаданные документа
        /// </summary>
        public dynamic CreateDocument(string version, string configuration)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Document"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateDocumentMetadata, response));
        }

        /// <summary>
        /// Удалить метаданные указанного документа
        /// </summary>
        /// <returns>Результат удаления метаданных документа</returns>
        public dynamic DeleteDocument(string version, string configuration, string name)
        {
            var response = RequestExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlMetadataElementById(version, configuration, "Document", name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteDocumentMetadata, response));
        }

        /// <summary>
        /// Получить метаданные документа
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="name">Наименование документа</param>
        /// <returns>Метаданные документа</returns>
        public dynamic GetDocument(string version, string configuration, string name)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataElementById(version, configuration, "Document", name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetDocumentMetadata, response));
        }

        /// <summary>
        /// Получить метаданные всех документов конфигурации
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <returns>Метаданные документа</returns>
        public dynamic GetDocumentList(string version, string configuration)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Document"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetDocumentMetadata, response));
        }

        /// <summary>
        /// Добавить метаданные указанного сценария
        /// </summary>
        /// <returns>Результат добавления метаданных указанного сценария</returns>
        public dynamic InsertScenario(dynamic scenarioMetadata, string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "Scenario"), scenarioMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertScenarioMetadata, response));
        }

        /// <summary>
        /// Обновить метаданные указанного сценария
        /// </summary>
        /// <returns>Результат обновления метаданных сценария</returns>
        public dynamic UpdateScenario(dynamic scenarioMetadata, string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "Scenario"), scenarioMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateScenarioMetadata, response));
        }

        /// <summary>
        /// Создать метаданные сценария
        /// </summary>
        public dynamic CreateScenario(string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "Scenario"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateScenarioMetadata, response));
        }

        /// <summary>
        /// Удалить метаданные указанного сценария
        /// </summary>
        /// <returns>Результат удаления метаданных сценария</returns>
        public dynamic DeleteScenario(string version, string configuration, string document, string scenario)
        {
            var response = RequestExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "Scenario", scenario));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteScenarioMetadata, response));
        }

        /// <summary>
        /// Получить метаданные сценария
        /// </summary>
        public dynamic GetScenario(string version, string configuration, string document, string scenario)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "Scenario", scenario));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetScenarioMetadata, response));
        }

        /// <summary>
        /// Получить метаданные всех сценариев
        /// </summary>
        public dynamic GetScenarioItems(string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementList(version, configuration, document, "Scenario"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetScenarioMetadata, response));
        }

        /// <summary>
        /// Добавить метаданные указанного бизнес-процесса
        /// </summary>
        /// <returns>Результат добавления метаданных указанного бизнес-процесса</returns>
        public dynamic InsertProcess(dynamic processMetadata, string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "Process"), processMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertProcessMetadata, response));
        }

        /// <summary>
        /// Обновить метаданные указанного бизнес-процесса
        /// </summary>
        /// <returns>Результат обновления метаданных бизнес-процесса</returns>
        public dynamic UpdateProcess(dynamic processMetadata, string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "Process"), processMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateProcessMetadata, response));
        }

        /// <summary>
        /// Создать метаданные бизнес-процесса
        /// </summary>
        public dynamic CreateProcess(string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "Process"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateProcessMetadata, response));
        }

        /// <summary>
        /// Удалить метаданные указанного бизнес-процесса
        /// </summary>
        /// <returns>Результат удаления метаданных бизнес-процесса</returns>
        public dynamic DeleteProcess(string version, string configuration, string document, string process)
        {
            var response = RequestExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "Process", process));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteProcessMetadata, response));
        }

        /// <summary>
        /// Получить метаданные бизнес-процесса
        /// </summary>
        public dynamic GetProcess(string version, string configuration, string document, string process)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "Process", process));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetProcessMetadata, response));
        }

        /// <summary>
        /// Получить метаданные всех бизнес-процессов конфигурации
        /// </summary>
        public dynamic GetProcessItems(string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementList(version, configuration, document, "Process"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetProcessMetadata, response));
        }

        /// <summary>
        /// Добавить метаданные указанного сервиса
        /// </summary>
        /// <returns>Результат добавления метаданных указанного сервиса</returns>
        public dynamic InsertService(dynamic serviceMetadata, string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "Service"), serviceMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertServiceMetadata, response));
        }

        /// <summary>
        /// Обновить метаданные указанного сервиса
        /// </summary>
        /// <returns>Результат обновления метаданных сервиса</returns>
        public dynamic UpdateService(dynamic serviceMetadata, string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "Service"), serviceMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateServiceMetadata, response));
        }

        /// <summary>
        /// Создать метаданные сервиса
        /// </summary>
        public dynamic CreateService(string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "Service"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateServiceMetadata, response));
        }

        /// <summary>
        /// Удалить метаданные указанного сервиса
        /// </summary>
        /// <returns>Результат удаления метаданных сервиса</returns>
        public dynamic DeleteService(string version, string configuration, string document, string service)
        {
            var response = RequestExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "Service", service));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteServiceMetadata, response));
        }

        /// <summary>
        /// Получить метаданные сервиса
        /// </summary>
        public dynamic GetService(string version, string configuration, string document, string service)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "Service", service));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetServiceMetadata, response));
        }

        /// <summary>
        /// Получить метаданные сервис конфигурации
        /// </summary>
        public dynamic GetServiceItems(string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementList(version, configuration, document, "Service"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetServiceMetadata, response));
        }

        /// <summary>
        /// Добавить метаданные указанного представления
        /// </summary>
        /// <returns>Результат добавления метаданных указанного представления</returns>
        public dynamic InsertView(dynamic viewMetadata, string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "View"), viewMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertViewMetadata, response));
        }

        /// <summary>
        /// Обновить метаданные указанного представления
        /// </summary>
        /// <returns>Результат обновления метаданных представления</returns>
        public dynamic UpdateView(dynamic viewMetadata, string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "View"), viewMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateViewMetadata, response));
        }

        /// <summary>
        /// Создать метаданные представления
        /// </summary>
        public dynamic CreateView(string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "View"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateViewMetadata, response));
        }

        /// <summary>
        /// Удалить метаданные указанного представления
        /// </summary>
        /// <returns>Результат удаления метаданных представления</returns>
        public dynamic DeleteView(string version, string configuration, string document, string view)
        {
            var response = RequestExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "View", view));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteViewMetadaa, response));
        }

        /// <summary>
        /// Получить метаданные представления
        /// </summary>
        public dynamic GetView(string version, string configuration, string document, string view)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "View", view));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetViewMetadata, response));
        }

        /// <summary>
        /// Получить список метаданных представлений конфигурации
        /// </summary>
        public dynamic GetViewItems(string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementList(version, configuration, document, "View"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetViewMetadata, response));
        }

        /// <summary>
        /// Добавить метаданные указанного представления печатной формы
        /// </summary>
        /// <returns>Результат добавления метаданных указанного представления печатной формы</returns>
        public dynamic InsertPrintView(dynamic printViewMetadata, string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "PrintView"), printViewMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertPrintViewMetadata, response));
        }

        /// <summary>
        /// Обновить метаданные указанного представления печатной формы
        /// </summary>
        /// <returns>Результат обновления метаданных представления печатной формы</returns>
        public dynamic UpdatePrintView(dynamic printViewMetadata, string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "PrintView"), printViewMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdatePrintViewMetaata, response));
        }

        /// <summary>
        /// Удалить метаданные указанного представления печатной формы
        /// </summary>
        /// <returns>Результат удаления метаданных представления печатной формы</returns>
        public dynamic DeletePrintView(string version, string configuration, string document, string printView)
        {
            var response = RequestExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "PrintView", printView));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeletePrintViewMetadata, response));
        }

        /// <summary>
        /// Получить метаданные представления печатной формы
        /// </summary>
        public dynamic GetPrintView(string version, string configuration, string document, string printView)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "PrintView", printView));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetPrintViewMetadata, response));
        }

        /// <summary>
        /// Получить метаданные представления печатной формы
        /// </summary>
        public dynamic GetPrintViewItems(string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementList(version, configuration, document, "PrintView"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetPrintViewMetadata, response));
        }

        /// <summary>
        /// Создать метаданные печатного представления
        /// </summary>
        public dynamic CreatePrintView(string version, string configuration, string document)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "PrintView"));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdatePrintViewMetaata, response));
        }
    }
}