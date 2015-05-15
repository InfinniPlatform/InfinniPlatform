using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using InfinniPlatform.ReportDesigner.Properties;

namespace InfinniPlatform.ReportDesigner.Views
{
	static class CommonExtensions
	{
		public static bool IsValidName(this string target)
		{
			return (target != null) && Regex.IsMatch(target, "^[a-zA-Z]+[a-zA-Z0-9_]*$", RegexOptions.Compiled);
		}


		public static bool IsNullOrEmpty<T>(this IEnumerable<T> target)
		{
			return (target == null) || (target.Any() == false);
		}

		public static bool HasDuplicates<T, TResult>(this IEnumerable<T> target, Func<T, TResult> selector)
		{
			var items = target.Select(selector).ToArray();

			return items.Any(item => items.Count(i => Equals(item, i)) > 1);
		}


		public static void Replace<T>(this ICollection<T> target, T oldItem, T newItem)
		{
			var list = target as IList;

			if (list != null)
			{
				var index = list.IndexOf(oldItem);

				if (index >= 0)
				{
					list[index] = newItem;
				}
				else
				{
					list.Add(newItem);
				}
			}
			else
			{
				target.Remove(oldItem);
				target.Add(newItem);
			}
		}

		public static void MoveUp<T>(this ICollection<T> target, T item)
		{
			var list = target as IList;

			if (list != null)
			{
				var index = list.IndexOf(item);

				if (index > 0)
				{
					var prevIndex = index - 1;
					var prevItem = list[prevIndex];

					list[prevIndex] = item;
					list[index] = prevItem;
				}
			}
		}

		public static void MoveDown<T>(this ICollection<T> target, T item)
		{
			var list = target as IList;

			if (list != null)
			{
				var index = list.IndexOf(item);

				if (index >= 0 && index < list.Count - 1)
				{
					var nextIndex = index + 1;
					var nextItem = list[nextIndex];

					list[nextIndex] = item;
					list[index] = nextItem;
				}
			}
		}


		public static void MoveUp(this TreeNode node)
		{
			if (node.Parent != null && node.Index > 0)
			{
				var parentNodes = node.Parent.Nodes;

				parentNodes.Remove(node);
				parentNodes.Insert(node.Index - 1, node);

				node.TreeView.SelectedNode = node;
			}
		}

		public static void MoveDown(this TreeNode node)
		{
			if (node.Parent != null)
			{
				var parentNodes = node.Parent.Nodes;

				if (node.Index < parentNodes.Count - 1)
				{
					parentNodes.Remove(node);
					parentNodes.Insert(node.Index + 1, node);

					node.TreeView.SelectedNode = node;
				}
			}
		}


		public static void ShowError(this string message, params object[] args)
		{
			MessageBox.Show(string.Format(message, args), Resources.ReportDesignerName, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static void ShowWarning(this string message, params object[] args)
		{
			MessageBox.Show(string.Format(message, args), Resources.ReportDesignerName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		public static bool ShowQuestion(this string message, params object[] args)
		{
			return MessageBox.Show(string.Format(message, args), Resources.ReportDesignerName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
		}

		public static DialogResult ShowQuestionWithCancel(this string message, params object[] args)
		{
			return MessageBox.Show(string.Format(message, args), Resources.ReportDesignerName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
		}


		public static void AsyncAction<TResult>(this Control control, Func<TResult> work, Action<TResult> success = null, Action error = null)
		{
			var worker = new BackgroundWorker();

			worker.DoWork += (s, e) =>
								 {
									 e.Result = work();
								 };

			worker.RunWorkerCompleted += (s, e) =>
											 {
												 if (e.Error == null)
												 {
													 if (success != null)
													 {
														 success((TResult)e.Result);
													 }
												 }
												 else
												 {
													 if (error != null)
													 {
														 error();
													 }

													 e.Error.Message.ShowError();
												 }

												 control.Cursor = Cursors.Default;
												 control.Enabled = true;
											 };

			control.Cursor = Cursors.WaitCursor;
			control.Enabled = false;

			worker.RunWorkerAsync();
		}
	}
}