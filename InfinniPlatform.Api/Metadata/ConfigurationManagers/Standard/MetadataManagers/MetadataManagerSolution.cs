using System;

using InfinniPlatform.Api.Deprecated;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
    public sealed class MetadataManagerSolution : IDataManager
    {
        public MetadataManagerSolution(IDataReader metadataReader)
        {
            _metadataReader = metadataReader;
        }

        private readonly IDataReader _metadataReader;

        /// <summary>
        /// Сформировать предзаполненный объект метаданных
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Предзаполненный объект метаданных</returns>
        public dynamic CreateItem(string name)
        {
            return MetadataBuilderExtensions.BuildSolution(name, name, name);
        }

        /// <summary>
        /// Добавить метаданные объекта конфигурации
        /// </summary>
        /// <param name="objectToCreate">Метаданные создаваемого объекта</param>
        public void InsertItem(dynamic objectToCreate)
        {
            //изменяемая конфигурация - либо сохраненная конфигурация, либо вновь создаваемая
            var updatingSolution = MetadataExtensions.GetStoredMetadata(_metadataReader, objectToCreate);


            //если решение существует и идентификатор не совпадает, то происходит попытка
            //создания дубликата существующего решения
            if (updatingSolution != null &&
                (objectToCreate.Id == null || objectToCreate.Id != updatingSolution.Id))
            {
                throw new ApplicationException(string.Format("Solution with name {0} already exists", objectToCreate.Name));
            }

            objectToCreate = ((object)objectToCreate).ToDynamic();
            if (string.IsNullOrEmpty(objectToCreate.Name))
            {
                throw new ArgumentException();
            }

            if (updatingSolution != null)
            {
                DeleteItem(updatingSolution);
            }
            SetSolution(objectToCreate.Name, objectToCreate);
        }

        /// <summary>
        /// Удалить метаданные указанного объекта в указанной конфигурации
        /// </summary>
        /// <param name="metadataObject"></param>
        public void DeleteItem(dynamic metadataObject)
        {
            var solutionHeader =
                MetadataExtensions.GetStoredMetadata(
                    new MetadataReaderSolution(), metadataObject);

            if (solutionHeader != null)
            {
                RestQueryApi.QueryPostJsonRaw("SystemConfig", "solutionmetadata", "deletemetadata", solutionHeader.Name, new
                                                                                                                         {
                                                                                                                             Version = ""
                                                                                                                         });
            }
        }

        /// <summary>
        /// Обновить метаданные указанного объекта  в указанной конфигурации
        /// </summary>
        /// <param name="objectToCreate">Метаданные создаваемого объекта метаданных</param>
        public void MergeItem(dynamic objectToCreate)
        {
            InsertItem(objectToCreate);
        }

        private void SetSolution(string name, dynamic metadataSolution)
        {
            RestQueryApi.QueryPostRaw("SystemConfig", "solutionmetadata", "changemetadata", name, metadataSolution).ToDynamic();
        }
    }
}