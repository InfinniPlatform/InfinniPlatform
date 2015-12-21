using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Sdk.Tests.TestData
{
    public sealed class ActionUnitOnSaveComment
    {
        public void Action(IApplyContext target)
        {
            target.Item.Text = target.Item.Text + "123";
            var documentApi = target.Context.GetComponent<DocumentApi>();
            documentApi.SetDocument("gameshop", "review", new { Text = "test" });
        }
    }
}