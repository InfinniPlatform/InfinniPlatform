using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes;

namespace InfinniPlatform.Api.Hosting
{
	public interface IExtensionPointHandlerConfig
	{
		/// <summary>
		///   Инициализировать обработчик на основе конфигурации
		/// </summary>
		/// <param name="handler">Обработчик</param>
		void BuildHandler(IExtensionPointHandler handler);

		Dictionary<string, ContextTypeKind> WorkflowExtensionPoints { get; }
	}
}
