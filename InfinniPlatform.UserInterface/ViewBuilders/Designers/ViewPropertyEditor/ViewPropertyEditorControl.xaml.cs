using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.UserInterface.ViewBuilders.Actions;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ViewPropertyEditor
{
	public sealed partial class ViewPropertyEditorControl : UserControl
	{
		public ViewPropertyEditorControl()
		{
			InitializeComponent();

			Editor.ItemsSource = _propertyItems;
			EditorMenu.ItemsSource = _propertyMenuItems;
		}


		// PropertyEditors

		public static readonly DependencyProperty PropertyEditorsProperty = DependencyProperty.Register("PropertyEditors", typeof(IEnumerable<PropertyEditor>), typeof(ViewPropertyEditorControl), new FrameworkPropertyMetadata(null, OnPropertyEditorsChangedHandler));

		private static void OnPropertyEditorsChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var editor = d as ViewPropertyEditorControl;

			if (editor != null)
			{
				editor.RefreshPropertyEditorMenu(e.NewValue as IEnumerable<PropertyEditor>);
				editor.RefreshPropertyItems(editor.EditValue);
			}
		}

		/// <summary>
		/// Возвращает или устанавливает список редакторов свойств.
		/// </summary>
		public IEnumerable<PropertyEditor> PropertyEditors
		{
			get { return (IEnumerable<PropertyEditor>)GetValue(PropertyEditorsProperty); }
			set { SetValue(PropertyEditorsProperty, value); }
		}


		// EditValue

		public static readonly DependencyProperty EditValueProperty = DependencyProperty.Register("EditValue", typeof(object), typeof(ViewPropertyEditorControl), new FrameworkPropertyMetadata(null, OnEditValueChangedHandler));

		private static void OnEditValueChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var editor = d as ViewPropertyEditorControl;

			if (editor != null)
			{
				var oldProperty = editor.GetSelectedProperty();
				editor.RefreshPropertyItems(e.NewValue);

				var newProperty = (oldProperty != null) ? editor.FindProperty(oldProperty.Name) : null;
				editor.SetSelectedProperty(newProperty);

				editor.InvokeEditValueChanged(e.OldValue, e.NewValue);
			}
		}

		/// <summary>
		/// Возвращает или устанавливает значение.
		/// </summary>
		public object EditValue
		{
			get { return GetValue(EditValueProperty); }
			set { SetValue(EditValueProperty, value); }
		}


		// OnEditValueChanged

		public static readonly RoutedEvent EditValueChangedEvent = EventManager.RegisterRoutedEvent("EditValueChanged", RoutingStrategy.Bubble, typeof(ValueChangedRoutedEventHandler), typeof(ViewPropertyEditorControl));

		/// <summary>
		/// Событие окончания изменения значения.
		/// </summary>
		public event ValueChangedRoutedEventHandler EditValueChanged
		{
			add { AddHandler(EditValueChangedEvent, value); }
			remove { RemoveHandler(EditValueChangedEvent, value); }
		}

		private void InvokeEditValueChanged(object oldValue, object newValue)
		{
			RaiseEvent(new ValueChangedRoutedEventArgs(EditValueChangedEvent) { OldValue = oldValue, NewValue = newValue });
		}


		// ContextMenu

		private readonly ObservableCollection<MenuItem> _propertyMenuItems
			= new ObservableCollection<MenuItem>();

		private void RefreshPropertyEditorMenu(IEnumerable<PropertyEditor> propertyEditors)
		{
			var menuItems = new List<MenuItem>();

			if (propertyEditors != null)
			{
				foreach (var propertyEditor in propertyEditors)
				{
					var editor = propertyEditor;

					var menuItem = new MenuItem
								   {
									   Icon = new Image
											  {
												  Source = propertyEditor.Image
											  },
									   Header = propertyEditor.Text,
									   Tag = propertyEditor
								   };

					menuItem.Click += (sender, args) => AddProperty(editor);

					menuItems.Add(menuItem);
				}
			}

			_propertyMenuItems.Clear();

			foreach (var menuItem in menuItems)
			{
				_propertyMenuItems.Add(menuItem);
			}
		}


		// Items

		private readonly ObservableCollection<PropertyEditorItem> _propertyItems
			= new ObservableCollection<PropertyEditorItem>();

		private void RefreshPropertyItems(object editValue)
		{
			var propertyItems = new List<PropertyEditorItem>();

			if (editValue != null)
			{
				var propertyEditors = PropertyEditors;

				if (propertyEditors != null)
				{
					// Для каждого редактора свойства
					foreach (var propertyEditor in propertyEditors)
					{
						// Выборка значения свойства
						var propertyValue = editValue.GetProperty(propertyEditor.Property);

						if (propertyValue != null)
						{
							// Если свойство является коллекцией
							if (IsCollectionProperty(propertyEditor))
							{
								var propertyValueArray = propertyValue as IEnumerable;

								if (propertyValueArray != null)
								{
									// Добавление всех элементов коллекции
									foreach (dynamic item in propertyValueArray)
									{
										var unwrapPropertyValue = UnwrapPropertyValue(propertyEditor, item);

										if (unwrapPropertyValue != null)
										{
											var propertyItem = CreatePropertyItem(propertyEditor, unwrapPropertyValue);
											propertyItems.Add(propertyItem);
										}
									}
								}
							}
							else
							{
								var unwrapPropertyValue = UnwrapPropertyValue(propertyEditor, propertyValue);

								if (unwrapPropertyValue != null)
								{
									var propertyItem = CreatePropertyItem(propertyEditor, unwrapPropertyValue);
									propertyItems.Add(propertyItem);
								}
							}
						}
					}
				}
			}

			_propertyItems.Clear();

			foreach (var propertyItem in propertyItems)
			{
				_propertyItems.Add(propertyItem);
			}
		}


		// Add

		private void OnAddPropertyHandler(object sender, RoutedEventArgs e)
		{
			if (_propertyMenuItems.Count > 0)
			{
				if (_propertyMenuItems.Count == 1)
				{
					var propertyEditor = _propertyMenuItems[0].Tag as PropertyEditor;

					if (propertyEditor != null)
					{
						AddProperty(propertyEditor);
					}
				}
				else
				{
					EditorMenu.IsOpen = true;
				}
			}
		}

		private void AddProperty(PropertyEditor propertyEditor)
		{
			if (propertyEditor != null)
			{
				var propertyItem = CreatePropertyItem(propertyEditor, new DynamicWrapper());

				AddOrEditProperty(propertyItem, true);
			}
		}

		private bool InsertProperty(PropertyEditorItem propertyItem)
		{
			var propertyEditor = propertyItem.Editor;

			var insertPropertyFunc = IsCollectionProperty(propertyEditor)
				? InsertCollectionProperty(_propertyItems, propertyEditor)
				: InsertScalarProperty(_propertyItems, propertyEditor);

			return insertPropertyFunc(propertyItem);
		}

		private Func<PropertyEditorItem, bool> InsertCollectionProperty(ICollection<PropertyEditorItem> propertyItems, PropertyEditor propertyEditor)
		{
			return propertyItem =>
				   {
					   if (propertyItem != null)
					   {
						   var propertyValue = propertyItem.Item;

						   if (propertyValue != null)
						   {
							   var editValue = EditValue;

							   if (editValue != null)
							   {
								   // Добавление свойства в коллекцию редактируемого объекта

								   var collection = GetCollectionProperty(editValue, propertyEditor.Property, true);

								   if (collection != null)
								   {
									   var wrapPropertyValue = WrapPropertyValue(propertyEditor, propertyValue);
									   collection.AddItem(wrapPropertyValue);
								   }

								   // Добавление свойства в коллекцию визуального элемента
								   var newPropertyItem = CreatePropertyItem(propertyEditor, propertyValue);
								   propertyItems.AddItem(newPropertyItem);
								   SetSelectedProperty(newPropertyItem);

								   return true;
							   }
						   }
					   }

					   return false;
				   };
		}

		private Func<PropertyEditorItem, bool> InsertScalarProperty(ICollection<PropertyEditorItem> propertyItems, PropertyEditor propertyEditor)
		{
			return propertyItem =>
				   {
					   if (propertyItem != null)
					   {
						   var propertyValue = propertyItem.Item;

						   if (propertyValue != null)
						   {
							   dynamic editValue = EditValue;

							   if (editValue != null)
							   {
								   // Установка значения свойства редактируемого объекта
								   var wrapPropertyValue = WrapPropertyValue(propertyEditor, propertyValue);
								   editValue[propertyEditor.Property] = wrapPropertyValue;

								   // Добавление свойства в коллекцию визуального элемента
								   var newPropertyItem = CreatePropertyItem(propertyEditor, propertyValue);
								   propertyItems.AddItem(newPropertyItem);
								   SetSelectedProperty(newPropertyItem);

								   return true;
							   }
						   }
					   }

					   return false;
				   };
		}


		// Edit

		private void OnEditPropertyHandler(object sender, RoutedEventArgs e)
		{
			var propertyItem = GetSelectedProperty();

			EditProperty(propertyItem);
		}

		private void EditProperty(PropertyEditorItem propertyItem)
		{
			AddOrEditProperty(propertyItem, false);
		}

		private void AddOrEditProperty(PropertyEditorItem propertyItem, bool isNewPropertyItem)
		{
			if (propertyItem != null)
			{
				var editPropertyItem = isNewPropertyItem ? propertyItem : propertyItem.Clone();

				ViewHelper.ShowView(propertyItem,
									() => propertyItem.Editor.EditView,
									childDataSource => OnInitializeEditView(childDataSource, editPropertyItem),
									childDataSource => OnAcceptedEditView(isNewPropertyItem, propertyItem, editPropertyItem));
			}
		}

		private static void OnInitializeEditView(IDataSource childDataSource, PropertyEditorItem editPropertyItem)
		{
			childDataSource.SuspendUpdate();
			childDataSource.SetEditMode();
			childDataSource.ResumeUpdate();
			childDataSource.SetSelectedItem(editPropertyItem.Item);
		}

		private void OnAcceptedEditView(bool isNewPropertyItem, PropertyEditorItem propertyItem, PropertyEditorItem editPropertyItem)
		{
			var isEditValueChanged = isNewPropertyItem
				? InsertProperty(editPropertyItem)
				: UpdateProperty(propertyItem, editPropertyItem);

			if (isEditValueChanged)
			{
				InvokeEditValueChanged(EditValue, EditValue);
			}
		}

		private bool UpdateProperty(PropertyEditorItem oldPropertyItem, PropertyEditorItem newPropertyItem)
		{
			var propertyEditor = newPropertyItem.Editor;

			var updatePropertyFunc = IsCollectionProperty(propertyEditor)
				? UpdateCollectionProperty(_propertyItems, propertyEditor)
				: UpdateScalarProperty(_propertyItems, propertyEditor);

			return updatePropertyFunc(oldPropertyItem, newPropertyItem);
		}

		private Func<PropertyEditorItem, PropertyEditorItem, bool> UpdateCollectionProperty(ICollection<PropertyEditorItem> propertyItems, PropertyEditor propertyEditor)
		{
			return (oldPropertyItem, newPropertyItem) =>
				   {
					   if (oldPropertyItem != null && newPropertyItem != null)
					   {
						   var oldPropertyValue = oldPropertyItem.Item;
						   var newPropertyValue = newPropertyItem.Item;

						   if (oldPropertyValue != null && newPropertyValue != null)
						   {
							   var editValue = EditValue;

							   if (editValue != null)
							   {
								   // Обновление свойства в коллекции редактируемого объекта

								   var collection = GetCollectionProperty(editValue, propertyEditor.Property, false);

								   if (collection != null)
								   {
									   var wrapNewPropertyValue = WrapPropertyValue(propertyEditor, newPropertyValue);
									   ProcessCollection(collection, propertyEditor, oldPropertyValue, item => collection.ReplaceItem(item, wrapNewPropertyValue));
								   }

								   // Обновление свойства в коллекции визуального элемента
								   propertyItems.ReplaceItem(oldPropertyItem, newPropertyItem);
								   SetSelectedProperty(newPropertyItem);

								   return true;
							   }
						   }
					   }

					   return false;
				   };
		}

		private Func<PropertyEditorItem, PropertyEditorItem, bool> UpdateScalarProperty(ICollection<PropertyEditorItem> propertyItems, PropertyEditor propertyEditor)
		{
			return (oldPropertyItem, newPropertyItem) =>
				   {
					   if (oldPropertyItem != null && newPropertyItem != null)
					   {
						   var oldPropertyValue = oldPropertyItem.Item;
						   var newPropertyValue = newPropertyItem.Item;

						   if (oldPropertyValue != null && newPropertyValue != null)
						   {
							   dynamic editValue = EditValue;

							   if (editValue != null)
							   {
								   // Установка значения свойства редактируемого объекта
								   var wrapNewPropertyValue = WrapPropertyValue(propertyEditor, newPropertyValue);
								   editValue[propertyEditor.Property] = wrapNewPropertyValue;

								   // Обновление свойства в коллекции визуального элемента
								   propertyItems.ReplaceItem(oldPropertyItem, newPropertyItem);
								   SetSelectedProperty(newPropertyItem);

								   return true;
							   }
						   }
					   }

					   return false;
				   };
		}


		// Delete

		private void OnDeletePropertyHandler(object sender, RoutedEventArgs e)
		{
			var propertyItem = GetSelectedProperty();

			if (propertyItem != null && AcceptQuestion(Properties.Resources.PropertyEditorControlDeletePropertyQuestion, propertyItem.Name))
			{
				if (DeleteProperty(propertyItem))
				{
					SetSelectedProperty(null);
					InvokeEditValueChanged(EditValue, EditValue);
				}
			}
		}

		private bool DeleteProperty(PropertyEditorItem propertyItem)
		{
			var propertyEditor = propertyItem.Editor;

			var deletePropertyFunc = IsCollectionProperty(propertyEditor)
				? DeleteCollectionProperty(_propertyItems, propertyEditor)
				: DeleteScalarProperty(_propertyItems, propertyEditor);

			return deletePropertyFunc(propertyItem);
		}

		private Func<PropertyEditorItem, bool> DeleteCollectionProperty(ICollection<PropertyEditorItem> propertyItems, PropertyEditor propertyEditor)
		{
			return propertyItem =>
				   {
					   if (propertyItem != null)
					   {
						   var propertyValue = propertyItem.Item;

						   if (propertyValue != null)
						   {
							   var editValue = EditValue;

							   if (editValue != null)
							   {
								   // Удаление свойства из коллекции редактируемого объекта

								   var collection = GetCollectionProperty(editValue, propertyEditor.Property, false);

								   if (collection != null)
								   {
									   ProcessCollection(collection, propertyEditor, propertyValue, collection.RemoveItem);
								   }

								   // Удаление свойства из коллекции визуального элемента
								   propertyItems.RemoveItem(propertyItem);

								   return true;
							   }
						   }
					   }

					   return false;
				   };
		}

		private Func<PropertyEditorItem, bool> DeleteScalarProperty(ICollection<PropertyEditorItem> propertyItems, PropertyEditor propertyEditor)
		{
			return propertyItem =>
				   {
					   if (propertyItem != null)
					   {
						   var propertyValue = propertyItem.Item;

						   if (propertyValue != null)
						   {
							   dynamic editValue = EditValue;

							   if (editValue != null)
							   {
								   // Удаление значения свойства редактируемого объекта
								   editValue[propertyEditor.Property] = null;

								   // Удаление свойства из коллекции визуального элемента
								   propertyItems.RemoveItem(propertyItem);

								   return true;
							   }
						   }
					   }

					   return false;
				   };
		}


		private static PropertyEditorItem CreatePropertyItem(PropertyEditor propertyEditor, dynamic propertyValue)
		{
			return new PropertyEditorItem
				   {
					   Editor = propertyEditor,
					   Image = propertyEditor.Image,
					   Name = (propertyValue != null) ? propertyValue.Name : null,
					   Item = propertyValue
				   };
		}

		private static IList GetCollectionProperty(dynamic editValue, string propertyName, bool createIfNull)
		{
			var collection = editValue[propertyName];

			if (collection == null && createIfNull)
			{
				collection = new List<object>();
				editValue[propertyName] = collection;
			}

			return collection;
		}

		private static void ProcessCollection(IList collection, PropertyEditor propertyEditor, dynamic propertyValue, Action<object> action)
		{
			if (collection != null)
			{
				var valueType = propertyEditor.PropertyValueType;
				var noWrapValue = string.IsNullOrEmpty(valueType);

				foreach (dynamic item in collection)
				{
					if ((!noWrapValue && (item[valueType] == propertyValue)) || (noWrapValue && (item == propertyValue)))
					{
						action(item);
						break;
					}
				}
			}
		}

		private static object WrapPropertyValue(PropertyEditor propertyEditor, object propertyValue)
		{
			if (propertyValue != null)
			{
				var propertyValueType = propertyEditor.PropertyValueType;

				if (!string.IsNullOrEmpty(propertyValueType))
				{
					var wrapper = new DynamicWrapper();
					wrapper[propertyValueType] = propertyValue;
					propertyValue = wrapper;
				}
			}

			return propertyValue;
		}

		private static object UnwrapPropertyValue(PropertyEditor propertyEditor, dynamic propertyValue)
		{
			if (propertyValue != null)
			{
				var propertyValueType = propertyEditor.PropertyValueType;

				if (!string.IsNullOrEmpty(propertyValueType))
				{
					propertyValue = propertyValue[propertyValueType];
				}
			}

			return propertyValue;
		}

		private static bool IsCollectionProperty(PropertyEditor propertyEditor)
		{
			return string.Equals(propertyEditor.PropertyType, "Array", StringComparison.OrdinalIgnoreCase);
		}

		private static IDataSource GetMainDataSource(View editView)
		{
			var dataSources = editView.GetDataSources();

			return (dataSources != null) ? dataSources.FirstOrDefault() : null;
		}

		private PropertyEditorItem GetSelectedProperty()
		{
			return (Editor.EditValue as PropertyEditorItem);
		}

		private void SetSelectedProperty(PropertyEditorItem propertyItem)
		{
			Editor.EditValue = propertyItem;
		}

		private PropertyEditorItem FindProperty(string name)
		{
			return _propertyItems.FirstOrDefault(i => i.Name == name);
		}

		private static bool AcceptQuestion(string message, params object[] args)
		{
			return MessageBox.Show(string.Format(message, args), Properties.Resources.PropertyEditorControlCaption, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes;
		}
	}
}