using System.Collections.Generic;
using DigitalRune.Windows.TextEditor;
using DigitalRune.Windows.TextEditor.Document;
using DigitalRune.Windows.TextEditor.Folding;

namespace InfinniPlatform.DesignControls.ScriptEditor
{
    public sealed class CodeFoldingStrategy : IFoldingStrategy
    {
        public List<Fold> GenerateFolds(IDocument document, string fileName, object parseInformation)
        {
            // This is a simple folding strategy.
            // It searches for matching brackets ('{', '}') and creates folds
            // for each region.

            var folds = new List<Fold>();
            for (var offset = 0; offset < document.TextLength; ++offset)
            {
                var c = document.GetCharAt(offset);
                if (c == '{')
                {
                    var offsetOfClosingBracket = TextHelper.FindClosingBracket(document, offset + 1, '{', '}');
                    if (offsetOfClosingBracket > 0)
                    {
                        var length = offsetOfClosingBracket - offset + 1;
                        folds.Add(new Fold(document, offset, length, "{...}", false));
                    }
                }
            }
            return folds;
        }
    }
}