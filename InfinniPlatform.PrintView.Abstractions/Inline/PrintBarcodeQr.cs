using System;

using InfinniPlatform.PrintView.Abstractions.Properties;

namespace InfinniPlatform.PrintView.Abstractions.Inline
{
    /// <summary>
    /// Элемент для создания штрих-кода в формате QR.
    /// </summary>
    [Serializable]
    public class PrintBarcodeQr : PrintBarcode
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "BarcodeQr";


        /// <summary>
        /// Уровень защиты от ошибок.
        /// </summary>
        public PrintBarcodeQrErrorCorrection? ErrorCorrection { get; set; }


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintBarcodeQr;
        }
    }
}