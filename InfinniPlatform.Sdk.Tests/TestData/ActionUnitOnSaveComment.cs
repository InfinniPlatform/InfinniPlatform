using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Sdk.Tests.TestData
{
    public sealed class ActionUnitOnSaveComment
    {
        public ActionUnitOnSaveComment(IDocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly IDocumentApi _documentApi;

        public void Action(IActionContext target)
        {
            target.Item.Text = target.Item.Text + "123";

            _documentApi.SetDocument("gameshop", "review", new DynamicWrapper { ["Text"] = "test" });
        }
    }
}