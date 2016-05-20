using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Documents.Obsolete;

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
            string registerName,
            string documentId,
            DateTime? documentDate,
            dynamic document,
            bool isInfoRegister);

        /// <summary>
        /// Выполняет проведение данных документа в регистр.
        /// </summary>
        void PostEntries(
            string registerName,
            IEnumerable<object> registerEntries);

        /// <summary>
        /// Выполняет перепроведение документов до указанной даты.
        /// </summary>
        void RecarryingEntries(
            string registerName,
            DateTime aggregationDate,
            bool deteleExistingRegisterEntries = true
            );

        /// <summary>
        /// Рассчитывает итоги для регистров накопления на текущую дату.
        /// </summary>
        void RecalculateTotals();

        /// <summary>
        /// Удаляет запись регистра.
        /// </summary>
        void DeleteEntry(
            string registerName,
            string registar);

        /// <summary>
        /// Возвращает записи регистра.
        /// </summary>
        IEnumerable<object> GetEntries(
            string registerName,
            IEnumerable<FilterCriteria> filter,
            int pageNumber,
            int pageSize);

        /// <summary>
        /// Возвращает записи регистра.
        /// </summary>
        IEnumerable<object> GetValuesByDate(
            string registerName,
            DateTime aggregationDate,
            IEnumerable<FilterCriteria> filter = null,
            IEnumerable<string> dimensionsProperties = null,
            IEnumerable<string> valueProperties = null,
            IEnumerable<AggregationType> aggregationTypes = null);

        /// <summary>
        /// Возвращает значения ресурсов в указанном диапазоне дат для регистра.
        /// </summary>
        IEnumerable<object> GetValuesBetweenDates(
            string registerName,
            DateTime beginDate,
            DateTime endDate,
            IEnumerable<FilterCriteria> filter = null,
            IEnumerable<string> dimensionsProperties = null,
            IEnumerable<string> valueProperties = null,
            IEnumerable<AggregationType> aggregationTypes = null);

        /// <summary>
        /// Возвращает значения ресурсов в указанном диапазоне дат c разбиением на периоды.
        /// </summary>
        [Obsolete("It does not work currently! For more details see comments in implementation. Should be fix after refactoring.")]
        IEnumerable<object> GetValuesByPeriods(string registerName, DateTime beginDate, DateTime endDate, string interval, IEnumerable<FilterCriteria> filter, IEnumerable<string> dimensionsProperties = null, IEnumerable<string> valueProperties = null);

        /// <summary>
        /// Получение значений ресурсов по документу-регистратору.
        /// </summary>
        IEnumerable<object> GetValuesByRegistrar(
            string registerName,
            string registrar,
            IEnumerable<string> dimensionsProperties = null,
            IEnumerable<string> valueProperties = null);

        /// <summary>
        /// Получение значений ресурсов по типу документа-регистратора.
        /// </summary>
        IEnumerable<object> GetValuesByRegistrarType(
            string registerName,
            string registrar,
            IEnumerable<string> dimensionsProperties = null,
            IEnumerable<string> valueProperties = null);

        /// <summary>
        /// Получение значений из таблицы итогов на дату, ближайшую к заданной
        /// </summary>
        IEnumerable<object> GetTotals(
            string registerName,
            DateTime aggregationDate);

        /// <summary>
        /// Возвращает дату последнего подсчета итогов для регистра накоплений, ближайшей к заданной.
        /// </summary>
        DateTime? GetClosestDateTimeOfTotalCalculation(
            string registerName,
            DateTime aggregationDate);
    }
}