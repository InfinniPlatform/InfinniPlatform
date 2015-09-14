using InfinniPlatform.UserInterface.ViewBuilders.Data.DataProviders;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataSources
{
    internal sealed class ObjectDataSourceBuilder : BaseDataSourceBuilder
    {
        protected override IDataSource CreateDataSource(View parent, dynamic metadata)
        {
            var metadataProvider = new ObjectDataProvider(metadata.IdProperty, metadata.Items);

            return new DataSource(parent, metadata.IdProperty, metadataProvider);
        }
    }
}