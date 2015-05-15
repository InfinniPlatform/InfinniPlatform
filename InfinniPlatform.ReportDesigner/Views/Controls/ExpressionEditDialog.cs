using System;
using System.Reflection;
using System.Windows.Forms;

using FastReport;
using FastReport.Forms;

namespace InfinniPlatform.ReportDesigner.Views.Controls
{
	/// <summary>
	/// Диалог редактирования выражений отчета.
	/// </summary>
	sealed class ExpressionEditDialog : IDisposable
	{
		public ExpressionEditDialog(Report report)
		{
			_textEditorForm = SilentAction(CreateTextEditorForm, report);
		}


		private readonly BaseDialogForm _textEditorForm;


		/// <summary>
		/// Скобки для выражений.
		/// </summary>
		public string Brackets
		{
			get { return SilentAction(GetBrackets, _textEditorForm); }
			set { SilentAction(SetBrackets, _textEditorForm, value); }
		}

		/// <summary>
		/// Текст выражения.
		/// </summary>
		public string ExpressionText
		{
			get { return SilentAction(GetExpressionText, _textEditorForm); }
			set { SilentAction(SetExpressionText, _textEditorForm, value); }
		}

		/// <summary>
		/// Отобразить диалог.
		/// </summary>
		public DialogResult ShowDialog(IWin32Window owner)
		{
			if (_textEditorForm != null)
			{
				return _textEditorForm.ShowDialog(owner);
			}

			return DialogResult.Cancel;
		}

		/// <summary>
		/// Освободить ресурсы.
		/// </summary>
		public void Dispose()
		{
			if (_textEditorForm != null)
			{
				_textEditorForm.Dispose();
			}
		}


		private static Type _textEditorFormType;

		private static Type GetTextEditorFormType()
		{
			if (_textEditorFormType == null)
			{
				_textEditorFormType = typeof(Report).Assembly.GetType("FastReport.Forms.TextEditorForm");
			}

			return _textEditorFormType;
		}


		private static ConstructorInfo _textEditorFormConstructor;

		private static BaseDialogForm CreateTextEditorForm(Report report)
		{
			if (_textEditorFormConstructor == null)
			{
				var textEditorFormType = GetTextEditorFormType();

				if (textEditorFormType != null)
				{
					var textEditorFormConstructors = textEditorFormType.GetConstructors();

					if (textEditorFormConstructors.Length > 0)
					{
						_textEditorFormConstructor = textEditorFormConstructors[0];
					}
				}
			}

			if (_textEditorFormConstructor != null)
			{
				return _textEditorFormConstructor.Invoke(new object[] { report }) as BaseDialogForm;
			}

			return null;
		}


		private static PropertyInfo _bracketsProperty;

		private static PropertyInfo GetBracketsProperty()
		{
			if (_bracketsProperty == null)
			{
				var textEditorFormType = GetTextEditorFormType();

				if (textEditorFormType != null)
				{
					_bracketsProperty = textEditorFormType.GetProperty("Brackets");
				}
			}

			return _bracketsProperty;
		}

		private static string GetBrackets(BaseDialogForm textEditorForm)
		{
			if (textEditorForm != null)
			{
				var bracketsProperty = GetBracketsProperty();

				if (bracketsProperty != null)
				{
					return bracketsProperty.GetValue(textEditorForm) as string;
				}
			}

			return null;
		}

		private static void SetBrackets(BaseDialogForm textEditorForm, string value)
		{
			if (textEditorForm != null)
			{
				var bracketsProperty = GetBracketsProperty();

				if (bracketsProperty != null)
				{
					bracketsProperty.SetValue(textEditorForm, value);
				}
			}
		}


		private static PropertyInfo _expressionTextProperty;

		private static PropertyInfo GetExpressionTextProperty()
		{
			if (_expressionTextProperty == null)
			{
				var textEditorFormType = GetTextEditorFormType();

				if (textEditorFormType != null)
				{
					_expressionTextProperty = textEditorFormType.GetProperty("ExpressionText");
				}
			}

			return _expressionTextProperty;
		}

		private static string GetExpressionText(BaseDialogForm textEditorForm)
		{
			if (textEditorForm != null)
			{
				var expressionTextProperty = GetExpressionTextProperty();

				if (expressionTextProperty != null)
				{
					return expressionTextProperty.GetValue(textEditorForm) as string;
				}
			}

			return null;
		}

		private static void SetExpressionText(BaseDialogForm textEditorForm, string value)
		{
			if (textEditorForm != null)
			{
				var expressionTextProperty = GetExpressionTextProperty();

				if (expressionTextProperty != null)
				{
					expressionTextProperty.SetValue(textEditorForm, value);
				}
			}
		}


		private static TResult SilentAction<T1, TResult>(Func<T1, TResult> action, T1 argument1)
		{
			try
			{
				return action(argument1);
			}
			catch
			{
			}

			return default(TResult);
		}

		private static void SilentAction<T1, T2>(Action<T1, T2> action, T1 argument1, T2 argument2)
		{
			try
			{
				action(argument1, argument2);
			}
			catch
			{
			}
		}
	}
}