using InfinniPlatform.Api.Extensions;
using InfinniPlatform.Api.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq.Expressions;

namespace InfinniPlatform.Api.Dynamic
{
    /// <summary>
    /// Динамический объект.
    /// </summary>
    /// <remarks>
    /// Обеспечивает поведение, аналогичное поведению объекта в JavaScript. По аналогии можно осуществлять прототипное наследование.
    /// Прототип может быть определен, как наследник <see cref="DynamicWrapper"/> с определенными в нем прототипными членами, которые
    /// можно будет подменить у конкретного экземпляра - наследника прототипа. Другой способ создания наследника прототипа - клонирование
    /// с использованием метода <see cref="Clone"/> с дальнейшей подменой нужных членов.
    /// </remarks>
    [JsonConverter(typeof (DynamicWrapperJsonConverter))]
    public class DynamicWrapper : IDynamicMetaObjectProvider, IEnumerable, ICloneable, ICustomTypeDescriptor
    {
        public DynamicWrapper()
        {
            _isBaseClass = GetType() == typeof (DynamicWrapper);
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
                    invokeResult = ((Delegate) memberValue).FastDynamicInvoke(invokeArguments);
                    success = true;
                }
            }
            else
            {
                success = this.InvokeMember(memberName, invokeArguments, out invokeResult);
            }

            if (!success)
            {
                throw new InvalidOperationException(string.Format(Resources.MemberWasNotFoundOrCannotBeInvoked,
                    memberName));
            }

            return invokeResult;
        }

        public override string ToString()
        {
            return JToken.FromObject(_properties).ToString();
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


	    #region ICloneable

        /// <summary>
        /// Осуществляет клонирование объекта.
        /// </summary>
        public virtual object Clone()
        {
            return CloneObject(this, new Dictionary<object, object>());
        }

        private static object CloneObject(object target, Dictionary<object, object> clones)
        {
            object clone = null;

            if (target != null && clones.TryGetValue(target, out clone) == false)
            {
                if (TryCloneAsDynamicWrapper(target, clones, out clone) == false
                    && TryCloneAsEnumerable(target, clones, out clone) == false
                    && TryCloneAsCloneable(target, clones, out clone) == false)
                {
                    clone = target;
                }
            }

            return clone;
        }

        private static bool TryCloneAsDynamicWrapper(object target, Dictionary<object, object> clones, out object clone)
        {
            clone = null;

            if (target is DynamicWrapper)
            {
                var targetClone = new DynamicWrapper();
                clones.Add(target, targetClone);
                clone = targetClone;

                foreach (KeyValuePair<string, object> property in ((DynamicWrapper) target))
                {
                    targetClone[property.Key] = CloneObject(property.Value, clones);
                }

                return true;
            }

            return false;
        }

        private static bool TryCloneAsEnumerable(object target, Dictionary<object, object> clones, out object clone)
        {
            clone = null;

            if (!(target is string) && target is IEnumerable)
            {
                var targetClone = new List<object>();
                clones.Add(target, targetClone);
                clone = targetClone;

                foreach (var item in ((IEnumerable) target))
                {
                    var cloneItem = CloneObject(item, clones);
                    targetClone.Add(cloneItem);
                }

                return true;
            }

            return false;
        }
        
        private static bool TryCloneAsCloneable(object target, Dictionary<object, object> clones, out object clone)
        {
            clone = null;

            if (target is ICloneable)
            {
                var targetClone = ((ICloneable) target).Clone();
                clones.Add(target, targetClone);
                clone = targetClone;

                return true;
            }

            return false;
        }

        #endregion ICloneable

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
            return ((ICustomTypeDescriptor) this).GetProperties();
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion ICustomTypeDescriptor
    }
}