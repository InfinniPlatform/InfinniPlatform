using System;

using InfinniPlatform.Api.Validation.Serialization;

using NUnit.Framework;
using InfinniPlatform.Api.Validation.ValidationBuilders;

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
	        var validator = ValidationBuilder.ForObject(builder => builder.And(rules => rules
	            .IsEqual("TestField", "test")
	            .IsNotNull("MedicalWorkerIsRequired")
	            .IsNotNullOrWhiteSpace("FirstName", "MedicalWorkerFirstNameIsRequired")
	            .IsNotNullOrWhiteSpace("LastName", "MedicalWorkerLastNameIsRequired")
	            .IsNotNullOrWhiteSpace("EmployeeCode", "MedicalWorkerEmployeeCodeIsRequired")
	            .IsNotDefault("EmploymentDate", "MedicalWorkerEmploymentDateIsRequired")
	            .Collection("Addresses.$", coll => coll.And(c => c
	                .IsNotNullOrEmpty("AddressesIsRequired")))
	            .Collection("Phones.$", coll => coll.And(c => c
	                .All(item => item.And(prop => prop
	                    .IsNotNullOrEmpty("Value", "PhoneValueIsRequired")))))));

	        // When
	        var serializeResult = ValidationOperatorSerializer.Instance.Serialize(validator);
	        var deserializeResult = ValidationOperatorSerializer.Instance.Deserialize(serializeResult);

	        // Then
	        Assert.IsNotNull(serializeResult);
	        Assert.IsNotNull(deserializeResult);
	        Console.WriteLine(serializeResult);
	    }
	}
}