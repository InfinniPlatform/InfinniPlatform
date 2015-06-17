using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Registers;
using InfinniPlatform.Api.RestApi.DataApi;

using System;
using System.Linq;

namespace InfinniPlatform.SystemConfig.Configurator.RegisterQueries
{
    /// <summary>
    /// Получение даты последнего подсчета итогов для регистра накоплений, ближайшей к заданной
    /// </summary>
    public sealed class ActionUnitGetClosestDateTimeOfTotalCalculation
    {
        public void Action(IApplyResultContext target)
        {
            var requestDate = target.Item.Date;
            string configurationId = target.Item.Configuration.ToString();
            string registerId = target.Item.Register.ToString();

            // В таблице итогов нужно найти итог, ближайший к requestDate
            var dateToReturn = new DateTime();

            var min = long.MaxValue;
            var isDateFound = false;

            var page = 0;

            while (true)
            {
                // Постранично считываем данные и таблицы итогов и ищем итоги с датой, ближайшей к заданной
                var totals = target.Context.GetComponent<DocumentApi>(target.Version).GetDocument(
                    configurationId,
                    RegisterConstants.RegisterTotalNamePrefix + registerId,
                    f => f.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsLessThan(requestDate)),
                    page++,
                    10000).ToArray();

                if (totals.Length == 0)
                {
                    break;
                }

                foreach (var docWithTotals in totals)
                {
                    if (docWithTotals.DocumentDate != null)
                    {
                        if (Math.Abs(requestDate.Ticks - docWithTotals.DocumentDate.Ticks) < min)
                        {
                            min = docWithTotals.DocumentDate.Ticks - requestDate.Ticks;
                            dateToReturn = docWithTotals.DocumentDate;
                            isDateFound = true;
                        }
                    }
                }
            }

            target.Result = isDateFound ? new {Date = dateToReturn} : null;
        }
    }
}
