using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Compression;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Json;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Events;
using InfinniPlatform.SystemConfig.RoutingFactory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Update.Tests.Builders
{
    public static class AcceptanceTestBuilder
    {
        public static List<dynamic> CreateEventsFromArchive(this IDataCompressor dataCompressor, string pathToArchive)
        {
            JsonArrayStreamEnumerable jsonEnumerable = new GZipDataCompressor().ReadAsJsonEnumerable(pathToArchive);
            var result = new List<dynamic>();
            foreach (object json in jsonEnumerable)
            {
                result.Add(DynamicWrapperExtensions.ToDynamic(json));
            }
            return result;
        }

        public static void RebuildIntegrationIndexes()
        {
            var elasticFactory = new ElasticFactory(new RoutingFactoryBase());
            IIndexStateProvider indexProvider = elasticFactory.BuildIndexStateProvider();
            indexProvider.CreateIndexType("integration", "organization");
            indexProvider.CreateIndexType("integration", "patient");
        }

        public static void RebuildConfigurationIndexes()
        {
            IIndexStateProvider indexProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();
            indexProvider.RecreateIndex("update", "package");
        }


        public static string GenerateSnils()
        {
            var random = new Random((int) DateTime.Now.Ticks & 0x0000FFFF);
            string input = random.Next(100000000, 200000000).ToString();

            int actualCheckSum = input.Reverse().Select((c, index) => (index + 1)*(int) char.GetNumericValue(c)).Sum()%
                                 101;
            string checkSum = actualCheckSum.ToString("00", CultureInfo.InvariantCulture);

            return input + checkSum;
        }

        public static List<EventDefinition> RenderValidOrganizationEvents()
        {
            var events = new List<string>
                {
                    "{\"Property\":\"Url\",\"Value\":\"http://gkb1.ru/api/DocumentSource\",\"Action\":2}",
                    "{\"Property\":\"Code\",\"Value\":\"001\",\"Action\":2}",
                    "{\"Property\":\"FullName\",\"Value\":\"МБУЗ ОТКЗ Городская клиническая больница №1\",\"Action\":2}",
                    "{\"Property\":\"ShortName\",\"Value\":\"МБУЗ ОТКЗ ГКБ №1\",\"Action\":2}",
                    "{\"Property\":\"Website\",\"Value\":\"http://gkb1.ru\",\"Action\":2}",
                    "{\"Property\":\"Addresses\",\"Action\":8}",
                    "{\"Property\":\"Addresses\",\"Action\":16}",
                    "{\"Property\":\"Addresses.0.Type\",\"Action\":1}",
                    "{\"Property\":\"Addresses.0.Type.Code\",\"Value\":\"002\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.Type.CodeSystem\",\"Value\":\"EHR-RUS-PMD132\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.Type.CodeSystemVersion\",\"Value\":\"1.0\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.Type.DisplayName\",\"Value\":\"Юридический\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.PostalCode\",\"Value\":\"454010\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.Country\",\"Action\":1}",
                    "{\"Property\":\"Addresses.0.Country.Code\",\"Value\":\"643\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.Country.CodeSystem\",\"Value\":\"EHR-RUS-O00015\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.Country.CodeSystemVersion\",\"Value\":\"1.0\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.Country.DisplayName\",\"Value\":\"РОССИЯ\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.Region\",\"Action\":1}",
                    "{\"Property\":\"Addresses.0.Region.Code\",\"Value\":\"7400000000000\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.Region.CodeSystem\",\"Value\":\"EHR-RUS-KLD116\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.Region.CodeSystemVersion\",\"Value\":\"1.0\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.Region.DisplayName\",\"Value\":\"Челябинская обл.\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.City\",\"Action\":1}",
                    "{\"Property\":\"Addresses.0.City.Code\",\"Value\":\"7400000100000\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.City.CodeSystem\",\"Value\":\"EHR-RUS-KLD116\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.City.CodeSystemVersion\",\"Value\":\"1.0\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.City.DisplayName\",\"Value\":\"Челябинск\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.Street\",\"Action\":1}",
                    "{\"Property\":\"Addresses.0.Street.Code\",\"Value\":\"74000001000110100\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.Street.CodeSystem\",\"Value\":\"EHR-RUS-KLD116\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.Street.CodeSystemVersion\",\"Value\":\"1.0\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.Street.DisplayName\",\"Value\":\"ул. Чайковского\",\"Action\":2}",
                    "{\"Property\":\"Addresses.0.House\",\"Value\":1,\"Action\":2}",
                    "{\"Property\":\"Addresses.0.AddressLine\",\"Value\":\"454010, Россия, Челябинская обл., Челябинск, ул. Чайковского, д. 1\",\"Action\":2}",
                    "{\"Property\":\"Phones\",\"Action\":8}",
                    "{\"Property\":\"Phones\",\"Action\":16}",
                    "{\"Property\":\"Phones.0.Name\",\"Value\":\"Стол справок\",\"Action\":2}",
                    "{\"Property\":\"Phones.0.Value\",\"Value\":\"123-45-67\",\"Action\":2}",
                    "{\"Property\":\"Phones\",\"Action\":16}",
                    "{\"Property\":\"Phones.1.Name\",\"Value\":\"Регистратура\",\"Action\":2}",
                    "{\"Property\":\"Phones.1.Value\",\"Value\":\"123-45-68\",\"Action\":2}",
                    "{\"Property\":\"Emails\",\"Action\":8}",
                    "{\"Property\":\"Emails\",\"Action\":16}",
                    "{\"Property\":\"Emails.0.Name\",\"Value\":\"Администрация\",\"Action\":2}",
                    "{\"Property\":\"Emails.0.Value\",\"Value\":\"info@gkb1.ru\",\"Action\":2}",
                    "{\"Property\":\"Requisites\",\"Action\":8}",
                    "{\"Property\":\"Requisites\",\"Action\":16}",
                    "{\"Property\":\"Requisites.0.Type\",\"Action\":1}",
                    "{\"Property\":\"Requisites.0.Type.Code\",\"Value\":\"4BC8B45E-B458-41CD-8AB9-957DAA6D9D1E\",\"Action\":2}",
                    "{\"Property\":\"Requisites.0.Type.CodeSystem\",\"Value\":\"EHR-RUS-D6B7AA65-3546-4A53-90C0-5E9FAAF259D6\",\"Action\":2}",
                    "{\"Property\":\"Requisites.0.Type.CodeSystemVersion\",\"Value\":\"1.0\",\"Action\":2}",
                    "{\"Property\":\"Requisites.0.Type.DisplayName\",\"Value\":\"ИНН\",\"Action\":2}",
                    "{\"Property\":\"Requisites.0.Value\",\"Value\":\"7830002293\",\"Action\":2}",
                    "{\"Property\":\"Requisites\",\"Action\":16}",
                    "{\"Property\":\"Requisites.1.Type\",\"Action\":1}",
                    "{\"Property\":\"Requisites.1.Type.Code\",\"Value\":\"FE84DB21-78E0-4550-B255-D4E31799BC0E\",\"Action\":2}",
                    "{\"Property\":\"Requisites.1.Type.CodeSystem\",\"Value\":\"EHR-RUS-D6B7AA65-3546-4A53-90C0-5E9FAAF259D6\",\"Action\":2}",
                    "{\"Property\":\"Requisites.1.Type.CodeSystemVersion\",\"Value\":\"1.0\",\"Action\":2}",
                    "{\"Property\":\"Requisites.1.Type.DisplayName\",\"Value\":\"КПП\",\"Action\":2}",
                    "{\"Property\":\"Requisites.1.Value\",\"Value\":\"771501001\",\"Action\":2}"
                };

            return events.Select(e => ((JObject) JsonConvert.DeserializeObject(e)).ToObject<EventDefinition>()).ToList();
        }


        public static List<EventDefinition> RenderValidPatientAggregateEvents()
        {
            var events = new List<string>
                {
                    "{\"Property\":\"Confidentiality\",\"Value\":0,\"Action\":2}",
                    "{\"Property\":\"FirstName\",\"Value\":\"Петр\",\"Action\":2}",
                    "{\"Property\":\"MiddleName\",\"Value\":\"Петрович\",\"Action\":2}",
                    "{\"Property\":\"LastName\",\"Value\":\"Петров\",\"Action\":2}",
                    "{\"Property\":\"IsMultipleBirth\",\"Value\":true,\"Action\":2}",
                    "{\"Property\":\"MultipleBirthOrderNumber\",\"Value\":2,\"Action\":2}",
                    "{\"Property\":\"Birthplace\",\"Value\":\"Россия, Челябинская обл., г. Челябинск\",\"Action\":2}",
                    "{\"Property\":\"BirthTime\",\"Value\":\"1990-05-06\",\"Action\":2}",
                    "{\"Property\":\"IsDeceased\",\"Value\":true,\"Action\":2}",
                    "{\"Property\":\"Sex\",\"Action\":1}",
                    "{\"Property\":\"Sex.Code\",\"Value\":\"1\",\"Action\":2}",
                    "{\"Property\":\"Sex.CodeSystem\",\"Value\":\"EHR-RUS-C51007\",\"Action\":2}",
                    "{\"Property\":\"Sex.CodeSystemVersion\",\"Value\":\"1.0\",\"Action\":2}",
                    "{\"Property\":\"Sex.DisplayName\",\"Value\":\"Мужской\",\"Action\":2}",
                    "{\"Property\":\"IdentityCards\",\"Action\":8}",
                    "{\"Property\":\"IdentityCards\",\"Action\":16}",
                    "{\"Property\":\"IdentityCards.0.Type\",\"Action\":1}",
                    "{\"Property\":\"IdentityCards.0.Type.Code\",\"Value\":\"001\",\"Action\":2}",
                    "{\"Property\":\"IdentityCards.0.Type.CodeSystem\",\"Value\":\"EHR-RUS-C51006\",\"Action\":2}",
                    "{\"Property\":\"IdentityCards.0.Type.CodeSystemVersion\",\"Value\":\"1.0\",\"Action\":2}",
                    "{\"Property\":\"IdentityCards.0.Type.DisplayName\",\"Value\":\"Паспорт\",\"Action\":2}",
                    "{\"Property\":\"IdentityCards.0.Series\",\"Value\":\"0987\",\"Action\":2}",
                    "{\"Property\":\"IdentityCards.0.Number\",\"Value\":\"654321\",\"Action\":2}",
                    "{\"Property\":\"IdentityCards.0.Authority\",\"Value\":\"ОВД г. Челябинска\",\"Action\":2}",
                    "{\"Property\":\"IdentityCards.0.IssueDate\",\"Value\":\"2010-06-20\",\"Action\":2}",
                    "{\"Property\":\"Requisites\",\"Action\":8}",
                    "{\"Property\":\"Requisites\",\"Action\":16}",
                    "{\"Property\":\"Requisites.0.Type\",\"Action\":1}",
                    "{\"Property\":\"Requisites.0.Type.Code\",\"Value\":\"A9D55627-AF8C-4ADB-8052-230A1FA56977\",\"Action\":2}",
                    "{\"Property\":\"Requisites.0.Type.CodeSystem\",\"Value\":\"EHR-RUS-Requisites\",\"Action\":2}",
                    "{\"Property\":\"Requisites.0.Type.CodeSystemVersion\",\"Value\":\"1.0\",\"Action\":2}",
                    "{\"Property\":\"Requisites.0.Type.DisplayName\",\"Value\":\"СНИЛС\",\"Action\":2}",
                    "{\"Property\":\"Requisites.0.Value\",\"Value\":\"" + GenerateSnils() + "\",\"Action\":2}",
                };

            return events.Select(e => ((JObject) JsonConvert.DeserializeObject(e)).ToObject<EventDefinition>()).ToList();
        }
    }
}