using System.Collections.Generic;

using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.DocumentSchemaDesigner
{
	sealed class DocumentSchemaDesignerElementBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var schemaDesigner = new DocumentSchemaDesignerElement(parent);
			schemaDesigner.ApplyElementMeatadata((object)metadata);

			// Редакторы свойств модели данных

			if (metadata.Editors != null)
			{
				var editors = new List<ItemEditor>();

				foreach (var editor in metadata.Editors)
				{
					editors.Add(new ItemEditor
								{
									Text = editor.Text,
									Image = editor.Image,
									Container = editor.Container,
									MetadataType = editor.MetadataType,
									LinkView = context.Build(parent, editor.LinkView)
								});
				}

				schemaDesigner.SetEditors(editors);
			}

			// Привязка к источнику данных представления

			IElementDataBinding valueBinding = context.Build(parent, metadata.Value);

			if (valueBinding != null)
			{
				valueBinding.OnPropertyValueChanged += (c, a) => schemaDesigner.SetValue(a.Value);
				schemaDesigner.OnValueChanged += (c, a) => valueBinding.SetPropertyValue(a.Value);
			}

			return schemaDesigner;
		}
	}
}