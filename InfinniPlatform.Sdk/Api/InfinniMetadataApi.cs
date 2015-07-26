using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Metadata;
using InfinniPlatform.Sdk.Properties;

namespace InfinniPlatform.Sdk.Api
{
    public class InfinniMetadataApi : BaseApi
    {
        public InfinniMetadataApi(string server, string port) : base(server, port)
        {
        }

        /// <summary>
        ///   Добавить метаданные указанного решения
        /// </summary>
        /// <returns>Результат добавления метаданных решения</returns>
        public dynamic InsertSolution(SolutionMetadata solutionMetadata)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlMetadataSolution(), solutionMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertSolutionMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Обновить метаданные указанного решения
        /// </summary>
        /// <returns>Результат обновления метаданных решения</returns>
        public dynamic UpdateSolution(SolutionMetadata solutionMetadata)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataSolution(), solutionMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertSolutionMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Удалить метаданные указанного решения
        /// </summary>
        /// <returns>Результат удаления метаданных решения</returns>
        public dynamic DeleteSolution(string version, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlMetadataSolutionById(version,name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteSolutionMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Получить метаданные решения по уазанному идентификатору для указанной версии
        /// </summary>
        /// <param name="version">Версия приложения</param>
        /// <param name="name">Наименование приложения</param>
        /// <returns>Метаданные решения</returns>
        public dynamic GetSolution(string version, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataSolutionById(version, name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertSolutionMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Добавить метаданные указанной конфигурации
        /// </summary>
        /// <returns>Результат добавления метаданных конфигурации</returns>
        public dynamic InsertConfig(ConfigurationMetadata configMetadata)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlMetadataConfig(), configMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertConfigMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Обновить метаданные указанной конфигурации
        /// </summary>
        /// <returns>Результат обновления метаданных конфигурации</returns>
        public dynamic UpdateConfig(ConfigurationMetadata configMetadata)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataConfig(), configMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateConfigMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Удалить метаданные указанной конфигурации
        /// </summary>
        /// <returns>Результат удаления метаданных конфигурации</returns>
        public dynamic DeleteConfig(string version, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlMetadataConfigById(version, name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteConfigMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Получить метаданные конфигурации по уазанному идентификатору для указанной версии
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="name">Наименование конфигурации</param>
        /// <returns>Метаданные конфигурации</returns>
        public dynamic GetConfig(string version, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataConfigById(version, name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetConfigMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Добавить метаданные указанного меню
        /// </summary>
        /// <returns>Результат добавления метаданных указанного меню</returns>
        public dynamic InsertMenu(MenuMetadata menuMetadata, string version, string configuration)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlMetadataMenu(version, configuration), menuMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertMenuMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Обновить метаданные указанного меню
        /// </summary>
        /// <returns>Результат обновления метаданных меню</returns>
        public dynamic UpdateMenu(MenuMetadata menuMetadata, string version, string configuration)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataMenu(version, configuration), menuMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateMenuMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Удалить метаданные указанного меню
        /// </summary>
        /// <returns>Результат удаления метаданных меню</returns>
        public dynamic DeleteMenu(string version, string configuration, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlMetadataMenuById(version,configuration, name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteMenuMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Получить метаданные меню по указанному идентификатору
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="name">Наименование конфигурации</param>
        /// <returns>Метаданные конфигурации</returns>
        public dynamic GetMenu(string version, string configuration, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataMenuById(version, configuration, name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetMenuMetadata, response.GetErrorContent()));
        }

    }
}
