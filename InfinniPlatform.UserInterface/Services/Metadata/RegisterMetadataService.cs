using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    /// Сервис для работы с метаданными документов.
    /// </summary>
    internal sealed class RegisterMetadataService : BaseMetadataService
    {
        public RegisterMetadataService(string configId)
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

            File.WriteAllBytes(PackageMetadataLoader.GetRegisterPath(ConfigId, item.Name), serializedItem);

            PackageMetadataLoader.UpdateCache();
        }

        public override void DeleteItem(string itemId)
        {
            var registerDirectory = Path.GetDirectoryName(PackageMetadataLoader.GetRegisterPath(ConfigId, itemId));

            if (registerDirectory != null)
            {
                Directory.Delete(registerDirectory, true);
                PackageMetadataLoader.UpdateCache();
            }
        }

        public override object GetItem(string itemId)
        {
            return PackageMetadataLoader.GetRegister(ConfigId, itemId);
        }

        public override IEnumerable<object> GetItems()
        {
            return PackageMetadataLoader.GetRegisters(ConfigId);
        }
    }
}