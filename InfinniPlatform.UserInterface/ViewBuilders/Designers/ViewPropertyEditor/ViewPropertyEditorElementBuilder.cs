using System.Collections.Generic;

using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Images;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ViewPropertyEditor
{
	sealed class ViewPropertyEditorElementBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var element = new ViewPropertyEditorElement(parent);
			element.ApplyElementMeatadata((object)metadata);

			// Редакторы свойств

			if (metadata.Editors != null)
			{
				var editors = new List<PropertyEditor>();

				foreach (var editor in metadata.Editors)
				{
					editors.Add(new PropertyEditor
								{
									Text = editor.Text,
									Image = ImageRepository.GetImage(editor.Image),
									Property = editor.Property,
									PropertyType = editor.PropertyType,
									PropertyValueType = editor.PropertyValueType,
									EditView = context.Build(parent, editor.EditView)
								});
				}

				element.SetEditors(editors);
			}

			// Привязка к источнику данных

			IElementDataBinding valueBinding = context.Build(parent, metadata.Value);

			if (valueBinding != null)
			{
				valueBinding.OnPropertyValueChanged += (c, a) => element.SetValue(a.Value);
				element.OnValueChanged += (c, a) => valueBinding.SetPropertyValue(a.Value, force: true);
			}

			return element;
		}
	}
}