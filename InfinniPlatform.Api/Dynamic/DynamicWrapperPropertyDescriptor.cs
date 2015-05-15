using System;
using System.ComponentModel;

namespace InfinniPlatform.Api.Dynamic
{
	/// <summary>
	/// Описание свойства динамического объекта <see cref="DynamicWrapper"/>.
	/// </summary>
	public sealed class DynamicWrapperPropertyDescriptor : PropertyDescriptor
	{
		public DynamicWrapperPropertyDescriptor(string name)
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
			get { return typeof(DynamicWrapper); }
		}

		public override bool IsReadOnly
		{
			get { return false; }
		}

		public override Type PropertyType
		{
			get { return typeof(object); }
		}


		private static DynamicWrapper CastInstance(object component)
		{
			return (DynamicWrapper)component;
		}
	}
}