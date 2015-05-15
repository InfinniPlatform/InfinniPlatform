﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Metadata
{
	/// <summary>
	///   Провайдер наименований индексов метаданных
	/// </summary>
	public interface IMetadataIndexNameProvider
	{
		/// <summary>
		///   Получить наименование индекса с указанным идентификатором метаданных
		/// </summary>
		/// <param name="baseName">Идентификатор метаданных</param>
		/// <returns>Наименование индекса метаданных</returns>
		string GetMetadataIndexName(string baseName);
	}
}
