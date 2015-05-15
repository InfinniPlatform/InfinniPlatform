using System.Windows;

using InfinniPlatform.UserInterface.ViewBuilders.Actions;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Images;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.Buttons
{
	/// <summary>
	/// Элемент представления для кнопки.
	/// </summary>
	public sealed class ButtonElement : BaseElement<ButtonControl>
	{
		public ButtonElement(View view)
			: base(view)
		{
			Control.Click += OnClickButton;
		}

		private void OnClickButton(object sender, RoutedEventArgs e)
		{
			this.InvokeScript(OnClick);

			var action = GetAction();

			if (action != null)
			{
				action.Execute();
			}
		}


		// Text

		public override void SetText(string value)
		{
			base.SetText(value);

			Control.Text = value;
		}


		// Image

		private string _image;

		/// <summary>
		/// Возвращает изображение кнопки.
		/// </summary>
		public string GetImage()
		{
			return _image;
		}

		/// <summary>
		/// Устанавливает изображение кнопки.
		/// </summary>
		public void SetImage(string value)
		{
			_image = value;

			Control.Image = ImageRepository.GetImage(value);
		}


		// Action

		private BaseAction _action;

		/// <summary>
		/// Возвращает действие при нажатии на кнопку.
		/// </summary>
		public BaseAction GetAction()
		{
			return _action;
		}

		/// <summary>
		/// Устанавливает действие при нажатии на кнопку.
		/// </summary>
		public void SetAction(BaseAction value)
		{
			_action = value;
		}


		// OnClick

		/// <summary>
		/// Возвращает или устанавливает обработчик события нажатия на кнопку.
		/// </summary>
		public ScriptDelegate OnClick { get; set; }


		// Click

		/// <summary>
		/// Осуществляет программное нажатие на кнопку.
		/// </summary>
		public void Click()
		{
			Control.PerformClick();
		}
	}
}