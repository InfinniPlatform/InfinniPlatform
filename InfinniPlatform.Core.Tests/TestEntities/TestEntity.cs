using InfinniPlatform.Serialization;

namespace InfinniPlatform.TestEntities
{
    public class TestEntity
    {
        public TestEntity()
        {
        }

        public TestEntity(string publicField,
                          string publicReadonlyField,
                          string privateField,
                          string privateReadonlyField,
                          string privateVisibleField,
                          string privateVisibleReadonlyField,
                          string publicProperty,
                          string publicPropertyWithPrivateSetter,
                          string publicPropertyWithoutSetter,
                          string privateProperty,
                          string publicPropertyWithePrivateVisiblSetter,
                          string privateVisibleProperty)
        {
            PublicField = publicField;
            PublicReadonlyField = publicReadonlyField;

            PrivateField = privateField;
            PrivateReadonlyField = privateReadonlyField;

            PrivateVisibleField = privateVisibleField;
            PrivateVisibleReadonlyField = privateVisibleReadonlyField;

            PublicProperty = publicProperty;
            PublicPropertyWithPrivateSetter = publicPropertyWithPrivateSetter;
            PublicPropertyWithoutSetter = publicPropertyWithoutSetter;
            PrivateProperty = privateProperty;

            PublicPropertyWithePrivateVisiblSetter = publicPropertyWithePrivateVisiblSetter;
            PrivateVisibleProperty = privateVisibleProperty;
        }


        public string PublicField;
        public readonly string PublicReadonlyField;

        private string PrivateField;
        private readonly string PrivateReadonlyField;

        [SerializerVisible]
        private string PrivateVisibleField;
        [SerializerVisible]
        private readonly string PrivateVisibleReadonlyField;


        public string PublicProperty { get; set; }
        public string PublicPropertyWithPrivateSetter { get; private set; }
        public string PublicPropertyWithoutSetter { get; }
        private string PrivateProperty { get; set; }

        [SerializerVisible]
        public string PublicPropertyWithePrivateVisiblSetter { get; private set; }
        [SerializerVisible]
        private string PrivateVisibleProperty { get; set; }


        public string GetPublicField() { return PublicField; }
        public string GetPublicReadonlyField() { return PublicReadonlyField; }

        public string GetPrivateField() { return PrivateField; }
        public string GetPrivateReadonlyField() { return PrivateReadonlyField; }

        public string GetPrivateVisibleField() { return PrivateVisibleField; }
        public string GetPrivateVisibleReadonlyField() { return PrivateVisibleReadonlyField; }

        public string GetPublicProperty() { return PublicProperty; }
        public string GetPublicPropertyWithPrivateSetter() { return PublicPropertyWithPrivateSetter; }
        public string GetPublicPropertyWithoutSetter() { return PublicPropertyWithoutSetter; }
        public string GetPrivateProperty() { return PrivateProperty; }

        public string GetPublicPropertyWithePrivateVisiblSetter() { return PublicPropertyWithePrivateVisiblSetter; }
        public string GetPrivateVisibleProperty() { return PrivateVisibleProperty; }
    }
}