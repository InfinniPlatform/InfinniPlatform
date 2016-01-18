using System.Collections.Generic;

namespace InfinniPlatform.Core.Metadata
{
    public interface IMetadataApi
    {
        IEnumerable<string> GetMenuNames(string configuration);

        dynamic GetMenu(string configuration, string menuName);


        IEnumerable<string> GetRegisterNames(string configuration);

        dynamic GetRegister(string configuration, string registerName);


        IEnumerable<string> GetDocumentNames(string configuration);

        dynamic GetDocumentSchema(string configuration, string documentName);

        dynamic GetDocumentEvents(string configuration, string documentName);


        IEnumerable<string> GetActionNames(string configuration, string documentName);

        dynamic GetAction(string configuration, string documentName, string actionName);


        IEnumerable<string> GetViewNames(string configuration, string documentName);

        dynamic GetView(string configuration, string documentName, string viewName);


        IEnumerable<string> GetPrintViewNames(string configuration, string documentName);

        dynamic GetPrintView(string configuration, string documentName, string printViewName);
    }
}