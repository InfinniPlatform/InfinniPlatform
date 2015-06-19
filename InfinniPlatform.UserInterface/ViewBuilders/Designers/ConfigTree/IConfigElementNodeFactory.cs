using System.Collections.Generic;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree
{
    internal interface IConfigElementNodeFactory
    {
        void Create(ConfigElementNodeBuilder builder, ICollection<ConfigElementNode> elements,
            ConfigElementNode elementNode);
    }
}