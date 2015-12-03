using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    /// Сервис для работы с метаданными сборок.
    /// </summary>
    internal sealed class AssemblyMetadataService : BaseMetadataService
    {
        public AssemblyMetadataService(string configId)
        {
            ConfigId = configId;
        }

        public string ConfigId { get; }

        public override object CreateItem()
        {
            dynamic assembly = new DynamicWrapper();

            assembly.Id = Guid.NewGuid().ToString();
            assembly.Name = string.Empty;
            assembly.Caption = string.Empty;
            assembly.Description = string.Empty;

            return assembly;
        }

        public override void ReplaceItem(dynamic item)
        {
            throw new NotSupportedException();
        }

        public override void DeleteItem(string itemId)
        {
            throw new NotSupportedException();
        }

        public override object GetItem(string itemId)
        {
            throw new NotSupportedException();
        }

        public override IEnumerable<object> GetItems()
        {
            throw new NotSupportedException();
        }
    }
}