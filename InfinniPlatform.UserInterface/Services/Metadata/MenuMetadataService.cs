using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    /// Сервис для работы с метаданными меню.
    /// </summary>
    internal sealed class MenuMetadataService : BaseMetadataService
    {
        public MenuMetadataService(string configId)
        {
            ConfigId = configId;
        }

        public string ConfigId { get; }

        public override object CreateItem()
        {
            return new DynamicWrapper();
        }

        public override void ReplaceItem(dynamic item)
        {
            var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

            File.WriteAllBytes(PackageMetadataLoader.GetMenuPath(ConfigId, item.Name), serializedItem);

            PackageMetadataLoader.UpdateCache();
        }

        public override void DeleteItem(string itemId)
        {
            var menuDirectory = Path.GetDirectoryName(PackageMetadataLoader.GetMenuPath(ConfigId, itemId));

            if (menuDirectory != null)
            {
                Directory.Delete(menuDirectory, true);
                PackageMetadataLoader.UpdateCache();
            }
        }

        public override object GetItem(string itemId)
        {
            return PackageMetadataLoader.GetMenuContent(ConfigId, itemId);
        }

        public override IEnumerable<object> GetItems()
        {
            return PackageMetadataLoader.GetMenus(ConfigId);
        }
    }
}