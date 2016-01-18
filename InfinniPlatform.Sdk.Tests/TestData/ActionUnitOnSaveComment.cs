using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Sdk.Tests.TestData
{
    public sealed class ActionUnitOnSaveComment
    {
        public ActionUnitOnSaveComment(DocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly DocumentApi _documentApi;

        public void Action(IActionContext target)
        {
            target.Item.Text = target.Item.Text + "123";

            _documentApi.SetDocument("gameshop", "review", new DynamicWrapper { ["Text"] = "test" });
        }
    }
}