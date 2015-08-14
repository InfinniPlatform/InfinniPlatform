using System;
using System.Diagnostics;
using System.IO;

using InfinniPlatform.FlowDocument.Builders.Factories;
using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Properties;

namespace InfinniPlatform.FlowDocument.Converters.Pdf
{
    internal sealed class HtmlToPdfUtil
    {
        private const string ReplacePaddingBottom = "{padding-bottom}";
        private const string ReplacePaddingLeft = "{padding-left}";
        private const string ReplacePaddingRight = "{padding-right}";
        private const string ReplacePaddingTop = "{padding-top}";

        private const string ReplacePageWidth = "{page-width}";
        private const string ReplacePageHeight = "{page-height}";

        private const string ReplaceHtmlInput = "{file-html}";
        private const string ReplacePdfOutput = "{file-pdf}";


        public HtmlToPdfUtil(string htmlToPdfUtil, string htmlToPdfTemp)
        {
            if (string.IsNullOrWhiteSpace(htmlToPdfUtil))
            {
                throw new ArgumentNullException("htmlToPdfUtil");
            }

            if (string.IsNullOrWhiteSpace(htmlToPdfTemp))
            {
                throw new ArgumentNullException("htmlToPdfTemp");
            }

            _htmlToPdfUtil = htmlToPdfUtil;
            _htmlToPdfTemp = htmlToPdfTemp;
        }


        private readonly string _htmlToPdfUtil;
        private readonly string _htmlToPdfTemp;


        public void Convert(PrintElementSize size, PrintElementThickness padding, Stream inHtmlStream, Stream outPdfStream)
        {
            var fileName = Guid.NewGuid().ToString("N");
            var fileHtmlPath = Path.Combine(_htmlToPdfTemp, fileName + ".html");
            var filePdfPath = Path.Combine(_htmlToPdfTemp, fileName + ".pdf");

            try
            {
                using (var htmlFileStream = File.Create(fileHtmlPath))
                {
                    inHtmlStream.CopyTo(htmlFileStream);
                    htmlFileStream.Flush();
                }

                var htmlToPdfUtil = ReplaceString(size, padding, fileHtmlPath, filePdfPath);
                var htmlToPdfConvertResult = ExecuteShellCommand(htmlToPdfUtil, 60 * 1000);

                if (htmlToPdfConvertResult.Completed && htmlToPdfConvertResult.ExitCode == 0)
                {
                    using (var fileStream = File.OpenRead(filePdfPath))
                    {
                        fileStream.CopyTo(outPdfStream);
                        outPdfStream.Flush();
                    }
                }
                else
                {
                    if (htmlToPdfConvertResult.Completed)
                    {
                        throw new InvalidOperationException(string.Format(Resources.CannotConvertHtmlToPdf, htmlToPdfConvertResult.Output, htmlToPdfConvertResult.ExitCode));
                    }

                    throw new InvalidOperationException(Resources.CannotConvertHtmlToPdfByTimeout);
                }
            }
            finally
            {
                DeleteFile(fileHtmlPath);
                DeleteFile(filePdfPath);
            }
        }


        public static string GetDefaultHtmlToPdfUtil()
        {
            string wkhtmltopdf;

            if (RunningOnLinux())
            {
                wkhtmltopdf = "wkhtmltopdf";
            }
            else
            {
                var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                wkhtmltopdf = string.Format("\"{0}\" --disable-smart-shrinking --dpi 96", Path.Combine(programFiles, "wkhtmltopdf", "bin", "wkhtmltopdf.exe"));          
            }

            return string.Format("{0} -B {1} -L {2} -R {3} -T {4} --page-height {5} --page-width {6} {7} {8}",

                wkhtmltopdf,

                ReplacePaddingBottom,
                ReplacePaddingLeft,
                ReplacePaddingRight,
                ReplacePaddingTop,

                ReplacePageHeight,
                ReplacePageWidth,

                string.Format("\"{0}\"", ReplaceHtmlInput),
                string.Format("\"{0}\"", ReplacePdfOutput)
                );
        }

        private static bool RunningOnLinux()
        {
            var p = (int)Environment.OSVersion.Platform;

            return ((p == 4) || (p == 128));
        }


        private static void DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch
            {
                // ignored
            }
        }

        private string ReplaceString(PrintElementSize size, PrintElementThickness padding, string fileHtmlPath, string filePdfPath)
        {
            var mmPaddingBottom = (int)(padding.Bottom / SizeUnits.Mm);
            var mmPaddingLeft = (int)(padding.Left / SizeUnits.Mm);
            var mmPaddingRight = (int)(padding.Right / SizeUnits.Mm);
            var mmPaddingTop = (int)(padding.Right / SizeUnits.Mm);

            var htmlToPdfUtil = _htmlToPdfUtil
                .Replace(ReplaceHtmlInput, fileHtmlPath)
                .Replace(ReplacePdfOutput, filePdfPath)

                .Replace(ReplacePaddingBottom, mmPaddingBottom.ToString())
                .Replace(ReplacePaddingLeft, mmPaddingLeft.ToString())
                .Replace(ReplacePaddingRight, mmPaddingRight.ToString())
                .Replace(ReplacePaddingTop, mmPaddingTop.ToString())
                ;

            if (size != null)
            {
                if (size.Height != null)
                {
                    var mmPageHeight = (int)(size.Height / SizeUnits.Mm);
                    htmlToPdfUtil = htmlToPdfUtil.Replace(ReplacePageHeight, mmPageHeight.ToString());
                }

                if (size.Width != null)
                {
                    var mmPageWidth = (int)(size.Width / SizeUnits.Mm);
                    htmlToPdfUtil = htmlToPdfUtil.Replace(ReplacePageWidth, mmPageWidth.ToString());
                }
            }
            else
            {
                var defaultPageHeight = 297;
                var defaultPageWidth = 210;

                htmlToPdfUtil = htmlToPdfUtil.Replace(ReplacePageHeight, defaultPageHeight.ToString());
                htmlToPdfUtil = htmlToPdfUtil.Replace(ReplacePageWidth, defaultPageWidth.ToString());
            }

            return htmlToPdfUtil;
        }

        private static ProcessResult ExecuteShellCommand(string command, int timeout)
        {
            var result = new ProcessResult();

            using (var shellProcess = new Process())
            {
                shellProcess.StartInfo.FileName = command;
                shellProcess.StartInfo.UseShellExecute = false;
                shellProcess.StartInfo.RedirectStandardOutput = true;
                shellProcess.StartInfo.RedirectStandardError = true;
                shellProcess.StartInfo.CreateNoWindow = true;
                shellProcess.Start();

                if (shellProcess.WaitForExit(timeout))
                {
                    result.Completed = true;
                    result.ExitCode = shellProcess.ExitCode;
                    result.Output = shellProcess.StandardOutput.ReadToEnd();
                }
                else
                {
                    try
                    {
                        shellProcess.Kill();
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            return result;
        }


        private struct ProcessResult
        {
            public bool Completed;
            public int? ExitCode;
            public string Output;
        }
    }
}