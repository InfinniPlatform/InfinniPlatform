using System;
using System.ComponentModel;

namespace InfinniPlatform.Dynamic
{
    /// <summary>
    /// Properties description of <see cref="DynamicDocument"/>.
    /// </summary>
    public class DynamicDocumentPropertyDescriptor : PropertyDescriptor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Property name.</param>
        public DynamicDocumentPropertyDescriptor(string name)
            : base(name, null)
        {
        }


        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override void ResetValue(object component)
        {
            CastInstance(component)[Name] = null;
        }

        public override object GetValue(object component)
        {
            return CastInstance(component)[Name];
        }

        public override void SetValue(object component, object value)
        {
            CastInstance(component)[Name] = value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }


        public override Type ComponentType
        {
            get { return typeof(DynamicDocument); }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return typeof(object); }
        }


        private static DynamicDocument CastInstance(object component)
        {
            return (DynamicDocument)component;
        }
    }
}