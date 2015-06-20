using System.Collections.Generic;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.DocumentDesigner
{
    internal sealed class DocumentDesignerElementBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var documentDesigner = new DocumentDesignerElement(parent);
            documentDesigner.ApplyElementMeatadata((object) metadata);

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

                documentDesigner.SetEditors(editors);
            }

            // Привязка к источнику данных представления

            IElementDataBinding itemsDataBinding = context.Build(parent, metadata.Items);

            if (itemsDataBinding != null)
            {
                itemsDataBinding.OnPropertyValueChanged += (c, a) => documentDesigner.SetItems(a.Value);

                var sourceItemsDataBinding = itemsDataBinding as ISourceDataBinding;

                if (sourceItemsDataBinding != null)
                {
                    var dataSourceName = sourceItemsDataBinding.GetDataSource();
                    var dataSource = parent.GetDataSource(dataSourceName);

                    // Установка идентификатора конфигурации
                    documentDesigner.SetConfigId(dataSource.GetConfigId);

                    // Оповещение источника об изменениях в редакторе
                    documentDesigner.NotifyWhenEventAsync(i => i.OnUpdateItems, arguments =>
                    {
                        arguments.DataSource = dataSourceName;
                        return true;
                    });
                }
            }

            return documentDesigner;
        }
    }
}