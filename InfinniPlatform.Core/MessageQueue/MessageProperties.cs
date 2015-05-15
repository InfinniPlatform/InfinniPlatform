namespace InfinniPlatform.MessageQueue
{
	/// <summary>
	/// Свойства сообщения.
	/// </summary>
	public sealed class MessageProperties
	{
		/// <summary>
		/// Идентификатор приложения.
		/// </summary>
		public string AppId { get; set; }

		/// <summary>
		/// Идентификатор пользователя.
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// Наименование типа сообщения.
		/// </summary>
		public string TypeName { get; set; }

		/// <summary>
		/// Идентификатор сообщения.
		/// </summary>
		public string MessageId { get; set; }

		/// <summary>
		/// MIME тип сообщения.
		/// </summary>
		public string ContentType { get; set; }

		/// <summary>
		/// Кодировка сообщения.
		/// </summary>
		public string ContentEncoding { get; set; }

		/// <summary>
		/// Заголовок сообщения.
		/// </summary>
		public MessageHeaders Headers { get; set; }
	}
}