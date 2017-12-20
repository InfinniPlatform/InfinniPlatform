using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq.Expressions;

using InfinniPlatform.Properties;

namespace InfinniPlatform.Dynamic
{
    /// <summary>
    /// Dynamic object.
    /// </summary>
    /// <remarks>
    /// Provides behaviour similar to JavaScript object. Can implement prototype inheritance.
    /// Prototype can be specified as inheritor of <see cref="DynamicDocument"/> with prototype members
    /// suitable to override by instance of derived prototype.
    /// </remarks>
    public class DynamicDocument : IDynamicMetaObjectProvider, IEnumerable, ICustomTypeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DynamicDocument" />.
        /// </summary>
        public DynamicDocument()
        {
            _properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DynamicDocument" />.
        /// </summary>
        /// <param name="properties">Dictionary that represents object properties.</param>
        public DynamicDocument(IDictionary<string, object> properties)
        {
            _properties = properties ?? new Dictionary<string, object>();
        }


        private readonly IDictionary<string, object> _properties;


        /// <summary>
        /// Возвращает или устанавливает значение члена с заданным именем.
        /// </summary>
        /// <param name="memberName">Имя члена.</param>
        /// <returns>Значение члена.</returns>
        public object this[string memberName]
        {
            get => TryGetMember(memberName);
            set => TrySetMember(memberName, value);
        }


        /// <summary>
        /// Возвращает значение свойств объекта в виде словаря.
        /// </summary>
        public IDictionary<string, object> ToDictionary()
        {
            return _properties;
        }

        /// <summary>
        /// Устанавливает значение члена с заданным именем.
        /// </summary>
        /// <param name="memberName">Имя члена.</param>
        /// <param name="memberValue">Значение члена.</param>
        /// <remarks>
        /// Метод добавлен для возможности использования красивых инициализаторов.
        /// <example>
        /// <code>
        /// new DynamicDocument
        /// {
        ///   { "Property1", 1 },
        ///   { "Property2", 2 }
        /// }
        /// </code>
        /// </example>
        /// </remarks>
        public void Add(string memberName, object memberValue)
        {
            this[memberName] = memberValue;
        }

        /// <summary>
        /// Удаляет динамические свойства объекта.
        /// </summary>
        public void Clear()
        {
            _properties.Clear();
        }


        /// <summary>
        /// Возвращает значение члена с заданным именем.
        /// </summary>
        /// <param name="memberName">Имя члена.</param>
        /// <returns>Значение члена.</returns>
        public virtual object TryGetMember(string memberName)
        {
            _properties.TryGetValue(memberName, out object memberValue);

            return memberValue;
        }

        /// <summary>
        /// Устанавливает значение члена с заданным именем.
        /// </summary>
        /// <param name="memberName">Имя члена.</param>
        /// <param name="memberValue">Значение члена.</param>
        /// <returns>Значение члена.</returns>
        public virtual object TrySetMember(string memberName, object memberValue)
        {
            if (memberValue == null)
            {
                _properties.Remove(memberName);
            }
            else
            {
                _properties[memberName] = memberValue;
            }

            return memberValue;
        }

        /// <summary>
        /// Вызывает член с заданным именем.
        /// </summary>
        /// <param name="memberName">Имя члена.</param>
        /// <param name="invokeArguments">Аргументы вызова.</param>
        /// <returns>Результат вызова.</returns>
        public virtual object TryInvokeMember(string memberName, object[] invokeArguments)
        {
            if (!_properties.TryGetValue(memberName, out object memberValue))
            {
                throw new ArgumentException(string.Format(Resources.MemberIsUndefined, memberName), nameof(memberName));
            }

            var memberValueAsDelegate = memberValue as Delegate;

            if (memberValueAsDelegate == null)
            {
                throw new ArgumentException(string.Format(Resources.MemberIsNotDelegate, memberName), nameof(memberName));
            }

            var invokeResult = ReflectionExtensions.FastDynamicInvoke(memberValueAsDelegate, invokeArguments);

            return invokeResult;
        }


        // IDynamicMetaObjectProvider

        DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
        {
            return new DynamicDocumentMetaObject(parameter, this);
        }


        // IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }


        // ICustomTypeDescriptor

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            var descriptors = new PropertyDescriptorCollection(null);

            foreach (var pair in _properties)
            {
                descriptors.Add(new DynamicDocumentPropertyDescriptor(pair.Key));
            }

            return descriptors;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            return ((ICustomTypeDescriptor)this).GetProperties();
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
    }
}