using System;
using System.Diagnostics;

namespace InfinniPlatform.PrintView.Factories.Block
{
    /// <summary>
    /// Координаты ячеки в таблице.
    /// </summary>
    [DebuggerDisplay("Row:{" + nameof(Row) + "}, Column:{" + nameof(Column) + "}")]
    public class CellCoordinate : IEquatable<CellCoordinate>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="row">Номер строки.</param>
        /// <param name="column">Номер столбца.</param>
        public CellCoordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }

        /// <summary>
        /// Номер строки.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Номер столбца.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Проверяет равенство текущего значения с указанным.
        /// </summary>
        /// <param name="other">Значение для сравнение с текущим.</param>
        public bool Equals(CellCoordinate other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Column == other.Column && Row == other.Row;
        }
    }
}