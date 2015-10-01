using System;
using System.Linq;

namespace InfinniPlatform.Index.QueryLanguage.Implementation
{
    public sealed class ReferencedObject
    {
        public string Index { get; set; }
        public string Alias { get; set; }
        public string Path { get; set; }

        /// <summary>
        ///     Тип документа, получаемого из индекса.
        ///     Если не установлено, получаем документы всех типов
        /// </summary>
        public string Type { get; set; }

        public string GetProjectionPath()
        {
            //example
            //path = "menu.$.Id"
            //alias = "fullmenu"
            //return: "menu.$.fullmenu"
            var aliasItemsList = Path.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (aliasItemsList.Any())
            {
                //удаляем последний элемент, указывающий на идентификатор проецируемого элемента
                aliasItemsList.RemoveAt(aliasItemsList.Count - 1);
            }
            aliasItemsList.Add(Alias);
            return string.Join(".", aliasItemsList);
        }
    }
}