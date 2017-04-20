using System;
using System.Collections.Generic;
using System.IO;

namespace InfinniPlatform.PrintView.Writers.Html
{
    internal class HtmlBuilderContext
    {
        private readonly Dictionary<Type, IHtmlBuilder> _builders
            = new Dictionary<Type, IHtmlBuilder>();

        public void Register<TElement>(HtmlBuilderBase<TElement> builder) where TElement : PrintElement
        {
            _builders[typeof(TElement)] = builder;
        }

        public void Build(PrintElement element, TextWriter result)
        {
            if (element != null)
            {
                IHtmlBuilder builder;

                if (_builders.TryGetValue(element.GetType(), out builder))
                {
                    builder.Build(this, element, result);
                }
            }
        }
    }
}