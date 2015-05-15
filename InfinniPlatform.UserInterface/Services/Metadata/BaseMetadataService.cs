using InfinniPlatform.Api.Metadata;
using System;
using System.Collections;
using System.Threading;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.UserInterface.Dynamic;
using InfinniPlatform.UserInterface.Properties;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Базовый класс сервисов для работы с метаданными системы.
	/// </summary>
	abstract class BaseMetadataService : IMetadataService
	{
		protected BaseMetadataService()
		{
			_dataReader = new Lazy<IDataReader>(CreateDataReader, LazyThreadSafetyMode.ExecutionAndPublication);
			_dataManager = new Lazy<IDataManager>(CreateDataManager, LazyThreadSafetyMode.ExecutionAndPublication);
		}

		private readonly Lazy<IDataReader> _dataReader;
		private readonly Lazy<IDataManager> _dataManager;

		protected abstract IDataReader CreateDataReader();
		protected abstract IDataManager CreateDataManager();


		public virtual object CreateItem()
		{
			return _dataManager.Value.CreateItem(string.Empty);
		}

		public virtual void ReplaceItem(object item)
		{
			_dataManager.Value.MergeItem(item);
		}

		public virtual void DeleteItem(string itemId)
		{
		    var item = _dataReader.Value.GetItem(itemId);

		    if (item != null)
		    {
		        _dataManager.Value.DeleteItem(item);
		    }
		}

		public virtual object GetItem(string itemId)
		{
			return _dataReader.Value.GetItem(itemId);
		}

		public virtual object CloneItem(string itemId)
		{
			// Todo: Избавиться от этой конвертации после того, как в системе будет одна реализация Dynamic

			var item = _dataReader.Value.GetItem(itemId);

			if (item != null)
			{
				item.Id = Guid.NewGuid().ToString();
				item.Name = string.Format(Resources.CloneElementName, item.Name);
			}

			return DynamicExtensions.JsonToObject(item);
		}

		public virtual IEnumerable GetItems()
		{
			return _dataReader.Value.GetItems();
		}
	}
}