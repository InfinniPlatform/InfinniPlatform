using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Infinni.Agent.Settings;
using Infinni.Agent.Tasks;

using TaskStatus = Infinni.Agent.Tasks.TaskStatus;

namespace Infinni.Agent.Helpers
{
    public class InfinniNodeAdapter
    {
        public InfinniNodeAdapter(AgentSettings agentSettings,
                                  IAgentTaskStorage agentTaskStorage)
        {
            _command = $"{agentSettings.NodeDirectory}{Path.DirectorySeparatorChar}Infinni.Node.exe";

            _agentSettings = agentSettings;
            _agentTaskStorage = agentTaskStorage;
        }

        private readonly AgentSettings _agentSettings;
        private readonly IAgentTaskStorage _agentTaskStorage;
        private readonly string _command;

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
                FillProcessStartInfo(arguments, process);

                var outputCloseEvent = new TaskCompletionSource<bool>();

                process.OutputDataReceived += (s, e) =>
                                              {
                                                  if (string.IsNullOrEmpty(e.Data))
                                                  {
                                                      outputCloseEvent.SetResult(true);
                                                  }
                                                  else
                                                  {
                                                      _agentTaskStorage.AddOutput(taskId, e.Data);
                                                  }
                                              };

                var errorCloseEvent = new TaskCompletionSource<bool>();

                process.ErrorDataReceived += (s, e) =>
                                             {
                                                 if (string.IsNullOrEmpty(e.Data))
                                                 {
                                                     errorCloseEvent.SetResult(true);
                                                 }
                                                 else
                                                 {
                                                     _agentTaskStorage.AddOutput(taskId, e.Data);
                                                 }
                                             };

                bool isStarted;

                try
                {
                    isStarted = process.Start();
                }
                catch (Exception error)
                {
                    _agentTaskStorage.SetCompleted(taskId);
                    _agentTaskStorage.AddOutput(taskId, error.Message);

                    isStarted = false;
                }

                if (isStarted)
                {
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    var waitForExit = WaitForExitAsync(process, timeout);

                    var processTask = Task.WhenAll(waitForExit, outputCloseEvent.Task, errorCloseEvent.Task);

                    if ((await Task.WhenAny(Task.Delay(timeout), processTask) == processTask) && waitForExit.Result)
                    {
                        _agentTaskStorage.SetCompleted(taskId);
                    }
                    else
                    {
                        try
                        {
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
                FillProcessStartInfo(arguments, process);

                var outputBuilder = new StringBuilder();
                var outputCloseEvent = new TaskCompletionSource<bool>();

                process.OutputDataReceived += (s, e) =>
                                              {
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
                    result.Completed = true;
                    result.Error = error.Message;

                    isStarted = false;
                }

                if (isStarted)
                {
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    var waitForExit = WaitForExitAsync(process, timeout);

                    var processTask = Task.WhenAll(waitForExit, outputCloseEvent.Task, errorCloseEvent.Task);

                    if ((await Task.WhenAny(Task.Delay(timeout), processTask) == processTask) && waitForExit.Result)
                    {
                        result.Completed = true;
                        result.Output = $"{outputBuilder}{errorBuilder}";
                    }
                    else
                    {
                        try
                        {
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