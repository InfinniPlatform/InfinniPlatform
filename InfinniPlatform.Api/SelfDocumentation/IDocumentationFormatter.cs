using System.Collections.Generic;

namespace InfinniPlatform.Api.SelfDocumentation
{
    public interface IDocumentationFormatter
    {
        /// <summary>
        ///     Завершить форматирование целостного документа
        /// </summary>
        /// <param name="configuration">Имя конфигурации</param>
        /// <param name="content">Содержимое, которое необходимо оформить в виде документа</param>
        /// <returns>Отформатированный документ</returns>
        string CompleteDocumentFormatting(string configuration, string content);

        /// <summary>
        ///     Отформатировать серию запросов, объединенных общим заголовком
        /// </summary>
        /// <param name="header">Заголовок для блока справочной информации</param>
        /// <param name="info">Непосредственно запросы для форматирования</param>
        /// (указывается только если имя конфигурации одинаково для
        /// всех передаваемых запросов)
        /// <returns>Отформатированная справочная информация</returns>
        string FormatQueries(string header, IEnumerable<RestQueryInfo> info);
    }
}