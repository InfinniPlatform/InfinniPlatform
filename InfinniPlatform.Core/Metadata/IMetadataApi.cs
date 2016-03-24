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


        IEnumerable<string> GetViewNames();

        dynamic GetView(string viewName);


        IEnumerable<string> GetPrintViewNames();

        dynamic GetPrintView(string printViewName);
    }
}