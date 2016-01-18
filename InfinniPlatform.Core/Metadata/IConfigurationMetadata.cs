using System.Collections.Generic;

namespace InfinniPlatform.Core.Metadata
{
    /// <summary>
    /// Метаданные приложения.
    /// </summary>
    public interface IConfigurationMetadata
    {
        string Configuration { get; }


        IEnumerable<string> GetMenuNames();

        dynamic GetMenu(string menuName);


        IEnumerable<string> GetRegisterNames();

        dynamic GetRegister(string registerName);


        IEnumerable<string> GetDocumentNames();

        dynamic GetDocumentSchema(string documentName);

        dynamic GetDocumentEvents(string documentName);


        IEnumerable<string> GetActionNames(string documentName);

        dynamic GetAction(string documentName, string actionName);


        IEnumerable<string> GetViewNames(string documentName);

        dynamic GetView(string documentName, string viewName);


        IEnumerable<string> GetPrintViewNames(string documentName);

        dynamic GetPrintView(string documentName, string printViewName);
    }
}