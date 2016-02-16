using System.Collections.Generic;

namespace InfinniPlatform.Core.Metadata
{
    public interface IMetadataApi
    {
        IEnumerable<string> GetMenuNames();

        dynamic GetMenu(string menuName);


        IEnumerable<string> GetRegisterNames();

        dynamic GetRegister(string registerName);


        IEnumerable<string> GetDocumentNames();

        dynamic GetDocumentSchema(string documentName);

        dynamic GetDocumentEvents(string documentName);

        IEnumerable<object> GetDocumentIndexes(string documentName);


        IEnumerable<string> GetActionNames(string documentName);

        dynamic GetAction(string documentName, string actionName);


        IEnumerable<string> GetViewNames(string documentName);

        dynamic GetView(string documentName, string viewName);


        IEnumerable<string> GetPrintViewNames(string documentName);

        dynamic GetPrintView(string documentName, string printViewName);
    }
}