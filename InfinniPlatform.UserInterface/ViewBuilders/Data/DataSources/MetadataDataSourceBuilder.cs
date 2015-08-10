using InfinniPlatform.UserInterface.ViewBuilders.Data.DataProviders;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataSources
{
    internal sealed class MetadataDataSourceBuilder : BaseDataSourceBuilder
    {
        private readonly string _server;
        private readonly int _port;
        private readonly string _routeVersion;

        public MetadataDataSourceBuilder(string server, int port, string routeVersion)
        {
            _server = server;
            _port = port;
            _routeVersion = routeVersion;
        }

        protected override IDataSource CreateDataSource(View parent, dynamic metadata)
        {
            var metadataProvider = new MetadataProvider(metadata.MetadataType,_server,_port, _routeVersion);

            return new DataSource(parent, "Name", metadataProvider);
        }
    }
}