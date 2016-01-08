using System.Linq;

using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Sdk.Tests.TestData
{
    public sealed class ActionUnitLike
    {
        public ActionUnitLike(DocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly DocumentApi _documentApi;

        public void Action(IApplyContext target)
        {
            dynamic document = _documentApi.GetDocument("Gameshop", "review", f => f.AddCriteria(c => c.Property("Id").IsEquals(target.Item.DocumentId)), 0, 1).FirstOrDefault();

            document.Likes = document.Likes + 1;

            _documentApi.SetDocument("Gameshop", "review", document);
        }
    }
}