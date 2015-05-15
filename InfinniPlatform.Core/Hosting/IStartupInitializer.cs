namespace InfinniPlatform.Hosting
{
	/// <summary>
	/// Контракт инициализации модуля при событии старта сервера.
	/// </summary>
	public interface IStartupInitializer
	{
		/// <summary>
		/// При старте сервера.
		/// </summary>
		/// <param name="contextBuilder"></param>
		void OnStart(HostingContextBuilder contextBuilder);
	}
}