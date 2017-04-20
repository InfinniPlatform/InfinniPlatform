using System;
using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage.Transactions
{
    /// <summary>
    /// Журнал действий по изменению документов в рамках <see cref="UnitOfWork"/>.
    /// </summary>
    internal class UnitOfWorkLog
    {
        public UnitOfWorkLog()
        {
            _itemsSync = new object();
            _items = new List<UnitOfWorkItem>();
        }


        private readonly object _itemsSync;
        private volatile List<UnitOfWorkItem> _items;


        /// <summary>
        /// Список действий журнала.
        /// </summary>
        private List<UnitOfWorkItem> Items
        {
            get
            {
                var items = _items;

                if (items == null)
                {
                    lock (_itemsSync)
                    {
                        items = _items;

                        if (items == null)
                        {
                            items = new List<UnitOfWorkItem>();

                            _items = items;
                        }
                    }
                }

                return items;
            }
            set
            {
                var items = _items;

                if (items != value)
                {
                    lock (_itemsSync)
                    {
                        items = _items;

                        if (items != value)
                        {
                            _items = value;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Добавляет действие в журнал.
        /// </summary>
        public void Enqueue(Action<IDocumentBulkBuilder> action, string documentType)
        {
            var workItem = new UnitOfWorkItem(null, documentType, builder => action(((IDocumentBulkBuilder)builder)));

            lock (_itemsSync)
            {
                Items.Add(workItem);
            }
        }

        /// <summary>
        /// Добавляет действие в журнал.
        /// </summary>
        public void Enqueue<TDocument>(Action<IDocumentBulkBuilder<TDocument>> action, string documentType = null)
        {
            var workItem = new UnitOfWorkItem(typeof(TDocument), documentType, builder => action(((IDocumentBulkBuilder<TDocument>)builder)));

            lock (_itemsSync)
            {
                Items.Add(workItem);
            }
        }

        /// <summary>
        /// Добавляет действия в журнал.
        /// </summary>
        public void Enqueue(IEnumerable<UnitOfWorkItem> actions)
        {
            lock (_itemsSync)
            {
                Items.AddRange(actions);
            }
        }


        /// <summary>
        /// Возвращает все действия и очищает журнал.
        /// </summary>
        public IEnumerable<UnitOfWorkItem> Dequeue()
        {
            lock (_itemsSync)
            {
                var actions = Items;
                Items = null;
                return actions;
            }
        }
    }
}