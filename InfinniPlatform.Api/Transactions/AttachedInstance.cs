namespace InfinniPlatform.Api.Transactions
{
	public sealed class AttachedInstance
	{
		public string ConfigId
		{
			get { return Instance.Configuration; }
		}

		public string DocumentId
		{
			get { return Instance.Metadata; }
		}

		public dynamic Instance { get; set; }

		public string Version { get; set; }

		public string Routing { get; set; }
	}
}
