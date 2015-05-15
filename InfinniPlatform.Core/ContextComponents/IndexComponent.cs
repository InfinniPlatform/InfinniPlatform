using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Factories;

namespace InfinniPlatform.ContextComponents
{
	/// <summary>
	///   Компонент для работы с индексами в глобальном контексте
	/// </summary>
	public sealed class IndexComponent : IIndexComponent
	{
		private readonly IIndexFactory _indexFactory;

		public IndexComponent(IIndexFactory indexFactory)
		{
			_indexFactory = indexFactory;
		}

		/// <summary>
		///   Фабрика для работы с индексами
		/// </summary>
		public IIndexFactory IndexFactory
		{
			get { return _indexFactory; }
		}
	}
}
