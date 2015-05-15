namespace InfinniPlatform.FastReport.Templates.Elements
{
	/// <summary>
	/// Элемент отчета.
	/// </summary>
	public interface IElement
	{
		/// <summary>
		/// Расположение элемента.
		/// </summary>
		ElementLayout Layout { get; set; }
	}
}