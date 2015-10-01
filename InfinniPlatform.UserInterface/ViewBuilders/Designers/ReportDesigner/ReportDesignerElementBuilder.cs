using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ReportDesigner
{
    internal sealed class ReportDesignerElementBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var editor = new ReportDesignerElement(parent);
            editor.ApplyElementMeatadata((object) metadata);

            // Привязка к источнику данных

            IElementDataBinding valueBinding = context.Build(parent, metadata.Value);

            if (valueBinding != null)
            {
                valueBinding.OnPropertyValueChanged += (c, a) => editor.SetValue(a.Value);
                editor.OnValueChanged += (c, a) => valueBinding.SetPropertyValue(a.Value, force: true);
            }

            return editor;
        }
    }
}