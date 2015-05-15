using System;
using System.IO;

namespace InfinniPlatform.Api.Serialization
{
	/// <summary>
	/// Сериализатор объектов.
	/// </summary>
	public interface IObjectSerializer
	{
		/// <summary>
		/// Сериализовать объект.
		/// </summary>
		/// <param name="value">Объект.</param>
		/// <returns>Сериализованное представление объекта.</returns>
		byte[] Serialize(object value);

		/// <summary>
		/// Сериализовать объект.
		/// </summary>
		/// <param name="data">Поток для записи сериализованного представление объекта.</param>
		/// <param name="value">Объект.</param>
		void Serialize(Stream data, object value);


		/// <summary>
		/// Десериализовать объект.
		/// </summary>
		/// <param name="data">Сериализованное представление объекта.</param>
		/// <param name="type">Тип объекта.</param>
		/// <returns>Объект.</returns>
		object Deserialize(byte[] data, Type type);

		/// <summary>
		/// Десериализовать объект.
		/// </summary>
		/// <param name="data">Поток для чтения сериализованного представление объекта.</param>
		/// <param name="type">Тип объекта.</param>
		/// <returns>Объект.</returns>
		object Deserialize(Stream data, Type type);
	}
}