using InfinniPlatform.Api.Schema;
using InfinniPlatform.Sdk.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.Schema
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class SchemaCreatorBehavior
    {
        [Test]
        public void ShouldCreateSchema()
        {
            dynamic linkPatient = new DynamicWrapper();
            linkPatient.Type = "Object";
            linkPatient.Caption = "patient";
            linkPatient.TypeInfo = new DynamicWrapper();
            linkPatient.TypeInfo.DocumentLink = new DynamicWrapper();
            linkPatient.TypeInfo.DocumentLink.ConfigId = "TestConfig";
            linkPatient.TypeInfo.DocumentLink.DocumentId = "Patient";

            dynamic linkAddress = new DynamicWrapper();
            linkAddress.Type = "Object";
            linkAddress.Caption = "patient address ";
            linkAddress.TypeInfo = new DynamicWrapper();
            linkAddress.TypeInfo.DocumentLink = new DynamicWrapper();
            linkAddress.TypeInfo.DocumentLink.ConfigId = "TestConfig";
            linkAddress.TypeInfo.DocumentLink.DocumentId = "Address";

            dynamic linkPolicies = new DynamicWrapper();
            linkPolicies.Type = "Array";
            linkPolicies.Caption = "Policies";
            linkPolicies.Items = new DynamicWrapper();
            linkPolicies.Items.Type = "Object";
            linkPolicies.Items.TypeInfo = new DynamicWrapper();
            linkPolicies.Items.TypeInfo.DocumentLink = new DynamicWrapper();
            linkPolicies.Items.TypeInfo.DocumentLink.ConfigId = "TestConfig";
            linkPolicies.Items.TypeInfo.DocumentLink.DocumentId = "Policy";

            dynamic linkStreet = new DynamicWrapper();
            linkStreet.Type = "String";
            linkStreet.Caption = "Street";

            dynamic linkAddressDisplayName = new DynamicWrapper();
            linkAddressDisplayName.Type = "String";
            linkAddressDisplayName.Caption = "DisplayName";

            dynamic schemaAddress = new DynamicWrapper();

            schemaAddress.Name = "Address";
            schemaAddress.Caption = "Address";
            schemaAddress.Description = "Address description";
            schemaAddress.Properties = new DynamicWrapper();
            schemaAddress.Properties.Street = new DynamicWrapper();
            schemaAddress.Properties.Street.Type = "String";
            schemaAddress.Properties.Street.Caption = "Street";
            schemaAddress.Properties.House = new DynamicWrapper();
            schemaAddress.Properties.House.Type = "String";
            schemaAddress.Properties.House.Caption = "House";

            dynamic schemaPolicy = new DynamicWrapper();

            schemaPolicy.Name = "Policy";
            schemaPolicy.Caption = "Patient policy";
            schemaPolicy.Description = "Patient policy";
            schemaPolicy.Properties = new DynamicWrapper();
            schemaPolicy.Properties.Series = new DynamicWrapper();
            schemaPolicy.Properties.Series.Type = "String";
            schemaPolicy.Properties.Series.Caption = "Series";
            schemaPolicy.Properties.Number = new DynamicWrapper();
            schemaPolicy.Properties.Number.Type = "String";
            schemaPolicy.Properties.Number.Caption = "Number";

            dynamic schemaPatient = new DynamicWrapper();

            schemaPatient.Name = "Patient";
            schemaPatient.Caption = "Patient";
            schemaPatient.Properties = new DynamicWrapper();
            schemaPatient.Properties.Address = linkAddress;
            schemaPatient.Properties.Policies = linkPolicies;

            var item = new SchemaObject(null, "Patient", "Patient", linkPatient, schemaPatient);

            //список объектов выборки в запросе

            var item1 = new SchemaObject(item, "Address", "Address", linkAddress, schemaAddress);
                //select: Patient.Address

            var item2 = new SchemaObject(item1, "Street", "Street", linkStreet, null);
                //select: //Patient.Address.Street

            var item3 = new SchemaObject(item, "Policies.$", "Policies", linkPolicies, schemaPolicy);

            var item4 = new SchemaObject(item1, "DisplayName", "DisplayName", linkAddressDisplayName, null);
                //Patient.Address.DisplayName


            var schemaCreator = new SchemaCreator();
            dynamic schema = schemaCreator.BuildSchema(new[] {item1, item2, item3, item4});

            dynamic instance = DynamicWrapperExtensions.ToDynamic(schema);
            Assert.IsNotNull(instance);

            Assert.IsNotNull(instance.Properties.Patient);

            Assert.IsNotNull(instance.Properties.Patient.Properties.Address);
            Assert.IsNotNull(instance.Properties.Patient.Properties.Policies);

            Assert.IsNotNull(instance.Properties.Patient.Properties.Address.Properties);
            Assert.IsNotNull(instance.Properties.Patient.Properties.Address.Properties.Street);
            Assert.IsNotNull(instance.Properties.Patient.Properties.Address.Properties.House);

            Assert.IsNotNull(instance.Properties.Patient.Properties.Policies);
            Assert.IsNotNull(instance.Properties.Patient.Properties.Policies.Items);
            Assert.IsNotNull(instance.Properties.Patient.Properties.Policies.Items.Properties);
            Assert.IsNotNull(instance.Properties.Patient.Properties.Policies.Items.Properties.Series);
            Assert.IsNotNull(instance.Properties.Patient.Properties.Policies.Items.Properties.Number);
        }
    }
}