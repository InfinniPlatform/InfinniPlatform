using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Metadata;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    public class SolutionMetadataService : BaseMetadataService
    {
        private InfinniMetadataApi _metadataApi;

        public SolutionMetadataService(string version,string server, int port, string route) : base(version, server, port, route)
        {
            _metadataApi = new InfinniMetadataApi(server,port.ToString(),route);
        }

        public override object CreateItem()
        {
            return _metadataApi.CreateSolution();
        }

        public override void ReplaceItem(dynamic item)
        {
            _metadataApi.UpdateSolution(item);
        }

        public override void DeleteItem(string itemId)
        {
            _metadataApi.DeleteSolution(Version,itemId);
        }

        public override object GetItem(string itemId)
        {
            return _metadataApi.GetSolution(Version, itemId);
        }

        public override IEnumerable<object> GetItems()
        {
            return _metadataApi.GetSolutionItems();
        }
    }
}
