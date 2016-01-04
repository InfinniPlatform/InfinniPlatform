using System.Collections.Generic;
using System.Threading;

namespace InfinniPlatform.SystemConfig.Administration.Menu
{
    internal static class MenuCache
    {
        private static readonly ReaderWriterLockSlim Lock;
        private static volatile bool _initialized;
        private static readonly Dictionary<string, object> Items;
        private static readonly Dictionary<string, List<object>> Relations;

        static MenuCache()
        {
            Lock = new ReaderWriterLockSlim();

            _initialized = false;

            Items = new Dictionary<string, object>();
            Relations = new Dictionary<string, List<object>>();
        }

        public static void EnterRead()
        {
            Lock.EnterReadLock();
        }

        public static void ExitRead()
        {
            Lock.ExitReadLock();
        }

        public static void EnterWrite()
        {
            Lock.EnterWriteLock();

            _initialized = false;
        }

        public static void ExitWrite(bool success)
        {
            if (!success)
            {
                Clear();
            }

            _initialized = success;

            Lock.ExitWriteLock();
        }

        public static void AddMenuItem(string id, string parentId, object menuItem)
        {
            if (!Items.ContainsKey(id))
            {
                Items.Add(id, menuItem);

                List<object> children;
                parentId = parentId ?? string.Empty;

                if (!Relations.TryGetValue(parentId, out children))
                {
                    children = new List<object>();
                    Relations.Add(parentId, children);
                }

                children.Add(menuItem);
            }
        }

        public static bool TryGetMenuItems(string parentId, out IEnumerable<object> menuItems)
        {
            Lock.EnterReadLock();

            try
            {
                if (_initialized)
                {
                    if (string.IsNullOrEmpty(parentId))
                    {
                        menuItems = Items.Values;
                    }
                    else
                    {
                        List<object> children;
                        Relations.TryGetValue(parentId, out children);
                        menuItems = children;
                    }

                    return true;
                }

                menuItems = null;
                return false;
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        private static void Clear()
        {
            Items.Clear();
            Relations.Clear();
        }
    }
}