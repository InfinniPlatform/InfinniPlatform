using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.ReportDesigner.Views.Editors
{
	sealed class EditorFactory
	{
		private static readonly Dictionary<SchemaDataType, Func<string, EditorBase>> FactoryMethods
			= new Dictionary<SchemaDataType, Func<string, EditorBase>>
				  {
					  { SchemaDataType.String, format => new StringEditor(format) },
					  { SchemaDataType.Float, format => new FloatEditor(format) },
					  { SchemaDataType.Integer, format => new IntegerEditor(format) },
					  { SchemaDataType.Boolean, format => new BooleanEditor(format) },
					  { SchemaDataType.DateTime, format => new DateTimeEditor(format) }
				  };


		public EditorBase CreateSimpleEditor(SchemaDataType dataType)
		{
			return FactoryMethods[dataType](GetValueFormat(dataType));
		}

		public EditorBase CreateObjectEditor(SchemaDataType dataType)
		{
			return new ObjectEditor(dataType.ToClrType(), GetValueFormat(dataType));
		}

		public EditorBase CreateArrayEditor(SchemaDataType dataType)
		{
			return new ArrayEditor(dataType.ToClrType(), GetValueFormat(dataType));
		}

		private static string GetValueFormat(SchemaDataType dataType)
		{
			return (dataType == SchemaDataType.DateTime) ? "dd.MM.yyyy" : string.Empty;
		}
	}
}