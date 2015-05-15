using System;
using System.Collections;
using System.Collections.Generic;

using FastReport;

namespace InfinniPlatform.FastReport.ReportObjectBuilders
{
	/// <summary>
	/// Контекст построителя объекта отчета FastReport.
	/// </summary>
	sealed class FrReportObjectBuilderContext : IReportObjectBuilderContext
	{
		/// <summary>
		/// Построенный отчет.
		/// </summary>
		public object Report { get; set; }


		private readonly Dictionary<Type, int> _typeNameIdentity
			= new Dictionary<Type, int>();

		private readonly Dictionary<Type, Action<object, object>> _builders
			= new Dictionary<Type, Action<object, object>>();


		/// <summary>
		/// Создать объект отчета.
		/// </summary>
		/// <typeparam name="T">Тип объекта отчета.</typeparam>
		public T CreateObject<T>() where T : new()
		{
			var objectType = typeof(T);
			var objectInstance = new T();

			var objectInstanceAsBase = objectInstance as Base;

			if (objectInstanceAsBase != null)
			{
				int identity;

				_typeNameIdentity.TryGetValue(objectType, out identity);
				_typeNameIdentity[objectType] = ++identity;

				objectInstanceAsBase.Name = (objectType.Name + identity);
			}

			return objectInstance;
		}

		/// <summary>
		/// Построить объект отчета по шаблону.
		/// </summary>
		/// <param name="template">Шаблон объекта отчета.</param>
		/// <param name="parent">Родительский объект отчета.</param>
		public void BuildObject(object template, object parent)
		{
			if (template != null)
			{
				Action<object, object> builder;

				if (_builders.TryGetValue(template.GetType(), out builder))
				{
					builder(template, parent);
				}
			}
		}

		/// <summary>
		/// Построить объекты отчета по шаблонам.
		/// </summary>
		/// <param name="templates">Список шаблонов объекта отчета.</param>
		/// <param name="parent">Родительский объект отчета.</param>
		public void BuildObjects(IEnumerable templates, object parent)
		{
			if (templates != null)
			{
				foreach (var template in templates)
				{
					BuildObject(template, parent);
				}
			}
		}


		/// <summary>
		/// Зарегистрировать построитель объекта отчета.
		/// </summary>
		/// <typeparam name="TTemplate">Шаблон объекта отчета.</typeparam>
		/// <param name="builder">Построитель объекта отчета.</param>
		public void RegisterBuilder<TTemplate>(IReportObjectBuilder<TTemplate> builder)
		{
			_builders.Add(typeof(TTemplate), (template, parent) => builder.BuildObject(this, (TTemplate)template, parent));
		}
	}
}