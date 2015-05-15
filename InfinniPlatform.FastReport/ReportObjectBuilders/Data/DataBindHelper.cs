using System;
using System.Collections.Generic;

using FastReport;
using FastReport.Data;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Data
{
	static class DataBindHelper
	{
		private static readonly List<Func<object, string, bool>> DataBindSetters
			= new List<Func<object, string, bool>>
				  {
					  SetTextObjectDataBind,
					  SetGroupConditionDataBind,
					  SetTotalExpressionDataBind
				  };


		/// <summary>
		/// Установить привязку данных для заданного элемента отчета.
		/// </summary>
		/// <param name="element">Элемент отчета.</param>
		/// <param name="expression">Выражение привязки данных.</param>
		public static void SetDataBind(object element, string expression)
		{
			foreach (var dataBindSetter in DataBindSetters)
			{
				if (dataBindSetter(element, expression))
				{
					break;
				}
			}
		}


		private static bool SetTextObjectDataBind(object element, string expression)
		{
			var textObject = element as TextObjectBase;

			if (textObject != null)
			{
				textObject.Text = expression;
				return true;
			}

			return false;
		}

		private static bool SetGroupConditionDataBind(object element, string expression)
		{
			var textObject = element as GroupHeaderBand;

			if (textObject != null)
			{
				textObject.Condition = expression;
				return true;
			}

			return false;
		}

		private static bool SetTotalExpressionDataBind(object element, string expression)
		{
			var total = element as Total;

			if (total != null)
			{
				total.Expression = expression;
				return true;
			}

			return false;
		}
	}
}