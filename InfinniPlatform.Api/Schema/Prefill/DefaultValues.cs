using System;
using System.Collections.Generic;
using System.IO;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Schema.Prefill
{
    public sealed class DefaultValues
    {
        private DynamicWrapper _settings;
        private readonly string _configId;

        public DefaultValues(string configId)
        {
            _configId = configId;
            ReadSettings();
        }

        private void ReadSettings()
        {
            var pathToSettings = Directory.GetCurrentDirectory() + @"\TestData\TestDatabases\Settings";
            if (!Directory.Exists(pathToSettings))
            {
                _settings = new DynamicWrapper();
            }

            var fileName = string.Format("TestDatabase_{0}.json", _configId);

            var pathToFile = pathToSettings + "\\" + fileName;
            if (File.Exists(pathToFile))
            {
                _settings = (File.ReadAllText(pathToFile)).ToDynamic();
            }
            else
            {
                _settings = new DynamicWrapper();
            }
        }

        public void SetSetting(string settingName, string settingValue)
        {
            ObjectHelper.SetProperty(_settings, settingName, settingValue);
        }

        public string GetSetting(string settingName)
        {
            var value = ObjectHelper.GetProperty(_settings, settingName);

            if (value == null)
            {
                return string.Empty;
            }

            var enumerable = value as IEnumerable<dynamic>;
            if (enumerable != null)
            {
                return enumerable.DynamicEnumerableToString();
            }

            return value.ToString();
        }

        public object GetRandomValue(string settingName, int times)
        {
            var value = GetSetting(settingName);
            if (!string.IsNullOrEmpty(value))
            {
                if (times > 1)
                {
                    return FillArrayResult(value, times);
                }
                return FillResult(value);
            }
            return string.Empty;
        }

        private dynamic FillResult(string value)
        {
            dynamic instance = DynamicWrapperExtensions.ToDynamicList(value);
            var random = new Random();
            var randomIndex = random.Next(0, instance.Count);
            return instance[randomIndex];
        }

        private dynamic FillArrayResult(string value, int times)
        {
            dynamic instance = DynamicWrapperExtensions.ToDynamicList(value);

            var result = new List<dynamic>();
            var random = new Random();

            var usedIndexes = new List<int>();
            for (var i = 0; i < times; i++)
            {
                var generated = false;
                while (!generated)
                {
                    var randomIndex = random.Next(0, instance.Count);
                    if (!usedIndexes.Contains(randomIndex))
                    {
                        usedIndexes.Add(randomIndex);
                        generated = true;
                    }
                }
            }

            foreach (var usedIndex in usedIndexes)
            {
                result.Add(((object) instance[usedIndex]).ToDynamic());
            }

            return result;
        }
    }
}