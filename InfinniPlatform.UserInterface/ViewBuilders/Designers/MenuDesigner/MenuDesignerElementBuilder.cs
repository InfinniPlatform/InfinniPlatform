using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.MenuDesigner
{
	sealed class MenuDesignerElementBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var menuDesigner = new MenuDesignerElement(parent);
			menuDesigner.ApplyElementMeatadata((object)metadata);

			// Редактор элементов меню
			var menuItemEditor = context.Build(parent, metadata.Editor);
			menuDesigner.SetEditor(menuItemEditor);

			// Привязка к источнику данных представления

			IElementDataBinding itemsDataBinding = context.Build(parent, metadata.Items);

			if (itemsDataBinding != null)
			{
				itemsDataBinding.OnPropertyValueChanged += (c, a) => menuDesigner.SetItems(a.Value);
				menuDesigner.OnUpdateItems += (c, a) => itemsDataBinding.SetPropertyValue(a.Value);
			}

			return menuDesigner;
		}
	}
}