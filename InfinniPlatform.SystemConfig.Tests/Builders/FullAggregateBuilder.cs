using System.Collections.Generic;

namespace InfinniPlatform.SystemConfig.Tests.Builders
{
    public class FullAggregateBuilder
    {
        public object GenerateOrganizationStructure()
        {
            return new
                {
                    Url = "http://gkb1.ru/api/DocumentSource",
                    Code = "001",
                    FullName = "МБУЗ ОТКЗ Городская клиническая больница №1",
                    ShortName = "МБУЗ ОТКЗ ГКБ №1",
                    Website = "http://gkb1.ru",
                    Addresses = new List<object>
                        {
                            new
                                {
                                    Type = new
                                        {
                                            Code = "002",
                                            CodeSystem = "EHR-RUS-PMD132",
                                            CodeSystemVersion = "1.0",
                                            DisplayName = "Юридический"
                                        },
                                    PostalCode = "454010",
                                    Country = new
                                        {
                                            Code = "643",
                                            CodeSystem = "EHR-RUS-O00015",
                                            CodeSystemVersion = "1.0",
                                            DisplayName = "РОССИЯ"
                                        },
                                    Region = new
                                        {
                                            Code = "7400000000000",
                                            CodeSystem = "EHR-RUS-KLD116",
                                            CodeSystemVersion = "1.0",
                                            DisplayName = "Челябинская обл."
                                        },
                                    City = new
                                        {
                                            Code = "7400000100000",
                                            CodeSystem = "EHR-RUS-KLD116",
                                            CodeSystemVersion = "1.0",
                                            DisplayName = "Челябинск"
                                        },
                                    Street = new
                                        {
                                            Code = "74000001000110100",
                                            CodeSystem = "EHR-RUS-KLD116",
                                            CodeSystemVersion = "1.0",
                                            DisplayName = "ул. Чайковского"
                                        },
                                    House = 1,
                                    AddressLine = "454010, Россия, Челябинская обл., Челябинск, ул. Чайковского, д. 1"
                                }
                        },
                    Phones = new List<object>
                        {
                            new
                                {
                                    Name = "Стол справок",
                                    Value = "123-45-67"
                                },
                            new
                                {
                                    Name = "Регистратура",
                                    Value = "123-45-68"
                                }
                        },
                    Emails = new List<object>
                        {
                            new
                                {
                                    Name = "Администрация",
                                    Value = "info@gkb1.ru"
                                }
                        },
                    Requisites = new List<object>
                        {
                            new
                                {
                                    Type = new
                                        {
                                            Code = "4BC8B45E-B458-41CD-8AB9-957DAA6D9D1E",
                                            CodeSystem = "EHR-RUS-D6B7AA65-3546-4A53-90C0-5E9FAAF259D6",
                                            CodeSystemVersion = "1.0",
                                            DisplayName = "ИНН"
                                        },
                                    Value = "7830002293"
                                },
                            new
                                {
                                    Type = new
                                        {
                                            Code = "FE84DB21-78E0-4550-B255-D4E31799BC0E",
                                            CodeSystem = "EHR-RUS-D6B7AA65-3546-4A53-90C0-5E9FAAF259D6",
                                            CodeSystemVersion = "1.0",
                                            DisplayName = "КПП"
                                        },
                                    Value = "771501001"
                                }
                        }
                };
        }
    }
}