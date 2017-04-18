using System.Collections.Generic;

namespace InfinniPlatform.PrintView.Abstractions
{
    /// <summary>
    /// Предоставляет метод для преобразования размеров из разных единиц измерения в унифицированный формат.
    /// </summary>
    public static class PrintSizeUnitConverter
    {
        /// <summary>
        /// Количество пунктов в пикселе.
        /// </summary>
        /// <remarks>
        /// 1 pt == (96/72) px == (1/72) in == (25.4/72) mm
        /// </remarks>
        private const double Pt = 96.0 / 72.0;

        /// <summary>
        /// Количество пикселей в пикселе.
        /// </summary>
        /// <remarks>
        /// 1 px == (72/96) pt == (1/96) in == (25.4/96) mm
        /// </remarks>
        private const double Px = 1.0;

        /// <summary>
        /// Количество пикселей в дюйме.
        /// </summary>
        /// <remarks>
        /// 1 in == 72 pt == 96 px == 25.4 mm
        /// </remarks>
        private const double In = 96.0;

        /// <summary>
        /// Количество пикселей в сантиметре.
        /// </summary>
        /// <remarks>
        /// 1 cm == (720/25.4) pt == (960/25.4) px == (10/25.4) in
        /// </remarks>
        private const double Cm = 960.0 / 25.4;

        /// <summary>
        /// Количество пикселей в миллиметре.
        /// </summary>
        /// <remarks>
        /// 1 mm == (72/25.4) pt == (96/25.4) px == (1/25.4) in
        /// </remarks>
        private const double Mm = 96.0 / 25.4;


        private static readonly Dictionary<PrintSizeUnit, double> Factors
            = new Dictionary<PrintSizeUnit, double>
              {
                { PrintSizeUnit.Pt, Pt },
                { PrintSizeUnit.Px, Px },
                { PrintSizeUnit.In, In },
                { PrintSizeUnit.Cm, Cm },
                { PrintSizeUnit.Mm, Mm }
              };


        /// <summary>
        /// Унифицированная единица измерения размера.
        /// </summary>
        public const PrintSizeUnit UnifiedSizeUnit = PrintSizeUnit.Px;


        /// <summary>
        /// Возвращает значение размера в формате <see cref="UnifiedSizeUnit" />.
        /// </summary>
        /// <param name="fromSize">Значение размера.</param>
        /// <param name="fromSizeUnit">Единица измерения размера.</param>
        public static double ToUnifiedSize(double fromSize, PrintSizeUnit fromSizeUnit)
        {
            if (double.IsNaN(fromSize))
            {
                return 0;
            }

            return fromSize * GetUnifiedSizeFactor(fromSizeUnit);
        }

        /// <summary>
        /// Преобразует значение размера из одного формата в другой.
        /// </summary>
        /// <param name="fromSize">Значение размера.</param>
        /// <param name="fromSizeUnit">Единица измерения размера.</param>
        /// <param name="toSizeUnit">Единица измерения результата.</param>
        public static double ToSpecifiedSize(double fromSize, PrintSizeUnit fromSizeUnit, PrintSizeUnit toSizeUnit)
        {
            if (double.IsNaN(fromSize))
            {
                return 0;
            }

            return fromSize * GetUnifiedSizeFactor(fromSizeUnit) / GetUnifiedSizeFactor(toSizeUnit);
        }


        private static double GetUnifiedSizeFactor(PrintSizeUnit sizeUnit)
        {
            double factor;

            return Factors.TryGetValue(sizeUnit, out factor) ? factor : Pt;
        }
    }
}