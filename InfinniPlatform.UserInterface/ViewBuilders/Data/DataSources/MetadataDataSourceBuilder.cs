using InfinniPlatform.UserInterface.ViewBuilders.Data.DataProviders;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataSources
{
    internal sealed class MetadataDataSourceBuilder : BaseDataSourceBuilder
    {
        private readonly string _server;
        private readonly int _port;

        public MetadataDataSourceBuilder(string server, int port)
        {
            _server = server;
            _port = port;
        }

        protected override IDataSource CreateDataSource(View parent, dynamic metadata)
        {
            var metadataProvider = new MetadataProvider(metadata.MetadataType,_server,_port);

            return new DataSource(parent, "Name", metadataProvider);
        }
    }
}