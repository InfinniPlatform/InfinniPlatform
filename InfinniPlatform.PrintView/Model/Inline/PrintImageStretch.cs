namespace InfinniPlatform.PrintView.Model.Inline
{
    /// <summary>
    /// Растягивание изображения.
    /// </summary>
    public enum PrintImageStretch
    {
        /// <summary>
        /// Не растягивать изображение.
        /// </summary>
        None,

        /// <summary>
        /// Растягивать изображение по размеру контейнера.
        /// </summary>
        Fill,

        /// <summary>
        /// Растягивать изображение по размеру контейнера с сохранением пропорций.
        /// </summary>
        Uniform
    }
}