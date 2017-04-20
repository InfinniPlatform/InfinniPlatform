using System;

using InfinniPlatform.PrintView.Properties;

namespace InfinniPlatform.PrintView.Inline
{
    /// <summary>
    /// Элемент для создания штрих-код в формате EAN13.
    /// </summary>
    [Serializable]
    public class PrintBarcodeEan13 : PrintBarcode
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "BarcodeEan13";


        /// <summary>
        /// Автоматически рассчитывать контрольную сумму.
        /// </summary>
        public bool? CalcCheckSum { get; set; }

        /// <summary>
        /// Относительная ширина штрихов в штрих-коде.
        /// </summary>
        public double? WideBarRatio { get; set; }


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintBarcodeEan13;
        }
    }
}