using System;
using System.Collections;

namespace InfinniPlatform.Agent.InfinniNode
{
    public class EnvironmentVariableProvider : IEnvironmentVariableProvider
    {
        public IDictionary GetAll()
        {
            return Environment.GetEnvironmentVariables();
        }

        public string Get(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }
    }
}