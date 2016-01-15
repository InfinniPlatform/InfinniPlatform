using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Sdk.Registers
{
    /// <summary>
    /// Предоставляет методы для работы с регистрами.
    /// </summary>
    public interface IRegisterApi
    {
        /// <summary>
        /// Создает (но не сохраняет) запись регистра.
        /// </summary>
        dynamic CreateEntry(
            string configuration,
            string registerName,
            string documentId,
            DateTime? documentDate,
            dynamic document,
            bool isInfoRegister);

        /// <summary>
        /// Выполняет проведение данных документа в регистр.
        /// </summary>
        void PostEntries(
            string configuration,
            string registerName,
            IEnumerable<object> registerEntries);

        /// <summary>
        /// Выполняет перепроведение документов до указанной даты.
        /// </summary>
        void RecarryingEntries(
            string configuration,
            string registerName,
            DateTime aggregationDate,
            bool deteleExistingRegisterEntries = true
            );

        /// <summary>
        /// Рассчитывает итоги для регистров накопления на текущую дату.
        /// </summary>
        void RecalculateTotals(
            string configuration,
            string registerName);

        /// <summary>
        /// Удаляет запись регистра.
        /// </summary>
        void DeleteEntry(
            string configuration,
            string registerName,
            string registar);

        /// <summary>
        /// Возвращает записи регистра.
        /// </summary>
        IEnumerable<object> GetEntries(
            string configuration,
            string registerName,
            IEnumerable<FilterCriteria> filter,
            int pageNumber,
            int pageSize);

        /// <summary>
        /// Возвращает записи регистра.
        /// </summary>
        IEnumerable<object> GetValuesByDate(
            string configuration,
            string registerName,
            DateTime aggregationDate,
            IEnumerable<FilterCriteria> filter,
            IEnumerable<string> dimensionsProperties,
            IEnumerable<string> valueProperties,
            IEnumerable<AggregationType> aggregationTypes);

        /// <summary>
        /// Возвращает значения ресурсов в указанном диапазоне дат для регистра.
        /// </summary>
        IEnumerable<object> GetValuesBetweenDates(
            string configuration,
            string registerName,
            DateTime beginDate,
            DateTime endDate,
            IEnumerable<FilterCriteria> filter,
            IEnumerable<string> dimensionsProperties = null,
            IEnumerable<string> valueProperties = null,
            IEnumerable<AggregationType> aggregationTypes = null);

        /// <summary>
        /// Возвращает значения ресурсов в указанном диапазоне дат c разбиением на периоды.
        /// </summary>
        IEnumerable<object> GetValuesByPeriods(
            string configuration,
            string registerName,
            DateTime beginDate,
            DateTime endDate,
            string interval,
            IEnumerable<FilterCriteria> filter,
            IEnumerable<string> dimensionsProperties = null,
            IEnumerable<string> valueProperties = null,
            string timezone = null);

        /// <summary>
        /// Получение значений ресурсов по документу-регистратору.
        /// </summary>
        IEnumerable<object> GetValuesByRegistrar(
            string configuration,
            string registerName,
            string registrar,
            IEnumerable<string> dimensionsProperties = null,
            IEnumerable<string> valueProperties = null);

        /// <summary>
        /// Получение значений ресурсов по типу документа-регистратора.
        /// </summary>
        IEnumerable<object> GetValuesByRegistrarType(
            string configuration,
            string registerName,
            string registrar,
            IEnumerable<string> dimensionsProperties = null,
            IEnumerable<string> valueProperties = null);

        /// <summary>
        /// Получение значений из таблицы итогов на дату, ближайшую к заданной
        /// </summary>
        IEnumerable<object> GetTotals(
            string configuration,
            string registerName,
            DateTime aggregationDate);

        /// <summary>
        /// Возвращает дату последнего подсчета итогов для регистра накоплений, ближайшей к заданной.
        /// </summary>
        DateTime? GetClosestDateTimeOfTotalCalculation(
            string configuration,
            string registerName,
            DateTime aggregationDate);
    }
}