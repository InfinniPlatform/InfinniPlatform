using System;
using System.Collections.Generic;

namespace InfinniPlatform.EventStorage
{
	/// <summary>
	/// Represents event storage.
	/// </summary>
	public interface IEventStorage
	{
		/// <summary>
		/// Returns events.
		/// </summary>
		/// <param name="id">The unique identifier of the aggregate.</param>
		/// <returns>The list events of the aggregate.</returns>
		IEnumerable<string> GetEvents(Guid id);

		/// <summary>
		/// Adds events.
		/// </summary>
		/// <param name="id">The unique identifier of the aggregate.</param>
		/// <param name="events">The list events of the aggregate.</param>
		void AddEvents(Guid id, IEnumerable<string> events);
	}
}