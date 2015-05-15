namespace InfinniPlatform.FastReport.Templates.Data
{
	/// <summary>
	/// Строковая константа или выражение.
	/// </summary>
	public sealed class ConstantBind : IDataBind
	{
		/// <summary>
		/// Константа или выражение.
		/// </summary>
		public object Value { get; set; }
	}
}