using System.Collections.Generic;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LinkViews
{
    /// <summary>
    ///     Реестр открытых представлений.
    /// </summary>
    internal static class ViewRegistry
    {
        private static readonly Dictionary<View, List<View>> ViewRelations
            = new Dictionary<View, List<View>>();

        /// <summary>
        ///     Регистрирует открываемое представление в реестре.
        /// </summary>
        /// <param name="view">Представление.</param>
        public static void OnOpeningView(View view)
        {
            List<View> children;

            var parent = GetParent(view);

            if (!ViewRelations.TryGetValue(parent, out children))
            {
                children = new List<View>();
                ViewRelations[parent] = children;
            }

            if (view != parent && !children.Contains(view))
            {
                children.Add(view);
            }
        }

        /// <summary>
        ///     Проверяет возможность закрытия представления и удаляет его из реестра.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="force">Закрыть принудительно.</param>
        public static bool OnClosingView(View view, bool force = false)
        {
            var canClose = true;

            List<View> children;

            // Представление может быть закрыто, если можно закрыть все дочерние
            if (ViewRelations.TryGetValue(view, out children))
            {
                foreach (var child in children.ToArray())
                {
                    // Попытка закрытия дочернего представления
                    canClose &= child.Close(force);

                    if (canClose)
                    {
                        // Удаление дочернего представления из реестра
                        children.Remove(child);
                    }
                }
            }

            // Если представление может быть закрыто, оно удаляется из родительского
            if (canClose && ViewRelations.TryGetValue(GetParent(view), out children))
            {
                // Удаление представления из реестра
                children.Remove(view);
            }

            return canClose;
        }

        private static View GetParent(View child)
        {
            return child.GetView() ?? child;
        }
    }
}