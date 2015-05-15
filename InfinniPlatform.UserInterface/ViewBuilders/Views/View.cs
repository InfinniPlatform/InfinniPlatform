using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels;
using InfinniPlatform.UserInterface.ViewBuilders.Messaging;
using InfinniPlatform.UserInterface.ViewBuilders.Parameter;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;

namespace InfinniPlatform.UserInterface.ViewBuilders.Views
{
	/// <summary>
	/// Визуальное представление.
	/// </summary>
	public sealed class View : BaseElement<UserControl>, ILayoutPanel
	{
		public View(View view)
			: base(view)
		{
		}


		// Text

		public override void SetText(string value)
		{
			base.SetText(value);
			this.InvokeScript(OnTextChanged, a => a.Value = value);
		}


		// ToolTip

		public override void SetToolTip(string value)
		{
			base.SetToolTip(value);
			Control.ToolTip = null;
		}


		// Image

		private string _image;

		/// <summary>
		/// Возвращает изображение заголовка представления.
		/// </summary>
		public string GetImage()
		{
			return _image;
		}

		/// <summary>
		/// Устанавливает изображение заголовка представления.
		/// </summary>
		public void SetImage(string value)
		{
			_image = value;
		}


		// Scripts

		private readonly Dictionary<string, ScriptDelegate> _scripts
			= new Dictionary<string, ScriptDelegate>();

		/// <summary>
		/// Возвращает прикладной скрипт.
		/// </summary>
		public ScriptDelegate GetScript(string name)
		{
			ScriptDelegate action;

			_scripts.TryGetValue(name, out action);

			return action;
		}

		/// <summary>
		/// Добавляет прикладной скрипт.
		/// </summary>
		public void AddScript(string name, ScriptDelegate action)
		{
			_scripts[name] = action;
			_context[name] = action;
		}


		// Parameters

		private readonly Dictionary<string, ParameterElement> _parameters
			= new Dictionary<string, ParameterElement>();

		/// <summary>
		/// Возвращает параметр.
		/// </summary>
		public ParameterElement GetParameter(string name)
		{
			ParameterElement parameter;

			_parameters.TryGetValue(name, out parameter);

			return parameter;
		}

		/// <summary>
		/// Добавляет параметр.
		/// </summary>
		public void AddParameter(ParameterElement parameter)
		{
			var name = parameter.GetName();

			_parameters[name] = parameter;
			_context[name] = parameter;
		}

		/// <summary>
		/// Возвращает список параметров.
		/// </summary>
		public IEnumerable<ParameterElement> GetParameters()
		{
			return _parameters.Values;
		}


		// DataSources

		private readonly Dictionary<string, IDataSource> _dataSources
			= new Dictionary<string, IDataSource>();

		/// <summary>
		/// Возвращает источник данных.
		/// </summary>
		public IDataSource GetDataSource(string name)
		{
			IDataSource dataSource;

			_dataSources.TryGetValue(name, out dataSource);

			return dataSource;
		}

		/// <summary>
		/// Добавляет источник данных.
		/// </summary>
		public void AddDataSource(IDataSource dataSource)
		{
			var name = dataSource.GetName();

			_dataSources[name] = dataSource;
			_context[name] = dataSource;
		}

		/// <summary>
		/// Возвращает список источников данных.
		/// </summary>
		public IEnumerable<IDataSource> GetDataSources()
		{
			return _dataSources.Values;
		}


		// LayoutPanel

		private ILayoutPanel _layoutPanel;

		/// <summary>
		/// Возвращает контейнер элементов.
		/// </summary>
		public ILayoutPanel GetLayoutPanel()
		{
			return _layoutPanel;
		}

		/// <summary>
		/// Устанавливает контейнер элементов.
		/// </summary>
		public void SetLayoutPanel(ILayoutPanel value)
		{
			if (_layoutPanel != value)
			{
				// Удаление старого контейнера и его элементов из контекста
				if (_layoutPanel != null)
				{
					RemoveElementFromContext(_layoutPanel);

					var childElements = _layoutPanel.GetAllChildElements();

					if (childElements != null)
					{
						foreach (var childElement in childElements)
						{
							RemoveElementFromContext(childElement);
						}
					}
				}

				object content = null;

				// Добавление нового контейнера и его элементов в контекст
				if (value != null)
				{
					AddElementToContext(value);

					var childElements = value.GetAllChildElements();

					if (childElements != null)
					{
						foreach (var childElement in childElements)
						{
							AddElementToContext(childElement);
						}
					}

					content = value.GetControl();
				}

				_layoutPanel = value;

				Control.Content = content;
			}
		}

		private void AddElementToContext(IElement element)
		{
			var name = element.GetName();

			if (!string.IsNullOrEmpty(name))
			{
				_context[name] = element;
			}
		}

		private void RemoveElementFromContext(IElement element)
		{
			var name = element.GetName();

			if (!string.IsNullOrEmpty(name))
			{
				_context[name] = null;
			}
		}


		// DialogResult

		private DialogResult _dialogResult;

		/// <summary>
		/// Возвращает результат работы представления.
		/// </summary>
		public DialogResult GetDialogResult()
		{
			return _dialogResult;
		}

		/// <summary>
		/// Устанавливает результат работы представления.
		/// </summary>
		public void SetDialogResult(DialogResult value)
		{
			_dialogResult = value;
		}


		// Focus

		/// <summary>
		/// Устанавливает фокус ввода на элемент.
		/// </summary>
		public override void Focus()
		{
			if (OnGotFocus != null)
			{
				this.InvokeScript(OnGotFocus);
			}
		}


		// Open

		/// <summary>
		/// Открывает представление.
		/// </summary>
		public void Open()
		{
			if (OnOpening != null)
			{
				this.InvokeScript(OnOpening);
			}

			if (OnOpened != null)
			{
				this.InvokeScript(OnOpened);
			}
		}


		// Close

		/// <summary>
		/// Закрывает представление.
		/// </summary>
		public bool Close(bool force = false)
		{
			if (OnClosing != null)
			{
				dynamic arguments = null;

				this.InvokeScript(OnClosing, a =>
											 {
												 a.Force = force;
												 arguments = a;
											 });

				if (!force && arguments != null && arguments.IsCancel == true)
				{
					return false;
				}
			}

			if (OnClosed != null)
			{
				this.InvokeScript(OnClosed);
			}

			return true;
		}


		// Context

		private readonly dynamic _context
			= ContextBuilder.Build();

		/// <summary>
		/// Возвращает контекст представления.
		/// </summary>
		public dynamic GetContext()
		{
			return _context;
		}


		// Exchange

		private static readonly IMessageBus MessageBus
			= new MessageBus();

		private IMessageExchange _messageExchange;

		/// <summary>
		/// Возвращает точку обмена сообщениями.
		/// </summary>
		public IMessageExchange GetExchange()
		{
			if (_messageExchange == null)
			{
				var exchangeName = GetName();

				_messageExchange = MessageBus.GetExchange(exchangeName);
			}

			return _messageExchange;
		}

		/// <summary>
		/// Возвращает точку обмена сообщениями.
		/// </summary>
		public IMessageExchange GetExchange(string name)
		{
			return MessageBus.GetExchange(name);
		}


		// Events

		/// <summary>
		/// Возвращает или устанавливает обработчик события о том, что текст изменен.
		/// </summary>
		public ScriptDelegate OnTextChanged { get; set; }

		/// <summary>
		/// Возвращает или устанавливает обработчик события о том, что представление получает фокус.
		/// </summary>
		public ScriptDelegate OnGotFocus { get; set; }

		/// <summary>
		/// Возвращает или устанавливает обработчик события о том, что представление теряет фокус.
		/// </summary>
		public ScriptDelegate OnLostFocus { get; set; }

		/// <summary>
		/// Возвращает или устанавливает обработчик события о том, что представление открывается.
		/// </summary>
		public ScriptDelegate OnOpening { get; set; }

		/// <summary>
		/// Возвращает или устанавливает обработчик события о том, что представление открыто.
		/// </summary>
		public ScriptDelegate OnOpened { get; set; }

		/// <summary>
		/// Возвращает или устанавливает обработчик события о том, что представление закрывается.
		/// </summary>
		public ScriptDelegate OnClosing { get; set; }

		/// <summary>
		/// Возвращает или устанавливает обработчик события о том, что представление закрыто.
		/// </summary>
		public ScriptDelegate OnClosed { get; set; }


		public override IEnumerable<IElement> GetChildElements()
		{
			var layoutPanel = _layoutPanel;

			if (layoutPanel != null)
			{
				return new[] { layoutPanel };
			}

			return null;
		}

		public override bool Validate()
		{
			var childElements = this.GetAllChildElements();

			if (childElements != null)
			{
				return childElements.All(e => e.Validate());
			}

			return base.Validate();
		}
	}
}