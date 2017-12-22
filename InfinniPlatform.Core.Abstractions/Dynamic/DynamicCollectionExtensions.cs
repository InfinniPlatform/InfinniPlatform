using System;
using System.Collections;

using InfinniPlatform.Properties;

namespace InfinniPlatform.Dynamic
{
    /// <summary>
    /// Extension methods for working with dynamic objects as collections.
    /// </summary>
    public static class DynamicCollectionExtensions
    {
        /// <summary>
        /// Возвращает элемент коллекции.
        /// </summary>
        /// <param name="collection">Исходная коллекция.</param>
        /// <param name="index">Индекс элемента.</param>
        /// <returns>Элемент коллекции.</returns>
        public static object GetItem(this object collection, int index)
        {
            object result = null;

            if (collection != null && index >= 0)
            {
                if (collection is IList list)
                {
                    if (index < list.Count)
                    {
                        result = list[index];
                    }
                }
                else
                {
                    if (collection is IEnumerable enumerable)
                    {
                        foreach (var item in enumerable)
                        {
                            if (index == 0)
                            {
                                result = item;
                                break;
                            }

                            --index;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Устанавливает элемент коллекции.
        /// </summary>
        /// <param name="collection">Исходная коллекция.</param>
        /// <param name="index">Индекс элемента.</param>
        /// <param name="item">Элемент коллекции.</param>
        public static void SetItem(this object collection, int index, object item)
        {
            ModifyCollection(collection, list => list[index] = item);
        }


        /// <summary>
        /// Добавляет элемент в коллекцию.
        /// </summary>
        /// <param name="collection">Исходная коллекция.</param>
        /// <param name="item">Элемент коллекции.</param>
        public static void AddItem(this object collection, object item)
        {
            ModifyCollection(collection, list => list.Add(item));
        }

        /// <summary>
        /// Добавляет элемент в коллекцию.
        /// </summary>
        /// <param name="collection">Исходная коллекция.</param>
        /// <param name="index">Индекс элемента.</param>
        /// <param name="item">Элемент коллекции.</param>
        public static void InsertItem(this object collection, int index, object item)
        {
            ModifyCollection(collection, list => list.Insert(index, item));
        }


        /// <summary>
        /// Удаляет элемент из коллекции.
        /// </summary>
        /// <param name="collection">Исходная коллекция.</param>
        /// <param name="item">Элемент коллекции.</param>
        public static void RemoveItem(this object collection, object item)
        {
            ModifyCollection(collection, list => list.Remove(item));
        }

        /// <summary>
        /// Удаляет элемент из коллекции.
        /// </summary>
        /// <param name="collection">Исходная коллекция.</param>
        /// <param name="index">Индекс элемента.</param>
        public static void RemoveItemAt(this object collection, int index)
        {
            ModifyCollection(collection, list => list.RemoveAt(index));
        }


        /// <summary>
        /// Перемещает элемент в коллекции.
        /// </summary>
        /// <param name="collection">Исходная коллекция.</param>
        /// <param name="item">Элемент коллекции.</param>
        /// <param name="delta">Смещение индекса.</param>
        public static void MoveItem(this object collection, object item, int delta)
        {
            ModifyCollection(collection, list =>
                                         {
                                             var index = list.IndexOf(item);

                                             if (index >= 0)
                                             {
                                                 var newIndex = index + delta;

                                                 if (newIndex != index && newIndex >= 0 && newIndex < list.Count)
                                                 {
                                                     list.RemoveAt(index);
                                                     list.Insert(newIndex, item);
                                                 }
                                             }
                                         });
        }


        private static void ModifyCollection(object collection, Action<IList> modify)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection), Resources.TargetObjectCannotBeNull);
            }

            var list = collection as IList;

            if (list == null)
            {
                throw new NotSupportedException(Resources.CollectionCanNotBeModified);
            }

            modify(list);
        }
    }
}