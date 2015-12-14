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
        private const int UtilTimeout = 60 * 1000;

        private const string PaddingBottom = "{padding-bottom}";
        private const string PaddingLeft = "{padding-left}";
        private const string PaddingRight = "{padding-right}";
        private const string PaddingTop = "{padding-top}";

        private const string PageWidth = "{page-width}";
        private const string PageHeight = "{page-height}";

        private const string HtmlInput = "{file-html}";
        private const string PdfOutput = "{file-pdf}";


        public HtmlToPdfUtil(string htmlToPdfUtilCommand, string htmlToPdfUtilArguments, string htmlToPdfTemp)
        {
            _htmlToPdfUtilCommand = string.IsNullOrWhiteSpace(htmlToPdfUtilCommand) ? GetDefaultHtmlToPdfUtilCommand() : htmlToPdfUtilCommand;
            _htmlToPdfUtilArguments = string.IsNullOrWhiteSpace(htmlToPdfUtilArguments) ? GetDefaultHtmlToPdfUtilArguments() : htmlToPdfUtilArguments;
            _htmlToPdfTemp = string.IsNullOrWhiteSpace(htmlToPdfTemp) ? GetDefaultHtmlToPdfTemp() : htmlToPdfTemp;
        }


        private readonly string _htmlToPdfUtilCommand;
        private readonly string _htmlToPdfUtilArguments;
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

                var htmlToPdfUtilArguments = BuildHtmlToPdfUtilArguments(size, padding, fileHtmlPath, filePdfPath);
                var htmlToPdfConvertResult = ExecuteShellCommand(_htmlToPdfUtilCommand, htmlToPdfUtilArguments, UtilTimeout);

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


        private static string GetDefaultHtmlToPdfUtilCommand()
        {
            var command = RunningOnLinux()
                ? "wkhtmltopdf"
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "wkhtmltopdf", "bin", "wkhtmltopdf.exe");

            return command;
        }

        private static string GetDefaultHtmlToPdfUtilArguments()
        {
            var arguments = RunningOnLinux()
                ? " "
                : " --disable-smart-shrinking --dpi 96";

            return $"{arguments} -B {PaddingBottom} -L {PaddingLeft} -R {PaddingRight} -T {PaddingTop} --page-height {PageHeight} --page-width {PageWidth} {$"\"{HtmlInput}\""} {$"\"{PdfOutput}\""}";
        }

        private static string GetDefaultHtmlToPdfTemp()
        {
            return Path.GetTempPath();
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


        private string BuildHtmlToPdfUtilArguments(PrintElementSize size, PrintElementThickness padding, string fileHtmlPath, string filePdfPath)
        {
            var mmPaddingBottom = (int)(padding.Bottom / SizeUnits.Mm);
            var mmPaddingLeft = (int)(padding.Left / SizeUnits.Mm);
            var mmPaddingRight = (int)(padding.Right / SizeUnits.Mm);
            var mmPaddingTop = (int)(padding.Right / SizeUnits.Mm);

            var htmlToPdfUtil = _htmlToPdfUtilArguments
                .Replace(HtmlInput, fileHtmlPath)
                .Replace(PdfOutput, filePdfPath)

                .Replace(PaddingBottom, mmPaddingBottom.ToString())
                .Replace(PaddingLeft, mmPaddingLeft.ToString())
                .Replace(PaddingRight, mmPaddingRight.ToString())
                .Replace(PaddingTop, mmPaddingTop.ToString())
                ;

            if (size != null)
            {
                if (size.Height != null)
                {
                    var mmPageHeight = (int)(size.Height / SizeUnits.Mm);
                    htmlToPdfUtil = htmlToPdfUtil.Replace(PageHeight, mmPageHeight.ToString());
                }

                if (size.Width != null)
                {
                    var mmPageWidth = (int)(size.Width / SizeUnits.Mm);
                    htmlToPdfUtil = htmlToPdfUtil.Replace(PageWidth, mmPageWidth.ToString());
                }
            }
            else
            {
                const int defaultPageHeight = 297;
                const int defaultPageWidth = 210;

                htmlToPdfUtil = htmlToPdfUtil.Replace(PageHeight, defaultPageHeight.ToString());
                htmlToPdfUtil = htmlToPdfUtil.Replace(PageWidth, defaultPageWidth.ToString());
            }

            return htmlToPdfUtil;
        }


        private static ProcessResult ExecuteShellCommand(string command, string arguments, int timeout)
        {
            var result = new ProcessResult();

            using (var shellProcess = new Process())
            {
                shellProcess.StartInfo.FileName = command;
                shellProcess.StartInfo.Arguments = arguments;
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