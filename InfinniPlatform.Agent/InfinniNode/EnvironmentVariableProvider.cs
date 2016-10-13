using System;
using System.Collections;
using System.Collections.Generic;

namespace InfinniPlatform.Agent.InfinniNode
{
    public class EnvironmentVariableProvider : IEnvironmentVariableProvider
    {
        public IDictionary GetAll()
        {
            return Environment.GetEnvironmentVariables();
        }

        public IDictionary Get(string name)
        {
            return new Dictionary<string, string>
                   {
                       { name, Environment.GetEnvironmentVariable(name) }
                   };
        }
    }
}