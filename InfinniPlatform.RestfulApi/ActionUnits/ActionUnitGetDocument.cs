using System;

using InfinniPlatform.ContextComponents;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
	public sealed class ActionUnitGetDocument
	{
	    private readonly IConfigurationMediatorComponent _configurationMediatorComponent;
	    private readonly IMetadataComponent _metadataComponent;
	    private readonly InprocessDocumentComponent _inprocessDocumentComponent;
	    private readonly IProfilerComponent _profilerComponent;
	    private readonly ILogComponent _logComponent;
	    private readonly IReferenceResolver _referenceResolver;

	    public ActionUnitGetDocument(IConfigurationMediatorComponent configurationMediatorComponent,
	                                 IMetadataComponent metadataComponent,
	                                 InprocessDocumentComponent inprocessDocumentComponent,
	                                 IProfilerComponent profilerComponent,
	                                 ILogComponent logComponent,
	                                 IReferenceResolver referenceResolver)
	    {
	        _configurationMediatorComponent = configurationMediatorComponent;
	        _metadataComponent = metadataComponent;
	        _inprocessDocumentComponent = inprocessDocumentComponent;
	        _profilerComponent = profilerComponent;
	        _logComponent = logComponent;
	        _referenceResolver = referenceResolver;
	    }

	    public void Action(IApplyContext target)
		{
	        var executor = new DocumentExecutor(_configurationMediatorComponent,
	                                            _metadataComponent,
	                                            _inprocessDocumentComponent,
	                                            _profilerComponent,
	                                            _logComponent,
	                                            _referenceResolver);

	        target.Result = executor.GetCompleteDocuments(null,
	                                                      target.Item.Configuration,
	                                                      target.Item.Metadata,
	                                                      target.UserName,
	                                                      Convert.ToInt32(target.Item.PageNumber),
	                                                      Convert.ToInt32(target.Item.PageSize),
	                                                      target.Item.Filter,
	                                                      target.Item.Sorting,
	                                                      target.Item.IgnoreResolve);
		}
	}
}