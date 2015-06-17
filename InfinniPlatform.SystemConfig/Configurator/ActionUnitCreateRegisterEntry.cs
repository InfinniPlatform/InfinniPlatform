﻿using System;
using System.IO;
using System.Linq;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Registers;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    /// Точка расширения для создания "заготовки" записи регистра
    /// </summary>
    public sealed class ActionUnitCreateRegisterEntry
    {
        public void Action(IApplyResultContext target)
        {
            string configuration = target.Item.Configuration;
            string registerId = target.Item.RegisterId;
            string documentId = target.Item.DocumentId;
            var sourceDocument = target.Item.SourceDocument;
            
            if (string.IsNullOrEmpty(configuration))
            {
                throw new ArgumentException("ConfigurationId name should be specified via 'configuration' property");
            }

            if (string.IsNullOrEmpty(registerId))
            {
                throw new ArgumentException("RegisterId should be specified via 'Register' property");
            }

            if (string.IsNullOrEmpty(documentId))
            {
                throw new ArgumentException("documentId should be specified via 'DocumentId' property");
            }

            if (sourceDocument == null)
            {
                throw new ArgumentException("Document entry should be specified via 'SourceDocument' property");
            }

            dynamic registerEntry = new DynamicWrapper();

            registerEntry[RegisterConstants.RegistrarProperty] = sourceDocument.Id;
            registerEntry[RegisterConstants.RegistrarTypeProperty] = documentId;
            registerEntry[RegisterConstants.EntryTypeProperty] = RegisterEntryType.Other;

            var documentDate = new DateTime();
            
            if (target.Item.DocumentDate != null)
            {
                documentDate = target.Item.DocumentDate;
            }
            else
            {
                // Дата документа явно не задана, используем дату из содержимого переданного документа
                var defaultProcess = target.Context.GetComponent<IMetadataComponent>(target.Version).GetMetadata(target.Version, configuration, documentId, MetadataType.Process, "Default");
                var customProcess = target.Context.GetComponent<IMetadataComponent>(target.Version).GetMetadata(target.Version, configuration, documentId, MetadataType.Process, "Custom");

                if (defaultProcess != null &&
                    defaultProcess.Transitions != null &&
                    defaultProcess.Transitions.Count > 0 &&
                    defaultProcess.Transitions[0].RegisterPoint != null)
                {
                    var dateFieldName = defaultProcess.Transitions[0].RegisterPoint.DocumentDateProperty;

                    if (!string.IsNullOrEmpty(dateFieldName))
                    {
                        documentDate = sourceDocument[dateFieldName];
                    }
                }
                else if (customProcess != null &&
                         customProcess.Transitions != null &&
                         customProcess.Transitions.Count > 0 &&
                         customProcess.Transitions[0].RegisterPoint != null)
                {
                    var dateFieldName = customProcess.Transitions[0].RegisterPoint.DocumentDateProperty;
                    
                    if (!string.IsNullOrEmpty(dateFieldName))
                    {
                        documentDate = sourceDocument[dateFieldName];
                    }
                }
            }

            var registerMetadata = target.Context.GetComponent<IMetadataComponent>(target.Version).GetMetadataList(target.Version, configuration, registerId, MetadataType.Register).FirstOrDefault();
            
            // Признак того, что необходимо создать запись для регистра сведений
            if (target.Item.IsInfoRegister == true && registerMetadata != null)
            {
                documentDate = RegisterPeriod.AdjustDateToPeriod(documentDate, registerMetadata.Period);
            }

            registerEntry[RegisterConstants.DocumentDateProperty] = documentDate;

            target.Result = registerEntry;
        }
    }
}
