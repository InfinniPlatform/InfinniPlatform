namespace InfinniPlatform.FastReport.TemplatesFluent.Data
{
	public static class DataExtensions
	{
		/// <summary>
		/// Зарегистрировать источник данных
		/// </summary>
		/// <typeparam name="T">Тип сущности источника данных</typeparam>
		/// <param name="target">Интерфейс для регистрации источников данных</param>
		public static DataSourcesConfig Register<T>(this DataSourcesConfig target)
		{
			return target.Register<T>(typeof(T).Name);
		}

		/// <summary>
		/// Зарегистрировать источник данных
		/// </summary>
		/// <typeparam name="T">Тип сущности источника данных</typeparam>
		/// <param name="target">Интерфейс для регистрации источников данных</param>
		/// <param name="name">Наименование источника данных</param>
		public static DataSourcesConfig Register<T>(this DataSourcesConfig target, string name)
		{
			var dataSourceType = typeof(T);

			return target.Register(name, d => d.Schema(dataSourceType));
		}
	}
}