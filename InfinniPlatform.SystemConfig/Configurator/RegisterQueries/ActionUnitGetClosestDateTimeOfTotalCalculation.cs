using System;
using System.Linq;
using InfinniPlatform.Api.Registers;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Register;

using FilterBuilder = InfinniPlatform.Api.SearchOptions.Builders.FilterBuilder;

namespace InfinniPlatform.SystemConfig.Configurator.RegisterQueries
{
    /// <summary>
    ///     Получение даты последнего подсчета итогов для регистра накоплений, ближайшей к заданной
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

            long min = long.MaxValue;
            bool isDateFound = false;

            int page = 0;

            while (true)
            {
                // Постранично считываем данные и таблицы итогов и ищем итоги с датой, ближайшей к заданной
                Action<FilterBuilder> filter = f => f.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty)
                                                                        .IsLessThan(requestDate));

                var totals = target.Context.GetComponent<DocumentApi>().GetDocument(configurationId, RegisterConstants.RegisterTotalNamePrefix + registerId, filter, page++, 10000)
                                   .ToArray();

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