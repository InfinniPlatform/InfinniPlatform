using System;
using System.Collections.Generic;

using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.RestApi.CommonApi
{
    [Obsolete]
    public sealed class SolutionUpdateApi
    {
        public SolutionUpdateApi(RestQueryApi restQueryApi)
        {
            _restQueryApi = restQueryApi;
        }

        public SolutionUpdateApi(string solutionId, string version)
        {
            _solutionId = solutionId;
            _version = version;
        }

        private readonly RestQueryApi _restQueryApi;
        private readonly string _solutionId;
        private readonly string _version;

        public dynamic InstallPackages(IEnumerable<dynamic> packages)
        {
            var readerSolution = ManagerFactorySolution.BuildSolutionReader();

            dynamic solution = readerSolution.GetItem(_solutionId);

            dynamic result = null;

            if (solution == null)
            {
                result = new DynamicWrapper();
                result.IsValid = false;
                result.ValidationMessage = string.Format(Resources.SolutionNotFound, _solutionId, _version);
                return result;
            }

            foreach (var referencedConfiguration in solution.ReferencedConfigurations)
            {
                _restQueryApi.QueryPostNotify(referencedConfiguration.Name);
            }

            result = new DynamicWrapper();
            result.IsValid = true;
            result.ValidationMessage = Resources.SolutionAssembliesSuccessfullyUploaded;

            return result;
        }
    }
}