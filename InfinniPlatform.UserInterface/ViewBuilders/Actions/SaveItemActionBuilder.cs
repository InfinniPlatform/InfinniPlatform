using System;
using System.Windows;

using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Actions
{
	sealed class SaveItemActionBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var action = new BaseAction(parent);
			action.SetAction(() => ExecuteAction(parent, metadata));
			return action;
		}

		private static void ExecuteAction(View parent, dynamic metadata)
		{
			if (parent.Validate())
			{
				parent.SetDialogResult(DialogResult.Accepted);

				if (metadata.CanClose == null || Convert.ToBoolean(metadata.CanClose))
				{
					parent.Close();
				}
			}
			else
			{
				MessageBox.Show(Resources.SaveValidateViewError, parent.GetText(), MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}
	}
}