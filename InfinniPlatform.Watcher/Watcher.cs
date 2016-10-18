﻿using System;
using System.IO;
using System.Linq;
using System.Threading;

using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Watcher.Properties;

namespace InfinniPlatform.Watcher
{
    /// <summary>
    /// Сервис синхронизации метаданных.
    /// </summary>
    public class Watcher : AppEventHandler
    {
        public Watcher(WatcherSettings settings) : base(1)
        {
            _settings = settings;
        }

        private readonly WatcherSettings _settings;
        private readonly int MaxCopyAttempts = 10;

        public override void OnAfterStart()
        {
            if (CheckSettings(_settings))
            {
                Console.WriteLine(Resources.SuccessStart);

                TryExecute(CheckDirectoriesSync);

                var watcher = new FileSystemWatcher
                              {
                                  Path = _settings.SourceDirectory,
                                  IncludeSubdirectories = true
                              };

                watcher.Changed += (sender, eventArgs) => TryExecute(() => SyncFileSystemEntity(eventArgs));
                watcher.Created += (sender, eventArgs) => TryExecute(() => CreateFileSystemEntity(eventArgs));
                watcher.Deleted += (sender, eventArgs) => TryExecute(() => DeleteFileSystemEntity(eventArgs));
                watcher.Renamed += (sender, eventArgs) => TryExecute(() => SyncFileSystemEntity(eventArgs));

                watcher.EnableRaisingEvents = true;

                ConsoleLog.Info(string.Format(Resources.ChangesWillBeTransferred, Environment.NewLine, _settings.SourceDirectory, _settings.DestinationDirectory));
            }
            else
            {
                ConsoleLog.Warning(Resources.FailedStart);
            }
        }

        private void CheckDirectoriesSync()
        {
            var sourceFiles = Directory.GetFiles(_settings.SourceDirectory, "*.*", SearchOption.AllDirectories);
            var destFiles = Directory.GetFiles(_settings.DestinationDirectory, "*.*", SearchOption.AllDirectories);

            if (destFiles.Length != sourceFiles.Length)
            {
                ConsoleLog.Warning(string.Format(Resources.SyncingContentDirectories, _settings.SourceDirectory, _settings.DestinationDirectory, Environment.NewLine));
            }
        }

        private void DeleteFileSystemEntity(FileSystemEventArgs eventArgs)
        {
            var extension = Path.GetExtension(eventArgs.FullPath);

            if (extension != string.Empty)
            {
                if (_settings.WatchingFileExtensions.Contains(extension))
                {
                    var part = eventArgs.FullPath.ToPartPath(_settings.SourceDirectory);

                    ConsoleLog.Info(string.Format(Resources.EventLog, Environment.NewLine, DateTime.Now.ToString("G"), part, eventArgs.ChangeType));

                    File.Delete(Path.Combine(_settings.DestinationDirectory, part));
                    ConsoleLog.Info(Resources.SyncComplete);
                }
            }
            else
            {
                var part = eventArgs.FullPath.ToPartPath(_settings.SourceDirectory);

                ConsoleLog.Info(string.Format(Resources.EventLog, Environment.NewLine, DateTime.Now.ToString("G"), part, eventArgs.ChangeType));

                Directory.Delete(Path.Combine(_settings.DestinationDirectory, part), true);
                ConsoleLog.Info(Resources.SyncComplete);
            }
        }

        private void CreateFileSystemEntity(FileSystemEventArgs eventArgs)
        {
            var extension = Path.GetExtension(eventArgs.FullPath);

            if (extension != string.Empty)
            {
                if (_settings.WatchingFileExtensions.Contains(extension))
                {
                    var part = eventArgs.FullPath.ToPartPath(_settings.SourceDirectory);

                    ConsoleLog.Info(string.Format(Resources.EventLog, Environment.NewLine, DateTime.Now.ToString("G"), part, eventArgs.ChangeType));

                    File.Copy(eventArgs.FullPath, Path.Combine(_settings.DestinationDirectory, part), true);

                    ConsoleLog.Info(Resources.SyncComplete);
                }
            }
            else
            {
                var part = eventArgs.FullPath.ToPartPath(_settings.SourceDirectory);

                ConsoleLog.Info(string.Format(Resources.EventLog, Environment.NewLine, DateTime.Now.ToString("G"), part, eventArgs.ChangeType));

                Directory.CreateDirectory(Path.Combine(_settings.DestinationDirectory, part));

                ConsoleLog.Info(Resources.SyncComplete);
            }
        }

        private bool CheckSettings(WatcherSettings settings)
        {
            var isCorrectSettings = true;

            if (string.IsNullOrEmpty(settings.SourceDirectory))
            {
                ConsoleLog.Warning(Resources.SourceDictionaryCannotBeEmpty);
                isCorrectSettings = false;
            }
            else if (!Directory.Exists(settings.SourceDirectory))
            {
                ConsoleLog.Warning($"Directory {settings.SourceDirectory} does not exist.");
                isCorrectSettings = false;
            }

            if (string.IsNullOrEmpty(settings.DestinationDirectory))
            {
                ConsoleLog.Warning(Resources.DestinationDirectoryCannotBeEmpty);
                isCorrectSettings = false;
            }
            else if (!Directory.Exists(settings.DestinationDirectory))
            {
                ConsoleLog.Warning($"Directory {settings.DestinationDirectory} does not exist, so it was created.");
                Directory.CreateDirectory(settings.DestinationDirectory);
            }

            if (!isCorrectSettings)
            {
                ConsoleLog.Info(Resources.SettingsExample);
            }

            return isCorrectSettings;
        }

        private void SyncFileSystemEntity(FileSystemEventArgs eventArgs)
        {
            var extension = Path.GetExtension(eventArgs.FullPath);

            if (extension != string.Empty)
            {
                if (_settings.WatchingFileExtensions.Contains(extension))
                {
                    var part = eventArgs.FullPath.ToPartPath(_settings.SourceDirectory);

                    ConsoleLog.Info(string.Format(Resources.EventLog, Environment.NewLine, DateTime.Now.ToString("G"), part, eventArgs.ChangeType));

                    for (var i = 1; i <= MaxCopyAttempts; i++)
                    {
                        try
                        {
                            File.Copy(eventArgs.FullPath, Path.Combine(_settings.DestinationDirectory, part), true);

                            ConsoleLog.Info(Resources.SyncComplete);

                            break;
                        }
                        catch (IOException)
                        {
                            Thread.Sleep(500);

                            ConsoleLog.Warning(string.Format(Resources.BusyFileCopyAttempt, i, MaxCopyAttempts));

                            if (i == MaxCopyAttempts)
                            {
                                ConsoleLog.Error(Resources.CantCopyFile);
                            }
                        }
                    }
                }
            }
        }

        private static void TryExecute(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                ConsoleLog.Error(e);
            }
        }
    }
}