using System;
using System.Diagnostics;
using System.IO;

using InfinniPlatform.Core.Properties;

namespace InfinniPlatform.Core.Threading
{
    /// <summary>
    ///     Диспетчер для управления процессом.
    /// </summary>
    public sealed class ProcessDispatcher : IDisposable
    {
        private volatile Process _process;
        private readonly string _fileName;
        private readonly object _processSync = new object();

        /// <summary>
        ///     Конструктор.
        /// </summary>
        /// <param name="fileName">Имя исполняемого файла.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public ProcessDispatcher(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(Resources.ExecutableFileNotFound, fileName);
            }

            _fileName = fileName;
        }

        /// <summary>
        ///     Освобождает ресурсы.
        /// </summary>
        public void Dispose()
        {
            StopProcess();
        }

        /// <summary>
        ///     Запускает процесс.
        /// </summary>
        public void StartProcess(string arguments)
        {
            if (_process == null)
            {
                lock (_processSync)
                {
                    if (_process == null)
                    {
                        var process = new Process
                        {
                            StartInfo =
                            {
                                FileName = _fileName,
                                Arguments = arguments,
                                UseShellExecute = false
                            },
                            EnableRaisingEvents = true
                        };

                        process.Exited += OnUnexpectedProcessExited;
                        process.Start();

                        _process = process;
                    }
                }
            }
        }

        /// <summary>
        ///     Останавливает процесс.
        /// </summary>
        public void StopProcess()
        {
            if (_process != null)
            {
                lock (_processSync)
                {
                    if (_process != null)
                    {
                        try
                        {
                            _process.Exited -= OnUnexpectedProcessExited;
                            _process.Kill();
                            _process.Dispose();
                        }
                        finally
                        {
                            _process = null;
                        }
                    }
                }
            }
        }

        private void OnUnexpectedProcessExited(object sender, EventArgs e)
        {
            if (_process != null)
            {
                lock (_processSync)
                {
                    if (_process != null)
                    {
                        try
                        {
                            _process.Exited -= OnUnexpectedProcessExited;
                            _process.Dispose();
                        }
                        finally
                        {
                            _process = null;
                        }
                    }
                }
            }
        }
    }
}