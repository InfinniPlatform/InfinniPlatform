using DevExpress.Xpf.Bars;
using InfinniPlatform.UserInterface.ViewBuilders.Actions;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Images;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ToolBar
{
    /// <summary>
    ///     Элемент панели инструментов в виде кнопки.
    /// </summary>
    public sealed class ToolBarButtonItem : ToolBarItem<BarButtonItem>
    {
        // Action

        private BaseAction _action;
        // Image

        private string _image;

        public ToolBarButtonItem(View view)
            : base(view)
        {
            Control.ItemClick += OnClickToolBarButton;
        }

        // OnClick

        /// <summary>
        ///     Возвращает или устанавливает обработчик события нажатия на кнопку.
        /// </summary>
        public ScriptDelegate OnClick { get; set; }

        private void OnClickToolBarButton(object sender, ItemClickEventArgs e)
        {
            this.InvokeScript(OnClick);

            var action = GetAction();

            if (action != null)
            {
                action.Execute();
            }
        }

        /// <summary>
        ///     Возвращает изображение кнопки.
        /// </summary>
        public string GetImage()
        {
            return _image;
        }

        /// <summary>
        ///     Устанавливает изображение кнопки.
        /// </summary>
        public void SetImage(string value)
        {
            _image = value;

            Control.Glyph = ImageRepository.GetImage(value);
        }

        /// <summary>
        ///     Возвращает действие при нажатии на кнопку.
        /// </summary>
        public BaseAction GetAction()
        {
            return _action;
        }

        /// <summary>
        ///     Устанавливает действие при нажатии на кнопку.
        /// </summary>
        public void SetAction(BaseAction value)
        {
            _action = value;
        }

        // Click

        /// <summary>
        ///     Осуществляет программное нажатие на кнопку.
        /// </summary>
        public void Click()
        {
            Control.PerformClick();
        }
    }
}