using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ViewDesigner
{
    internal sealed class ViewDesignerElementBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var editor = new ViewDesignerElement(parent);
            editor.ApplyElementMeatadata((object) metadata);

            // Привязка к источнику данных

            IElementDataBinding valueBinding = context.Build(parent, metadata.Value);

            if (valueBinding != null)
            {
                valueBinding.OnPropertyValueChanged += (c, a) => editor.SetValue(a.Value);
                editor.OnValueChanged += (c, a) => valueBinding.SetPropertyValue(a.Value);

                var sourceValueBinding = valueBinding as ISourceDataBinding;

                if (sourceValueBinding != null)
                {
                    // Передача контекста редактору

                    var dataSourceName = sourceValueBinding.GetDataSource();
                    var dataSource = parent.GetDataSource(dataSourceName);

                    editor.SetConfigId(dataSource.GetConfigId);
                    editor.SetDocumentId(dataSource.GetDocumentId);
                }
            }

            return editor;
        }
    }
}