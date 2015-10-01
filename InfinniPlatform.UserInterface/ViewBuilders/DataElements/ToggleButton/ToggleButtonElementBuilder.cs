using System;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.ToggleButton
{
    internal sealed class ToggleButtonElementBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var toggleButton = new ToggleButtonElement(parent);
            toggleButton.ApplyElementMeatadata((object) metadata);
            toggleButton.SetReadOnly(Convert.ToBoolean(metadata.ReadOnly));

            if (metadata.TextOn != null)
            {
                toggleButton.SetTextOn(metadata.TextOn);
            }

            if (metadata.TextOff != null)
            {
                toggleButton.SetTextOff(metadata.TextOff);
            }

            // Привязка к источнику данных

            IElementDataBinding valueBinding = context.Build(parent, metadata.Value);

            if (valueBinding != null)
            {
                valueBinding.OnPropertyValueChanged += (c, a) => toggleButton.SetValue(a.Value);
                toggleButton.OnValueChanged += (c, a) => valueBinding.SetPropertyValue(a.Value);
            }

            return toggleButton;
        }
    }
}