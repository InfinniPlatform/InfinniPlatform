using System;

namespace InfinniPlatform.FastReport.TemplatesFluent.Print
{
	public static class PrintExtensions
	{
		private static readonly Tuple<int, int> SizeA0 = new Tuple<int, int>(841, 1189);
		private static readonly Tuple<int, int> SizeA1 = new Tuple<int, int>(594, 841);
		private static readonly Tuple<int, int> SizeA2 = new Tuple<int, int>(420, 594);
		private static readonly Tuple<int, int> SizeA3 = new Tuple<int, int>(297, 420);
		private static readonly Tuple<int, int> SizeA4 = new Tuple<int, int>(210, 297);
		private static readonly Tuple<int, int> SizeA5 = new Tuple<int, int>(148, 210);


		/// <summary>
		/// Формат A0.
		/// </summary>
		public static PrintPaperConfig A0(this PrintPaperConfig target)
		{
			return target.Width(SizeA0.Item1).Height(SizeA0.Item2);
		}

		/// <summary>
		/// Формат A0.
		/// </summary>
		public static PrinterConfig A0(this PrinterConfig target)
		{
			return target.PaperWidth(SizeA0.Item1).PaperHeight(SizeA0.Item2);
		}


		/// <summary>
		/// Формат A1.
		/// </summary>
		public static PrintPaperConfig A1(this PrintPaperConfig target)
		{
			return target.Width(SizeA1.Item1).Height(SizeA1.Item2);
		}

		/// <summary>
		/// Формат A1.
		/// </summary>
		public static PrinterConfig A1(this PrinterConfig target)
		{
			return target.PaperWidth(SizeA1.Item1).PaperHeight(SizeA1.Item2);
		}


		/// <summary>
		/// Формат A2.
		/// </summary>
		public static PrintPaperConfig A2(this PrintPaperConfig target)
		{
			return target.Width(SizeA2.Item1).Height(SizeA2.Item2);
		}

		/// <summary>
		/// Формат A2.
		/// </summary>
		public static PrinterConfig A2(this PrinterConfig target)
		{
			return target.PaperWidth(SizeA2.Item1).PaperHeight(SizeA2.Item2);
		}


		/// <summary>
		/// Формат A3.
		/// </summary>
		public static PrintPaperConfig A3(this PrintPaperConfig target)
		{
			return target.Width(SizeA3.Item1).Height(SizeA3.Item2);
		}

		/// <summary>
		/// Формат A3.
		/// </summary>
		public static PrinterConfig A3(this PrinterConfig target)
		{
			return target.PaperWidth(SizeA3.Item1).PaperHeight(SizeA3.Item2);
		}


		/// <summary>
		/// Формат A4.
		/// </summary>
		public static PrintPaperConfig A4(this PrintPaperConfig target)
		{
			return target.Width(SizeA4.Item1).Height(SizeA4.Item2);
		}

		/// <summary>
		/// Формат A4.
		/// </summary>
		public static PrinterConfig A4(this PrinterConfig target)
		{
			return target.PaperWidth(SizeA4.Item1).PaperHeight(SizeA4.Item2);
		}


		/// <summary>
		/// Формат A5.
		/// </summary>
		public static PrintPaperConfig A5(this PrintPaperConfig target)
		{
			return target.Width(SizeA5.Item1).Height(SizeA5.Item2);
		}

		/// <summary>
		/// Формат A5.
		/// </summary>
		public static PrinterConfig A5(this PrinterConfig target)
		{
			return target.PaperWidth(SizeA5.Item1).PaperHeight(SizeA5.Item2);
		}
	}
}