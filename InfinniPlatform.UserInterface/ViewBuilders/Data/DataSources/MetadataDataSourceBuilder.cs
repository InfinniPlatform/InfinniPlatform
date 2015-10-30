using InfinniPlatform.UserInterface.ViewBuilders.Data.DataProviders;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataSources
{
    internal sealed class MetadataDataSourceBuilder : BaseDataSourceBuilder
    {
	    protected override IDataSource CreateDataSource(View parent, dynamic metadata)
        {
            var metadataProvider = new MetadataProvider(metadata.MetadataType);

            return new DataSource(parent, "Name", metadataProvider);
        }
    }
}