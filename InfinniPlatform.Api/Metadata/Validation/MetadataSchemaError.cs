using System;

using InfinniPlatform.Api.Properties;

namespace InfinniPlatform.Api.Metadata.Validation
{
	/// <summary>
	/// Ошибка в схеме объекта метаданных.
	/// </summary>
	[Serializable]
	public sealed class MetadataSchemaError
	{
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="message">Сообщение об ошибке.</param>
		/// <param name="path">Путь к свойству.</param>
		/// <param name="lineNumber">Номер строки.</param>
		/// <param name="lineColumn">Номер столбца.</param>
		public MetadataSchemaError(string message, string path, int lineNumber, int lineColumn)
		{
			if (string.IsNullOrEmpty(message))
			{
				message = path;
			}

			if (!string.IsNullOrEmpty(message) && !message.Contains(path))
			{
				message += string.Format(Resources.SchemaPathInfo, path);
			}

			Message = message;
			Path = path;
			LineNumber = lineNumber;
			LineColumn = lineColumn;
		}

		/// <summary>
		/// Сообщение об ошибке.
		/// </summary>
		public readonly string Message;

		/// <summary>
		/// Путь к свойству.
		/// </summary>
		public readonly string Path;

		/// <summary>
		/// Номер строки.
		/// </summary>
		public readonly int LineNumber;

		/// <summary>
		/// Номер столбца.
		/// </summary>
		public readonly int LineColumn;
	}
}