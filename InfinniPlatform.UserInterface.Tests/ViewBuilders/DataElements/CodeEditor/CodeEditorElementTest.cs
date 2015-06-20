using System.Collections.Generic;
using InfinniPlatform.Sdk.Application.Dynamic;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements.CodeEditor;
using InfinniPlatform.UserInterface.ViewBuilders.Views;
using NUnit.Framework;

namespace InfinniPlatform.UserInterface.Tests.ViewBuilders.DataElements.CodeEditor
{
    [RequiresSTA]
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class CodeEditorElementTest
    {
        private static readonly View View = new View(null);

        private class FailObjectFormatterStub : IObjectFormatter
        {
            private readonly IEnumerable<CodeEditorError> _errorMessages;
            private readonly object _errorValue;

            public FailObjectFormatterStub(object errorValue, string errorMessage)
            {
                _errorValue = errorValue;
                _errorMessages = new[] {new CodeEditorError(CodeEditorErrorCategory.Error, errorMessage, null, 0, 0)};
            }

            public string ConvertToString(object value, out IEnumerable<CodeEditorError> errors)
            {
                errors = null;

                if (Equals(value, _errorValue))
                {
                    errors = _errorMessages;
                    return null;
                }

                return (value != null) ? value.ToString() : null;
            }

            public object ConvertFromString(string value, out IEnumerable<CodeEditorError> errors)
            {
                errors = null;

                if (Equals(value, _errorValue))
                {
                    errors = _errorMessages;
                    return null;
                }

                return value;
            }
        }


        [Test]
        public void SetTextWithFormatter()
        {
            // Given
            const string text = "{ 'Property': 'Value' }";
            var element = new CodeEditorElement(View);
            element.SetFormatter(new JsonObjectFormatter());

            // When
            element.SetText(text);
            string actualText = element.GetText();

            // Then
            Assert.IsNotNull(actualText);
            Assert.AreEqual("{ 'Property': 'Value' }", actualText);
        }

        [Test]
        public void ShouldSetTextWithoutFormatter()
        {
            // Given
            const string text = "{ 'Property': 'Value' }";
            var element = new CodeEditorElement(View);

            // When
            element.SetText(text);
            string actualText = element.GetText();

            // Then
            Assert.AreEqual(text, actualText);
        }

        [Test]
        public void ShouldSetValueWithFormatter()
        {
            // Given
            dynamic value = new DynamicWrapper();
            value.Property = "Value";
            var element = new CodeEditorElement(View);
            element.SetFormatter(new JsonObjectFormatter());

            // When
            element.SetValue(value);
            string actualText = element.GetText();
            dynamic actualValue = element.GetValue();

            // Then
            Assert.IsNotNull(actualText);
            Assert.IsNotNull(actualValue);
            Assert.AreEqual("{\r\n  \"Property\": \"Value\"\r\n}", actualText);
            Assert.AreEqual("Value", actualValue.Property);
        }

        [Test]
        public void ShouldSetValueWithoutFormatter()
        {
            // Given
            const string value = "{ 'Property': 'Value' }";
            var element = new CodeEditorElement(View);

            // When
            element.SetValue(value);
            string actualText = element.GetText();
            dynamic actualValue = element.GetValue();

            // Then
            Assert.AreEqual(value, actualText);
            Assert.AreEqual(value, actualValue);
        }
    }
}