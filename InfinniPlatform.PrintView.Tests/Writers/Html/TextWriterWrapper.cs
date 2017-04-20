using System.IO;

namespace InfinniPlatform.PrintView.Writers.Html
{
    internal sealed class TextWriterWrapper
    {
        private readonly MemoryStream _stream;
        private readonly StreamWriter _writer;

        public TextWriterWrapper()
        {
            _stream = new MemoryStream();
            _writer = new StreamWriter(_stream);
        }

        public TextWriter Writer
        {
            get { return _writer; }
        }

        public string GetText()
        {
            _writer.Flush();
            _stream.Position = 0;

            using (var reader = new StreamReader(_stream))
            {
                return reader.ReadToEnd();
            }
        }

        public override string ToString()
        {
            return GetText();
        }
    }
}