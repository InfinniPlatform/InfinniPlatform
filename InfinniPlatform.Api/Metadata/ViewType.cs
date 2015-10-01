using System.Collections.Generic;

namespace InfinniPlatform.Api.Metadata
{
    public static class ViewType
    {
        public const string ListView = "ListView";
        public const string EditView = "EditView";
        public const string SelectView = "SelectView";
        public const string HomePage = "HomePage";
        public const string Menu = "Menu";

        public static IEnumerable<string> GetViewTypes()
        {
            return new[]
            {
                ListView, EditView, SelectView, HomePage, Menu
            };
        }
    }
}