using InfinniPlatform.PrintView.Properties;

namespace InfinniPlatform.PrintView.Model.Inline
{
    /// <summary>
    /// Элемент для отображения изображения.
    /// </summary>
    public class PrintImage : PrintInline
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "Image";


        /// <summary>
        /// Данные изображения.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Размеры изображения.
        /// </summary>
        public PrintSize Size { get; set; }

        /// <summary>
        /// Поворот изображения.
        /// </summary>
        public PrintImageRotation? Rotation { get; set; }

        /// <summary>
        /// Растягивание изображения.
        /// </summary>
        public PrintImageStretch? Stretch { get; set; }


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintImage;
        }
    }
}