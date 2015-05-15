using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.RestApi.CommonApi;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsMetadataDataSource
{
    public sealed class ActionUnitGetMenuMetadata
    {
        public void Action(IApplyResultContext target)
        {
	        IEnumerable<dynamic> menuList = target.Context.GetComponent<IMetadataComponent>().GetMetadata(target.Item.ConfigId, "Common", MetadataType.Menu);			

            target.Result = menuList.FirstOrDefault();
            
            var service = target.Context.GetComponent<IMetadataComponent>()
                                .GetMetadata(target.Item.ConfigId, "Common", MetadataType.Service, "FilterMenu");

            if (service != null)
            {
                var filteredMenu = RestQueryApi.QueryPostJsonRaw(target.Item.ConfigId, "Common", "FilterMenu", null,
                                                                           target.Result).ToDynamic();

                if (filteredMenu != null)
                {
                    target.Result = filteredMenu;
                }
            }

        }
    }
}
