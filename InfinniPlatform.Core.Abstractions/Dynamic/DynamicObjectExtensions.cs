using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using InfinniPlatform.Serialization;
using Microsoft.CSharp.RuntimeBinder;

namespace InfinniPlatform.Dynamic
{
    /// <summary>
    /// Extensions for dynamic objects.
    /// </summary>
    public static class DynamicObjectExtensions
    {
        private static readonly TypePropertyNameResolver TypePropertyNameResolver
            = new TypePropertyNameResolver();

        private static readonly IEnumerable<CSharpArgumentInfo> GetPropertyArgumentInfo
            = new List<CSharpArgumentInfo>
              {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
              };

        private static readonly IEnumerable<CSharpArgumentInfo> SetPropertyArgumentInfo
            = new List<CSharpArgumentInfo>
              {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
              };


        /// <summary>
        /// Returns the property value of a specified object.
        /// </summary>
        /// <param name="target">The object whose property value will be returned.</param>
        /// <param name="propertyName">The string containing the name of the property to get.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="target"/> or <paramref name="propertyName"/> is <c>null</c>.</exception>
        /// <returns>The property value of the specified object or <c>null</c> if property does not exist.</returns>
        /// <remarks>
        /// This method tryes to get the property value of a specified object using the dynamic context.
        /// If the first attempt complets with an <see cref="RuntimeBinderException"/>, it means the target
        /// instance does not contain the property with given name. In this case if the target instance is not
        /// dynamic, the second attempt are performed and <paramref name="propertyName"/> is used as an alias
        /// of the property. If there is a property which has the <see cref="SerializerPropertyNameAttribute"/>
        /// with the alias it will be used. If the property is not found <c>null</c> is returned.
        /// </remarks>
        /// <example>
        /// Using with typed instances:
        ///  
        /// <code>
        /// class A
        /// {
        ///     [SerializerPropertyName("p1")]
        ///     public int Property1 { get; set; }
        /// }
        ///  
        /// var target = new A { Property1 = 123 };
        ///  
        /// // Access to the property by name
        /// var value1 = target.TryGetPropertyValue("Property1"); // 123
        ///  
        /// // Access to the property by alias
        /// var value2 = target.TryGetPropertyValue("p1"); // 123
        /// </code>
        ///  
        /// Using with dynamic instances:
        ///  
        /// <code>
        /// var target = new DynamicDocument { { "Property1", 123 } };
        ///  
        /// var value = target.TryGetPropertyValue("Property1"); // 123
        /// </code>
        /// </example>
        public static object TryGetPropertyValue(this object target, string propertyName)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            try
            {
                return GetPropertyValue(target, propertyName);
            }
            catch (RuntimeBinderException)
            {
                // The property does not exist
            }

            // The source object is dynamic
            if (target is IDynamicMetaObjectProvider)
            {
                return null;
            }

            // Resolves the property name by its alias
            var realPropertyName = TypePropertyNameResolver.TryGetPropertyName(target.GetType(), propertyName);

            // If the source does not contain a property with that alias
            if (string.IsNullOrEmpty(realPropertyName))
            {
                return null;
            }

            try
            {
                return GetPropertyValue(target, realPropertyName);
            }
            catch (RuntimeBinderException)
            {
                // The property does not exist
            }

            return null;
        }

        /// <summary>
        /// Sets the property value of a specified object.
        /// </summary>
        /// <param name="target">The object whose property value will be set.</param>
        /// <param name="propertyName">The string containing the name of the property to set.</param>
        /// <param name="propertyValue">The new property value.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="target"/> or <paramref name="propertyName"/> is <c>null</c>.</exception>
        /// <remarks>
        /// This method tryes to set the property value of a specified object using the dynamic context.
        /// If the first attempt complets with an <see cref="RuntimeBinderException"/>, it means the target
        /// instance does not contain the property with given name. In this case if the target instance is not
        /// dynamic, the second attempt are performed and <paramref name="propertyName"/> is used as an alias
        /// of the property. If there is a property which has the <see cref="SerializerPropertyNameAttribute"/>
        /// with the alias it will be used. If the property is not found nothing happens.
        /// </remarks>
        /// <example>
        /// Using with typed instances:
        ///  
        /// <code>
        /// class A
        /// {
        ///     [SerializerPropertyName("p1")]
        ///     public int Property1 { get; set; }
        /// }
        ///  
        /// var target = new A();
        ///  
        /// // Access to the property by name
        /// target.TrySetPropertyValue("Property1", 123); // target.Property1 == 123
        ///  
        /// // Access to the property by alias
        /// target.TrySetPropertyValue("p1", 123); // target.Property1 == 123
        /// </code>
        ///  
        /// Using with dynamic instances:
        ///  
        /// <code>
        /// var target = new DynamicDocument();
        ///  
        /// target.TrySetPropertyValue("Property1", 123); // target.Property1 == 123
        /// </code>
        /// </example>
        public static void TrySetPropertyValue(this object target, string propertyName, object propertyValue)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            try
            {
                SetPropertyValue(target, propertyName, propertyValue);
                return;
            }
            catch (RuntimeBinderException)
            {
                // The property does not exist
            }

            // The source object is dynamic
            if (target is IDynamicMetaObjectProvider)
            {
                return;
            }

            // Resolves the property name by its alias
            var realPropertyName = TypePropertyNameResolver.TryGetPropertyName(target.GetType(), propertyName);

            // If the source does not contain a property with that alias
            if (string.IsNullOrEmpty(realPropertyName))
            {
                return;
            }

            try
            {
                SetPropertyValue(target, realPropertyName, propertyValue);
            }
            catch (RuntimeBinderException)
            {
                // The property does not exist
            }
        }


        /// <summary>
        /// Returns the property value of a specified object.
        /// </summary>
        /// <param name="target">The object whose property value will be returned.</param>
        /// <param name="propertyPath">The string containing the path of the property to get.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="target"/> or <paramref name="propertyPath"/> is <c>null</c>.</exception>
        /// <returns>The property value of the specified object or <c>null</c> if property does not exist.</returns>
        /// <remarks>
        /// The path of the property <paramref name="propertyPath"/> is a reference to the property by using the dot notation.
        ///  
        /// <code>
        /// Property.SubProperty.SubSubProperty
        /// </code>
        ///  
        /// Each term of the path is a property name or collection index (if previous property is a collection).
        ///  
        /// If the property path is not found <c>null</c> is returned.
        /// </remarks>
        /// <example>
        /// Using with typed instances:
        ///  
        /// <code>
        /// class A
        /// {
        ///     [SerializerPropertyName("p1")]
        ///     public B Property1 { get; set; }
        /// }
        ///  
        /// class B
        /// {
        ///     [SerializerPropertyName("p2")]
        ///     public int Property2 { get; set; }
        /// }
        ///  
        /// var target = new A { Property1 = new B { Property2 = 123 } };
        ///  
        /// // Access to the property by name
        /// var value1 = target.TryGetPropertyValueByPath("Property1.Property2"); // 123
        ///  
        /// // Access to the property by alias
        /// var value2 = target.TryGetPropertyValueByPath("p1.p2"); // 123
        /// </code>
        ///  
        /// Using with dynamic instances:
        ///  
        /// <code>
        /// var target = new DynamicDocument { { "Property1", new DynamicDocument { { "Property2", 123 } } } };
        ///  
        /// var value = target.TryGetPropertyValueByPath("Property1.Property2"); // 123
        /// </code>
        ///  
        /// Using with collection properties:
        ///  
        /// <code>
        /// class C
        /// {
        ///     [SerializerPropertyName("p1")]
        ///     public B[] Property1 { get; set; }
        /// }
        ///  
        /// class B
        /// {
        ///     [SerializerPropertyName("p2")]
        ///     public int Property2 { get; set; }
        /// }
        ///  
        /// var target = new C { Property1 = new[] { new B { Property2 = 123 } } };
        ///  
        /// // Access to the property by name
        /// var value1 = target.TryGetPropertyValueByPath("Property1.0.Property2"); // 123
        ///  
        /// // Access to the property by alias
        /// var value2 = target.TryGetPropertyValueByPath("p1.0.p2"); // 123
        /// </code>
        /// </example>
        public static object TryGetPropertyValueByPath(this object target, string propertyPath)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (string.IsNullOrEmpty(propertyPath))
            {
                throw new ArgumentNullException(nameof(propertyPath));
            }

            var propertyNames = propertyPath.Split('.');

            foreach (var propertyName in propertyNames)
            {
                if (target == null)
                {
                    break;
                }

                target = TryParseCollectionIndex(propertyName, out int collectionIndex)
                    ? target.GetItem(collectionIndex)
                    : target.TryGetPropertyValue(propertyName);
            }

            return target;
        }

        /// <summary>
        /// Sets the property value of a specified object.
        /// </summary>
        /// <param name="target">The object whose property value will be set.</param>
        /// <param name="propertyPath">The string containing the path of the property to set.</param>
        /// <param name="propertyValue">The new property value.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="target"/> or <paramref name="propertyPath"/> is <c>null</c>.</exception>
        /// <remarks>
        /// The path of the property <paramref name="propertyPath"/> is a reference to the property by using the dot notation.
        ///  
        /// <code>
        /// Property.SubProperty.SubSubProperty
        /// </code>
        ///  
        /// Each term of the path is a property name or collection index (if previous property is a collection).
        ///  
        /// If the property path is not found nothing happens.
        /// </remarks>
        /// <example>
        /// Using with typed instances:
        ///  
        /// <code>
        /// class A
        /// {
        ///     [SerializerPropertyName("p1")]
        ///     public B Property1 { get; set; }
        /// }
        ///  
        /// class B
        /// {
        ///     [SerializerPropertyName("p2")]
        ///     public int Property2 { get; set; }
        /// }
        ///  
        /// var target = new A { Property1 = new B() };
        ///  
        /// // Access to the property by name
        /// target.TrySetPropertyValueByPath("Property1.Property2", 123); // target.Property1.Property2 == 123
        ///  
        /// // Access to the property by alias
        /// target.TrySetPropertyValueByPath("p1.p2", 123); // target.Property1.Property2 == 123
        /// </code>
        ///  
        /// Using with dynamic instances:
        ///  
        /// <code>
        /// var target = new DynamicDocument { { "Property1", new DynamicDocument() } };
        ///  
        /// target.TrySetPropertyValueByPath("Property1.Property2", 123); // target.Property1.Property2 == 123
        /// </code>
        ///  
        /// Using with collection properties:
        ///  
        /// <code>
        /// class C
        /// {
        ///     [SerializerPropertyName("p1")]
        ///     public B[] Property1 { get; set; }
        /// }
        ///  
        /// class B
        /// {
        ///     [SerializerPropertyName("p2")]
        ///     public int Property2 { get; set; }
        /// }
        ///  
        /// var target = new C { Property1 = new[] { new B() } };
        ///  
        /// // Access to the property by name
        /// target.TrySetPropertyValueByPath("Property1.0.Property2", 123); // target.Property1[0].Property2 == 123
        ///  
        /// // Access to the property by alias
        /// target.TrySetPropertyValueByPath("p1.0.p2", 123); // target.Property1[0].Property2 == 123
        /// </code>
        /// </example>
        public static void TrySetPropertyValueByPath(this object target, string propertyPath, object propertyValue)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (string.IsNullOrEmpty(propertyPath))
            {
                throw new ArgumentNullException(nameof(propertyPath));
            }

            var propertyNames = propertyPath.Split('.');

            var b = target is ICollection;

            for (var i = 0; i < propertyNames.Length - 1; ++i)
            {
                if (target == null)
                {
                    break;
                }

                var propertyName = propertyNames[i];

                target = TryParseCollectionIndex(propertyName, out int collectionIndex)
                    ? target.GetItem(collectionIndex)
                    : target.TryGetPropertyValue(propertyName);
            }

            // TODO Workaround for BlobInfo collections in documents.
            if (target is ICollection)
            {
                TryParseCollectionIndex(propertyNames[propertyNames.Length - 1], out var collectionIndex);

                target.SetItem(collectionIndex, propertyValue);
            }
            else
            {
                target?.TrySetPropertyValue(propertyNames[propertyNames.Length - 1], propertyValue);
            }
        }


        private static object GetPropertyValue(object target, string propertyName)
        {
            var targetContext = GetTargetContext(target);
            var callSiteBinder = Binder.GetMember(CSharpBinderFlags.None, propertyName, targetContext, GetPropertyArgumentInfo);
            var callSite = CallSite<Func<CallSite, object, object>>.Create(callSiteBinder);
            return callSite.Target(callSite, target);
        }

        private static void SetPropertyValue(object target, string propertyName, object propertyValue)
        {
            var targetContext = GetTargetContext(target);
            var callSiteBinder = Binder.SetMember(CSharpBinderFlags.None, propertyName, targetContext, SetPropertyArgumentInfo);
            var callSite = CallSite<Func<CallSite, object, object, object>>.Create(callSiteBinder);
            callSite.Target(callSite, target, propertyValue);
        }

        private static bool TryParseCollectionIndex(string propertyName, out int collectionIndex)
        {
            collectionIndex = -1;

            return char.IsDigit(propertyName[0]) && int.TryParse(propertyName, out collectionIndex);
        }

        private static Type GetTargetContext(object target)
        {
            var context = target.GetType();

            if (context.IsArray)
            {
                context = typeof(object);
            }

            return context;
        }
    }
}