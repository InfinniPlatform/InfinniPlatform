using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Runtime;

namespace InfinniPlatform.Metadata.Implementation.Handlers
{
	/// <summary>
	///   Обработчик системных изменений, выполняющий нотификацию слушателей изменений
	/// </summary>
	public sealed class SystemEventsHandler
	{
		private readonly IChangeListener _changeListener;

		public SystemEventsHandler(IChangeListener changeListener)
		{
			_changeListener = changeListener;
		}

		/// <summary>
		///   Уведомить слушателей об изменениях
		/// </summary>
		public void Notify(string metadataConfigurationId)
		{
		    if (_changeListener != null)
		    {
		        _changeListener.Invoke(metadataConfigurationId);
		    }
		}

	}
}
