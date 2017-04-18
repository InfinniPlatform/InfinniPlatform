using System;

namespace InfinniPlatform.PrintView.Abstractions
{
    /// <summary>
    /// Размеры прямоугольника.
    /// </summary>
    [Serializable]
    public class PrintSize
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public PrintSize()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="width">Ширина.</param>
        /// <param name="height">Высота.</param>
        /// <param name="sizeUnit">Единица измерения размеров.</param>
        public PrintSize(double width, double height, PrintSizeUnit sizeUnit)
        {
            Width = width;
            Height = height;
            SizeUnit = sizeUnit;
        }


        /// <summary>
        /// Ширина.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Высота.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Единица измерения размеров.
        /// </summary>
        public PrintSizeUnit SizeUnit { get; set; }
    }
}