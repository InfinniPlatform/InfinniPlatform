using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InfinniPlatform.SystemConfig.Installers
{
	sealed class ActionUnitsEntryList
	{
		public const string PrefillIndex = "prefill";

		private IDictionary<string,string> _entries = new Dictionary<string, string>();

		public ActionUnitsEntryList(Assembly assembly, string searchString)
		{
			FindEntries(assembly,searchString);
		}

		/// <summary>
		///   Список найденных в сборке модулей
		/// </summary>
		public IDictionary<string, string> Entries
		{
			get { return _entries; }
		}

		private void FindEntries(Assembly assembly, string searchString)
		{
			var prefillItems = assembly.GetTypes().Where(t => t.Name.ToLowerInvariant().Contains(PrefillIndex.ToLowerInvariant()) && t.Name.ToLowerInvariant().Contains("actionunit")).ToList();
			foreach (Type prefillItem in prefillItems)
			{
				var registerName = prefillItem.Name.ToLowerInvariant().Replace("actionunit", "");
				if (Entries.ContainsKey(registerName))
				{
					continue;					
				}

				Entries.Add(registerName,prefillItem.Name);
			}
		}

	}

}
