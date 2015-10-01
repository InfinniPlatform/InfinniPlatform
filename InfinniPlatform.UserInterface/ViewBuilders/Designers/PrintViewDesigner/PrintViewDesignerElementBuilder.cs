using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.PrintViewDesigner
{
    internal sealed class PrintViewDesignerElementBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var printViewDesigner = new PrintViewDesignerElement(parent);
            printViewDesigner.ApplyElementMeatadata((object) metadata);

            // Привязка к источнику данных представления

            IElementDataBinding valueBinding = context.Build(parent, metadata.Value);

            if (valueBinding != null)
            {
                valueBinding.OnPropertyValueChanged += (c, a) => printViewDesigner.SetValue(a.Value);
                printViewDesigner.OnValueChanged += (c, a) => valueBinding.SetPropertyValue(a.Value, force: true);
            }

            return printViewDesigner;
        }
    }
}