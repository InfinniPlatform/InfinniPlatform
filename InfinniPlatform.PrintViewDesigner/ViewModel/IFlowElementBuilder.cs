using InfinniPlatform.FlowDocument.Model;

namespace InfinniPlatform.PrintViewDesigner.ViewModel
{
    interface IFlowElementBuilder
    {
        object Build(FlowElementBuilderContext context, PrintElement element);
    }
}