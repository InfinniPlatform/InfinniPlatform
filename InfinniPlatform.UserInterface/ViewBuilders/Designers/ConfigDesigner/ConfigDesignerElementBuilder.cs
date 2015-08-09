using System.Collections.Generic;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigDesigner
{
    internal sealed class ConfigDesignerElementBuilder : IObjectBuilder
    {
        private readonly string _server;
        private readonly int _port;

        public ConfigDesignerElementBuilder(string server, int port)
        {
            _server = server;
            _port = port;
        }

        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var configDesigner = new ConfigDesignerElement(parent, _server, _port);
            configDesigner.ApplyElementMeatadata((object) metadata);

            // Редакторы элементов метаданных

            if (metadata.Editors != null)
            {
                var editors = new List<ItemEditor>();

                foreach (var editor in metadata.Editors)
                {
                    editors.Add(new ItemEditor
                    {
                        Text = editor.Text,
                        Image = editor.Image,
                        Container = editor.Container,
                        MetadataType = editor.MetadataType,
                        LinkView = context.Build(parent, editor.LinkView)
                    });
                }

                configDesigner.SetEditors(editors);
            }

            // Привязка к источнику данных представления

            IElementDataBinding itemsDataBinding = context.Build(parent, metadata.Items);

            if (itemsDataBinding != null)
            {
                itemsDataBinding.OnPropertyValueChanged += (c, a) => configDesigner.SetItems(a.Value);

                var sourceItemsDataBinding = itemsDataBinding as ISourceDataBinding;

                if (sourceItemsDataBinding != null)
                {
                    var dataSourceName = sourceItemsDataBinding.GetDataSource();

                    // Оповещение источника об изменениях в редакторе
                    configDesigner.NotifyWhenEventAsync(i => i.OnUpdateItems, arguments =>
                    {
                        arguments.DataSource = dataSourceName;
                        return true;
                    });
                }
            }

            return configDesigner;
        }
    }
}