using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.SystemConfig.Properties;

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    ///   Удалить конфигурацию
    /// </summary>
    public sealed class ActionUnitDeleteConfig
    {
        public void Action(IApplyContext target)
        {
            //наименование должно быть уникальным
            var config = target.Item.ConfigurationName;

            IEnumerable<dynamic> deleteConfig = DynamicWrapperExtensions.ToEnumerable(
                new DocumentApi().GetDocument("system", "configuration", f => f.AddCriteria(c => c.Property("Id").IsEquals(config)), 0, 1));

            if (!deleteConfig.Any())
            {
                target.IsValid = false;
                target.ValidationMessage = string.Format(Resources.SpecifiedConfigurationNameNotFound,target.Item.ConfigurationName);
                return;
            }

            new DocumentApi().DeleteDocument("system", "configuration", deleteConfig.First().Id);
        }
    }
}
