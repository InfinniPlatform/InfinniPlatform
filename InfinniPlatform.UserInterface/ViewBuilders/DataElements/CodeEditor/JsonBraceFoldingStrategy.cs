using System.Collections.Generic;

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.CodeEditor
{
	/// <summary>
	/// Стратегия выделения блоков кода JSON для сворачивания и разворачивания.
	/// </summary>
	internal sealed class JsonBraceFoldingStrategy
	{
		public void UpdateFoldings(FoldingManager manager, TextDocument document)
		{
			int firstErrorOffset;
			var newFoldings = CreateNewFoldings(document, out firstErrorOffset);
			manager.UpdateFoldings(newFoldings, firstErrorOffset);
		}

		public IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, out int firstErrorOffset)
		{
			firstErrorOffset = -1;
			return CreateNewFoldings(document);
		}

		public IEnumerable<NewFolding> CreateNewFoldings(ITextSource document)
		{
			var newFoldings = new List<NewFolding>();

			var lastNewLineOffset = 0;
			var curlyBracketOffsets = new Stack<int>();
			var squareBracketOffsets = new Stack<int>();

			for (var i = 0; i < document.TextLength; ++i)
			{
				var c = document.GetCharAt(i);

				if (c == '{')
				{
					curlyBracketOffsets.Push(i);
				}
				else if (c == '[')
				{
					squareBracketOffsets.Push(i);
				}
				else if (c == '}' && curlyBracketOffsets.Count > 0)
				{
					var startOffset = curlyBracketOffsets.Pop();

					if (startOffset < lastNewLineOffset)
					{
						newFoldings.Add(new NewFolding(startOffset, i + 1));
					}
				}
				else if (c == ']' && squareBracketOffsets.Count > 0)
				{
					var startOffset = squareBracketOffsets.Pop();

					if (startOffset < lastNewLineOffset)
					{
						newFoldings.Add(new NewFolding(startOffset, i + 1));
					}
				}
				else if (c == '\n' || c == '\r')
				{
					lastNewLineOffset = i + 1;
				}
			}

			newFoldings.Sort((a, b) => a.StartOffset.CompareTo(b.StartOffset));

			return newFoldings;
		}
	}
}