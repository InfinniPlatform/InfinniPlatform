using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Parameter
{
    internal sealed class ParameterElementBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var element = new ParameterElement(parent);
            element.SetName(metadata.Name);

            IElementDataBinding valueBinding = context.Build(parent, metadata.Value);

            if (valueBinding != null)
            {
                valueBinding.OnPropertyValueChanged += (c, a) => element.SetValue(a.Value);
                element.OnValueChanged += (c, a) => valueBinding.SetPropertyValue(a.Value);
            }

            if (metadata.OnValueChanged != null)
            {
                element.OnValueChanged += parent.GetScript(metadata.OnValueChanged);
            }

            return element;
        }
    }
}