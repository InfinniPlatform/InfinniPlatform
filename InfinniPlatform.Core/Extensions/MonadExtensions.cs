using System;

namespace InfinniPlatform.Core.Extensions
{
    public static class MonadExtensions
    {
        public static T Maybe<T>(this T target, Action<T> action) where T : class
        {
            if (target != null)
            {
                action(target);
            }

            return target;
        }

        public static TResult Maybe<T, TResult>(this T target, Func<T, TResult> result,
            TResult defaultResult = default(TResult)) where T : class
        {
            if (target != null)
            {
                return result(target);
            }

            return defaultResult;
        }
    }
}