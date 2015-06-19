using System;
using System.Collections;
using System.Linq;

namespace InfinniPlatform.Expressions.BuiltInTypes
{
    internal static class TextFunctions
    {
        public static readonly string Empty = string.Empty;

        public static int Length(string value)
        {
            return (value != null) ? value.Length : 0;
        }

        public static bool Equals(string left, string right)
        {
            return string.Equals(left, right);
        }

        public static bool Equals(string left, string right, bool ignoreCase)
        {
            return ignoreCase
                ? string.Equals(left, right, StringComparison.OrdinalIgnoreCase)
                : string.Equals(left, right);
        }

        public static bool IsNullOrEmpty(string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhiteSpace(string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string Concat(params object[] arguments)
        {
            return string.Concat(arguments);
        }

        public static string Concat(IEnumerable arguments)
        {
            if (arguments is string)
            {
                return string.Concat(arguments);
            }

            return string.Concat((arguments != null) ? arguments.Cast<object>().ToArray() : new object[] {});
        }

        public static string Format(string format, params object[] arguments)
        {
            return string.Format(format, arguments);
        }

        public static string Format(string format, IEnumerable arguments)
        {
            if (arguments is string)
            {
                return string.Format(format, arguments);
            }

            return string.Format(format, (arguments != null) ? arguments.Cast<object>().ToArray() : new object[] {});
        }

        public static string Join(string separator, params object[] arguments)
        {
            return string.Join(separator, arguments);
        }

        public static string Join(string separator, IEnumerable arguments)
        {
            if (arguments is string)
            {
                return string.Join(separator, arguments);
            }

            return string.Join(separator, (arguments != null) ? arguments.Cast<object>().ToArray() : new object[] {});
        }

        public static string[] Split(string value, params char[] separators)
        {
            return (value != null)
                ? value.Split(separators, StringSplitOptions.None)
                : new string[] {};
        }

        public static string[] Split(string value, params string[] separators)
        {
            return (value != null)
                ? value.Split(separators, StringSplitOptions.None)
                : new string[] {};
        }

        public static bool Contains(string value, string substring, bool ignoreCase = false)
        {
            return (value != null && substring != null)
                   &&
                   (ignoreCase
                       ? value.IndexOf(substring, StringComparison.OrdinalIgnoreCase) >= 0
                       : value.Contains(substring));
        }

        public static bool StartsWith(string value, string substring, bool ignoreCase = false)
        {
            return (value != null && substring != null)
                   &&
                   (ignoreCase
                       ? value.StartsWith(substring, StringComparison.OrdinalIgnoreCase)
                       : value.StartsWith(substring));
        }

        public static bool EndsWith(string value, string substring, bool ignoreCase = false)
        {
            return (value != null && substring != null)
                   &&
                   (ignoreCase
                       ? value.EndsWith(substring, StringComparison.OrdinalIgnoreCase)
                       : value.EndsWith(substring));
        }

        public static int IndexOf(string value, char symbol)
        {
            return (value != null)
                ? value.IndexOf(symbol)
                : -1;
        }

        public static int IndexOf(string value, char symbol, int startIndex)
        {
            startIndex = Math.Max(startIndex, 0);

            return (value != null && startIndex < value.Length)
                ? value.IndexOf(symbol, startIndex)
                : -1;
        }

        public static int IndexOf(string value, string substring, bool ignoreCase = false)
        {
            return (value != null && substring != null)
                ? value.IndexOf(substring,
                    ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.CurrentCulture)
                : -1;
        }

        public static int IndexOf(string value, string substring, int startIndex = 0, bool ignoreCase = false)
        {
            startIndex = Math.Max(startIndex, 0);

            return (value != null && substring != null)
                ? value.IndexOf(substring, Math.Min(startIndex, value.Length - 1),
                    ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.CurrentCulture)
                : -1;
        }

        public static int LastIndexOf(string value, char symbol)
        {
            return (value != null)
                ? value.LastIndexOf(symbol)
                : -1;
        }

        public static int LastIndexOf(string value, char symbol, int startIndex)
        {
            startIndex = Math.Max(startIndex, 0);

            return (value != null)
                ? value.LastIndexOf(symbol, Math.Min(startIndex, value.Length - 1))
                : -1;
        }

        public static int LastIndexOf(string value, string substring, bool ignoreCase = false)
        {
            return (value != null && substring != null)
                ? value.LastIndexOf(substring,
                    ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.CurrentCulture)
                : -1;
        }

        public static int LastIndexOf(string value, string substring, int startIndex, bool ignoreCase = false)
        {
            startIndex = Math.Max(startIndex, 0);

            return (value != null && substring != null)
                ? value.LastIndexOf(substring, Math.Min(startIndex, value.Length - 1),
                    ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.CurrentCulture)
                : -1;
        }

        public static string Substring(string value, int startIndex)
        {
            startIndex = Math.Max(startIndex, 0);

            return (value != null && startIndex < value.Length)
                ? value.Substring(startIndex)
                : null;
        }

        public static string Substring(string value, int startIndex, int length)
        {
            startIndex = Math.Max(startIndex, 0);

            return (value != null && startIndex < value.Length)
                ? value.Substring(startIndex, Math.Min(value.Length - startIndex, length))
                : null;
        }

        public static string ToLower(string value)
        {
            return (value != null)
                ? value.ToLower()
                : null;
        }

        public static string ToUpper(string value)
        {
            return (value != null)
                ? value.ToUpper()
                : null;
        }

        public static string Trim(string value)
        {
            return (value != null)
                ? value.Trim()
                : null;
        }

        public static string Trim(string value, params char[] trimChars)
        {
            return (value != null)
                ? value.Trim(trimChars)
                : null;
        }

        public static string TrimStart(string value)
        {
            return (value != null)
                ? value.TrimStart()
                : null;
        }

        public static string TrimStart(string value, params char[] trimChars)
        {
            return (value != null)
                ? value.TrimStart(trimChars)
                : null;
        }

        public static string TrimEnd(string value)
        {
            return (value != null)
                ? value.TrimEnd()
                : null;
        }

        public static string TrimEnd(string value, params char[] trimChars)
        {
            return (value != null)
                ? value.TrimEnd(trimChars)
                : null;
        }

        public static string PadLeft(string value, int totalWidth, char paddingChar = ' ')
        {
            return (value != null)
                ? value.PadLeft(Math.Max(totalWidth, 0), paddingChar)
                : null;
        }

        public static string PadRight(string value, int totalWidth, char paddingChar = ' ')
        {
            return (value != null)
                ? value.PadRight(Math.Max(totalWidth, 0), paddingChar)
                : null;
        }

        public static string Remove(string value, int startIndex)
        {
            startIndex = Math.Max(startIndex, 0);

            return (value != null && startIndex < value.Length)
                ? value.Remove(startIndex)
                : value;
        }

        public static string Remove(string value, int startIndex, int length)
        {
            startIndex = Math.Max(startIndex, 0);

            return (value != null && startIndex < value.Length)
                ? value.Remove(startIndex, Math.Min(value.Length - startIndex, length))
                : value;
        }

        public static string Replace(string value, char oldChar, char newChar)
        {
            return (value != null)
                ? value.Replace(oldChar, newChar)
                : null;
        }

        public static string Replace(string value, string oldValue, string newValue)
        {
            return (value != null && !string.IsNullOrEmpty(oldValue))
                ? value.Replace(oldValue, newValue ?? string.Empty)
                : value;
        }

        public static string Insert(string value, int startIndex, string substring)
        {
            return (value != null && substring != null)
                ? value.Insert(Math.Min(Math.Max(startIndex, 0), value.Length), substring)
                : value;
        }
    }
}