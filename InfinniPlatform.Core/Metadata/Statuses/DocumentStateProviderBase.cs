using System.Collections.Generic;
using InfinniPlatform.Api.DocumentStatus;

namespace InfinniPlatform.Metadata.Statuses
{
    /// <summary>
    /// Провайдер основных "физических" состояний документа, необходимых для реализации создания документов, удаления и т.д.
    /// </summary>
	public class DocumentStateProviderBase : IDocumentStateProvider
	{
	    private readonly IStatusFactory _statusFactory;
        private const string StateFieldName = "Status";

	    public DocumentStateProviderBase(IStatusFactory statusFactory)
	    {
	        _statusFactory = statusFactory;
	    }

        public IStatus DefaultStatus
		{
            get { return Temporary; }
		}

        public IEnumerable<IStatus> GetStatusesList()
        {
            return new[] {Temporary, Saved, Published};
        }

		public IStatus Saved
		{
			get { return _statusFactory.Get(StateFieldName, "Saved", null, null); }
		}

        public IStatus Temporary
		{
            get { return _statusFactory.Get(StateFieldName, "Temporary", null, null); }
		}

        public IStatus Published
		{
            get { return _statusFactory.Get(StateFieldName, "Published", null, null); }
		}

        public IStatus Invalid
		{
            get { return _statusFactory.Get(StateFieldName, "Invalid", null, null); }
		}

        public IStatus Deleted
		{
            get { return _statusFactory.Get(StateFieldName, "Deleted", null, null); }
		}
	}
}
