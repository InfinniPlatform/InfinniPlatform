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

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    ///     Точка расширения для проведения данных документа в регистр
    /// </summary>
    public sealed class ActionUnitPostRegisterEntries
    {
        public void Action(IApplyContext target)
        {
            string configuration = target.Item.Configuration;
            string register = target.Item.Register;
            dynamic registerEntries = target.Item.RegisterEntries;
            string version = target.Context.GetVersion(configuration, target.UserName);

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

            var registerMetadata =
                target.Context.GetComponent<IMetadataComponent>()
                      .GetMetadataList(version, configuration, register, MetadataType.Register)
                      .FirstOrDefault();

            var dimensionNames = new List<string>();

            foreach (var property in registerMetadata.Properties)
            {
                if (property.Type == RegisterPropertyType.Dimension)
                {
                    dimensionNames.Add(property.Property);
                }
            }

            foreach (dynamic registerEntry in registerEntries)
            {
                // Id генерируется по следующему алгоритму:
                // формируется уникальный ключ записи по всем полям-измерениям и по полю даты,
                // далее из уникального ключа рассчитывается Guid записи

                if (registerEntry[RegisterConstants.DocumentDateProperty] == null)
                {
                    throw new ArgumentException("Property 'DocumentDate' should be in the registry entry");
                }

                string uniqueKey = registerEntry[RegisterConstants.DocumentDateProperty].ToString();

                foreach (string dimensionName in dimensionNames)
                {
                    if (registerEntry[dimensionName] != null)
                    {
                        uniqueKey += registerEntry[dimensionName].ToString();
                    }
                }

                using (MD5 md5 = MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(uniqueKey));
                    registerEntry.Id = new Guid(hash).ToString();
                }
            }

            target.Context.GetComponent<DocumentApi>()
                  .SetDocuments(configuration, RegisterConstants.RegisterNamePrefix + register, registerEntries);
        }
    }
}