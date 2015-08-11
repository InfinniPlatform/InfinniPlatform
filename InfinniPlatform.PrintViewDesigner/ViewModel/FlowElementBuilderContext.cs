using System;
using System.Collections.Generic;
using System.Windows.Documents;

using InfinniPlatform.FlowDocument.Model;

namespace InfinniPlatform.PrintViewDesigner.ViewModel
{
    internal class FlowElementBuilderContext
    {
        private readonly Dictionary<Type, IFlowElementBuilder> _builders
            = new Dictionary<Type, IFlowElementBuilder>();

        public FlowElementBuilderContext Register<TElement, TBuilder>()
            where TElement : PrintElement
            where TBuilder : IFlowElementBuilderBase<TElement>, new()
        {
            _builders[typeof(TElement)] = new TBuilder();

            return this;
        }

        public object Build(PrintElement element)
        {
            if (element != null)
            {
                IFlowElementBuilder builder;

                if (_builders.TryGetValue(element.GetType(), out builder))
                {
                    return builder.Build(this, element);
                }
            }

            return null;
        }

        public TResult Build<TResult>(PrintElement element) where TResult : TextElement
        {
            return (TResult)Build(element);
        }
    }
}