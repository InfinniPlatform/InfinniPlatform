using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders
{
	/// <summary>
	/// Контекст для построения объекта по метаданным.
	/// </summary>
	sealed class ObjectBuilderContext
	{
		/// <summary>
		/// Главное представление приложения.
		/// </summary>
		public View AppView { get; set; }


		private readonly Dictionary<string, IObjectBuilder> _objectBuilders
			= new Dictionary<string, IObjectBuilder>();


		/// <summary>
		/// Зарегистрировать построитель.
		/// </summary>
		/// <param name="metadataType">Тип метаданных объекта.</param>
		/// <param name="objectBuilder">Реализация построителя объекта.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public void Register(string metadataType, IObjectBuilder objectBuilder)
		{
			if (string.IsNullOrEmpty(metadataType))
			{
				throw new ArgumentNullException("metadataType");
			}

			if (objectBuilder == null)
			{
				throw new ArgumentNullException("objectBuilder");
			}

			_objectBuilders.Add(metadataType, objectBuilder);
		}


		/// <summary>
		/// Построить объект по метаданным.
		/// </summary>
		/// <param name="parentView">Родительское представление.</param>
		/// <param name="metadataValue">Метаданные объекта.</param>
		public object Build(View parentView, dynamic metadataValue)
		{
			if (metadataValue is IDynamicMetaObjectProvider)
			{
				foreach (var property in metadataValue)
				{
					return Build(parentView, property.Key, property.Value);					
				}
			}

			return null;
		}

		/// <summary>
		/// Построить объект по метаданным.
		/// </summary>
		/// <param name="parentView">Родительское представление.</param>
		/// <param name="metadataType">Тип метаданных объекта.</param>
		/// <param name="metadataValue">Метаданные объекта.</param>
		public object Build(View parentView, string metadataType, dynamic metadataValue)
		{
			if (metadataValue != null)
			{
				IObjectBuilder builder;

				if (_objectBuilders.TryGetValue(metadataType, out builder))
				{
					return builder.Build(this, parentView, metadataValue);
				}
			}

			return null;
		}


		/// <summary>
		/// Построить набор объектов по набору метаданных.
		/// </summary>
		/// <param name="parentView">Родительское представление.</param>
		/// <param name="metadataValues">Метаданные объектов.</param>
		public IEnumerable BuildMany(View parentView, IEnumerable metadataValues)
		{
			if (metadataValues != null)
			{
				var items = new List<object>();

				foreach (var metadataValue in metadataValues)
				{
					var item = Build(parentView, metadataValue);

					if (item != null)
					{
						items.Add(item);
					}
				}

				return items;
			}

			return null;
		}

		/// <summary>
		/// Построить набор объектов по набору метаданных.
		/// </summary>
		/// <param name="parentView">Родительское представление.</param>
		/// <param name="metadataType">Тип метаданных объектов.</param>
		/// <param name="metadataValues">Метаданные объектов.</param>
		public IEnumerable BuildMany(View parentView, string metadataType, IEnumerable metadataValues)
		{
			if (metadataValues != null)
			{
				var items = new List<object>();

				foreach (var metadataValue in metadataValues)
				{
					var item = Build(parentView, metadataType, metadataValue);

					if (item != null)
					{
						items.Add(item);
					}
				}

				return items;
			}

			return null;
		}
	}
}