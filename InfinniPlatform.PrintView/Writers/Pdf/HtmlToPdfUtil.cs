﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Properties;

namespace InfinniPlatform.PrintView.Writers.Pdf
{
    internal class HtmlToPdfUtil
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
            _htmlToPdfTemp = string.IsNullOrWhiteSpace(htmlToPdfTemp) ? Path.GetTempPath() : htmlToPdfTemp;
        }


        private readonly string _htmlToPdfUtilCommand;
        private readonly string _htmlToPdfUtilArguments;
        private readonly string _htmlToPdfTemp;


        public async Task Convert(PrintSize size, PrintThickness padding, Stream inHtmlStream, Stream outPdfStream)
        {
            var fileName = Guid.NewGuid().ToString("N");
            var fileHtmlPath = Path.Combine(_htmlToPdfTemp, fileName + ".html");
            var filePdfPath = Path.Combine(_htmlToPdfTemp, fileName + ".pdf");

            try
            {
                using (var htmlFileStream = File.Create(fileHtmlPath))
                {
                    await inHtmlStream.CopyToAsync(htmlFileStream);
                    htmlFileStream.Flush();
                }

                var htmlToPdfUtilArguments = BuildHtmlToPdfUtilArguments(size, padding, fileHtmlPath, filePdfPath);
                var htmlToPdfConvertResult = await ProcessHelper.ExecuteShellCommand(_htmlToPdfUtilCommand, htmlToPdfUtilArguments, UtilTimeout);

                if (htmlToPdfConvertResult.Completed && htmlToPdfConvertResult.ExitCode == 0)
                {
                    using (var fileStream = File.OpenRead(filePdfPath))
                    {
                        await fileStream.CopyToAsync(outPdfStream);
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
            string command;

            // Linux
            if (IsRunningOnLinux())
            {
                // В случае, если 'X server' на сервере не установлен, что более вероятно, то утилиту wkhtmltopdf придется
                // запускать через какую-либо подсистему виртуализации, например, через xvfb. Предполагается, что в этом
                // случае команда запуска будет находится в файле '/usr/local/bin/wkhtmltopdf.sh'. Например, для xvfb
                // содержимое файла wkhtmltopdf.sh будет таким: 'xvfb-run -a -s "-screen 0 640x480x16" wkhtmltopdf "$@"'.

                command = "/usr/local/bin/wkhtmltopdf.sh";

                if (!File.Exists(command))
                {
                    command = "wkhtmltopdf";
                }
            }
            // Windows
            else
            {
                // Метод Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) работает неоднозначно для
                // различных комбинаций Environment.Is64BitOperatingSystem и Environment.Is64BitProcess, поэтому
                // местоположение ProgramFiles определяется явно по значениям переменных окружения

                // x64
                if (Is64BitOperatingSystem())
                {
                    // "C:\Program Files"
                    var programFiles = Environment.GetEnvironmentVariable("ProgramW6432");

                    // "C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe"
                    command = Path.Combine(programFiles ?? "", "wkhtmltopdf", "bin", "wkhtmltopdf.exe");

                    if (!File.Exists(command))
                    {
                        // "C:\Program Files (x86)\wkhtmltopdf\bin\wkhtmltopdf.exe"
                        var programFilesX86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)");

                        // "C:\Program Files (x86)\wkhtmltopdf\bin\wkhtmltopdf.exe"
                        command = Path.Combine(programFilesX86 ?? "", "wkhtmltopdf", "bin", "wkhtmltopdf.exe");
                    }
                }
                // x32
                else
                {
                    var programFiles = Environment.GetEnvironmentVariable("ProgramFiles");

                    command = Path.Combine(programFiles ?? "", "wkhtmltopdf", "bin", "wkhtmltopdf.exe");
                }
            }

            return command;
        }

        private static string GetDefaultHtmlToPdfUtilArguments()
        {
            string arguments;

            if (IsRunningOnLinux())
            {
                arguments = " ";
            }
            else
            {
                arguments = " --disable-smart-shrinking --dpi 96";
            }

            return $"{arguments} -B {PaddingBottom} -L {PaddingLeft} -R {PaddingRight} -T {PaddingTop} --page-height {PageHeight} --page-width {PageWidth} \"{HtmlInput}\" \"{PdfOutput}\"";
        }


        private static bool IsRunningOnLinux()
        {
            return !RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        private static bool Is64BitOperatingSystem()
        {
            return (RuntimeInformation.OSArchitecture == Architecture.X64)
                   || (RuntimeInformation.OSArchitecture == Architecture.Arm64);
        }


        private string BuildHtmlToPdfUtilArguments(PrintSize size, PrintThickness padding, string fileHtmlPath, string filePdfPath)
        {
            var paddingBottomMm = (int)PrintSizeUnitConverter.ToSpecifiedSize(padding.Bottom, padding.SizeUnit, PrintSizeUnit.Mm);
            var paddingLeftMm = (int)PrintSizeUnitConverter.ToSpecifiedSize(padding.Left, padding.SizeUnit, PrintSizeUnit.Mm);
            var paddingRightMm = (int)PrintSizeUnitConverter.ToSpecifiedSize(padding.Right, padding.SizeUnit, PrintSizeUnit.Mm);
            var paddingTopMm = (int)PrintSizeUnitConverter.ToSpecifiedSize(padding.Top, padding.SizeUnit, PrintSizeUnit.Mm);

            var htmlToPdfUtil = _htmlToPdfUtilArguments
                .Replace(HtmlInput, fileHtmlPath)
                .Replace(PdfOutput, filePdfPath)

                .Replace(PaddingBottom, paddingBottomMm.ToString())
                .Replace(PaddingLeft, paddingLeftMm.ToString())
                .Replace(PaddingRight, paddingRightMm.ToString())
                .Replace(PaddingTop, paddingTopMm.ToString())
                ;

            if (size != null)
            {
                var pageHeightMm = (int)PrintSizeUnitConverter.ToSpecifiedSize(size.Height, size.SizeUnit, PrintSizeUnit.Mm);
                htmlToPdfUtil = htmlToPdfUtil.Replace(PageHeight, pageHeightMm.ToString());

                var pageWidthMm = (int)PrintSizeUnitConverter.ToSpecifiedSize(size.Width, size.SizeUnit, PrintSizeUnit.Mm);
                htmlToPdfUtil = htmlToPdfUtil.Replace(PageWidth, pageWidthMm.ToString());
            }
            else
            {
                // A4
                const int defaultPageHeight = 297;
                const int defaultPageWidth = 210;

                htmlToPdfUtil = htmlToPdfUtil.Replace(PageHeight, defaultPageHeight.ToString());
                htmlToPdfUtil = htmlToPdfUtil.Replace(PageWidth, defaultPageWidth.ToString());
            }

            return htmlToPdfUtil;
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
    }
}