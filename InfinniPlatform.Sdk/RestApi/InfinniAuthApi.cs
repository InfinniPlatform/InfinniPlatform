using InfinniPlatform.Sdk.Properties;

namespace InfinniPlatform.Sdk.RestApi
{
    public sealed class InfinniAuthApi : BaseApi
    {
        public InfinniAuthApi(string server, int port) : base(server, port)
        {
        }

        public dynamic GetSessionData(string userName, string claimType)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingToSpecifiedUserClaim(userName, claimType));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetUserClaim, response));
        }

        public void RemoveSessionData(string userName, string claimType)
        {
            var response = RequestExecutor.QueryDelete(RouteBuilder.BuildRestRoutingToSpecifiedUserClaim(userName, claimType));

            ProcessAsObjectResult(response, string.Format(Resources.UnableToRemoveUserClaim, response));
        }

        public void SetSessionData(string userName, string claimType, string claimValue)
        {
            dynamic body = new { ClaimValue = claimValue };

            var response = RequestExecutor.QueryPut(RouteBuilder.BuildRestRoutingToSpecifiedUserClaim(userName, claimType), body);

            ProcessAsObjectResult(response, string.Format(Resources.UnableToAddUserClaim, response));
        }
    }
}