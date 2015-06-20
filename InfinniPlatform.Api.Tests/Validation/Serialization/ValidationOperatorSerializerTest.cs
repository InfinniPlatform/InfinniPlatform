using System;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.Api.Validation.Serialization;
using InfinniPlatform.Api.Validation.ValidationBuilders;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.Validation.Serialization
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class ValidationOperatorSerializerTest
    {
        [Test]
        public void ShouldSerializeValidationOperator()
        {
            // Given
            IValidationOperator validator = ValidationBuilder.ForObject(builder => builder.And(rules => rules
                                                                                                            .IsEqual(
                                                                                                                "TestField",
                                                                                                                "test")
                                                                                                            .IsNotNull(
                                                                                                                "MedicalWorkerIsRequired")
                                                                                                            .IsNotNullOrWhiteSpace
                                                                                                            ("FirstName",
                                                                                                             "MedicalWorkerFirstNameIsRequired")
                                                                                                            .IsNotNullOrWhiteSpace
                                                                                                            ("LastName",
                                                                                                             "MedicalWorkerLastNameIsRequired")
                                                                                                            .IsNotNullOrWhiteSpace
                                                                                                            ("EmployeeCode",
                                                                                                             "MedicalWorkerEmployeeCodeIsRequired")
                                                                                                            .IsNotDefault
                                                                                                            ("EmploymentDate",
                                                                                                             "MedicalWorkerEmploymentDateIsRequired")
                                                                                                            .Collection(
                                                                                                                "Addresses.$",
                                                                                                                coll =>
                                                                                                                coll.And
                                                                                                                    (c
                                                                                                                     =>
                                                                                                                     c
                                                                                                                         .IsNotNullOrEmpty
                                                                                                                         ("AddressesIsRequired")))
                                                                                                            .Collection(
                                                                                                                "Phones.$",
                                                                                                                coll =>
                                                                                                                coll.And
                                                                                                                    (c
                                                                                                                     =>
                                                                                                                     c
                                                                                                                         .All
                                                                                                                         (item
                                                                                                                          =>
                                                                                                                          item
                                                                                                                              .And
                                                                                                                              (prop
                                                                                                                               =>
                                                                                                                               prop
                                                                                                                                   .IsNotNullOrEmpty
                                                                                                                                   ("Value",
                                                                                                                                    "PhoneValueIsRequired")))))));

            // When
            dynamic serializeResult = ValidationOperatorSerializer.Instance.Serialize(validator);
            dynamic deserializeResult = ValidationOperatorSerializer.Instance.Deserialize(serializeResult);

            // Then
            Assert.IsNotNull(serializeResult);
            Assert.IsNotNull(deserializeResult);
            Console.WriteLine(serializeResult);
        }
    }
}