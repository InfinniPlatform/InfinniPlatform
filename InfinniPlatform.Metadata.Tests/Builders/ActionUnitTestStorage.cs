using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.Metadata.Tests.Builders
{
    public sealed class ActionUnitTestStorage
    {
        public void Action(IApplyContext applyContext)
        {
            new IndexApi().InsertDocument(applyContext.Item, "Handlers", "patienttest");
        }
    }
}