using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Registers;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Register;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    /// <summary>
    /// Точка расширения для проведения данных документа в регистр
    /// </summary>
    public sealed class ActionUnitPostRegisterEntries
    {
        public ActionUnitPostRegisterEntries(IMetadataComponent metadataComponent, DocumentApi documentApi)
        {
            _metadataComponent = metadataComponent;
            _documentApi = documentApi;
        }

        private readonly DocumentApi _documentApi;
        private readonly IMetadataComponent _metadataComponent;

        public void Action(IApplyContext target)
        {
            string configuration = target.Item.Configuration;
            string register = target.Item.Register;
            dynamic registerEntries = target.Item.RegisterEntries;

            if (string.IsNullOrEmpty(configuration))
            {
                throw new ArgumentException("ConfigurationId name should be specified via 'configuration' property");
            }

            if (string.IsNullOrEmpty(register))
            {
                throw new ArgumentException("RegisterId should be specified via 'Register' property");
            }

            if (registerEntries == null)
            {
                throw new ArgumentException("Register entries should be specified via 'RegisterEntries' property");
            }

            var registerMetadata = _metadataComponent.GetMetadataList(configuration, register, MetadataType.Register).FirstOrDefault();

            var dimensionNames = new List<string>();

            foreach (var property in registerMetadata.Properties)
            {
                if (property.Type == RegisterPropertyType.Dimension)
                {
                    dimensionNames.Add(property.Property);
                }
            }

            foreach (var registerEntry in registerEntries)
            {
                // Id генерируется по следующему алгоритму:
                // формируется уникальный ключ записи по всем полям-измерениям и по полю даты,
                // далее из уникального ключа рассчитывается Guid записи

                if (registerEntry[RegisterConstants.DocumentDateProperty] == null)
                {
                    throw new ArgumentException("Property 'DocumentDate' should be in the registry entry");
                }

                string uniqueKey = registerEntry[RegisterConstants.DocumentDateProperty].ToString();

                foreach (var dimensionName in dimensionNames)
                {
                    if (registerEntry[dimensionName] != null)
                    {
                        uniqueKey += registerEntry[dimensionName].ToString();
                    }
                }

                using (var md5 = MD5.Create())
                {
                    var hash = md5.ComputeHash(Encoding.Default.GetBytes(uniqueKey));
                    registerEntry.Id = new Guid(hash).ToString();
                }
            }

            _documentApi.SetDocuments(configuration, RegisterConstants.RegisterNamePrefix + register, registerEntries);
        }
    }
}