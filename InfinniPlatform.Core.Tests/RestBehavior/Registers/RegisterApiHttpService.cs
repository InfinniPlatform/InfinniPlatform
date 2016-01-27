using System.Threading.Tasks;

using InfinniPlatform.Sdk.Registers;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Core.Tests.RestBehavior.Registers
{
    internal sealed class RegisterApiHttpService : IHttpService
    {
        public RegisterApiHttpService(IRegisterApi registerApi)
        {
            _registerApi = registerApi;
        }

        private readonly IRegisterApi _registerApi;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/System/Registers/";
            builder.Post["/GetEntries"] = GetEntries;
            builder.Post["/GetValuesByDate"] = GetValuesByDate;
            builder.Post["/GetValuesBetweenDates"] = GetValuesBetweenDates;
            builder.Post["/RecalculateTotals"] = RecalculateTotals;
        }

        private Task<object> GetEntries(IHttpRequest request)
        {
            var registerRequest = GetRegisterRequest(request);

            var result = _registerApi.GetEntries(
                registerRequest.Configuration,
                registerRequest.RegisterName,
                registerRequest.Filter,
                registerRequest.PageNumber.Value,
                registerRequest.PageSize.Value);

            return Task.FromResult<object>(result);
        }

        private Task<object> GetValuesByDate(IHttpRequest request)
        {
            var registerRequest = GetRegisterRequest(request);

            var result = _registerApi.GetValuesByDate(
                registerRequest.Configuration,
                registerRequest.RegisterName,
                registerRequest.EndDate.Value,
                registerRequest.Filter,
                registerRequest.DimensionsProperties,
                registerRequest.ValueProperties,
                registerRequest.AggregationTypes);

            return Task.FromResult<object>(result);
        }

        private Task<object> GetValuesBetweenDates(IHttpRequest request)
        {
            var registerRequest = GetRegisterRequest(request);

            var result = _registerApi.GetValuesBetweenDates(
                registerRequest.Configuration,
                registerRequest.RegisterName,
                registerRequest.BeginDate.Value,
                registerRequest.EndDate.Value,
                registerRequest.Filter,
                registerRequest.DimensionsProperties,
                registerRequest.ValueProperties,
                registerRequest.AggregationTypes);

            return Task.FromResult<object>(result);
        }

        private Task<object> RecalculateTotals(IHttpRequest request)
        {
            string configuration = request.Form.Configuration;

            _registerApi.RecalculateTotals(configuration);

            return Task.FromResult<object>(null);
        }

        private static RegisterApiRequest GetRegisterRequest(IHttpRequest request)
        {
            return JsonObjectSerializer.Default.ConvertFromDynamic<RegisterApiRequest>((object)request.Form);
        }
    }
}