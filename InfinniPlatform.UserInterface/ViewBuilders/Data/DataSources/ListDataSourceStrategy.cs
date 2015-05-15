using System.Collections;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataSources
{
	/// <summary>
	/// Стратегия режима работы источника данных для представлений со списком элементов. 
	/// </summary>
	sealed class ListDataSourceStrategy : BaseDataSourceStrategy
	{
		public ListDataSourceStrategy(IDataSource dataSource)
			: base(dataSource)
		{
		}


		public override void OnPageNumberChanged(dynamic value)
		{
			InvokeEvent(DataSource.OnPageNumberChanged, value);
		}

		public override void OnPageSizeChanged(dynamic value)
		{
			InvokeEvent(DataSource.OnPageSizeChanged, value);
		}

		public override void OnSelectedItemChanged(dynamic value)
		{
			InvokeEvent(DataSource.OnSelectedItemChanged, value);
		}

		public override void OnPropertyFiltersChanged(dynamic value)
		{
			InvokeEvent(DataSource.OnPropertyFiltersChanged, value);
		}

		public override void OnTextFilterChanged(dynamic value)
		{
			InvokeEvent(DataSource.OnTextFilterChanged, value);
		}

		public override void OnItemSaved(dynamic value)
		{
			InvokeEvent(DataSource.OnItemSaved, value);
		}

		public override void OnItemDeleted(dynamic value)
		{
			InvokeEvent(DataSource.OnItemDeleted, value);
		}

		public override void OnItemsUpdated(dynamic value)
		{
			InvokeEvent(DataSource.OnItemsUpdated, value);
		}

		public override void OnError(dynamic value)
		{
			InvokeEvent(DataSource.OnError, value);
		}

		public override IEnumerable GetItems(IDataProvider dataProvider)
		{
			var criterias = DataSource.GetPropertyFilters(); // Todo
			var pageNumber = DataSource.GetPageNumber();
			var pageSize = DataSource.GetPageSize();

			return dataProvider.GetItems(criterias, pageNumber, pageSize);
		}
	}
}