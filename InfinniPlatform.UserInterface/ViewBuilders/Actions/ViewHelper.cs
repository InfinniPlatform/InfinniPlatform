using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.LinkViews;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Actions
{
    internal static class ViewHelper
    {
        private static readonly Dictionary<object, View> OpenedViews = new Dictionary<object, View>();

        /// <summary>
        ///     Открыть представление или переключить фокус на открытое представление.
        /// </summary>
        /// <param name="viewKey">Уникальный ключ представления.</param>
        /// <param name="linkViewFactory">Метод получения ссылки на представление.</param>
        /// <param name="onInit">Обработчик инициализации открываемого представления.</param>
        /// <param name="onAccept">Обработчик сохранения или подтверждения данных представления.</param>
        public static void ShowView(object viewKey, Func<LinkView> linkViewFactory, Action<IDataSource> onInit = null,
            Action<IDataSource> onAccept = null)
        {
            View view;

            // Если представление уже открыто
            if (TryGetView(viewKey, out view))
            {
                view.Focus();
            }
            else
            {
                var linkView = linkViewFactory();

                if (linkView != null)
                {
                    // Создание представления
                    view = linkView.CreateView();

                    if (view != null)
                    {
                        // Выборка источника данных
                        var viewDataSources = view.GetDataSources();
                        var mainDataSource = (viewDataSources != null) ? viewDataSources.FirstOrDefault() : null;

                        // Инициализация представления
                        TryInvoke(onInit, mainDataSource);

                        var saved = false;

                        // Обработка события сохранения данных
                        ScriptDelegate onSaved = (c, a) =>
                        {
                            saved = true;

                            TryInvoke(onAccept, mainDataSource);
                        };

                        if (mainDataSource != null)
                        {
                            mainDataSource.OnItemSaved += onSaved;
                        }

                        ScriptDelegate onClosed = null;

                        // Обработка события закрытия представления
                        onClosed
                            = (c, a) =>
                            {
                                if (mainDataSource != null)
                                {
                                    mainDataSource.OnItemSaved -= onSaved;
                                }

                                view.OnClosed -= onClosed;
                                RemoveView(viewKey);

                                // Если окно закрыли с подтверждением
                                if (saved == false && view.GetDialogResult() == DialogResult.Accepted)
                                {
                                    TryInvoke(onAccept, mainDataSource);
                                }
                            };

                        view.OnClosed += onClosed;
                        AddView(viewKey, view);

                        view.Open();
                    }
                }
            }
        }

        private static void TryInvoke(Action<IDataSource> action, IDataSource dataSource)
        {
            if (action != null)
            {
                action(dataSource);
            }
        }

        private static bool TryGetView(object viewKey, out View view)
        {
            view = null;

            return (viewKey != null) && OpenedViews.TryGetValue(viewKey, out view);
        }

        private static void AddView(object viewKey, View view)
        {
            if (viewKey != null)
            {
                OpenedViews[viewKey] = view;
            }
        }

        private static void RemoveView(object viewKey)
        {
            if (viewKey != null)
            {
                OpenedViews.Remove(viewKey);
            }
        }
    }
}