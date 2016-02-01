using System.Collections;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders
{
    internal static class ObjectBuilderContextExtensions
    {
        public static View BuildView(this ObjectBuilderContext context, View parentView, dynamic metadataValue)
        {
            return context.Build(parentView, "View", metadataValue);
        }

        public static IEnumerable BuildScripts(this ObjectBuilderContext context, View parentView, dynamic metadataValue)
        {
            return context.Build(parentView, "Scripts", metadataValue);
        }

        public static IEnumerable BuildParameters(this ObjectBuilderContext context, View parentView,
            dynamic metadataValue)
        {
            return context.BuildMany(parentView, "Parameter", metadataValue);
        }

        public static IEnumerable BuildTabPages(this ObjectBuilderContext context, View parentView,
            dynamic metadataValue)
        {
            return context.BuildMany(parentView, "TabPage", metadataValue);
        }

        public static IElementDataBinding BuildOneWayBinding(this ObjectBuilderContext context, View parentView,
            object element, string elementProperty, dynamic bindingMetadata)
        {
            var propertyBinding = context.Build(parentView, bindingMetadata) as IElementDataBinding;
            var setPropertyMethod = "Set" + elementProperty;

            object setPropertyResult;

            if (propertyBinding != null)
            {
                propertyBinding.OnPropertyValueChanged +=
                    (c, a) => element.InvokeMember(setPropertyMethod, new object[] {a.Value}, out setPropertyResult);
            }
            else
            {
                element.InvokeMember(setPropertyMethod, new object[] {bindingMetadata}, out setPropertyResult);
            }

            return propertyBinding;
        }
    }
}