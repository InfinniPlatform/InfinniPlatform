using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Deprecated;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
    public sealed class MetadataManagerSolution : IDataManager
    {

        private readonly IDataReader _metadataReader;
        private readonly string _version;

        public MetadataManagerSolution(IDataReader metadataReader, string version)
        {
            _metadataReader = metadataReader;
            _version = version;
        }

        /// <summary>
        ///     Сформировать предзаполненный объект метаданных
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Предзаполненный объект метаданных</returns>
        public dynamic CreateItem(string name)
        {
            return MetadataBuilderExtensions.BuildSolution(name, name, name, _version);
        }

        /// <summary>
        ///     Добавить метаданные объекта конфигурации
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
            if (String.IsNullOrEmpty(objectToCreate.Name))
            {
                throw new ArgumentException();
            }

            if (updatingSolution != null)
            {
                DeleteItem(updatingSolution);
            }
            SetSolution(objectToCreate.Name, objectToCreate);

        }

        private void SetSolution(string name, dynamic metadataSolution)
        {
            RestQueryApi.QueryPostRaw("SystemConfig", "solutionmetadata", "changemetadata", name, metadataSolution).ToDynamic();
        }

        /// <summary>
        ///     Удалить метаданные указанного объекта в указанной конфигурации
        /// </summary>
        /// <param name="metadataObject"></param>
        public void DeleteItem(dynamic metadataObject)
        {
            var solutionHeader =
                MetadataExtensions.GetStoredMetadata(
                   new MetadataReaderSolution(_version), metadataObject);

            if (solutionHeader != null)
            {
                RestQueryApi.QueryPostJsonRaw("SystemConfig", "solutionmetadata", "deletemetadata", solutionHeader.Name, new
                    {
                        Version = _version
                    });
            }
        }

        /// <summary>
        ///     Обновить метаданные указанного объекта  в указанной конфигурации
        /// </summary>
        /// <param name="objectToCreate">Метаданные создаваемого объекта метаданных</param>
        public void MergeItem(dynamic objectToCreate)
        {
            InsertItem(objectToCreate);
        }
    }
}
