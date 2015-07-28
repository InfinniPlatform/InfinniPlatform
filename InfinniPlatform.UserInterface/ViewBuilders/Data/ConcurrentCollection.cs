using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data
{
    /// <summary>
    ///     Потокобезопасная упорядоченная коллекция.
    /// </summary>
    internal sealed class ConcurrentCollection : IEnumerable<object>, INotifyCollectionChanged
    {
        private static readonly object NullId = new object();
        private readonly Dictionary<object, object> _dictionaryItems = new Dictionary<object, object>();
        private readonly Func<object, object> _idItem;
        private readonly List<object> _listItems = new List<object>();

        private readonly ReaderWriterLockSlim _lockItems =
            new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        ///     Конструктор.
        /// </summary>
        /// <param name="idProperty">Свойство, которое хранит идентификатор элемента.</param>
        public ConcurrentCollection(string idProperty = null)
        {
            if (string.IsNullOrEmpty(idProperty))
            {
                _idItem = item => item ?? NullId;
            }
            else
            {
                _idItem = item => item.GetProperty(idProperty) ?? NullId;
            }
        }

        public IEnumerator<object> GetEnumerator()
        {
            return ReadLock(() => new List<object>(_listItems).GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        ///     Возвращает элемент коллекции с заданным идентификатором.
        /// </summary>
        public object GetById(object itemId)
        {
            itemId = itemId ?? NullId;

            return ReadLock(() =>
            {
                object item;

                _dictionaryItems.TryGetValue(itemId, out item);

                return item;
            });
        }

        /// <summary>
        ///     Возвращает элемент коллекции с заданным индексом.
        /// </summary>
        public object GetByIndex(int itemIndex)
        {
            if (itemIndex >= 0)
            {
                return ReadLock(() =>
                {
                    if (itemIndex < _listItems.Count)
                    {
                        return _listItems[itemIndex];
                    }

                    return null;
                });
            }

            return null;
        }

        /// <summary>
        ///     Заменяет элемент коллекции с заданным идентификатором.
        /// </summary>
        public bool SetById(object itemId, object newItem)
        {
            return WriteLock(() =>
            {
                object item;

                if (_dictionaryItems.TryGetValue(itemId, out item))
                {
                    return ReplaceItem(item, newItem);
                }

                return false;
            });
        }

        /// <summary>
        ///     Заменяет элемент коллекции с заданным индексом.
        /// </summary>
        public bool SetByIndex(int itemIndex, object newItem)
        {
            if (itemIndex >= 0)
            {
                return WriteLock(() =>
                {
                    if (itemIndex < _listItems.Count)
                    {
                        return ReplaceItem(_listItems[itemIndex], newItem);
                    }

                    return false;
                });
            }

            return false;
        }

        /// <summary>
        ///     Возвращает индекс элемента коллекции.
        /// </summary>
        public int Index(object item)
        {
            return ReadLock(() => _listItems.IndexOf(item));
        }

        /// <summary>
        ///     Возвращает индекс элемента с заданным идентификатором.
        /// </summary>
        public int IndexById(object itemId)
        {
            itemId = itemId ?? NullId;

            return ReadLock(() =>
            {
                object item;

                if (_dictionaryItems.TryGetValue(itemId, out item))
                {
                    return _listItems.IndexOf(item);
                }

                return -1;
            });
        }

        /// <summary>
        ///     Добавляет элемент коллецию.
        /// </summary>
        public bool AddOrUpdate(object item)
        {
            return WriteLock(() => AddOrUpdateItem(item));
        }

        /// <summary>
        ///     Добавляет элементы в коллецию.
        /// </summary>
        public bool AddOrUpdate(IEnumerable items)
        {
            if (items != null)
            {
                return WriteLock(() =>
                {
                    var modify = false;

                    foreach (var item in items)
                    {
                        modify |= AddOrUpdateItem(item);
                    }

                    return modify;
                });
            }

            return false;
        }

        private bool AddOrUpdateItem(object item)
        {
            var modify = false;
            var itemId = _idItem(item);

            object prevItem;

            if (_dictionaryItems.TryGetValue(itemId, out prevItem))
            {
                if (!Equals(prevItem, item))
                {
                    var itemIndex = _listItems.IndexOf(prevItem);

                    _listItems[itemIndex] = item;
                    _dictionaryItems[itemId] = item;

                    modify = true;
                }
            }
            else
            {
                _listItems.Add(item);
                _dictionaryItems.Add(itemId, item);

                modify = true;
            }

            return modify;
        }

        /// <summary>
        ///     Вставляет элемент в заданную позицию коллекции.
        /// </summary>
        public bool InsertOrUpdate(object item, int itemIndex)
        {
            return WriteLock(() => InsertOrUpdate(item, itemIndex));
        }

        /// <summary>
        ///     Вставляет элементы в заданную позицию коллекции.
        /// </summary>
        public bool InsertOrUpdate(IEnumerable items, int itemIndex)
        {
            if (items != null)
            {
                return WriteLock(() =>
                {
                    var modify = false;

                    foreach (var item in items)
                    {
                        modify |= InsertOrUpdateItem(item, itemIndex++);
                    }

                    return modify;
                });
            }

            return false;
        }

        private bool InsertOrUpdateItem(object item, int itemIndex)
        {
            var modify = false;
            var itemId = _idItem(item);

            object prevItem;

            if (_dictionaryItems.TryGetValue(itemId, out prevItem))
            {
                if (itemIndex < 0 || itemIndex >= _listItems.Count)
                {
                    itemIndex = (_listItems.Count > 0) ? (_listItems.Count - 1) : 0;
                }

                var prevItemIndex = _listItems.IndexOf(prevItem);

                if (prevItemIndex != itemIndex)
                {
                    _listItems.RemoveAt(prevItemIndex);
                    _listItems.Insert(itemIndex, item);

                    modify = true;
                }

                if (!Equals(prevItem, item))
                {
                    _listItems[itemIndex] = item;
                    _dictionaryItems[itemId] = item;

                    modify = true;
                }
            }
            else
            {
                if (itemIndex < 0 || itemIndex > _listItems.Count)
                {
                    itemIndex = _listItems.Count;
                }

                _listItems.Insert(itemIndex, item);
                _dictionaryItems.Add(itemId, item);

                modify = true;
            }

            return modify;
        }

        /// <summary>
        ///     Удаляет из коллекции элемент.
        /// </summary>
        public bool Remove(object item)
        {
            return WriteLock(() => RemoveItem(item));
        }

        /// <summary>
        ///     Удаляет из коллекции элементы.
        /// </summary>
        public bool Remove(IEnumerable items)
        {
            if (items != null)
            {
                return WriteLock(() =>
                {
                    var modify = false;

                    foreach (var item in items)
                    {
                        modify |= RemoveItem(item);
                    }

                    return modify;
                });
            }

            return false;
        }

        private bool RemoveItem(object item)
        {
            var itemId = _idItem(item);

            if (_dictionaryItems.Remove(itemId))
            {
                _listItems.Remove(item);

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Удаляет из коллекции элемент с заданным идентификатором.
        /// </summary>
        public bool RemoveById(object itemId)
        {
            return WriteLock(() => RemoveItemById(itemId));
        }

        /// <summary>
        ///     Удаляет из коллекции элемент с заданными идентификаторами.
        /// </summary>
        public bool RemoveById(IEnumerable itemIds)
        {
            if (itemIds != null)
            {
                return WriteLock(() =>
                {
                    var modify = false;

                    foreach (var itemId in itemIds)
                    {
                        modify |= RemoveItemById(itemId);
                    }

                    return modify;
                });
            }

            return false;
        }

        private bool RemoveItemById(object itemId)
        {
            itemId = itemId ?? NullId;

            object item;

            if (_dictionaryItems.TryGetValue(itemId, out item))
            {
                _listItems.Remove(item);
                _dictionaryItems.Remove(itemId);

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Удаляет из коллекции элемент с заданным индексом.
        /// </summary>
        public bool RemoveByIndex(int itemIndex)
        {
            return WriteLock(() => RemoveItemByIndex(itemIndex));
        }

        /// <summary>
        ///     Удаляет из коллекции элемент с заданными индексами.
        /// </summary>
        public bool RemoveByIndex(IEnumerable<int> itemIndexes)
        {
            if (itemIndexes != null)
            {
                return WriteLock(() =>
                {
                    var modify = false;

                    foreach (var itemIndex in itemIndexes.OrderByDescending(i => i))
                    {
                        modify |= RemoveItemByIndex(itemIndex);
                    }

                    return modify;
                });
            }

            return false;
        }

        private bool RemoveItemByIndex(int itemIndex)
        {
            if (itemIndex >= 0 && itemIndex < _listItems.Count)
            {
                var item = _listItems[itemIndex];

                _listItems.RemoveAt(itemIndex);
                _dictionaryItems.Remove(_idItem(item));

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Удаляет элемент из коллекции по условию.
        /// </summary>
        public bool RemoveByPredicate(Func<object, bool> itemPredicate)
        {
            return WriteLock(() =>
            {
                var removeItems = _dictionaryItems.Where(p => itemPredicate(p.Value)).ToArray();

                if (removeItems.Length > 0)
                {
                    foreach (var pair in removeItems)
                    {
                        _listItems.Remove(pair.Value);
                        _dictionaryItems.Remove(pair.Key);
                    }

                    return true;
                }

                return false;
            });
        }

        /// <summary>
        ///     Заменяет элемент коллекции на указанный.
        /// </summary>
        public bool Replace(object item, object newItem)
        {
            return WriteLock(() => ReplaceItem(item, newItem));
        }

        /// <summary>
        ///     Заменяет элементы коллекции на указанные.
        /// </summary>
        public bool Replace(IEnumerable<object> items, IEnumerable<object> newItems)
        {
            var arrayItems = (items != null) ? items.ToArray() : new object[] {};
            var arrayNewItems = (newItems != null) ? newItems.ToArray() : new object[] {};

            if (arrayItems.Length > 0)
            {
                return WriteLock(() =>
                {
                    var i = 0;
                    var modify = false;

                    if (arrayNewItems.Length > 0)
                    {
                        var replaceCount = (arrayNewItems.Length > arrayItems.Length)
                            ? arrayItems.Length
                            : arrayNewItems.Length;

                        for (; i < replaceCount; ++i)
                        {
                            modify |= ReplaceItem(arrayItems[i], arrayNewItems[i]);
                        }

                        for (; i < arrayNewItems.Length; ++i)
                        {
                            modify |= AddOrUpdateItem(arrayNewItems[i]);
                        }
                    }

                    for (; i < arrayItems.Length; ++i)
                    {
                        modify |= RemoveItem(arrayItems[i]);
                    }

                    return modify;
                });
            }

            return false;
        }

        private bool ReplaceItem(object item, object newItem)
        {
            var modify = false;

            if (!Equals(item, newItem))
            {
                var itemId = _idItem(item);

                if (_dictionaryItems.Remove(itemId))
                {
                    var newItemId = _idItem(newItem);
                    var itemIndex = _listItems.IndexOf(item);

                    if (_dictionaryItems.ContainsKey(newItemId))
                    {
                        var newItemIndex = _listItems.IndexOf(newItem);
                        _listItems[itemIndex] = newItem;
                        _listItems.RemoveAt(newItemIndex);
                    }
                    else
                    {
                        _listItems[itemIndex] = newItem;
                    }

                    _dictionaryItems[newItemId] = newItem;

                    modify = true;
                }
            }

            return modify;
        }

        /// <summary>
        ///     Заменяет содержимое коллеции указанным списком элементов.
        /// </summary>
        public void ReplaceAll(IEnumerable items)
        {
            var newItems = (items != null) ? items.Cast<object>().ToLookup(_idItem) : null;

            WriteLock(() =>
            {
                _listItems.Clear();
                _dictionaryItems.Clear();

                if (newItems != null)
                {
                    foreach (var group in newItems)
                    {
                        var item = group.FirstOrDefault();

                        _listItems.Add(item);
                        _dictionaryItems.Add(group.Key, item);
                    }
                }

                return true;
            });
        }

        /// <summary>
        ///     Удаляет все элементы из коллекции.
        /// </summary>
        public void Clear()
        {
            WriteLock(() =>
            {
                _listItems.Clear();
                _dictionaryItems.Clear();

                return true;
            });
        }

        private bool WriteLock(Func<bool> write)
        {
            bool modify;

            _lockItems.EnterWriteLock();

            try
            {
                modify = write();
            }
            finally
            {
                _lockItems.ExitWriteLock();
            }

            if (modify)
            {
                RaiseCollectionChanged();
            }

            return modify;
        }

        private TResult ReadLock<TResult>(Func<TResult> read)
        {
            _lockItems.EnterReadLock();

            try
            {
                return read();
            }
            finally
            {
                _lockItems.ExitReadLock();
            }
        }

        private void RaiseCollectionChanged()
        {
            if (CollectionChanged != null)
            {
                // Коллекция полностью обновляется, так как она доступна из множества потоков

                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
    }
}