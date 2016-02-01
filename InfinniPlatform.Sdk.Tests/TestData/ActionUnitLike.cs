using System.Linq;

using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Sdk.Tests.TestData
{
    public sealed class ActionUnitLike
    {
        public ActionUnitLike(IDocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly IDocumentApi _documentApi;

        public void Action(IActionContext target)
        {
            dynamic document = _documentApi.GetDocument("Gameshop", "review", f => f.AddCriteria(c => c.Property("Id").IsEquals(target.Item.DocumentId)), 0, 1).FirstOrDefault();

            document.Likes = document.Likes + 1;

            _documentApi.SetDocument("Gameshop", "review", document);
        }
    }
}