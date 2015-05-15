using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Inlines
{
	sealed class PrintElementImageFactory : IPrintElementFactory
	{
		public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
		{
			var element = new InlineUIContainer();

			BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
			BuildHelper.ApplyTextProperties(element, elementMetadata);

			BuildHelper.ApplyInlineProperties(element, buildContext.ElementStyle);
			BuildHelper.ApplyInlineProperties(element, elementMetadata);

			ApplyImageData(element, buildContext, elementMetadata);

			return element;
		}

		private static void ApplyImageData(InlineUIContainer element, PrintElementBuildContext buildContext, dynamic elementMetadata)
		{
			var imageData = GetImageDataStream(buildContext, elementMetadata.Data);

			if (imageData != null)
			{
				try
				{
					var imageSource = new BitmapImage();
					imageSource.BeginInit();
					imageSource.StreamSource = imageData;
					ApplyRotation(imageSource, elementMetadata.Rotation);
					imageSource.EndInit();

					var imageControl = new Image();
					imageControl.BeginInit();
					imageControl.Width = imageSource.Width;
					imageControl.Height = imageSource.Height;
					ApplySize(imageControl, elementMetadata.Size);
					ApplyStretch(imageControl, elementMetadata.Stretch);
					imageControl.Source = imageSource;
					imageControl.EndInit();

					element.Child = imageControl;
				}
				catch
				{
				}
			}
		}

		private static Stream GetImageDataStream(PrintElementBuildContext buildContext, dynamic imageData)
		{
			Stream result = null;

			byte[] imageDataBytes;

			// Если данные не заданы, они берутся из источника
			if (!ConvertHelper.TryToBytes(imageData, out imageDataBytes) || imageDataBytes == null)
			{
				if (!ConvertHelper.TryToBytes(buildContext.ElementSourceValue, out imageDataBytes) || imageDataBytes == null)
				{
					if (buildContext.IsDesignMode)
					{
						// В режиме дизайна отображается наименование свойства источника данных или выражение

						if (!string.IsNullOrEmpty(buildContext.ElementSourceProperty))
						{
							result = GetImageDataStreamStub(string.Format("[{0}]", buildContext.ElementSourceProperty));
						}
						else if (!string.IsNullOrEmpty(buildContext.ElementSourceExpression))
						{
							result = GetImageDataStreamStub(string.Format("[{0}]", buildContext.ElementSourceExpression));
						}
					}
				}
				else
				{
					result = new MemoryStream(imageDataBytes);
				}
			}
			else
			{
				result = new MemoryStream(imageDataBytes);
			}

			return result;
		}

		private static Stream GetImageDataStreamStub(string imageText)
		{
			var formattedText = new FormattedText(imageText, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 12, Brushes.Black)
								{
									MaxTextWidth = 180,
									MaxTextHeight = 180,
									Trimming = TextTrimming.None,
									TextAlignment = TextAlignment.Center
								};

			var drawingVisual = new DrawingVisual();
			var drawingContext = drawingVisual.RenderOpen();
			drawingContext.DrawRectangle(Brushes.WhiteSmoke, new Pen(Brushes.LightGray, 1), new Rect(0, 0, 200, 200));
			drawingContext.DrawText(formattedText, new Point(10, 10));
			drawingContext.Close();

			var renderTargetBitmap = new RenderTargetBitmap(200, 200, 96, 96, PixelFormats.Default);
			renderTargetBitmap.Render(drawingVisual);

			var bitmapEncoder = new PngBitmapEncoder();
			bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

			var bitmapImageStream = new MemoryStream();
			bitmapEncoder.Save(bitmapImageStream);

			return bitmapImageStream;
		}

		private static void ApplyRotation(BitmapImage bitmap, dynamic rotation)
		{
			string rotationString;

			if (ConvertHelper.TryToNormString(rotation, out rotationString))
			{
				switch (rotationString)
				{
					case "rotate0":
						bitmap.Rotation = Rotation.Rotate0;
						break;
					case "rotate90":
						bitmap.Rotation = Rotation.Rotate90;
						break;
					case "rotate180":
						bitmap.Rotation = Rotation.Rotate180;
						break;
					case "rotate270":
						bitmap.Rotation = Rotation.Rotate270;
						break;
				}
			}
		}

		private static void ApplySize(Image image, dynamic size)
		{
			if (size != null)
			{
				double width, height;

				if (BuildHelper.TryToSizeInPixels(size.Width, size.SizeUnit, out width))
				{
					image.Width = width;
				}

				if (BuildHelper.TryToSizeInPixels(size.Height, size.SizeUnit, out height))
				{
					image.Height = height;
				}
			}
		}

		private static void ApplyStretch(Image image, dynamic stretch)
		{
			string stretchString;

			if (ConvertHelper.TryToNormString(stretch, out stretchString))
			{
				switch (stretchString)
				{
					case "none":
						image.Stretch = Stretch.None;
						break;
					case "fill":
						image.Stretch = Stretch.Fill;
						break;
					case "uniform":
						image.Stretch = Stretch.Uniform;
						break;
				}
			}
		}
	}
}