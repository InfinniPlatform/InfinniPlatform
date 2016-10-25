using System;

namespace InfinniPlatform.PrintView.Model
{
    /// <summary>
    /// Толщина сторон прямоугольника.
    /// </summary>
    [Serializable]
    public class PrintThickness
    {
        /// <summary>
        /// Нулевая толщина.
        /// </summary>
        public static readonly PrintThickness Zero = new PrintThickness();


        /// <summary>
        /// Конструктор.
        /// </summary>
        public PrintThickness() : this(0, PrintSizeUnit.Pt)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="all">Все границы.</param>
        /// <param name="sizeUnit">Единица измерения.</param>
        public PrintThickness(double all, PrintSizeUnit sizeUnit) : this(all, all, all, all, sizeUnit)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="left">Левая граница.</param>
        /// <param name="top">Верхняя граница.</param>
        /// <param name="right">Правая граница.</param>
        /// <param name="bottom">Нижняя граница.</param>
        /// <param name="sizeUnit">Единица измерения.</param>
        public PrintThickness(double left, double top, double right, double bottom, PrintSizeUnit sizeUnit)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
            SizeUnit = sizeUnit;
        }


        /// <summary>
        /// Левая граница.
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// Верхняя граница.
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        /// Правая граница.
        /// </summary>
        public double Right { get; set; }

        /// <summary>
        /// Нижняя граница.
        /// </summary>
        public double Bottom { get; set; }

        /// <summary>
        /// Единица измерения.
        /// </summary>
        public PrintSizeUnit SizeUnit { get; set; }
    }
}