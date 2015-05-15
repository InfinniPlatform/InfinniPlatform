namespace InfinniPlatform.PrintViewDesigner.Common
{
	sealed class AppClipboardEntry
	{
		public AppClipboardEntry(object data, bool copy)
		{
			Data = data;
			Copy = copy;
		}

		public readonly object Data;

		public readonly bool Copy;
	}
}