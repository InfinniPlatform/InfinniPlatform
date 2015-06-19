using System;
using System.Collections;
using System.Windows.Threading;

namespace InfinniPlatform.UserInterface
{
    internal static class CommonExtensions
    {
        public static void InvokeControl(this DispatcherObject control, Action action)
        {
            if (control.Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                control.Dispatcher.BeginInvoke(DispatcherPriority.Background, action);
            }
        }

        public static bool IsNullOrEmpty(this IEnumerable collection)
        {
            var result = true;

            if (collection != null)
            {
                var enumerator = collection.GetEnumerator();

                try
                {
                    if (enumerator.MoveNext())
                    {
                        result = false;
                    }
                }
                finally
                {
                    var disposable = enumerator as IDisposable;

                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }

            return result;
        }

        public static object FirstOrNull(this IEnumerable collection)
        {
            object result = null;

            if (collection != null)
            {
                var list = collection as IList;

                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        result = list[0];
                    }
                }
                else
                {
                    var enumerator = collection.GetEnumerator();

                    try
                    {
                        if (enumerator.MoveNext())
                        {
                            result = enumerator.Current;
                        }
                    }
                    finally
                    {
                        var disposable = enumerator as IDisposable;

                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }
                }
            }

            return result;
        }
    }
}