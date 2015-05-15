using System.Linq;
using System.Xml.Linq;

using InfinniPlatform.Reporting.DataProviders;

using NUnit.Framework;

namespace InfinniPlatform.Reporting.Tests.DataProviders
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class XmlDataProviderTest
	{
		[Test]
		public void ShouldEnumerateData()
		{
			// Given

			var rootElement = XElement.Parse(@"
				<items>
					<item/>
					<item/>
					<item/>
				</items>
			");

			var target = new XmlDataProvider(rootElement);

			// When
			var result = target.Cast<object>().ToArray();

			// Then
			Assert.AreEqual(3, result.Length);
		}

		[Test]
		public void ShouldEnumerateDataWhenSourceDataIsNull()
		{
			// Given
			var target = new XmlDataProvider(null);

			// When
			var result = target.Cast<object>().ToArray();

			// Then
			Assert.AreEqual(0, result.Length);
		}

		[Test]
		public void ShouldReturnNullWhenPropertyIsNotExists()
		{
			// Given
			const string rootElement = @"
				<items>
					<item/>
				</items>
			";

			// When
			var result = GetPropertyValue(rootElement, "NotExistsProperty");

			// Then
			Assert.IsNull(result);
		}

		[Test]
		public void ShouldReturnAttributeStyleProperty()
		{
			// Given
			const string rootElement = @"
				<items>
					<item
						property1='123'
						property2='456'
						property3='789'
						/>
				</items>
			";

			// When
			var property1 = GetPropertyValue(rootElement, "property1");
			var property2 = GetPropertyValue(rootElement, "property2");
			var property3 = GetPropertyValue(rootElement, "property3");

			// Then
			Assert.AreEqual("123", property1);
			Assert.AreEqual("456", property2);
			Assert.AreEqual("789", property3);
		}

		[Test]
		public void ShouldReturnAttributeStylePropertyWithNamespace()
		{
			// Given
			const string rootElement = @"
				<items xmlns:ns1='http://ns1'>
					<item
						ns1:property1='123'
						ns1:property2='456'
						property3='789'
						/>
				</items>
			";

			// When
			var property1 = GetPropertyValue(rootElement, "property1");
			var property2 = GetPropertyValue(rootElement, "property2");
			var property3 = GetPropertyValue(rootElement, "property3");

			// Then
			Assert.AreEqual("123", property1);
			Assert.AreEqual("456", property2);
			Assert.AreEqual("789", property3);
		}

		[Test]
		public void ShouldReturnElementStyleProperty()
		{
			// Given
			const string rootElement = @"
				<items>
					<item>
						<property1>123</property1>
						<property2>456</property2>
						<property3>789</property3>
					</item>
				</items>
			";

			// When
			var property1 = GetPropertyValue(rootElement, "property1");
			var property2 = GetPropertyValue(rootElement, "property2");
			var property3 = GetPropertyValue(rootElement, "property3");

			// Then
			Assert.AreEqual("123", property1);
			Assert.AreEqual("456", property2);
			Assert.AreEqual("789", property3);
		}

		[Test]
		public void ShouldReturnElementStylePropertyWhithNamespace()
		{
			// Given
			const string rootElement = @"
				<items xmlns='http://default' xmlns:ns1='http://ns1'>
					<item>
						<ns1:property1>123</ns1:property1>
						<ns1:property2>456</ns1:property2>
						<property3>789</property3>
					</item>
				</items>
			";

			// When
			var property1 = GetPropertyValue(rootElement, "property1");
			var property2 = GetPropertyValue(rootElement, "property2");
			var property3 = GetPropertyValue(rootElement, "property3");

			// Then
			Assert.AreEqual("123", property1);
			Assert.AreEqual("456", property2);
			Assert.AreEqual("789", property3);
		}

		[Test]
		public void ShouldReturnMixedStyleProperty()
		{
			// Given
			const string rootElement = @"
				<items>
					<item property1='123'>
						<property2>456</property2>
						<property3>789</property3>
					</item>
				</items>
			";

			// When
			var property1 = GetPropertyValue(rootElement, "property1");
			var property2 = GetPropertyValue(rootElement, "property2");
			var property3 = GetPropertyValue(rootElement, "property3");

			// Then
			Assert.AreEqual("123", property1);
			Assert.AreEqual("456", property2);
			Assert.AreEqual("789", property3);
		}

		[Test]
		public void ShouldReturnMixedStylePropertyWithNamespace()
		{
			// Given
			const string rootElement = @"
				<items xmlns='http://default' xmlns:ns1='http://ns1'>
					<item ns1:property1='123'>
						<ns1:property2>456</ns1:property2>
						<property3>789</property3>
					</item>
				</items>
			";

			// When
			var property1 = GetPropertyValue(rootElement, "property1");
			var property2 = GetPropertyValue(rootElement, "property2");
			var property3 = GetPropertyValue(rootElement, "property3");

			// Then
			Assert.AreEqual("123", property1);
			Assert.AreEqual("456", property2);
			Assert.AreEqual("789", property3);
		}

		[Test]
		public void ShouldReturnNestedProperty()
		{
			// Given
			const string rootElement = @"
				<items>
					<item>
						<object1>
							<object2>
								<object3 property1='123'>
									<property2>456</property2>
									<property3>789</property3>
								</object3>
							</object2>
						</object1>
					</item>
				</items>
			";

			// When
			var property1 = GetPropertyValue(rootElement, "object1.object2.object3.property1");
			var property2 = GetPropertyValue(rootElement, "object1.object2.object3.property2");
			var property3 = GetPropertyValue(rootElement, "object1.object2.object3.property3");

			// Then
			Assert.AreEqual("123", property1);
			Assert.AreEqual("456", property2);
			Assert.AreEqual("789", property3);
		}

		[Test]
		public void ShouldReturnNestedPropertyWithNamespace()
		{
			// Given
			const string rootElement = @"
				<items xmlns='http://default' xmlns:ns1='http://ns1'>
					<item>
						<object1>
							<object2>
								<object3 ns1:property1='123'>
									<ns1:property2>456</ns1:property2>
									<property3>789</property3>
								</object3>
							</object2>
						</object1>
					</item>
				</items>
			";

			// When
			var property1 = GetPropertyValue(rootElement, "object1.object2.object3.property1");
			var property2 = GetPropertyValue(rootElement, "object1.object2.object3.property2");
			var property3 = GetPropertyValue(rootElement, "object1.object2.object3.property3");

			// Then
			Assert.AreEqual("123", property1);
			Assert.AreEqual("456", property2);
			Assert.AreEqual("789", property3);
		}


		private static object GetPropertyValue(string rootElement, string propertyPath)
		{
			var provider = new XmlDataProvider(XElement.Parse(rootElement));
			var instance = provider.Cast<object>().FirstOrDefault();

			foreach (var propertyName in propertyPath.Split('.'))
			{
				instance = provider.GetPropertyValue(instance, propertyName);

				if (instance == null)
				{
					return null;
				}
			}

			return instance;
		}
	}
}