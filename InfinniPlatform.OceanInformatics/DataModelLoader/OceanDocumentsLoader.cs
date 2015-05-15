using EhrReaders.Template;

using Ocean.KnowledgeRepository.ArtefactBuilders.OperationalTemplate;

using OpenEhr.V1.Its.Xml;
using OpenEhr.V1.Its.Xml.AM;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using XMLParser.OpenEhr.V1.Its.Xml.AM;

namespace InfinniPlatform.OceanInformatics.DataModelLoader
{
    internal sealed class OceanDocumentsLoader
    {
        private readonly IDictionary<string, ARCHETYPE> _loadedArchetypes;
        private readonly IDictionary<string, TEMPLATE> _loadedTemplates;

        internal OceanDocumentsLoader()
        {
            _loadedArchetypes = new Dictionary<string, ARCHETYPE>();
            _loadedTemplates = new Dictionary<string, TEMPLATE>();
        }

        /*
         * Способ загрузки OPERATIONAL_TEMPLATE из opt файла более предпочтителен, по сравнению с 
         * загрузкой из oet, по следующим причинам:
         * 
         * 1. Нет необходимости загружать все архетипы из репозитория (следовательно нет 
         * необходимости держать словари _loadedArchetypes и _loadedTemplates)
         * 
         * 2. Нет ограничений на количество загрузок шаблонов. При использовании
         * загрузки из oet файла, если делать это в цикле более 100 раз,
         * может произойти StackOveflowException (это связано с особенностями
         * реализации библиотеки OceanInformatics)
         * 
         * 3. Загрузка происходит гораздо быстрее
         * 
         * Способ загрузки из oet файлов не удален, так как в настоящее время
         * не все шаблоны представлены в виде opt файлов (в частности, на 
         * проекте СИМИ нет сгенерированных opt файлов.
         * 
         * В будущем, когда будут доступны opt файлы, нужно будет удалить 
         * метод BuildOperationalTemplateFromOetFile и использовать только 
         * BuildOperationalTemplateFromOptFile.
         * 
         * 
         * 
         * ВНИМАНИЕ! Важные особенности работы библиотеки:
         * 
         * 1. Настройка файла app.config
         * 
         * Для корректной работы загрузки архетипов и шаблонов необходимо 
         * включить следующие разделы в app.config исполняемого файла:
         * 
            <configSections>
                <section name="terminologyServiceConfiguration" type="OceanEhr.Terminology.Configuration.TerminologyServiceSettings, OceanEhr.OpenEhrV1" />
                <section name="ehrServerConfiguration" type="OceanEhr.EhrBank.EhrServerSettings, OceanEhr.EhrBank" />
            </configSections>
        
            <terminologyServiceConfiguration defaultProvider="default">
                <terminologyServiceProviders>
                <add name="default" type="OceanEhr.Terminology.TerminologyService, OceanEhr.OpenEhrV1">
                    <terminologyAccessProviders>
                <add name="openehr" type="OceanEhr.Terminology.ArchetypeEditorTerminologyAccess, OceanEhr.OpenEhrV1" />
                </terminologyAccessProviders>
                    <codeSetAccessProviders>
                        <add name="ISO_639-1" type="OceanEhr.Terminology.CodeSetAccess, OceanEhr.OpenEhrV1" />
                        <add name="ISO_3166-1" type="OceanEhr.Terminology.CodeSetAccess, OceanEhr.OpenEhrV1" />
                        <add name="IANA_character-sets" type="OceanEhr.Terminology.CodeSetAccess, OceanEhr.OpenEhrV1" />
                    </codeSetAccessProviders>
                </add>
                </terminologyServiceProviders>
            </terminologyServiceConfiguration>

            <ehrServerConfiguration>
                <ehrServerProviders>
                    <add name="OpenHealthEhrServer" type="OpenHealth.Storage.Ehr.EhrServer, OpenHealth.Storage.Ehr" />
                </ehrServerProviders>
            </ehrServerConfiguration>
         * 
         * 2. Target platform
         * 
         * Используемые сборки от Ocean скомпиллированны под архитектуру x86, следовательно проект InfinniPlatform.OceanInformatics
         * собран также под 32-ую архитектуру. Проект исполняемого приложения должен быть настроен так, чтобы была установлена опция prefer 32-bit,
         * иначе при использовании библиотеки произойдет исключение.
         * Альтернативный вариант: с помощью рефлектора изменить настройки сборок от Ocean и проблема должна уйти.
         * 
         * 
        */


        internal static bool BuildOperationalTemplateFromOptFile(
            Stream optContent,
            out string errorMessage,
            out OPERATIONAL_TEMPLATE operationalTemplate)
        {
            errorMessage = string.Empty;
            try
            {
                operationalTemplate = XmlSerializer.DeserializeOperationalTemplate(optContent);
                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.ToString();
                operationalTemplate = null;
                return false;
            }
        }

        internal bool BuildOperationalTemplateFromOetFile(
            string oetFilePath,
            string templatesFolder,
            string archetypesFolder,
            out string errorMessage,
            out OPERATIONAL_TEMPLATE operationalTemplate)
        {
            var archetypes = _loadedArchetypes.Values.ToList();
            var templates = _loadedTemplates.Values.ToList();
            errorMessage = string.Empty;
            
            foreach (var file in Directory.GetFiles(templatesFolder, "*.*", SearchOption.AllDirectories))
            {
                if (file.EndsWith("oet") && Path.GetFileName(oetFilePath) != Path.GetFileName(file) && !_loadedTemplates.ContainsKey(file))
                {
                    try
                    {
                        var template = (TEMPLATE)TemplateParser.TemplateSerialiser.Deserialize(new StreamReader(file, Encoding.UTF8));

                        if (templates.FirstOrDefault(t => t.id == template.id) == null)
                        {
                            templates.Add(template);
                            _loadedTemplates.Add(file, template);
                        }
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.ToString();
                    }
                }
            }

            foreach (var file in Directory.GetFiles(archetypesFolder, "*.*", SearchOption.AllDirectories))
            {
                if (file.EndsWith("adl") && !_loadedArchetypes.ContainsKey(file))
                {
                    ARCHETYPE archetype;
                    if (BuildFromAdlFile(file, out errorMessage, out archetype))
                    {
                        archetypes.Add(archetype);
                        _loadedArchetypes.Add(file, archetype);
                    }
                }
            }

            TEMPLATE mainTemplate;

            if (_loadedTemplates.ContainsKey(oetFilePath))
            {
                mainTemplate = _loadedTemplates[oetFilePath];
            }
            else
            {
                mainTemplate = (TEMPLATE)TemplateParser.TemplateSerialiser.Deserialize(new StreamReader(oetFilePath, Encoding.UTF8));
                _loadedTemplates.Add(oetFilePath, mainTemplate);
            }
            
            var templateBuilder = new OperationalTemplateBuilder();

            try
            {
                operationalTemplate = templateBuilder.BuildOperationalTemplate(
                    mainTemplate,
                    archetypes.ToArray(),
                    templates.ToArray(),
                    new OperationalTemplateSettings(CultureInfo.CurrentCulture, new List<CultureInfo>(), false, true, true));
                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.ToString();
                operationalTemplate = null;
                return false;
            }
        }

        internal static bool BuildFromAdlFile(string adlFilePath, out string errorMessage, out ARCHETYPE archetype)
        {
            errorMessage = string.Empty;
            try
            {
                archetype = ArchetypeModelBuilder.BuildFromAdlFile(adlFilePath, new CloneConstraintVisitor());
                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.ToString();
                archetype = null;
                return false;
            }
        }
    }
}
