using System;

using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.FindAndReplace
{
	sealed class FindAndReplaceElementBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var element = new FindAndReplaceElement(parent);
			element.ApplyElementMeatadata((object)metadata);
			element.SetReplaceMode(Convert.ToBoolean(metadata.ReplaceMode));
			element.SetFindWhat(metadata.FindWhat);
			element.SetReplaceWith(metadata.ReplaceWith);
			element.SetMatchCase(Convert.ToBoolean(metadata.MatchCase));
			element.SetWholeWord(Convert.ToBoolean(metadata.WholeWord));

			if (metadata.OnFindPrevious != null)
			{
				element.OnFindPrevious += parent.GetScript(metadata.OnFindPrevious);
			}

			if (metadata.OnFindNext != null)
			{
				element.OnFindNext += parent.GetScript(metadata.OnFindNext);
			}

			if (metadata.OnReplace != null)
			{
				element.OnReplace += parent.GetScript(metadata.OnReplace);
			}

			if (metadata.OnReplaceAll != null)
			{
				element.OnReplaceAll += parent.GetScript(metadata.OnReplaceAll);
			}

			return element;
		}
	}
}