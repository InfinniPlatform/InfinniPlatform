using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Events;

namespace InfinniPlatform.Runtime.Implementation.Packages
{
	/// <summary>
	///   Пакет обновления
	/// </summary>
	public sealed class UpdatePackage
	{
		/// <summary>
		///   Заголовок пакета обновления
		/// </summary>
		public EventDefinition PackageHeader { get; set; }

		/// <summary>
		///   Заголовок версии обновления
		/// </summary>
		public EventDefinition VersionHeader { get; set; }

		/// <summary>
		///   Содержимое пакета обновления
		/// </summary>
		public EventDefinition PackageContent { get; set; }

        /// <summary>
        ///   Заголовок конфигурации обновления
        /// </summary>
	    public EventDefinition ConfigurationHeader { get; set; }

	    public IEnumerable<EventDefinition> GetEvents()
		{
			return new[] {PackageHeader,ConfigurationHeader,VersionHeader,PackageContent};
		}
	}
}
