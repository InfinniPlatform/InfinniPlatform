using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using InfinniPlatform.Agent.Settings;
using InfinniPlatform.Agent.Tasks;

using TaskStatus = InfinniPlatform.Agent.Tasks.TaskStatus;

namespace InfinniPlatform.Agent.Helpers
{
    public class InfinniNodeAdapter
    {
        public InfinniNodeAdapter(AgentSettings agentSettings,
                                  INodeTaskStorage nodeTaskStorage)
        {
            _command = $"{agentSettings.NodeDirectory}{Path.DirectorySeparatorChar}Infinni.Node.exe";

            _agentSettings = agentSettings;
            _nodeTaskStorage = nodeTaskStorage;
        }

        private readonly AgentSettings _agentSettings;
        private readonly string _command;
        private readonly INodeTaskStorage _nodeTaskStorage;

        /// <summary>
        /// Запускает процесс и перехватывает его вывод.
        /// </summary>
        /// <param name="arguments">Аргументы запуска процесса.</param>
        /// <param name="timeout">Таймаут выполнения процесса.</param>
        /// <param name="taskId">Идентификатор задачи.</param>
        public async Task ExecuteCommand(string arguments, int timeout, string taskId)
        {
            using (var process = new Process())
            {
                // При запуске на Linux bash-скриптов, возможен код ошибки 255.
                // Решением является добавление заголовка #!/bin/bash в начало скрипта.

                FillProcessStartInfo(arguments, process);

                // Подписка на события записи в выходные потоки процесса
                var outputCloseEvent = new TaskCompletionSource<bool>();

                process.OutputDataReceived += (s, e) =>
                                              {
                                                  // Поток output закрылся (процесс завершил работу)
                                                  if (string.IsNullOrEmpty(e.Data))
                                                  {
                                                      outputCloseEvent.SetResult(true);
                                                  }
                                                  else
                                                  {
                                                      _nodeTaskStorage.AddOutput(taskId, e.Data);
                                                  }
                                              };

                var errorCloseEvent = new TaskCompletionSource<bool>();

                process.ErrorDataReceived += (s, e) =>
                                             {
                                                 // Поток error закрылся (процесс завершил работу)
                                                 if (string.IsNullOrEmpty(e.Data))
                                                 {
                                                     errorCloseEvent.SetResult(true);
                                                 }
                                                 else
                                                 {
                                                     _nodeTaskStorage.AddOutput(taskId, e.Data);
                                                 }
                                             };

                bool isStarted;

                try
                {
                    isStarted = process.Start();
                }
                catch (Exception error)
                {
                    // Usually it occurs when an executable file is not found or is not executable

                    _nodeTaskStorage.SetCompleted(taskId);
                    _nodeTaskStorage.AddOutput(taskId, error.Message);

                    isStarted = false;
                }

                if (isStarted)
                {
                    // Reads the output stream first and then waits because deadlocks are possible
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    // Creates task to wait for process exit using timeout
                    var waitForExit = WaitForExitAsync(process, timeout);

                    // Create task to wait for process exit and closing all output streams
                    var processTask = Task.WhenAll(waitForExit, outputCloseEvent.Task, errorCloseEvent.Task);

                    // Waits process completion and then checks it was not completed by timeout
                    if ((await Task.WhenAny(Task.Delay(timeout), processTask) == processTask) && waitForExit.Result)
                    {
                        _nodeTaskStorage.SetCompleted(taskId);
                    }
                    else
                    {
                        try
                        {
                            // Kill hung process
                            process.Kill();
                        }
                        catch
                        {
                            //ignored
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Запускает процесс и перехватывает его вывод.
        /// </summary>
        /// <param name="arguments">Аргументы запуска процесса.</param>
        /// <param name="timeout">Таймаут выполнения процесса.</param>
        public async Task<TaskStatus> ExecuteCommand(string arguments, int timeout)
        {
            var result = new TaskStatus();

            using (var process = new Process())
            {
                // При запуске на Linux bash-скриптов, возможен код ошибки 255.
                // Решением является добавление заголовка #!/bin/bash в начало скрипта.

                FillProcessStartInfo(arguments, process);

                // Подписка на события записи в выходные потоки процесса
                var outputBuilder = new StringBuilder();
                var outputCloseEvent = new TaskCompletionSource<bool>();

                process.OutputDataReceived += (s, e) =>
                                              {
                                                  // Поток output закрылся (процесс завершил работу)
                                                  if (string.IsNullOrEmpty(e.Data))
                                                  {
                                                      outputCloseEvent.SetResult(true);
                                                  }
                                                  else
                                                  {
                                                      outputBuilder.AppendLine(e.Data);
                                                  }
                                              };

                var errorBuilder = new StringBuilder();
                var errorCloseEvent = new TaskCompletionSource<bool>();

                process.ErrorDataReceived += (s, e) =>
                                             {
                                                 // Поток error закрылся (процесс завершил работу)
                                                 if (string.IsNullOrEmpty(e.Data))
                                                 {
                                                     errorCloseEvent.SetResult(true);
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
                    // Usually it occurs when an executable file is not found or is not executable
                    result.Completed = true;
                    result.Error = error.Message;

                    isStarted = false;
                }

                if (isStarted)
                {
                    // Reads the output stream first and then waits because deadlocks are possible
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    // Creates task to wait for process exit using timeout
                    var waitForExit = WaitForExitAsync(process, timeout);

                    // Create task to wait for process exit and closing all output streams
                    var processTask = Task.WhenAll(waitForExit, outputCloseEvent.Task, errorCloseEvent.Task);

                    // Waits process completion and then checks it was not completed by timeout
                    if ((await Task.WhenAny(Task.Delay(timeout), processTask) == processTask) && waitForExit.Result)
                    {
                        result.Completed = true;
                        result.Output = $"{outputBuilder}{errorBuilder}";
                    }
                    else
                    {
                        try
                        {
                            // Kill hung process
                            process.Kill();
                        }
                        catch
                        {
                            //ignored
                        }
                    }
                }
            }

            return result;
        }

        private void FillProcessStartInfo(string arguments, Process process)
        {
            process.StartInfo.FileName = _command;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WorkingDirectory = _agentSettings.NodeDirectory;
        }

        private static Task<bool> WaitForExitAsync(Process process, int timeout)
        {
            return Task.Run(() => process.WaitForExit(timeout));
        }
    }
}