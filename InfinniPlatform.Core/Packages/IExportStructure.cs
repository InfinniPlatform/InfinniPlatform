using System.Collections.Generic;

namespace InfinniPlatform.Core.Packages
{
    //структура хранения конфигурации в zip-файле
    /*  zip
     * Configuration.json (file
     * Documents (folder)
     * -- Document1 (folder) 
     * ---- Scenarios  (folder)
     * ------Scenario1.json (file)
     * ------....
     * ---- Services   (folder)
     * ------Service1.json (file)
     * ------....
     * ---- Processes  (folder)
     * ------Process1.json (file)
     * ------....
     * ---- Generators (folder)
     * ------Generator1.json (file)
     * ---- Views (folder)
     * ------View1.json (file)
     * ------....
     * Registers (folder)
     * -- Register1 (folder) 
     * ----Register1.json (file)
     * Assemblies
     * -- Assembly1.json (file)
     * -- ...
     * Menu
     * -- Menu1.json (file)
     * -- ...
     */

    /// <summary>
    ///     Структура файлов и папок конфигурации
    /// </summary>
    public interface IExportStructure
    {
        void AddConfiguration(IEnumerable<string> configuration);
        void AddDocument(string documentName, IEnumerable<string> document);
        void AddRegister(string registerName, IEnumerable<string> register);
        void AddMenu(string menuName, IEnumerable<string> menu);
        void AddAssembly(string assemblyName, IEnumerable<string> assembly);
        void AddReport(string reportName, IEnumerable<string> report);
        void AddSolution(IEnumerable<string> solution);

        void AddDocumentMetadataType(string document, string metadataName, string metadataType,
            IEnumerable<string> metadata);

        void Start();
        void End();
        dynamic GetConfiguration();
        dynamic GetSolution();
        dynamic GetDocument(string documentName);
        dynamic GetRegister(string registerName);
        dynamic GetDocumentMetadataType(string document, string metadataName, string metadataType);
        dynamic GetMenu(string menuName);
        dynamic GetReport(string reportName);
    }
}