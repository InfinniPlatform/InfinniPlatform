using System;
using System.Collections.Generic;

namespace InfinniPlatform.Core.Metadata
{
    /// <summary>
    ///     Типы метаданных системы.
    /// </summary>
    public static class MetadataType
    {
        public const string Solution = "Solution";

        public const string Configuration = "Configuration";
        // Наименования типов элементов метаданных

        public const string Menu = "Menu";
        public const string Document = "Document";
        public const string Register = "Register";
        public const string Assembly = "Assembly";
        public const string View = "View";
        public const string PrintView = "PrintView";
        public const string Service = "Service";
        public const string Process = "Process";
        public const string Scenario = "Scenario";
        public const string Generator = "Generator";
        public const string Report = "Report";
        public const string ValidationWarning = "ValidationWarning";
        public const string ValidationError = "ValidationError";
        // Имя Status использовать нельзя, так как в документах уже есть стандартное поле с именем Status 
        // и возникают проблемы при исполнении IQL запросов
        public const string Status = "DocumentStatus";
        public const string Schema = "Schema";
        [Obsolete] public const string Classifier = "Classifier";
        [Obsolete] public const string ComplexType = "ComplexType";
        // Наименования контейнеров элементов метаданных

        public const string ValidationErrorsContainer = "ValidationErrors";
        public const string ValidationWarningsContainer = "ValidationWarnings";
        public const string MenuContainer = "Menu";
        public const string DocumentContainer = "Documents";
        public const string RegisterContainer = "Registers";
        public const string AssemblyContainer = "Assemblies";
        public const string ViewContainer = "Views";
        public const string PrintViewContainer = "PrintViews";
        public const string ServiceContainer = "Services";
        public const string ProcessContainer = "Processes";
        public const string ScenarioContainer = "Scenarios";
        public const string GeneratorContainer = "Generators";
        public const string ReportContainer = "Reports";
        public const string StatusContainer = "DocumentStatuses";
        [Obsolete] public const string ClassifierContainer = "Classifiers";
        [Obsolete] public const string ComplexTypeContainer = "ComplexTypes";

        public static IEnumerable<string> GetContainedMetadataTypes()
        {
            return new[]
            {
                Menu,
                Document,
                Register,
                Assembly,
                View,
                PrintView,
                Service,
                Process,
                Scenario,
                Generator,
                Report,
                ValidationWarning,
                ValidationError,
                Status
            };
        }

        public static IEnumerable<string> GetDocumentMetadataTypes()
        {
            return new[]
            {
                View,
                PrintView,
                Service,
                Process,
                Scenario,
                Generator,
                ValidationWarning,
                ValidationError,
                Status
            };
        }

        public static IEnumerable<string> GetConfigMetadataTypes()
        {
            return new[]
            {
                Menu,
                Document,
                Register,
                Assembly,
                Report
            };
        }
    }
}