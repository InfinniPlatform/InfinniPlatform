using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi;

namespace InfinniConfiguration.SystemConfig.Configurator
{
	public sealed class ActionUnitGetRegisteredConfigList
	{
		public void Action(IApplyResultContext target)
		{
			//получаем список всех прикладных конфигураций в системе
			target.Result = new DynamicInstance();
			target.Result.ConfigList = ConfigurationApi.GetDocument("update", "configuration", null, 0, 1000).ToEnumerable();
		}
	}
}
