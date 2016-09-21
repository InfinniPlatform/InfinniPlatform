using System;

namespace InfinniPlatform.PrintView.Expressions.BuiltInTypes
{
    internal static class MathFunctions
    {
        public const double E = Math.E;
        public const double PI = Math.PI;
        private static readonly Random RandomGenerator = new Random(DateTime.Now.Millisecond);

        public static dynamic Abs(dynamic value)
        {
            return Math.Abs(value);
        }

        public static dynamic Floor(dynamic value)
        {
            return Math.Floor(value);
        }

        public static dynamic Ceiling(dynamic value)
        {
            return Math.Ceiling(value);
        }

        public static dynamic Round(dynamic value)
        {
            return Math.Round(value);
        }

        public static dynamic Round(dynamic value, int digits)
        {
            return Math.Round(value, digits);
        }

        public static dynamic Truncate(dynamic value)
        {
            return Math.Truncate(value);
        }

        public static dynamic Min(dynamic value1, dynamic value2)
        {
            return Math.Min(value1, value2);
        }

        public static dynamic Max(dynamic value1, dynamic value2)
        {
            return Math.Max(value1, value2);
        }

        public static dynamic Pow(dynamic value, dynamic power)
        {
            return Math.Pow(value, power);
        }

        public static dynamic Exp(dynamic value)
        {
            return Math.Exp(value);
        }

        public static dynamic Log(dynamic value)
        {
            return Math.Log(value);
        }

        public static dynamic Log(dynamic value, dynamic logBase)
        {
            return Math.Log(value, logBase);
        }

        public static dynamic Log10(dynamic value)
        {
            return Math.Log10(value);
        }

        public static dynamic Sin(dynamic value)
        {
            return Math.Sin(value);
        }

        public static dynamic Cos(dynamic value)
        {
            return Math.Cos(value);
        }

        public static dynamic Tan(dynamic value)
        {
            return Math.Tan(value);
        }

        public static dynamic Asin(dynamic value)
        {
            return Math.Asin(value);
        }

        public static dynamic Acos(dynamic value)
        {
            return Math.Acos(value);
        }

        public static dynamic Atan(dynamic value)
        {
            return Math.Atan(value);
        }

        public static dynamic Sinh(dynamic value)
        {
            return Math.Sinh(value);
        }

        public static dynamic Cosh(dynamic value)
        {
            return Math.Cosh(value);
        }

        public static dynamic Tanh(dynamic value)
        {
            return Math.Tanh(value);
        }

        public static dynamic Random()
        {
            return RandomGenerator.Next();
        }

        public static dynamic Random(int minValue, int maxValue)
        {
            return RandomGenerator.Next(minValue, maxValue);
        }
    }
}