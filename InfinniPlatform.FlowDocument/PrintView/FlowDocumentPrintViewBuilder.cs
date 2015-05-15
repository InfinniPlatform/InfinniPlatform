using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Api.PrintView;

namespace InfinniPlatform.FlowDocument.PrintView
{
	/// <summary>
	/// Построитель печатного представления на основе System.Windows.Documents.FlowDocument.
	/// </summary>
	public sealed class FlowDocumentPrintViewBuilder : IPrintViewBuilder
	{
		public FlowDocumentPrintViewBuilder()
			: this(new FlowDocumentPrintViewFactory(), new FlowDocumentPrintViewConverter())
		{
		}

		public FlowDocumentPrintViewBuilder(IFlowDocumentPrintViewFactory printViewFactory, IFlowDocumentPrintViewConverter printViewConverter)
		{
			if (printViewFactory == null)
			{
				throw new ArgumentNullException("printViewFactory");
			}

			if (printViewConverter == null)
			{
				throw new ArgumentNullException("printViewConverter");
			}

			_printViewFactory = printViewFactory;
			_printViewConverter = printViewConverter;
		}


		private readonly IFlowDocumentPrintViewFactory _printViewFactory;
		private readonly IFlowDocumentPrintViewConverter _printViewConverter;


		/// <summary>
		/// Создает файл печатного представления.
		/// </summary>
		/// <param name="printView">Шаблон печатного представления.</param>
		/// <param name="printViewSource">Данные печатного представления.</param>
		/// <param name="printViewFileFormat">Формат файла печатного представления.</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <returns>Файл печатного представления.</returns>
		public byte[] BuildFile(object printView, object printViewSource, PrintViewFileFormat printViewFileFormat = PrintViewFileFormat.Pdf)
		{
			if (printView == null)
			{
				throw new ArgumentNullException("printView");
			}

			// Текущая реализация на базе WPF требует STA-поток
			return ExecuteInStaThread(() =>
									  {
										  // Формирование документа печатного представления
										  var document = _printViewFactory.Create(printView, printViewSource);

										  if (document != null)
										  {
											  // Сохранение документа печатного представления в указанном формате
											  using (var documentStream = new MemoryStream())
											  {
												  _printViewConverter.Convert(document, documentStream, printViewFileFormat);

												  documentStream.Flush();

												  return documentStream.ToArray();
											  }
										  }

										  return null;
									  });
		}


		private static T ExecuteInStaThread<T>(Func<T> func)
		{
			if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
			{
				return func();
			}

			var taskSource = new TaskCompletionSource<T>();

			var thread = new Thread(() =>
									{
										try
										{
											taskSource.SetResult(func());
										}
										catch (Exception error)
										{
											taskSource.SetException(error);
										}
									});

			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();

			try
			{
				return taskSource.Task.Result;
			}
			catch (AggregateException error)
			{
				if (error.InnerException != null)
				{
					throw error.InnerException;
				}

				throw;
			}
		}
	}
}