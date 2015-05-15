using System.Threading.Tasks;

using Microsoft.Owin;

namespace InfinniPlatform.Owin.Formatting
{
	/// <summary>
	/// Предоставляет методы чтения и записи тела запроса и ответа.
	/// </summary>
	public interface IBodyFormatter
	{
		/// <summary>
		/// Тип данных.
		/// </summary>
		string ContentType { get; }

		/// <summary>
		/// Возвращает значение тела запроса.
		/// </summary>
		object ReadBody(IOwinRequest request);

		/// <summary>
		/// Записывает значение в тело ответа.
		/// </summary>
		Task WriteBody(IOwinResponse response, object value);
	}
}