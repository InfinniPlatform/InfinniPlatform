using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using InfinniPlatform.Api.Schema;

namespace InfinniPlatform.FastReport.Templates.Data
{
	public static class DataExtensions
	{
		public static Type ToClrType(this SchemaDataType dataType)
		{
			Type result;
			ClrDataTypes.TryGetValue(dataType, out result);

			return result;
		}

		public static DbType ToDbType(this SchemaDataType dataType)
		{
			DbType result;
			DbDataTypes.TryGetValue(dataType, out result);

			return result;
		}


		public static object ConvertTo(this object value, SchemaDataType dataType)
		{
			var result = value;

			if (dataType == SchemaDataType.String)
			{
				result = (value != null) ? value.ToString() : null;
			}
			else if (dataType != SchemaDataType.Object)
			{
				var type = dataType.ToClrType();
				result = (value != null) ? Convert.ChangeType(value, type) : GetDefault(type);
			}

			return result;
		}

		private static object GetDefault(Type type)
		{
			return type.IsValueType ? Activator.CreateInstance(type) : null;
		}


		public static byte[] ConvertToBytes(this string value)
		{
			return (value != null) ? Encoding.UTF8.GetBytes(value) : null;
		}


		private static readonly Dictionary<SchemaDataType, Type> ClrDataTypes
			= new Dictionary<SchemaDataType, Type>
			  {
				  { SchemaDataType.None, typeof(string) },
				  { SchemaDataType.String, typeof(string) },
				  { SchemaDataType.Float, typeof(double) },
				  { SchemaDataType.Integer, typeof(int) },
				  { SchemaDataType.Boolean, typeof(bool) },
				  { SchemaDataType.DateTime, typeof(DateTime) },
				  { SchemaDataType.Object, typeof(object) },
				  { SchemaDataType.Array, typeof(IEnumerable) },
			  };

		private static readonly Dictionary<SchemaDataType, DbType> DbDataTypes
			= new Dictionary<SchemaDataType, DbType>
			  {
				  { SchemaDataType.None, DbType.String },
				  { SchemaDataType.String, DbType.String },
				  { SchemaDataType.Float, DbType.Double },
				  { SchemaDataType.Integer, DbType.Int32 },
				  { SchemaDataType.Boolean, DbType.Boolean },
				  { SchemaDataType.DateTime, DbType.DateTime },
				  { SchemaDataType.Object, DbType.Object },
				  { SchemaDataType.Array, DbType.Object },
			  };
	}
}