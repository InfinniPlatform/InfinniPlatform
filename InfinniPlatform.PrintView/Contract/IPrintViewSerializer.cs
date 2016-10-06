using System;
using System.IO;

using InfinniPlatform.PrintView.Model;

namespace InfinniPlatform.PrintView.Contract
{
    /// <summary>
    /// Сериализатор для <see cref="PrintDocument" />.
    /// </summary>
    public interface IPrintViewSerializer
    {
        /// <summary>
        /// Сериализует документ.
        /// </summary>
        /// <param name="stream">Поток для записи.</param>
        /// <param name="document">Документ печатного представления.</param>
        void Serialize(Stream stream, PrintDocument document);

        /// <summary>
        /// Десериализует документ.
        /// </summary>
        /// <param name="stream">Поток для чтения.</param>
        /// <returns>Документ печатного представления.</returns>
        PrintDocument Deserialize(Stream stream);
    }
}