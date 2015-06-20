using System;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.CheckBox
{
    internal sealed class CheckBoxElementBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var checkBox = new CheckBoxElement(parent);
            checkBox.ApplyElementMeatadata((object) metadata);
            checkBox.SetReadOnly(Convert.ToBoolean(metadata.ReadOnly));

            // Привязка к источнику данных

            IElementDataBinding valueBinding = context.Build(parent, metadata.Value);

            if (valueBinding != null)
            {
                valueBinding.OnPropertyValueChanged += (c, a) => checkBox.SetValue(a.Value);
                checkBox.OnValueChanged += (c, a) => valueBinding.SetPropertyValue(a.Value);
            }

            return checkBox;
        }
    }
}