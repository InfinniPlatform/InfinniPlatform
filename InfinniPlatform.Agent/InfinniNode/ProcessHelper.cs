using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using InfinniPlatform.Agent.Settings;

namespace InfinniPlatform.Agent.InfinniNode
{
    public class ProcessHelper
    {
        public delegate void NodeOutputEventHandler(object sender, NodeOutputEventArgs e);


        public ProcessHelper(AgentSettings agentSettings)
        {
            _workingDirectory = agentSettings.NodeDirectory;
            _command = $"{agentSettings.NodeDirectory}{Path.DirectorySeparatorChar}Infinni.Node.exe";
        }

        private readonly string _command;
        private readonly string _workingDirectory;

        public event NodeOutputEventHandler OnNodeOutputDataRecieved;

        /// <summary>
        /// Запускает процесс и перехватывает его вывод.
        /// </summary>
        /// <param name="arguments">Аргументы запуска процесса.</param>
        /// <param name="timeout">Таймаут выполнения процесса.</param>
        /// <param name="taskId">Идентификатор задачи.</param>
        public async Task<ProcessResult> ExecuteCommand(string arguments, int timeout, string taskId)
        {
            var result = new ProcessResult();

            using (var process = new Process())
            {
                // При запуске на Linux bash-скриптов, возможен код ошибки 255.
                // Решением является добавление заголовка #!/bin/bash в начало скрипта.

                process.StartInfo.FileName = _command;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WorkingDirectory = _workingDirectory;

                // Подписка на события записи в выходные потоки процесса
                var outputBuilder = new StringBuilder();
                var outputCloseEvent = new TaskCompletionSource<bool>();

                process.OutputDataReceived += (s, e) =>
                                              {
                                                  // Поток output закрылся (процесс завершил работу)
                                                  if (string.IsNullOrEmpty(e.Data))
                                                  {
                                                      outputCloseEvent.SetResult(true);
                                                      OnNodeOutputDataRecieved?.Invoke(this, new NodeOutputEventArgs(e.Data, taskId, true));
                                                  }
                                                  else
                                                  {
                                                      outputBuilder.AppendLine(e.Data);
                                                      OnNodeOutputDataRecieved?.Invoke(this, new NodeOutputEventArgs(e.Data, taskId));
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
                                                     OnNodeOutputDataRecieved?.Invoke(this, new NodeOutputEventArgs(e.Data, taskId, true));
                                                 }
                                                 else
                                                 {
                                                     errorBuilder.AppendLine(e.Data);
                                                     OnNodeOutputDataRecieved?.Invoke(this, new NodeOutputEventArgs(e.Data, taskId));
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
                    result.ExitCode = -1;
                    result.Output = error.Message;

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
                        result.ExitCode = process.ExitCode;
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

        private static Task<bool> WaitForExitAsync(Process process, int timeout)
        {
            return Task.Run(() => process.WaitForExit(timeout));
        }


        public struct ProcessResult
        {
            public bool Completed;
            public int? ExitCode;
            public string Output;
        }
    }


    public class NodeOutputEventArgs
    {
        public NodeOutputEventArgs(string output, string taskId, bool isOutputClosed = false)
        {
            Output = output;
            TaskId = taskId;
            IsOutputClosed = isOutputClosed;
        }

        public string Output { get; set; }

        public string TaskId { get; set; }

        public bool IsOutputClosed { get; set; }
    }
}