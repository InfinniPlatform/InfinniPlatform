using System;
using System.Collections.Generic;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Schema.Prefill
{
    public sealed class TestDataBuilder
    {
        private readonly string _configId;
        private readonly DefaultValues _defaultValues;

        private readonly Dictionary<string, Func<dynamic, dynamic>> _fillSettings =
            new Dictionary<string, Func<dynamic, dynamic>>();

        public TestDataBuilder(string configId)
        {
            _configId = configId;
            _defaultValues = new DefaultValues(_configId);
        }

        public dynamic Build(dynamic instance = null)
        {
            instance = instance ?? new DynamicWrapper();
            foreach (var fillSetting in _fillSettings)
            {
                ObjectHelper.SetProperty(instance, fillSetting.Key, fillSetting.Value(instance));
            }
            return instance;
        }

        public TestDataBuilder FillProperty(string propertyName, string settingPropertyName, int count = 1)
        {
            _fillSettings.Add(propertyName, buildInstance => _defaultValues.GetRandomValue(settingPropertyName, count));
            return this;
        }

        public TestDataBuilder FillExpressionProperty(string propertyName, Func<dynamic, dynamic> fillPropertyAction,
            int count = 1)
        {
            _fillSettings.Add(propertyName,
                buildInstance => _defaultValues.GetRandomValue(fillPropertyAction(buildInstance), count));
            return this;
        }

        public TestDataBuilder FillCalculatedProperty(string propertyName, Func<dynamic, dynamic> fillConstantAction)
        {
            _fillSettings.Add(propertyName, buildInstance => fillConstantAction.Invoke(buildInstance));
            return this;
        }
    }
}