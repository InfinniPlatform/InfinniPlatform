using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Rendering;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.CodeEditor
{
    /// <summary>
    ///     Стратегия выделения цветом текущей строки.
    /// </summary>
    internal sealed class CurrentLineBackgroundRenderer : IBackgroundRenderer
    {
        private readonly TextEditor _editor;

        public CurrentLineBackgroundRenderer(TextEditor editor)
        {
            _editor = editor;
        }

        public KnownLayer Layer
        {
            get { return KnownLayer.Caret; }
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (_editor.Document != null)
            {
                textView.EnsureVisualLines();

                var currentLine = _editor.Document.GetLineByOffset(_editor.CaretOffset);
                var lineWidth = textView.ActualWidth + _editor.HorizontalOffset;

                foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, currentLine))
                {
                    drawingContext.DrawRectangle(null, CurrentLinePen,
                        new Rect(rect.Location, new Size(lineWidth, rect.Height)));
                }
            }
        }

        /// <summary>
        ///     Добавляет стратегию для редактора.
        /// </summary>
        public static void AddRendererFor(TextEditor editor)
        {
            editor.Loaded += (loadedSender, loadedArgs) =>
            {
                editor.TextArea.TextView.BackgroundRenderers.Add(new CurrentLineBackgroundRenderer(editor));
                editor.TextArea.Caret.PositionChanged +=
                    (sender, e) => editor.TextArea.TextView.InvalidateLayer(KnownLayer.Background);
            };
        }

        private static readonly Brush CurrentLineBrush
            = new SolidColorBrush(Color.FromArgb(0x40, Colors.LightGray.R, Colors.LightGray.G, Colors.LightGray.B));

        private static readonly Pen CurrentLinePen
            = new Pen(CurrentLineBrush, 2);
    }
}