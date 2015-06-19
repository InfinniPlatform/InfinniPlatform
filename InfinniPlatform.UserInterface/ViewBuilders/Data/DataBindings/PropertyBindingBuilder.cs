using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataBindings
{
    internal sealed class PropertyBindingBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var dataBinding = new PropertyBinding(parent, metadata.DataSource, metadata.Property);

            IDataSource dataSource = parent.GetDataSource(metadata.DataSource);

            if (dataSource != null)
            {
                dataSource.AddDataBinding(dataBinding);
            }

            return dataBinding;
        }
    }
}