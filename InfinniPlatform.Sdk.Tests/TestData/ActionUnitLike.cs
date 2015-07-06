using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Global;

namespace InfinniPlatform.Sdk.Tests.TestData
{
    public sealed class ActionUnitLike
    {
        public void Action(IApplyContext target)
        {
            ScriptContextApp scriptContext = target.Context.GetComponent<ScriptContextApp>();

            var documentApi = scriptContext.GetDocumentApi();

            dynamic document = documentApi.GetDocumentById("Gameshop","review", target.Item.DocumentId);

            document.Likes = document.Likes + 1;

            documentApi.SetDocument("Gameshop", "review", document.Id, document);
        }
    }
}
