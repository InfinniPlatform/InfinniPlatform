using System.Collections;
using System.Linq;

namespace InfinniPlatform.PrintView.Expressions.BuiltInTypes
{
    internal static class ConditionFunctions
    {
        public static object If(bool expression, object whenTrue, object whenFalse)
        {
            return expression ? whenTrue : whenFalse;
        }

        public static object Switch(object expression, IEnumerable keys, IEnumerable values, object defaultValue = null)
        {
            if (keys != null)
            {
                var i = 0;

                foreach (var key in keys)
                {
                    if (Equals(expression, key))
                    {
                        return (values != null) ? values.Cast<object>().ElementAtOrDefault(i) : null;
                    }

                    ++i;
                }
            }

            return defaultValue;
        }
    }
}