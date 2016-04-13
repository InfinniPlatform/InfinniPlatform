using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

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
            string command;

            // Linux
            if (RunningOnLinux())
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
                if (Environment.Is64BitOperatingSystem)
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

            if (RunningOnLinux())
            {
                arguments = " ";
            }
            else
            {
                arguments = " --disable-smart-shrinking --dpi 96";
            }

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

            using (var process = new Process())
            {
                // При запуске на Linux bash-скриптов, возможен код ошибки 255.
                // Решением является добавление заголовка #!/bin/bash в начало скрипта.

                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                var outputBuilder = new StringBuilder();
                var errorBuilder = new StringBuilder();

                using (var outputCloseEvent = new AutoResetEvent(false))
                using (var errorCloseEvent = new AutoResetEvent(false))
                {
                    // Подписка на события записи в выходные потоки процесса

                    var copyOutputCloseEvent = outputCloseEvent;

                    process.OutputDataReceived += (s, e) =>
                                                  {
                                                      // Поток output закрылся (процесс завершил работу)
                                                      if (string.IsNullOrEmpty(e.Data))
                                                      {
                                                          copyOutputCloseEvent.Set();
                                                      }
                                                      else
                                                      {
                                                          outputBuilder.AppendLine(e.Data);
                                                      }
                                                  };

                    var copyErrorCloseEvent = errorCloseEvent;

                    process.ErrorDataReceived += (s, e) =>
                                                 {
                                                     // Поток error закрылся (процесс завершил работу)
                                                     if (string.IsNullOrEmpty(e.Data))
                                                     {
                                                         copyErrorCloseEvent.Set();
                                                     }
                                                     else
                                                     {
                                                         errorBuilder.AppendLine(e.Data);
                                                     }
                                                 };

                    bool isStarted;

                    try
                    {
                        isStarted = process.Start();
                    }
                    catch (Exception error)
                    {
                        // Не удалось запустить процесс, скорей всего, файл не существует или не является исполняемым

                        result.Completed = true;
                        result.ExitCode = -1;
                        result.Output = string.Format(Resources.CannotExecuteCommand, command, arguments, error.Message);

                        isStarted = false;
                    }

                    if (isStarted)
                    {
                        // Начало чтения выходных потоков процесса в асинхронном режиме, чтобы не создать блокировку
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        // Ожидание завершения процесса и закрытия выходных потоков
                        if (process.WaitForExit(timeout)
                            && outputCloseEvent.WaitOne(timeout)
                            && errorCloseEvent.WaitOne(timeout))
                        {
                            result.Completed = true;
                            result.ExitCode = process.ExitCode;

                            // Вывод актуален только при наличии ошибки
                            if (process.ExitCode != 0)
                            {
                                result.Output = $"{outputBuilder}{errorBuilder}";
                            }
                        }
                        else
                        {
                            try
                            {
                                // Зависшие процессы завершаются принудительно
                                process.Kill();
                            }
                            catch
                            {
                                // Любые ошибки в данном случае игнорируются
                            }
                        }
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