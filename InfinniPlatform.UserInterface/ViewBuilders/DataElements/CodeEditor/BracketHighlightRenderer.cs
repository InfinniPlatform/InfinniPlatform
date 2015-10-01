using System.Collections.Generic;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.CodeEditor
{
    /// <summary>
    ///     Стратегия выделения парных скобок.
    /// </summary>
    internal sealed class BracketHighlightRenderer : IBackgroundRenderer
    {
        private static readonly Dictionary<char, char> OpeningBrackets
            = new Dictionary<char, char>
            {
                {'{', '}'},
                {'[', ']'},
                {'(', ')'}
            };

        private static readonly Dictionary<char, char> ClosingBrackets
            = new Dictionary<char, char>
            {
                {'}', '{'},
                {']', '['},
                {')', '('}
            };

        private static readonly Brush CurrentBracketBrush
            = new SolidColorBrush(Color.FromArgb(0x40, Colors.Cyan.R, Colors.Cyan.G, Colors.Cyan.B));

        private readonly TextEditor _editor;

        public BracketHighlightRenderer(TextEditor editor)
        {
            _editor = editor;
        }

        public KnownLayer Layer
        {
            get { return KnownLayer.Caret; }
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            var text = _editor.Text;
            var caretOffset = _editor.CaretOffset;

            if (caretOffset >= 0 && caretOffset <= text.Length)
            {
                var openingBracketOffset = -1;
                var closingBracketOffset = -1;

                if (caretOffset < text.Length)
                {
                    var openingBracket = text[caretOffset];
                    char closingBracket;

                    if (OpeningBrackets.TryGetValue(openingBracket, out closingBracket))
                    {
                        openingBracketOffset = caretOffset;
                        closingBracketOffset = FindClosingBracketOffset(text, openingBracketOffset, openingBracket,
                            closingBracket);
                    }
                }

                if (!(openingBracketOffset >= 0 && closingBracketOffset >= 0) && caretOffset > 0)
                {
                    var closingBracket = text[caretOffset - 1];
                    char openingBracket;

                    if (ClosingBrackets.TryGetValue(closingBracket, out openingBracket))
                    {
                        closingBracketOffset = caretOffset - 1;
                        openingBracketOffset = FindOpeningBracketOffset(text, closingBracketOffset, openingBracket,
                            closingBracket);
                    }
                }

                if (openingBracketOffset >= 0 && closingBracketOffset >= 0)
                {
                    var builder = new BackgroundGeometryBuilder {CornerRadius = 1, AlignToMiddleOfPixels = true};
                    builder.AddSegment(textView, new TextSegment {StartOffset = openingBracketOffset, Length = 1});
                    builder.CloseFigure(); // сегменты не связаны друг с другом
                    builder.AddSegment(textView, new TextSegment {StartOffset = closingBracketOffset, Length = 1});

                    var geometry = builder.CreateGeometry();

                    if (geometry != null)
                    {
                        drawingContext.DrawGeometry(CurrentBracketBrush, null, geometry);
                    }
                }
            }
        }

        private static int FindClosingBracketOffset(string text, int openingBracketOffset, char openingBracket,
            char closingBracket)
        {
            var counter = 0;

            for (var i = openingBracketOffset + 1; i < text.Length; ++i)
            {
                var c = text[i];

                if (c == openingBracket)
                {
                    ++counter;
                }
                else if (c == closingBracket)
                {
                    if (counter == 0)
                    {
                        return i;
                    }

                    --counter;
                }
            }

            return -1;
        }

        private static int FindOpeningBracketOffset(string text, int closingBracketOffset, char openingBracket,
            char closingBracket)
        {
            var counter = 0;

            for (var i = closingBracketOffset - 1; i >= 0; --i)
            {
                var c = text[i];

                if (c == closingBracket)
                {
                    ++counter;
                }
                else if (c == openingBracket)
                {
                    if (counter == 0)
                    {
                        return i;
                    }

                    --counter;
                }
            }

            return -1;
        }

        public static void AddRendererFor(TextEditor editor)
        {
            editor.Loaded += (loadedSender, loadedArgs) =>
            {
                editor.TextArea.TextView.BackgroundRenderers.Add(new BracketHighlightRenderer(editor));
                editor.TextArea.Caret.PositionChanged +=
                    (sender, e) => editor.TextArea.TextView.InvalidateLayer(KnownLayer.Background);
            };
        }
    }
}