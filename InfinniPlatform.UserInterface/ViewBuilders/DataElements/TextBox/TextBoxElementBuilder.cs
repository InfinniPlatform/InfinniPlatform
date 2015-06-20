using System;
using InfinniPlatform.Api.Extensions;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.TextBox
{
    internal sealed class TextBoxElementBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var textBox = new TextBoxElement(parent);

            // Установка формата данных
            var valueFormat = context.Build(parent, metadata.Format);
            textBox.SetFormat(valueFormat);
            textBox.ApplyElementMeatadata((object) metadata);
            textBox.SetPlaceholder(metadata.Placeholder);
            textBox.SetReadOnly(Convert.ToBoolean(metadata.ReadOnly));
            textBox.SetMultiline(Convert.ToBoolean(metadata.Multiline));
            textBox.SetLineCount(Convert.ToInt32(metadata.LineCount));
            textBox.SetHorizontalTextAlignment(
                (metadata.HorizontalTextAlignment as string).ToEnum(HorizontalTextAlignment.Left));
            textBox.SetVerticalTextAlignment(
                (metadata.VerticalTextAlignment as string).ToEnum(VerticalTextAlignment.Center));

            // Привязка к источнику данных

            IElementDataBinding valueBinding = context.Build(parent, metadata.Value);

            if (valueBinding != null)
            {
                valueBinding.OnPropertyValueChanged += (c, a) => textBox.SetValue(a.Value);
                textBox.OnValueChanged += (c, a) => valueBinding.SetPropertyValue(a.Value);
            }

            return textBox;
        }
    }
}