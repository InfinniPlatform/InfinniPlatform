using System;
using System.Linq;

namespace InfinniPlatform.Core.Metadata
{
    public class MetadataUniqueName : IEquatable<MetadataUniqueName>
    {
        private const char NamespaceSeparator = '.';


        public MetadataUniqueName(string ns, string name)
        {
            Namespace = ns;
            Name = name;
        }

        public MetadataUniqueName(string fullQuelifiedName)
        {
            Namespace = fullQuelifiedName.Remove(fullQuelifiedName.LastIndexOf(NamespaceSeparator));
            Name = fullQuelifiedName.Split(NamespaceSeparator).Last();
        }


        public string Namespace { get; }

        public string Name { get; }


        public bool Equals(MetadataUniqueName other)
        {
            return string.Equals(Namespace, other.Namespace, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return ToString().ToLower().GetHashCode();
        }

        public override string ToString()
        {
            return (string.IsNullOrEmpty(Namespace)) ? Name : $"{Namespace}{NamespaceSeparator}{Name}";
        }
    }
}