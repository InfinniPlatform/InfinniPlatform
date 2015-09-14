using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.ApiContracts
{
    /// <summary>
    ///   Контракт для работы с печатными формами
    /// </summary>
    public interface IPrintViewApi
    {
        /// <summary>
        ///  Получить печатное представление
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации (приложения)</param>
        /// <param name="documentId">Идентификатор типа документа</param>
        /// <param name="printViewId">Идентификатор печатной формы</param>
        /// <param name="printViewType">Тип печатной формы</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="filter">Фильтр данных</param>
        /// <returns>Сформированная печатная форма</returns>
        dynamic GetPrintView(string configId, string documentId, string printViewId, string printViewType,
            int pageNumber, int pageSize, Action<FilterBuilder> filter);
    }
}
