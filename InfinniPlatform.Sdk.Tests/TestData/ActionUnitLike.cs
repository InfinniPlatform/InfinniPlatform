using System.Linq;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Sdk.Tests.TestData
{
    public sealed class ActionUnitLike
    {
        public void Action(IApplyContext target)
        {
            var documentApi = target.Context.GetComponent<DocumentApi>();

            dynamic document = documentApi.GetDocument("Gameshop", "review", f => f.AddCriteria(c => c.Property("Id").IsEquals(target.Item.DocumentId)), 0, 1).FirstOrDefault();

            document.Likes = document.Likes + 1;

            documentApi.SetDocument("Gameshop", "review", document);
        }
    }
}