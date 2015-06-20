using System;
using InfinniPlatform.Api.Extensions;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.Label
{
    internal sealed class LabelElementBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var label = new LabelElement(parent);

            // Установка формата данных
            var valueFormat = context.Build(parent, metadata.Format);
            label.SetFormat(valueFormat);
            label.ApplyElementMeatadata((object) metadata);
            label.SetLineCount(Convert.ToInt32(metadata.LineCount));
            label.SetHorizontalTextAlignment(
                (metadata.HorizontalTextAlignment as string).ToEnum(HorizontalTextAlignment.Left));

            // Привязка к источнику данных

            IElementDataBinding valueBinding = context.Build(parent, metadata.Value);

            if (valueBinding != null)
            {
                valueBinding.OnPropertyValueChanged += (c, a) => label.SetValue(a.Value);
            }

            return label;
        }
    }
}