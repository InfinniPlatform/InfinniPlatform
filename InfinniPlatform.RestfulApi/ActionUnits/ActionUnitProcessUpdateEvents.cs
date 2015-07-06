using System.Linq;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitProcessUpdateEvents
    {
        public void Action(IProcessEventContext target)
        {
            var aggregateProvider = new AggregateProvider();

            dynamic item = null;

            item = aggregateProvider.CreateAggregate();

            dynamic document = null;

            if (!string.IsNullOrEmpty(target.Id))
            {
                document = target.Context.GetComponent<DocumentApi>().GetDocument(target.Id);
                if (document != null)
                {
                    item = document;
                }
            }


            if (target.Configuration.ToLowerInvariant() != "systemconfig" && document != null)
            {
                var eventsDocument =
                    target.Events.Where(e => e.Property.ToLowerInvariant().StartsWith("document.")).ToList();
                foreach (var eventDefinition in eventsDocument)
                {
                    eventDefinition.Property = eventDefinition.Property.Substring("document.".Length);
                }

                aggregateProvider.ApplyChanges(ref document, eventsDocument);
                target.Events =
                    target.Events.Except(eventsDocument)
                          .Where(e => e.Property.ToLowerInvariant() != "document")
                          .ToList();

                item.Id = document.Id;
                item.Document = document;
            }

            //устанавливаем результат обработки точки расширения
            target.Item = item;
        }
    }
}