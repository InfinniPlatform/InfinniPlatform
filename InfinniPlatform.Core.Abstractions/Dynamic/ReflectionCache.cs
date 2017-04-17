using System;
using System.Collections.Generic;
using System.Reflection;

namespace InfinniPlatform.Core.Abstractions.Dynamic
{
    internal static class ReflectionCache
    {
        private static readonly Dictionary<ReflectionCacheKey, List<MemberInfo>> Cache
            = new Dictionary<ReflectionCacheKey, List<MemberInfo>>();

        public static IEnumerable<MemberInfo> FindMembers(Type type, string memberName, BindingFlags bindingFlags)
        {
            var key = new ReflectionCacheKey(type, memberName, bindingFlags);

            List<MemberInfo> value;

            if (!Cache.TryGetValue(key, out value))
            {
                lock (Cache)
                {
                    if (!Cache.TryGetValue(key, out value))
                    {
                        value = new List<MemberInfo>();

                        while (type != null)
                        {
                            var members = type.GetTypeInfo().GetMember(memberName, bindingFlags);

                            if (members.Length > 0)
                            {
                                value.AddRange(members);
                            }

                            type = type.GetTypeInfo().BaseType;
                        }

                        value.Sort(MemberInfoComparer.Instance);

                        Cache.Add(key, value);
                    }
                }
            }

            return value;
        }

        private sealed class ReflectionCacheKey
        {
            public readonly BindingFlags BindingFlags;
            public readonly string MemberName;
            public readonly Type Type;

            public ReflectionCacheKey(Type type, string memberName, BindingFlags bindingFlags)
            {
                Type = type;
                MemberName = memberName;
                BindingFlags = bindingFlags;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    // FNV Hash
                    var hash = (int) 2166136261;
                    hash = hash*16777619 ^ Type.GetHashCode();
                    hash = hash*16777619 ^ MemberName.GetHashCode();
                    hash = hash*16777619 ^ BindingFlags.GetHashCode();
                    return hash;
                }
            }

            public override bool Equals(object obj)
            {
                var other = obj as ReflectionCacheKey;

                return (other != null)
                       && (Type == other.Type)
                       && (MemberName == other.MemberName)
                       && (BindingFlags == other.BindingFlags);
            }
        }

        private sealed class MemberInfoComparer : IComparer<MemberInfo>
        {
            public static readonly MemberInfoComparer Instance = new MemberInfoComparer();

            public int Compare(MemberInfo x, MemberInfo y)
            {
                if (x != y)
                {
                    var type1 = x.DeclaringType;
                    var type2 = y.DeclaringType;

                    var method1 = x as MethodInfo;
                    var method2 = y as MethodInfo;

                    if (method1 != null && method2 != null)
                    {
                        if (type1 == type2)
                        {
                            var parameters1 = GetParameterCount(method1);
                            var parameters2 = GetParameterCount(method2);

                            return parameters1.CompareTo(parameters2);
                        }
                    }

                    return type1.GetTypeInfo().IsAssignableFrom(type2) ? 1 : -1;
                }

                return 0;
            }

            private static int GetParameterCount(MethodInfo method)
            {
                var count = 0;
                var parameters = method.GetParameters();

                foreach (var parameter in parameters)
                {
                    if (parameter.GetCustomAttribute<ParamArrayAttribute>() != null)
                    {
                        count = int.MaxValue;
                        break;
                    }

                    ++count;
                }

                return count;
            }
        }
    }
}