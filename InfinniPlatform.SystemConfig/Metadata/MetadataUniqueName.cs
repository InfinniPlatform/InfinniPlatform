using System;
using System.Collections.Generic;
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
            return Namespace.Equals(other.Namespace) && Name.Equals(other.Name);
        }

        public override string ToString()
        {
            return $"{Namespace}.{Name}";
        }

        public override int GetHashCode()
        {
            return Namespace.GetHashCode() ^ Name.GetHashCode();
        }
    }
}