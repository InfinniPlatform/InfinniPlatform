using InfinniPlatform.Api.Profiling;

namespace InfinniPlatform.Logging
{
	/// <summary>
	/// Фабрика для создания <see cref="ILog"/>.
	/// </summary>
	public interface ILogFactory
	{
		/// <summary>
		/// Создает <see cref="ILog"/>.
		/// </summary>
		ILog CreateLog();
	}
}