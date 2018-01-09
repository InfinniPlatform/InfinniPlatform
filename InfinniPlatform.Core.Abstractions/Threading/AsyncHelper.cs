using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace InfinniPlatform.Threading
{
    /// <summary>
    /// Wrapper methods to run asynchronous as synchronous.
    /// </summary>
    public static class AsyncHelper
    {
        private static readonly TaskFactory InternalTaskFactory
            = new TaskFactory(CancellationToken.None,
                              TaskCreationOptions.None,
                              TaskContinuationOptions.None,
                              TaskScheduler.Default);


        /// <summary>
        /// Runs task synchronously.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func">Task.</param>
        /// <returns>Task result.</returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            var culture = CultureInfo.CurrentCulture;
            var cultureUi = CultureInfo.CurrentUICulture;

            var syncTask = InternalTaskFactory.StartNew(() =>
                                                        {
                                                            CultureInfo.CurrentCulture = culture;
                                                            CultureInfo.CurrentUICulture = cultureUi;
                                                            return func();
                                                        });

            return syncTask.Unwrap().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Runs task synchronously.
        /// </summary>
        /// <param name="func">Task.</param>
        public static void RunSync(Func<Task> func)
        {
            var culture = CultureInfo.CurrentCulture;
            var cultureUi = CultureInfo.CurrentUICulture;

            var syncTask = InternalTaskFactory.StartNew(() =>
                                                        {
                                                            CultureInfo.CurrentCulture = culture;
                                                            CultureInfo.CurrentUICulture = cultureUi;
                                                            return func();
                                                        });

            syncTask.Unwrap().GetAwaiter().GetResult();
        }
    }
}