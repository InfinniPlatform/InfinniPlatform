using InfinniPlatform.FlowDocument.Model;

namespace InfinniPlatform.PrintViewDesigner.ViewModel
{
    abstract class IFlowElementBuilderBase<TElement> : IFlowElementBuilder where TElement: PrintElement
    {
        public object Build(FlowElementBuilderContext context, PrintElement element)
        {
            return Build(context, (TElement)element);
        }

        public abstract object Build(FlowElementBuilderContext context, TElement element);
    }
}