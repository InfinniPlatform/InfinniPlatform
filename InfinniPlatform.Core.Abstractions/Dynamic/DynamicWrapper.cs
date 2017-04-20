using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq.Expressions;

namespace InfinniPlatform.Dynamic
{
    /// <summary>
    /// Динамический объект.
    /// </summary>
    /// <remarks>
    /// Обеспечивает поведение, аналогичное поведению объекта в JavaScript. По аналогии можно осуществлять прототипное наследование.
    /// Прототип может быть определен, как наследник <see cref="DynamicWrapper"/> с определенными в нем прототипными членами, которые
    /// можно будет подменить у конкретного экземпляра - наследника прототипа.
    /// </remarks>
    public class DynamicWrapper : IDynamicMetaObjectProvider, IEnumerable, ICustomTypeDescriptor
    {
        public DynamicWrapper()
        {
            _isBaseClass = GetType() == typeof(DynamicWrapper);
        }


        private readonly bool _isBaseClass;
        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();


        /// <summary>
        /// Возвращает или устанавливает значение члена с заданным именем.
        /// </summary>
        /// <param name="memberName">Имя члена.</param>
        /// <returns>Значение члена.</returns>
        public object this[string memberName]
        {
            get { return TryGetMember(memberName); }
            set { TrySetMember(memberName, value); }
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
        /// new DynamicWrapper
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
        /// Возвращает значение члена с заданным именем.
        /// </summary>
        /// <param name="memberName">Имя члена.</param>
        /// <returns>Значение члена.</returns>
        public virtual object TryGetMember(string memberName)
        {
            object memberValue;

            if (!_properties.TryGetValue(memberName, out memberValue) && !_isBaseClass)
            {
                memberValue = this.GetMemberValue(memberName);
            }

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
            if (_isBaseClass || !this.SetMemberValue(memberName, memberValue))
            {
                if (memberValue == null)
                {
                    _properties.Remove(memberName);
                }
                else
                {
                    _properties[memberName] = memberValue;
                }
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
            var success = false;
            object invokeResult = null;

            object memberValue;

            if (_properties.TryGetValue(memberName, out memberValue))
            {
                if (memberValue is Delegate)
                {
                    invokeResult = ((Delegate)memberValue).FastDynamicInvoke(invokeArguments);
                    success = true;
                }
            }
            else
            {
                success = this.InvokeMember(memberName, invokeArguments, out invokeResult);
            }

            if (!success)
            {
                throw new InvalidOperationException(memberName);
            }

            return invokeResult;
        }


        /// <summary>
        /// Возвращает значение свойств объекта в виде словаря.
        /// </summary>
        public IDictionary<string, object> ToDictionary()
        {
            return _properties;
        }


        DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
        {
            return new DynamicWrapperMetaObject(parameter, this);
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }


        /// <summary>
        /// Удаляет динамические свойства объекта.
        /// </summary>
        public void Clear()
        {
            _properties.Clear();
        }

        #region ICustomTypeDescriptor

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
                descriptors.Add(new DynamicWrapperPropertyDescriptor(pair.Key));
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

        #endregion ICustomTypeDescriptor
    }
}