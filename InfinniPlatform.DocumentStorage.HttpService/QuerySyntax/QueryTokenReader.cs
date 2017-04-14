using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using InfinniPlatform.DocumentStorage.HttpService.Properties;

namespace InfinniPlatform.DocumentStorage.HttpService.QuerySyntax
{
    /// <summary>
    /// Предоставляет интерфейс для чтения токенов строки запроса.
    /// </summary>
    public class QueryTokenReader
    {
        private const char OpenBracket = '(';
        private const char CloseBracket = ')';
        private const char NegativeSign = '-';
        private const char PositiveSign = '+';
        private const char DecimalSeparator = '.';
        private const char ArgumentSeparator = ',';
        private const char StringQuote = '\'';
        private const char EscapeSymbol = '\\';
        private const string NullTerm = "null";
        private const string TrueTerm = "true";
        private const string FalseTerm = "false";


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="query">Строка запроса.</param>
        public QueryTokenReader(string query)
        {
            _query = query ?? string.Empty;
        }


        private readonly string _query;
        private int _index;


        /// <summary>
        /// Начинает разбор строки запроса с начала.
        /// </summary>
        public void Reset()
        {
            _index = 0;
        }


        /// <summary>
        /// Проверяет, возможно ли чтение очередного токена строки запроса.
        /// </summary>
        public bool CanRead()
        {
            return (_index < _query.Length);
        }


        /// <summary>
        /// Возвращает очередной токен строки запроса.
        /// </summary>
        public QueryToken ReadNext()
        {
            for (; _index < _query.Length; ++_index)
            {
                var c = _query[_index];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                QueryToken token;

                if (c == OpenBracket)
                {
                    token = new QueryToken(QueryTokenKind.OpenBracket, OpenBracket);
                }
                else if (c == CloseBracket)
                {
                    token = new QueryToken(QueryTokenKind.CloseBracket, CloseBracket);
                }
                else if (c == ArgumentSeparator)
                {
                    token = new QueryToken(QueryTokenKind.ArgumentSeparator, ArgumentSeparator);
                }
                else if (c == NegativeSign || c == PositiveSign || c == DecimalSeparator || char.IsDigit(c))
                {
                    token = TryReadNumber();
                }
                else if (c == StringQuote)
                {
                    token = TryReadString();
                }
                else
                {
                    token = TryReadIdentifier();
                }

                if (token == null)
                {
                    var errorIndex = (_index < _query.Length) ? _index : _query.Length;
                    var errorSymbol = (_index < _query.Length) ? _query[_index] : ' ';

                    throw new InvalidOperationException(string.Format(Resources.IllegalSymbolInExpression, errorSymbol, errorIndex));
                }

                ++_index;

                return token;
            }

            return null;
        }


        private QueryToken TryReadNumber()
        {
            var value = new StringBuilder();
            var isIntegerPart = true;

            for (var i = 0; _index < _query.Length; ++_index, ++i)
            {
                var c = _query[_index];

                if (char.IsDigit(c))
                {
                    value.Append(c);
                }
                else if (c == DecimalSeparator)
                {
                    if (!isIntegerPart)
                    {
                        return null;
                    }

                    isIntegerPart = false;
                    value.Append(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                }
                else if (c == NegativeSign)
                {
                    if (i != 0)
                    {
                        return null;
                    }

                    value.Append(c);
                }
                else if (c == PositiveSign)
                {
                    if (i != 0)
                    {
                        return null;
                    }
                }
                else if (IsFinalSeparator(c))
                {
                    --_index;

                    break;
                }
                else
                {
                    return null;
                }
            }

            if (value.Length > 1 || (value.Length == 1 && value[0] != NegativeSign))
            {
                if (isIntegerPart)
                {
                    int intValue;

                    if (int.TryParse(value.ToString(), out intValue))
                    {
                        return new QueryToken(QueryTokenKind.Integer, intValue);
                    }

                    long longValue;

                    if (long.TryParse(value.ToString(), out longValue))
                    {
                        return new QueryToken(QueryTokenKind.Integer, longValue);
                    }
                }
                else
                {
                    double floatValue;

                    if (double.TryParse(value.ToString(), out floatValue))
                    {
                        return new QueryToken(QueryTokenKind.Float, floatValue);
                    }
                }
            }

            return null;
        }

        private QueryToken TryReadString()
        {
            var value = new StringBuilder();

            var hasEscape = false;

            for (var i = 0; _index < _query.Length; ++_index, ++i)
            {
                var c = _query[_index];

                if (c == StringQuote)
                {
                    if (i != 0)
                    {
                        var stringValue = hasEscape
                            ? Regex.Unescape(value.ToString())
                            : value.ToString();

                        return new QueryToken(QueryTokenKind.String, stringValue);
                    }
                }
                else if (c == EscapeSymbol)
                {
                    if (_index + 1 < _query.Length)
                    {
                        c = _query[++_index];

                        if (c != StringQuote)
                        {
                            hasEscape = true;
                            value.Append(EscapeSymbol);
                        }

                        value.Append(c);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    value.Append(c);
                }
            }

            return null;
        }

        private QueryToken TryReadIdentifier()
        {
            var value = new StringBuilder();

            for (; _index < _query.Length; ++_index)
            {
                var c = _query[_index];

                if (IsSeparator(c))
                {
                    --_index;

                    break;
                }

                value.Append(c);
            }

            if (value.Length > 0)
            {
                var stringValue = value.ToString();

                if (stringValue.Equals(NullTerm, StringComparison.OrdinalIgnoreCase))
                {
                    return new QueryToken(QueryTokenKind.Null, null);
                }

                if (stringValue.Equals(TrueTerm, StringComparison.OrdinalIgnoreCase))
                {
                    return new QueryToken(QueryTokenKind.Boolean, true);
                }

                if (stringValue.Equals(FalseTerm, StringComparison.OrdinalIgnoreCase))
                {
                    return new QueryToken(QueryTokenKind.Boolean, false);
                }

                return new QueryToken(QueryTokenKind.Identifier, stringValue);
            }

            return null;
        }


        private static bool IsSeparator(char c)
        {
            return (c == OpenBracket || IsFinalSeparator(c));
        }

        private static bool IsFinalSeparator(char c)
        {
            return (c == CloseBracket || c == ArgumentSeparator || char.IsWhiteSpace(c));
        }
    }
}