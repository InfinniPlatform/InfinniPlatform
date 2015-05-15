using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Hosting;

namespace InfinniPlatform.Modules
{
	/// <summary>
	///   Контракт регистрации шаблонов обработчиков
	/// </summary>
	public interface ITemplateInstaller
	{
		/// <summary>
		///   Зарегистрировать шаблоны в конфигурации
		/// </summary>
		void RegisterTemplates();
	}
}
