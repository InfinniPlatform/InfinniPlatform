using System;

using InfinniPlatform.Api.Extensions;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.Extensions
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ReflectionExtensionsTest
	{
		// GetDefaultValue()

		[Test]
		public void GetDefaultValueReturnDefaultValueForGivenType()
		{
			Assert.AreEqual(default(int), typeof(int).GetDefaultValue());
			Assert.AreEqual(default(Guid), typeof(Guid).GetDefaultValue());
			Assert.AreEqual(default(DateTime), typeof(DateTime).GetDefaultValue());
			Assert.AreEqual(default(string), typeof(string).GetDefaultValue());
		}


		// GetMemberValue()

		[Test]
		public void GetMemberValueShouldReturnNullWhenNotExistsMember()
		{
			// Given
			var baseInstance = new BaseClass();
			var childInstance = new ChildClass();

			// Then
			Assert.IsNull(baseInstance.GetMemberValue("NotExists"));
			Assert.IsNull(childInstance.GetMemberValue("NotExists"));
		}

		[Test]
		public void GetMemberValueShouldReturnConstValue()
		{
			// Given
			var baseInstance = new BaseClass();
			var childInstance = new ChildClass();

			// Then
			Assert.AreEqual(baseInstance.GetMemberValue("BaseConst"), BaseClass.BaseConst);
			Assert.AreEqual(childInstance.GetMemberValue("BaseConst"), BaseClass.BaseConst);
			Assert.AreEqual(childInstance.GetMemberValue("ChildConst"), ChildClass.ChildConst);
		}

		[Test]
		public void GetMemberValueShouldReturnFieldValue()
		{
			// Given
			var baseInstance = new BaseClass();
			var childInstance = new ChildClass();

			// Then
			Assert.AreEqual(baseInstance.GetMemberValue("BaseField"), baseInstance.BaseField);
			Assert.AreEqual(baseInstance.GetMemberValue("BaseReadonlyField"), baseInstance.BaseReadonlyField);
			Assert.AreEqual(baseInstance.GetMemberValue("BaseStaticField"), BaseClass.BaseStaticField);
			Assert.AreEqual(baseInstance.GetMemberValue("BaseStaticReadonlyField"), BaseClass.BaseStaticReadonlyField);
			Assert.AreEqual(childInstance.GetMemberValue("BaseField"), childInstance.BaseField);
			Assert.AreEqual(childInstance.GetMemberValue("BaseReadonlyField"), childInstance.BaseReadonlyField);
			Assert.AreEqual(childInstance.GetMemberValue("BaseStaticField"), BaseClass.BaseStaticField);
			Assert.AreEqual(childInstance.GetMemberValue("BaseStaticReadonlyField"), BaseClass.BaseStaticReadonlyField);
			Assert.AreEqual(childInstance.GetMemberValue("ChildField"), childInstance.ChildField);
			Assert.AreEqual(childInstance.GetMemberValue("ChildReadonlyField"), childInstance.ChildReadonlyField);
			Assert.AreEqual(childInstance.GetMemberValue("ChildStaticField"), ChildClass.ChildStaticField);
			Assert.AreEqual(childInstance.GetMemberValue("ChildStaticReadonlyField"), ChildClass.ChildStaticReadonlyField);
		}

		[Test]
		public void GetMemberValueShouldReturnPropertyValue()
		{
			// Given
			var baseInstance = new BaseClass();
			var childInstance = new ChildClass();

			// Then
			Assert.AreEqual(baseInstance.GetMemberValue("BaseProperty"), baseInstance.BaseProperty);
			Assert.AreEqual(baseInstance.GetMemberValue("BaseReadonlyProperty"), baseInstance.BaseReadonlyProperty);
			Assert.AreEqual(baseInstance.GetMemberValue("VirtualProperty"), baseInstance.VirtualProperty);
			Assert.AreEqual(baseInstance.GetMemberValue("VirtualReadonlyProperty"), baseInstance.VirtualReadonlyProperty);
			Assert.AreEqual(baseInstance.GetMemberValue("BaseStaticProperty"), BaseClass.BaseStaticProperty);
			Assert.AreEqual(baseInstance.GetMemberValue("BaseStaticReadonlyProperty"), BaseClass.BaseStaticReadonlyProperty);
			Assert.AreEqual(childInstance.GetMemberValue("BaseProperty"), childInstance.BaseProperty);
			Assert.AreEqual(childInstance.GetMemberValue("BaseReadonlyProperty"), childInstance.BaseReadonlyProperty);
			Assert.AreEqual(childInstance.GetMemberValue("BaseStaticProperty"), BaseClass.BaseStaticProperty);
			Assert.AreEqual(childInstance.GetMemberValue("BaseStaticReadonlyProperty"), BaseClass.BaseStaticReadonlyProperty);
			Assert.AreEqual(childInstance.GetMemberValue("ChildProperty"), childInstance.ChildProperty);
			Assert.AreEqual(childInstance.GetMemberValue("ChildReadonlyProperty"), childInstance.ChildReadonlyProperty);
			Assert.AreEqual(childInstance.GetMemberValue("ChildStaticProperty"), ChildClass.ChildStaticProperty);
			Assert.AreEqual(childInstance.GetMemberValue("ChildStaticReadonlyProperty"), ChildClass.ChildStaticReadonlyProperty);
			Assert.AreEqual(childInstance.GetMemberValue("VirtualProperty"), childInstance.VirtualProperty);
			Assert.AreEqual(childInstance.GetMemberValue("VirtualReadonlyProperty"), childInstance.VirtualReadonlyProperty);
		}

		[Test]
		public void GetMemberValueShouldReturnEventDelegate()
		{
			// Given
			var baseInstance = new BaseClass();
			baseInstance.BaseEvent += () => "BaseClass.BaseEvent";
			baseInstance.VirtualEvent += () => "BaseClass.VirtualEvent";
			BaseClass.BaseStaticEvent += () => "BaseClass.BaseStaticEvent";
			var childInstance = new ChildClass();
			childInstance.BaseEvent += () => "ChildClass.BaseEvent";
			childInstance.ChildEvent += () => "ChildClass.ChildEvent";
			childInstance.VirtualEvent += () => "ChildClass.VirtualEvent";
			ChildClass.ChildStaticEvent += () => "ChildClass.ChildStaticEvent";

			// Then
			Assert.AreEqual(((Func<object>)baseInstance.GetMemberValue("BaseEvent"))(), baseInstance.InvokeBaseEvent());
			Assert.AreEqual(((Func<object>)baseInstance.GetMemberValue("VirtualEvent"))(), baseInstance.InvokeVirtualEvent());
			Assert.AreEqual(((Func<object>)baseInstance.GetMemberValue("BaseStaticEvent"))(), BaseClass.InvokeBaseStaticEvent());
			Assert.AreEqual(((Func<object>)childInstance.GetMemberValue("BaseEvent"))(), childInstance.InvokeBaseEvent());
			Assert.AreEqual(((Func<object>)childInstance.GetMemberValue("ChildEvent"))(), childInstance.InvokeChildEvent());
			Assert.AreEqual(((Func<object>)childInstance.GetMemberValue("VirtualEvent"))(), childInstance.InvokeVirtualEvent());
			Assert.AreEqual(((Func<object>)childInstance.GetMemberValue("ChildStaticEvent"))(), ChildClass.InvokeChildStaticEvent());
		}

		[Test]
		public void GetMemberValueShouldReturnMethodDelegate()
		{
			// Given
			var baseInstance = new BaseClass();
			var childInstance = new ChildClass();

			// Then
			Assert.AreEqual(((Func<object>)baseInstance.GetMemberValue("BaseMethodWithoutOverload"))(), baseInstance.BaseMethodWithoutOverload());
			Assert.AreEqual(((Func<object>)baseInstance.GetMemberValue("VirtualMethodWithoutOverload"))(), baseInstance.VirtualMethodWithoutOverload());
			Assert.AreEqual(((Func<object>)baseInstance.GetMemberValue("BaseStaticMethodWithoutOverload"))(), BaseClass.BaseStaticMethodWithoutOverload());
			Assert.AreEqual(((Func<object>)childInstance.GetMemberValue("BaseMethodWithoutOverload"))(), childInstance.BaseMethodWithoutOverload());
			Assert.AreEqual(((Func<object>)childInstance.GetMemberValue("VirtualMethodWithoutOverload"))(), childInstance.VirtualMethodWithoutOverload());
			Assert.AreEqual(((Func<object>)childInstance.GetMemberValue("BaseStaticMethodWithoutOverload"))(), BaseClass.BaseStaticMethodWithoutOverload());
			Assert.AreEqual(((Func<object>)childInstance.GetMemberValue("ChildMethodWithoutOverload"))(), childInstance.ChildMethodWithoutOverload());
			Assert.AreEqual(((Func<object>)childInstance.GetMemberValue("VirtualMethodWithoutOverload"))(), childInstance.VirtualMethodWithoutOverload());
			Assert.AreEqual(((Func<object>)childInstance.GetMemberValue("ChildStaticMethodWithoutOverload"))(), ChildClass.ChildStaticMethodWithoutOverload());
		}


		// SetMemberValue()

		[Test]
		public void SetMemberValueCannotChangeNotExistsMember()
		{
			// Given
			var baseInstance = new BaseClass();
			var childInstance = new ChildClass();

			// Then
			Assert.IsFalse(baseInstance.SetMemberValue("NotExists", "NewValue"));
			Assert.IsFalse(childInstance.SetMemberValue("NotExists", "NewValue"));
		}

		[Test]
		public void SetMemberValueCannotChangeConstant()
		{
			// Given
			var baseInstance = new BaseClass();
			var childInstance = new ChildClass();

			// Then
			Assert.IsFalse(baseInstance.SetMemberValue("BaseConst", "NewValue"));
			Assert.IsFalse(childInstance.SetMemberValue("BaseConst", "NewValue"));
			Assert.IsFalse(childInstance.SetMemberValue("ChildConst", "NewValue"));
		}

		[Test]
		public void SetMemberValueCannotChangeReadonlyField()
		{
			// Given
			var baseInstance = new BaseClass();
			var childInstance = new ChildClass();

			// Then
			Assert.IsFalse(baseInstance.SetMemberValue("BaseReadonlyField", "NewValue"));
			Assert.IsFalse(baseInstance.SetMemberValue("BaseStaticReadonlyField", "NewValue"));
			Assert.IsFalse(childInstance.SetMemberValue("BaseReadonlyField", "NewValue"));
			Assert.IsFalse(childInstance.SetMemberValue("BaseStaticReadonlyField", "NewValue"));
			Assert.IsFalse(childInstance.SetMemberValue("ChildReadonlyField", "NewValue"));
			Assert.IsFalse(childInstance.SetMemberValue("ChildStaticReadonlyField", "NewValue"));
		}

		[Test]
		public void SetMemberValueCannotChangeFieldWhenValueIsWrongType()
		{
			// Given
			var baseInstance = new BaseClass();

			// Then
			Assert.IsFalse(baseInstance.SetMemberValue("BaseIntField", "NotInt"));
		}

		[Test]
		public void SetMemberValueCannotChangeReadonlyProperty()
		{
			// Given
			var baseInstance = new BaseClass();
			var childInstance = new ChildClass();

			// Then
			Assert.IsFalse(baseInstance.SetMemberValue("BaseReadonlyProperty", "NewValue"));
			Assert.IsFalse(baseInstance.SetMemberValue("VirtualReadonlyProperty", "NewValue"));
			Assert.IsFalse(baseInstance.SetMemberValue("BaseStaticReadonlyProperty", "NewValue"));
			Assert.IsFalse(childInstance.SetMemberValue("BaseReadonlyProperty", "NewValue"));
			Assert.IsFalse(childInstance.SetMemberValue("BaseStaticReadonlyProperty", "NewValue"));
			Assert.IsFalse(childInstance.SetMemberValue("ChildReadonlyProperty", "NewValue"));
			Assert.IsFalse(childInstance.SetMemberValue("ChildStaticReadonlyProperty", "NewValue"));
			Assert.IsFalse(childInstance.SetMemberValue("VirtualReadonlyProperty", "NewValue"));
		}

		[Test]
		public void SetMemberValueCannotChangePropertyWhenValueIsWrongType()
		{
			// Given
			var baseInstance = new BaseClass();

			// Then
			Assert.IsFalse(baseInstance.SetMemberValue("BaseIntProperty", "NotInt"));
		}

		[Test]
		public void SetMemberValueCannotChangeEventWhenValueIsWrongType()
		{
			// Given
			var baseInstance = new BaseClass();

			// Then
			Assert.IsFalse(baseInstance.SetMemberValue("BaseEvent", 123));
		}

		[Test]
		public void SetMemberValueCannotChangeMethod()
		{
			// Given
			var baseInstance = new BaseClass();

			// Then
			Assert.IsFalse(baseInstance.SetMemberValue("BaseMethodWithoutOverload", new Func<object>(() => null)));
		}

		[Test]
		public void SetMemberValueShouldChangeFildValue()
		{
			// Given
			var baseInstance = new BaseClass();
			var childInstance = new ChildClass();

			// Then
			Assert.IsTrue(baseInstance.SetMemberValue("BaseField", "BaseClass.NewBaseField") && Equals(baseInstance.BaseField, "BaseClass.NewBaseField"));
			Assert.IsTrue(baseInstance.SetMemberValue("BaseStaticField", "BaseClass.NewBaseStaticField") && Equals(BaseClass.BaseStaticField, "BaseClass.NewBaseStaticField"));
			Assert.IsTrue(childInstance.SetMemberValue("BaseField", "ChildClass.NewBaseField") && Equals(childInstance.BaseField, "ChildClass.NewBaseField"));
			Assert.IsTrue(childInstance.SetMemberValue("BaseStaticField", "ChildClass.NewBaseStaticField") && Equals(BaseClass.BaseStaticField, "ChildClass.NewBaseStaticField"));
			Assert.IsTrue(childInstance.SetMemberValue("ChildField", "ChildClass.NewChildField") && Equals(childInstance.ChildField, "ChildClass.NewChildField"));
			Assert.IsTrue(childInstance.SetMemberValue("ChildStaticField", "ChildClass.NewChildStaticField") && Equals(ChildClass.ChildStaticField, "ChildClass.NewChildStaticField"));
		}

		[Test]
		public void SetMemberValueShouldChangePropertyValue()
		{
			// Given
			var baseInstance = new BaseClass();
			var childInstance = new ChildClass();

			// Then
			Assert.IsTrue(baseInstance.SetMemberValue("BaseProperty", "BaseClass.NewBaseProperty") && Equals(baseInstance.BaseProperty, "BaseClass.NewBaseProperty"));
			Assert.IsTrue(baseInstance.SetMemberValue("VirtualProperty", "BaseClass.NewVirtualProperty") && Equals(baseInstance.VirtualProperty, "BaseClass.NewVirtualProperty"));
			Assert.IsTrue(baseInstance.SetMemberValue("BaseStaticProperty", "BaseClass.NewBaseStaticProperty") && Equals(BaseClass.BaseStaticProperty, "BaseClass.NewBaseStaticProperty"));
			Assert.IsTrue(childInstance.SetMemberValue("BaseProperty", "ChildClass.NewBaseProperty") && Equals(childInstance.BaseProperty, "ChildClass.NewBaseProperty"));
			Assert.IsTrue(childInstance.SetMemberValue("VirtualProperty", "ChildClass.NewVirtualProperty") && Equals(childInstance.VirtualProperty, "ChildClass.NewVirtualProperty"));
			Assert.IsTrue(childInstance.SetMemberValue("BaseStaticProperty", "ChildClass.NewBaseStaticProperty") && Equals(BaseClass.BaseStaticProperty, "ChildClass.NewBaseStaticProperty"));
			Assert.IsTrue(childInstance.SetMemberValue("ChildProperty", "ChildClass.NewChildProperty") && Equals(childInstance.ChildProperty, "ChildClass.NewChildProperty"));
			Assert.IsTrue(childInstance.SetMemberValue("VirtualProperty", "ChildClass.NewVirtualProperty") && Equals(childInstance.VirtualProperty, "ChildClass.NewVirtualProperty"));
			Assert.IsTrue(childInstance.SetMemberValue("ChildStaticProperty", "ChildClass.NewChildStaticProperty") && Equals(ChildClass.ChildStaticProperty, "ChildClass.NewChildStaticProperty"));
		}

		[Test]
		public void SetMemberValueShouldChangeEventDelegate()
		{
			// Given
			var baseInstance = new BaseClass();
			baseInstance.BaseEvent += () => "BaseClass.BaseEvent";
			baseInstance.VirtualEvent += () => "BaseClass.VirtualEvent";
			BaseClass.BaseStaticEvent += () => "BaseClass.BaseStaticEvent";
			var childInstance = new ChildClass();
			childInstance.BaseEvent += () => "ChildClass.BaseEvent";
			childInstance.ChildEvent += () => "ChildClass.ChildEvent";
			childInstance.VirtualEvent += () => "ChildClass.VirtualEvent";
			ChildClass.ChildStaticEvent += () => "ChildClass.ChildStaticEvent";

			// Then
			Assert.IsTrue(baseInstance.SetMemberValue("BaseEvent", new Func<object>(() => "BaseClass.NewBaseEvent")) && Equals(baseInstance.InvokeBaseEvent(), "BaseClass.NewBaseEvent"));
			Assert.IsTrue(baseInstance.SetMemberValue("VirtualEvent", new Func<object>(() => "BaseClass.NewVirtualEvent")) && Equals(baseInstance.InvokeVirtualEvent(), "BaseClass.NewVirtualEvent"));
			Assert.IsTrue(baseInstance.SetMemberValue("BaseStaticEvent", new Func<object>(() => "BaseClass.NewBaseStaticEvent")) && Equals(BaseClass.InvokeBaseStaticEvent(), "BaseClass.NewBaseStaticEvent"));
			Assert.IsTrue(childInstance.SetMemberValue("BaseEvent", new Func<object>(() => "ChildClass.NewBaseEvent")) && Equals(childInstance.InvokeBaseEvent(), "ChildClass.NewBaseEvent"));
			Assert.IsTrue(childInstance.SetMemberValue("ChildEvent", new Func<object>(() => "ChildClass.NewChildEvent")) && Equals(childInstance.InvokeChildEvent(), "ChildClass.NewChildEvent"));
			Assert.IsTrue(childInstance.SetMemberValue("VirtualEvent", new Func<object>(() => "ChildClass.NewVirtualEvent")) && Equals(childInstance.InvokeVirtualEvent(), "ChildClass.NewVirtualEvent"));
			Assert.IsTrue(childInstance.SetMemberValue("ChildStaticEvent", new Func<object>(() => "ChildClass.NewChildStaticEvent")) && Equals(ChildClass.InvokeChildStaticEvent(), "ChildClass.NewChildStaticEvent"));
		}


		// InvokeMember()

		[Test]
		public void InvokeMemberCannotInvokeNotExistsMember()
		{
			// Given
			var baseInstance = new BaseClass();
			var childInstance = new ChildClass();

			// Then
			object result;
			Assert.IsFalse(baseInstance.InvokeMember("NotExists", new object[] { 1, 2, 3 }, out result));
			Assert.IsFalse(baseInstance.InvokeMember("BaseMethod", new object[] { 1, 2, 3 }, out result));
			Assert.IsFalse(childInstance.InvokeMember("NotExists", new object[] { 1, 2, 3 }, out result));
			Assert.IsFalse(childInstance.InvokeMember("ChildMethod", new object[] { 1, 2, 3 }, out result));
		}

		[Test]
		public void InvokeMemberShouldInvokeDelegateField()
		{
			// Given

			var baseInstance = new BaseClass();
			var childInstance = new ChildClass();

			baseInstance.BaseField = new Func<object>(() => "BaseClass.BaseField");
			BaseClass.BaseStaticField = new Func<object>(() => "BaseClass.BaseStaticField");

			childInstance.BaseField = new Func<object>(() => "ChildClass.BaseField");
			childInstance.ChildField = new Func<object>(() => "ChildClass.ChildField");
			ChildClass.ChildStaticField = new Func<object>(() => "ChildClass.ChildStaticField");

			// Then
			object result;
			Assert.IsTrue(baseInstance.InvokeMember("BaseField", null, out result) && Equals(result, "BaseClass.BaseField"));
			Assert.IsTrue(baseInstance.InvokeMember("BaseStaticField", null, out result) && Equals(result, "BaseClass.BaseStaticField"));
			Assert.IsTrue(childInstance.InvokeMember("BaseField", null, out result) && Equals(result, "ChildClass.BaseField"));
			Assert.IsTrue(childInstance.InvokeMember("ChildField", null, out result) && Equals(result, "ChildClass.ChildField"));
			Assert.IsTrue(childInstance.InvokeMember("ChildStaticField", null, out result) && Equals(result, "ChildClass.ChildStaticField"));
		}

		[Test]
		public void InvokeMemberShouldInvokeDelegateProperty()
		{
			// Given

			var baseInstance = new BaseClass();
			var childInstance = new ChildClass();

			baseInstance.BaseProperty = new Func<object>(() => "BaseClass.BaseProperty");
			baseInstance.VirtualProperty = new Func<object>(() => "BaseClass.VirtualProperty");
			BaseClass.BaseStaticProperty = new Func<object>(() => "BaseClass.BaseStaticProperty");

			childInstance.BaseProperty = new Func<object>(() => "ChildClass.BaseProperty");
			childInstance.VirtualProperty = new Func<object>(() => "ChildClass.VirtualProperty");
			childInstance.ChildProperty = new Func<object>(() => "ChildClass.ChildProperty");
			ChildClass.ChildStaticProperty = new Func<object>(() => "ChildClass.ChildStaticProperty");

			// Then
			object result;
			Assert.IsTrue(baseInstance.InvokeMember("BaseProperty", null, out result) && Equals(result, "BaseClass.BaseProperty"));
			Assert.IsTrue(baseInstance.InvokeMember("VirtualProperty", null, out result) && Equals(result, "BaseClass.VirtualProperty"));
			Assert.IsTrue(baseInstance.InvokeMember("BaseStaticProperty", null, out result) && Equals(result, "BaseClass.BaseStaticProperty"));
			Assert.IsTrue(childInstance.InvokeMember("BaseProperty", null, out result) && Equals(result, "ChildClass.BaseProperty"));
			Assert.IsTrue(childInstance.InvokeMember("VirtualProperty", null, out result) && Equals(result, "ChildClass.VirtualProperty"));
			Assert.IsTrue(childInstance.InvokeMember("ChildProperty", null, out result) && Equals(result, "ChildClass.ChildProperty"));
			Assert.IsTrue(childInstance.InvokeMember("ChildStaticProperty", null, out result) && Equals(result, "ChildClass.ChildStaticProperty"));
		}

		[Test]
		public void InvokeMemberSholdInvokeEvent()
		{
			// Given
			var baseInstance = new BaseClass();
			baseInstance.BaseEvent += () => "BaseClass.BaseEvent";
			baseInstance.VirtualEvent += () => "BaseClass.VirtualEvent";
			BaseClass.BaseStaticEvent += () => "BaseClass.BaseStaticEvent";
			var childInstance = new ChildClass();
			childInstance.BaseEvent += () => "ChildClass.BaseEvent";
			childInstance.ChildEvent += () => "ChildClass.ChildEvent";
			childInstance.VirtualEvent += () => "ChildClass.VirtualEvent";
			ChildClass.ChildStaticEvent += () => "ChildClass.ChildStaticEvent";

			// Then
			object result;
			Assert.IsTrue(baseInstance.InvokeMember("BaseEvent", null, out result) && Equals(result, "BaseClass.BaseEvent"));
			Assert.IsTrue(baseInstance.InvokeMember("VirtualEvent", null, out result) && Equals(result, "BaseClass.VirtualEvent"));
			Assert.IsTrue(baseInstance.InvokeMember("BaseStaticEvent", null, out result) && Equals(result, "BaseClass.BaseStaticEvent"));
			Assert.IsTrue(childInstance.InvokeMember("BaseEvent", null, out result) && Equals(result, "ChildClass.BaseEvent"));
			Assert.IsTrue(childInstance.InvokeMember("ChildEvent", null, out result) && Equals(result, "ChildClass.ChildEvent"));
			Assert.IsTrue(childInstance.InvokeMember("VirtualEvent", null, out result) && Equals(result, "ChildClass.VirtualEvent"));
			Assert.IsTrue(childInstance.InvokeMember("ChildStaticEvent", null, out result) && Equals(result, "ChildClass.ChildStaticEvent"));
		}

		[Test]
		public void InvokeMemberSholdInvokeMethod()
		{
			// Given
			var baseInstance = new BaseClass();
			var childInstance = new ChildClass();

			// Then
			object result;
			Assert.IsTrue(baseInstance.InvokeMember("BaseMethod", new object[] { '1' }, out result) && Equals(result, baseInstance.BaseMethod('1')));
			Assert.IsTrue(baseInstance.InvokeMember("BaseMethod", new object[] { '2', "2" }, out result) && Equals(result, baseInstance.BaseMethod('2', "2")));
			Assert.IsTrue(baseInstance.InvokeMember("BaseMethod", new object[] { '3', "3", 3 }, out result) && Equals(result, baseInstance.BaseMethod('3', "3", 3)));
			Assert.IsTrue(baseInstance.InvokeMember("VirtualMethod", new object[] { '1' }, out result) && Equals(result, baseInstance.VirtualMethod('1')));
			Assert.IsTrue(baseInstance.InvokeMember("VirtualMethod", new object[] { '2', "2" }, out result) && Equals(result, baseInstance.VirtualMethod('2', "2")));
			Assert.IsTrue(baseInstance.InvokeMember("VirtualMethod", new object[] { '3', "3", 3 }, out result) && Equals(result, baseInstance.VirtualMethod('3', "3", 3)));
			Assert.IsTrue(baseInstance.InvokeMember("BaseStaticMethod", new object[] { '1' }, out result) && Equals(result, BaseClass.BaseStaticMethod('1')));
			Assert.IsTrue(baseInstance.InvokeMember("BaseStaticMethod", new object[] { '2', "2" }, out result) && Equals(result, BaseClass.BaseStaticMethod('2', "2")));
			Assert.IsTrue(baseInstance.InvokeMember("BaseStaticMethod", new object[] { '3', "3", 3 }, out result) && Equals(result, BaseClass.BaseStaticMethod('3', "3", 3)));
			Assert.IsTrue(childInstance.InvokeMember("BaseMethod", new object[] { '1' }, out result) && Equals(result, childInstance.BaseMethod('1')));
			Assert.IsTrue(childInstance.InvokeMember("BaseMethod", new object[] { '2', "2" }, out result) && Equals(result, childInstance.BaseMethod('2', "2")));
			Assert.IsTrue(childInstance.InvokeMember("BaseMethod", new object[] { '3', "3", 3 }, out result) && Equals(result, childInstance.BaseMethod('3', "3", 3)));
			Assert.IsTrue(childInstance.InvokeMember("VirtualMethod", new object[] { '1' }, out result) && Equals(result, childInstance.VirtualMethod('1')));
			Assert.IsTrue(childInstance.InvokeMember("VirtualMethod", new object[] { '2', "2" }, out result) && Equals(result, childInstance.VirtualMethod('2', "2")));
			Assert.IsTrue(childInstance.InvokeMember("VirtualMethod", new object[] { '3', "3", 3 }, out result) && Equals(result, childInstance.VirtualMethod('3', "3", 3)));
			Assert.IsTrue(childInstance.InvokeMember("BaseStaticMethod", new object[] { '1' }, out result) && Equals(result, BaseClass.BaseStaticMethod('1')));
			Assert.IsTrue(childInstance.InvokeMember("BaseStaticMethod", new object[] { '2', "2" }, out result) && Equals(result, BaseClass.BaseStaticMethod('2', "2")));
			Assert.IsTrue(childInstance.InvokeMember("BaseStaticMethod", new object[] { '3', "3", 3 }, out result) && Equals(result, BaseClass.BaseStaticMethod('3', "3", 3)));
			Assert.IsTrue(childInstance.InvokeMember("ChildMethod", new object[] { '1' }, out result) && Equals(result, childInstance.ChildMethod('1')));
			Assert.IsTrue(childInstance.InvokeMember("ChildMethod", new object[] { '2', "2" }, out result) && Equals(result, childInstance.ChildMethod('2', "2")));
			Assert.IsTrue(childInstance.InvokeMember("ChildMethod", new object[] { '3', "3", 3 }, out result) && Equals(result, childInstance.ChildMethod('3', "3", 3)));
			Assert.IsTrue(childInstance.InvokeMember("ChildStaticMethod", new object[] { '1' }, out result) && Equals(result, ChildClass.ChildStaticMethod('1')));
			Assert.IsTrue(childInstance.InvokeMember("ChildStaticMethod", new object[] { '2', "2" }, out result) && Equals(result, ChildClass.ChildStaticMethod('2', "2")));
			Assert.IsTrue(childInstance.InvokeMember("ChildStaticMethod", new object[] { '3', "3", 3 }, out result) && Equals(result, ChildClass.ChildStaticMethod('3', "3", 3)));
		}


		// ReSharper disable All

		class BaseClass
		{
			static BaseClass()
			{
				BaseStaticProperty = new object();
			}

			public BaseClass()
			{
				BaseProperty = new object();
				VirtualProperty = new object();
			}

			// Const
			public const string BaseConst = "BaseClass";

			// Fields
			public int BaseIntField;
			public object BaseField = new object();
			public readonly object BaseReadonlyField = new object();
			public static object BaseStaticField = new object();
			public static readonly object BaseStaticReadonlyField = new object();

			// Properties
			public int BaseIntProperty { get; set; }
			public object BaseProperty { get; set; }
			public object BaseReadonlyProperty { get { return "BaseClass.BaseReadonlyProperty"; } }
			public virtual object VirtualProperty { get; set; }
			public virtual object VirtualReadonlyProperty { get { return "BaseClass.VirtualReadonlyProperty"; } }
			public static object BaseStaticProperty { get; set; }
			public static object BaseStaticReadonlyProperty { get { return "BaseClass.BaseStaticReadonlyProperty"; } }

			// Events
			public event Func<object> BaseEvent;
			public object InvokeBaseEvent() { return BaseEvent.Invoke(); }
			public virtual event Func<object> VirtualEvent;
			public virtual object InvokeVirtualEvent() { return VirtualEvent.Invoke(); }
			public static event Func<object> BaseStaticEvent;
			public static object InvokeBaseStaticEvent() { return BaseStaticEvent.Invoke(); }

			// Methods
			public object BaseMethodWithoutOverload() { return "BaseClass.BaseMethodWithoutOverload"; }
			public virtual object VirtualMethodWithoutOverload() { return "BaseClass.VirtualMethodWithoutOverload"; }
			public static object BaseStaticMethodWithoutOverload() { return "BaseClass.BaseStaticMethodWithoutOverload"; }
			public object BaseMethod(object arg1) { return string.Format("BaseClass.BaseMethod1: {0}", arg1); }
			public object BaseMethod(object arg1, string arg2) { return string.Format("BaseClass.BaseMethod2: {0}, {1}", arg1, arg2); }
			public object BaseMethod(object arg1, string arg2, int arg3) { return string.Format("BaseClass.BaseMethod3: {0}, {1}, {2}", arg1, arg2, arg3); }
			public virtual object VirtualMethod(object arg1) { return string.Format("BaseClass.VirtualMethod1: {0}", arg1); }
			public virtual object VirtualMethod(object arg1, string arg2) { return string.Format("BaseClass.VirtualMethod2: {0}, {1}", arg1, arg2); }
			public virtual object VirtualMethod(object arg1, string arg2, int arg3) { return string.Format("BaseClass.VirtualMethod3: {0}, {1}, {2}", arg1, arg2, arg3); }
			public static object BaseStaticMethod(object arg1) { return string.Format("BaseClass.BaseStaticMethod1: {0}", arg1); }
			public static object BaseStaticMethod(object arg1, string arg2) { return string.Format("BaseClass.BaseStaticMethod2: {0}, {1}", arg1, arg2); }
			public static object BaseStaticMethod(object arg1, string arg2, int arg3) { return string.Format("BaseClass.BaseStaticMethod3: {0}, {1}, {2}", arg1, arg2, arg3); }
		}

		sealed class ChildClass : BaseClass
		{
			static ChildClass()
			{
				ChildStaticProperty = new object();
			}

			public ChildClass()
			{
				ChildProperty = new object();
				VirtualProperty = new object();
			}

			// Const
			public const string ChildConst = "ChildClass";

			// Fields
			public object ChildField = new object();
			public readonly object ChildReadonlyField = new object();
			public static object ChildStaticField = new object();
			public static readonly object ChildStaticReadonlyField = new object();

			// Properties
			public object ChildProperty { get; set; }
			public object ChildReadonlyProperty { get { return "ChildClass.ChildReadonlyProperty"; } }
			public override object VirtualProperty { get; set; }
			public override object VirtualReadonlyProperty { get { return "ChildClass.VirtualReadonlyProperty"; } }
			public static object ChildStaticProperty { get; set; }
			public static object ChildStaticReadonlyProperty { get { return "ChildClass.ChildStaticReadonlyProperty"; } }

			// Events
			public event Func<object> ChildEvent;
			public object InvokeChildEvent() { return ChildEvent.Invoke(); }
			public override event Func<object> VirtualEvent;
			public override object InvokeVirtualEvent() { return VirtualEvent.Invoke(); }
			public static event Func<object> ChildStaticEvent;
			public static object InvokeChildStaticEvent() { return ChildStaticEvent.Invoke(); }

			// Methods
			public object ChildMethodWithoutOverload() { return "ChildClass.ChildMethodWithoutOverload"; }
			public override object VirtualMethodWithoutOverload() { return "ChildClass.VirtualMethodWithoutOverload"; }
			public static object ChildStaticMethodWithoutOverload() { return "ChildClass.ChildStaticMethodWithoutOverload"; }
			public object ChildMethod(object arg1) { return string.Format("ChildClass.ChildMethod1: {0}", arg1); }
			public object ChildMethod(object arg1, string arg2) { return string.Format("ChildClass.ChildMethod2: {0}, {1}", arg1, arg2); }
			public object ChildMethod(object arg1, string arg2, int arg3) { return string.Format("ChildClass.ChildMethod3: {0}, {1}, {2}", arg1, arg2, arg3); }
			public override object VirtualMethod(object arg1) { return string.Format("ChildClass.VirtualMethod1: {0}", arg1); }
			public override object VirtualMethod(object arg1, string arg2) { return string.Format("ChildClass.VirtualMethod2: {0}, {1}", arg1, arg2); }
			public override object VirtualMethod(object arg1, string arg2, int arg3) { return string.Format("ChildClass.VirtualMethod3: {0}, {1}, {2}", arg1, arg2, arg3); }
			public static object ChildStaticMethod(object arg1) { return string.Format("ChildClass.ChildStaticMethod1: {0}", arg1); }
			public static object ChildStaticMethod(object arg1, string arg2) { return string.Format("ChildClass.ChildStaticMethod2: {0}, {1}", arg1, arg2); }
			public static object ChildStaticMethod(object arg1, string arg2, int arg3) { return string.Format("ChildClass.ChildStaticMethod3: {0}, {1}, {2}", arg1, arg2, arg3); }
		}

		// ReSharper restore All
	}
}