using System.Linq;
using System.Windows;
using InfinniPlatform.UserInterface.Properties;

namespace InfinniPlatform.UserInterface.ViewBuilders.Views
{
    internal sealed class ViewBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var view = new View(parent);
            view.SetName(metadata.Name);
            view.SetImage(metadata.Image);

            // Прикладные скрипты

            var scripts = context.BuildScripts(view, (object) metadata.Scripts);

            if (scripts != null)
            {
                foreach (dynamic script in scripts)
                {
                    view.AddScript(script.Name, script.Action);
                }
            }

            // Параметры

            var parameters = context.BuildParameters(view, (object) metadata.Parameters);

            if (parameters != null)
            {
                foreach (dynamic parameter in parameters)
                {
                    view.AddParameter(parameter);
                }
            }

            // Источники данных

            var dataSources = context.BuildMany(view, metadata.DataSources);

            if (dataSources != null)
            {
                foreach (var dataSource in dataSources)
                {
                    view.AddDataSource(dataSource);
                }
            }

            // Общие свойства

            context.BuildOneWayBinding(view, view, "Text", (object) metadata.Text);

            // Элементы представления

            var layoutPanel = context.Build(view, metadata.LayoutPanel);
            view.SetLayoutPanel(layoutPanel);

            // Обработчики событий

            if (metadata.OnTextChanged != null)
            {
                view.OnTextChanged += view.GetScript(metadata.OnTextChanged);
            }

            if (metadata.OnOpening != null)
            {
                view.OnOpening += view.GetScript(metadata.OnOpening);
            }

            if (metadata.OnOpened != null)
            {
                view.OnOpened += view.GetScript(metadata.OnOpened);
            }

            if (metadata.OnClosing != null)
            {
                view.OnClosing += view.GetScript(metadata.OnClosing);
            }
            else
            {
                view.OnClosing += (c, a) => OnClosingDefaultHandler(view, a);
            }

            if (metadata.OnClosed != null)
            {
                view.OnClosed += view.GetScript(metadata.OnClosed);
            }

            if (metadata.OnLoaded != null)
            {
                view.OnLoaded += view.GetScript(metadata.OnLoaded);
            }

            // Публикация сообщений в шину при возникновении событий
            view.NotifyWhenEventAsync(i => i.OnLoaded);

            // Подписка на сообщения шины от внешних элементов
            view.SubscribeOnEvent(OnError);

            return view;
        }

        // Обработчики сообщений шины (внимание: наименования обработчиков совпадают с наименованиями событий)

        private static void OnError(View view, dynamic arguments)
        {
            // Todo: Обработка валидационных исключений

            MessageBox.Show(arguments.Value, view.GetText(), MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static void OnClosingDefaultHandler(View view, dynamic arguments)
        {
            arguments.IsCancel = (arguments.Force != true)
                                 && (view.GetDialogResult() != DialogResult.Accepted)
                                 && view.GetDataSources().Any(d => d.IsModified())
                                 &&
                                 MessageBox.Show(Resources.CloseViewQuestion, view.GetText(), MessageBoxButton.YesNo,
                                     MessageBoxImage.Warning) == MessageBoxResult.No;
        }
    }
}