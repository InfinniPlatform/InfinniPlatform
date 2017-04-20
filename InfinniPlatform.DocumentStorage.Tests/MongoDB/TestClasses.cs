using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    internal class Doc : Document
    {
        public A Property1 { get; set; }
    }


    internal class A
    {
        public List<Base> List { get; set; }
    }


    internal abstract class Base
    {
        public string BaseProperty { get; set; }
    }


    internal class C1 : Base
    {
        public C1()
        {
            C1Property = nameof(C1);
        }

        public string C1Property { get; set; }
    }


    internal class C2 : Base
    {
        public C2()
        {
            C2Property = nameof(C2);
        }

        public string C2Property { get; set; }
    }
}