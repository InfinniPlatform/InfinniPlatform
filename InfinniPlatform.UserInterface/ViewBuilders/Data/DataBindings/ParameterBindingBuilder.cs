using InfinniPlatform.UserInterface.ViewBuilders.Parameter;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataBindings
{
    internal sealed class ParameterBindingBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var dataBinding = new ParameterBinding(parent, metadata.Parameter, metadata.Property);

            ParameterElement dataSource = parent.GetParameter(metadata.Parameter);

            if (dataSource != null)
            {
                dataSource.AddDataBinding(dataBinding);
            }

            return dataBinding;
        }
    }
}