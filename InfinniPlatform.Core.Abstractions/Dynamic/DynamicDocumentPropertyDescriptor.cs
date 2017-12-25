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
        /// Initializes new instance of <see cref="DynamicDocumentPropertyDescriptor"/>.
        /// </summary>
        /// <param name="name">Property name.</param>
        public DynamicDocumentPropertyDescriptor(string name)
            : base(name, null)
        {
        }

        /// <inheritdoc />
        public override bool CanResetValue(object component)
        {
            return true;
        }

        /// <inheritdoc />
        public override void ResetValue(object component)
        {
            CastInstance(component)[Name] = null;
        }

        /// <inheritdoc />
        public override object GetValue(object component)
        {
            return CastInstance(component)[Name];
        }

        /// <inheritdoc />
        public override void SetValue(object component, object value)
        {
            CastInstance(component)[Name] = value;
        }

        /// <inheritdoc />
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        /// <inheritdoc />
        public override Type ComponentType => typeof(DynamicDocument);

        /// <inheritdoc />
        public override bool IsReadOnly => false;

        /// <inheritdoc />
        public override Type PropertyType => typeof(object);


        private static DynamicDocument CastInstance(object component)
        {
            return (DynamicDocument)component;
        }
    }
}