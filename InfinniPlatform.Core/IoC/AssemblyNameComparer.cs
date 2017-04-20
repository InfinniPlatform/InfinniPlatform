using System;
using System.Collections.Generic;
using System.Reflection;

namespace InfinniPlatform.IoC
{
    /// <summary>
    /// Реализует интерфейсы сравнения экземпляров класса <see cref="AssemblyName"/>.
    /// </summary>
    public class AssemblyNameComparer : IComparer<AssemblyName>, IEqualityComparer<AssemblyName>
    {
        public static readonly AssemblyNameComparer Default = new AssemblyNameComparer();


        public int Compare(AssemblyName x, AssemblyName y)
        {
            if (ReferenceEquals(x, y) || (x == null && y == null))
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            var result = string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);

            if (result == 0)
            {
                result = x.Version.CompareTo(y.Version);
            }

            return result;
        }


        public bool Equals(AssemblyName x, AssemblyName y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(AssemblyName x)
        {
            return (x.Name + ',' + x.Version).ToLower().GetHashCode();
        }
    }
}