using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.RestApi.CommonApi
{
    public sealed class SolutionUpdateApi
    {
        private readonly string _solutionId;
        private readonly string _version;

        public SolutionUpdateApi(string solutionId, string version)
        {
            _solutionId = solutionId;
            _version = version;
        }

        public dynamic InstallPackages(IEnumerable<dynamic> packages)
        {
            var readerSolution = ManagerFactorySolution.BuildSolutionReader(_version);

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
                new UpdateApi(referencedConfiguration.Version).InstallPackages(packages.Where(p => p.ConfigurationName == referencedConfiguration.Name && p.Version == referencedConfiguration.Version).ToList());
                RestQueryApi.QueryPostNotify(referencedConfiguration.Version, referencedConfiguration.Name);
            }

            result = new DynamicWrapper();
            result.IsValid = true;
            result.ValidationMessage = Resources.SolutionAssembliesSuccessfullyUploaded;

            return result;
        }
    }
}
