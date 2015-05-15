using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Metadata;

namespace InfinniPlatform.Factories
{
	/// <summary>
	///   Конструктор фабрик прикладных скриптов
	/// </summary>
	public interface IScriptFactoryBuilder
	{
	    /// <summary>
	    ///   Создать фабрику прикладных скриптов для указанной версии конфигурации
	    /// </summary>
	    /// <returns>Фабрика скриптов</returns>
        IScriptFactory BuildScriptFactory(string metadataConfigurationId);
	}
}
