using InfinniPlatform.Api.Metadata.Validation;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.CodeEditor
{
    internal sealed class CodeEditorElementBuilder : IObjectBuilder
    {
        private static readonly IMetadataSchemaValidatorFactory MetadataSchemaValidatorFactory
            = new MetadataJsonSchemaValidatorFactory();

        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var codeEditor = new CodeEditorElement(parent);
            codeEditor.SetSyntax(BuildSyntax(metadata.Syntax));
            codeEditor.SetFormatter(BuildFormatter(metadata.Formatter));
            codeEditor.ApplyElementMeatadata((object) metadata);

            // Привязка к источнику данных

            IElementDataBinding valueBinding = context.Build(parent, metadata.Value);

            if (valueBinding != null)
            {
                valueBinding.OnPropertyValueChanged += (c, a) => codeEditor.SetValue(a.Value);
                codeEditor.OnValueChanged += (c, a) => valueBinding.SetPropertyValue(a.Value);
            }

            return codeEditor;
        }

        private static string BuildSyntax(string syntax)
        {
            return syntax ?? "JavaScript";
        }

        private static IObjectFormatter BuildFormatter(string formatter)
        {
            if (formatter == "JsonObjectFormatter")
            {
                return new JsonObjectFormatter();
            }

            if (formatter == "TextObjectFormatter")
            {
                return new TextObjectFormatter();
            }

            if (formatter == "ViewJsonObjectFormatter")
            {
                var validator = MetadataSchemaValidatorFactory.CreateViewValidator(true);
                return new JsonObjectFormatter(validator.Validate);
            }

            if (formatter == "PrintViewJsonObjectFormatter")
            {
                var validator = MetadataSchemaValidatorFactory.CreatePrintViewValidator(true);
                return new JsonObjectFormatter(validator.Validate);
            }

            return null;
        }
    }
}