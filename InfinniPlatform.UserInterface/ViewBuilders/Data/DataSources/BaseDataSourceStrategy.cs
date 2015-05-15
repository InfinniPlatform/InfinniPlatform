using System.Collections;

using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataSources
{
	/// <summary>
	/// Стратегия режима работы источника данных представления.
	/// </summary>
	abstract class BaseDataSourceStrategy
	{
		protected BaseDataSourceStrategy(IDataSource dataSource)
		{
			// Конечно, не самая лучшая идея передавать сюда сам источник данных, но на данном этапе так оказалось проще

			DataSource = dataSource;
		}


		protected readonly IDataSource DataSource;


		public abstract void OnPageNumberChanged(dynamic value);
		public abstract void OnPageSizeChanged(dynamic value);
		public abstract void OnSelectedItemChanged(dynamic value);
		public abstract void OnPropertyFiltersChanged(dynamic value);
		public abstract void OnTextFilterChanged(dynamic value);
		public abstract void OnItemSaved(dynamic value);
		public abstract void OnItemDeleted(dynamic value);
		public abstract void OnItemsUpdated(dynamic value);
		public abstract void OnError(dynamic value);

		public abstract IEnumerable GetItems(IDataProvider dataProvider);

		protected void InvokeEvent(ScriptDelegate handler, dynamic value)
		{
			DataSource.InvokeScript(handler, args =>
				{
					args.DataSource = DataSource.GetName();
					args.Value = value;
				});
		}
	}
}