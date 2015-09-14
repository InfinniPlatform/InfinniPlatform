using System;
using System.Text.RegularExpressions;

namespace InfinniPlatform.Api.Extensions
{
    public static class CommonExtensions
    {
        /// <summary>
        ///     Определяет, является ли значение числом.
        /// </summary>
        public static bool IsNumber(this object target)
        {
            return (target != null)
                   && (target is byte
                       || target is sbyte
                       || target is short
                       || target is ushort
                       || target is int
                       || target is uint
                       || target is long
                       || target is ulong
                       || target is float
                       || target is double
                       || target is decimal);
        }

        /// <summary>
        ///     Преобразует строку в перечисление заданного типа.
        /// </summary>
        public static TEnum ToEnum<TEnum>(this string target, TEnum defaultValue = default(TEnum)) where TEnum : struct
        {
            TEnum result;

            if (Enum.TryParse(target, true, out result) == false)
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        ///     Проверяет, содержится ли подстрока в строке.
        /// </summary>
        public static bool Contains(this string target, string value, StringComparison comparisonType)
        {
            return target.IndexOf(value, comparisonType) >= 0;
        }

        /// <summary>
        ///     Заменяет в строке одну подстроку на другую.
        /// </summary>
        public static string Replace(this string target, string oldValue, string newValue, bool matchCase = false,
            bool wholeWord = false)
        {
            if (!string.IsNullOrEmpty(target) && !string.IsNullOrEmpty(oldValue))
            {
                if (matchCase && !wholeWord)
                {
                    target = target.Replace(oldValue, newValue);
                }
                else
                {
                    oldValue = Regex.Escape(oldValue);

                    if (wholeWord)
                    {
                        var targetCopy = target;
                        var evaluator = new MatchEvaluator(match => WholeWordEvaluator(match, targetCopy, newValue));

                        target = Regex.Replace(target, oldValue, evaluator,
                            matchCase ? RegexOptions.None : RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        target = Regex.Replace(target, oldValue, newValue, RegexOptions.IgnoreCase);
                    }
                }
            }

            return target;
        }

        /// <summary>
        ///     Возвращает индекс первого вхождения подстроки в строку.
        /// </summary>
        public static int FindNextIndexOf(this string target, string value, int startIndex = 0, bool matchCase = false,
            bool wholeWord = false)
        {
            var index = -1;

            if (!string.IsNullOrEmpty(target) && !string.IsNullOrEmpty(value))
            {
                if (wholeWord)
                {
                    value = Regex.Escape(value);

                    var matches = Regex.Matches(target, value, matchCase ? RegexOptions.None : RegexOptions.IgnoreCase);

                    foreach (Match match in matches)
                    {
                        if (match.Index >= startIndex && IsWholeWord(match, target))
                        {
                            index = match.Index;
                            break;
                        }
                    }
                }
                else
                {
                    var comparisonType = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

                    index = target.IndexOf(value, startIndex, comparisonType);
                }
            }

            return index;
        }

        /// <summary>
        ///     Возвращает индекс последнего вхождения подстроки в строку.
        /// </summary>
        public static int FindPreviousIndexOf(this string target, string value, int startIndex = 0,
            bool matchCase = false, bool wholeWord = false)
        {
            var index = -1;

            if (!string.IsNullOrEmpty(target) && !string.IsNullOrEmpty(value))
            {
                if (wholeWord)
                {
                    value = Regex.Escape(value);

                    var matches = Regex.Matches(target, value, matchCase ? RegexOptions.None : RegexOptions.IgnoreCase);

                    foreach (Match match in matches)
                    {
                        if (match.Index + match.Length <= startIndex)
                        {
                            if (IsWholeWord(match, target))
                            {
                                index = match.Index;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    var comparisonType = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

                    index = (startIndex > 0) ? target.LastIndexOf(value, startIndex, comparisonType) : -1;

                    if (index != -1 && index + value.Length > startIndex)
                    {
                        index = (index > 0) ? target.LastIndexOf(value, index - 1, comparisonType) : -1;
                    }
                }
            }

            return index;
        }

        private static string WholeWordEvaluator(Match match, string target, string newValue)
        {
            return IsWholeWord(match, target) ? newValue : match.Value;
        }

        private static bool IsWholeWord(Match match, string target)
        {
            // Длина строк совпадает, либо начинается и заканчивается не с символа и не со строки

            return (match.Length == target.Length)
                   || ((match.Index == 0 || !char.IsLetterOrDigit(target[match.Index - 1]))
                       &&
                       (match.Index + match.Length == target.Length ||
                        !char.IsLetterOrDigit(target[match.Index + match.Length])));
        }
    }
}