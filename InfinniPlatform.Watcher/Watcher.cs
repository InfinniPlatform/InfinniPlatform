using System;
using System.IO;
using System.Linq;

using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Watcher.Properties;

namespace InfinniPlatform.Watcher
{
    /// <summary>
    /// Сервис синхронизации метаданных.
    /// </summary>
    public class Watcher : AppEventHandler
    {
        public Watcher(WatcherSettings settings, ILog log) : base(1)
        {
            _settings = settings;
            _log = log;
        }

        private readonly ILog _log;

        private readonly WatcherSettings _settings;

        public override void OnAfterStart()
        {
            if (CheckSettings(_settings))
            {
                Console.WriteLine(Resources.SuccessStart);

                SyncDirectories();

                var watcher = new FileSystemWatcher
                              {
                                  Path = _settings.SourceDirectory,
                                  IncludeSubdirectories = true
                              };

                watcher.Changed += (sender, eventArgs) => SyncFileSystemEntity(eventArgs);
                watcher.Created += (sender, eventArgs) => CreateFileSystemEntity(eventArgs);
                watcher.Deleted += (sender, eventArgs) => DeleteFileSystemEntity(eventArgs);
                watcher.Renamed += (sender, eventArgs) => SyncFileSystemEntity(eventArgs);

                watcher.EnableRaisingEvents = true;

                _log.Info(string.Format(Resources.ChangesWillBeTransferred, Environment.NewLine, _settings.SourceDirectory, Environment.NewLine, Environment.NewLine, _settings.DestinationDirectory));
            }

            Console.WriteLine(Resources.FailedStart);
        }

        private void SyncDirectories()
        {
            var sourceFiles = Directory.GetFiles(_settings.SourceDirectory, "*.*", SearchOption.AllDirectories);
            var destFiles = Directory.GetFiles(_settings.DestinationDirectory, "*.*", SearchOption.AllDirectories);

            if (destFiles.Length != sourceFiles.Length)
            {
                _log.Info(string.Format(Resources.SyncingContentDirectories, _settings.SourceDirectory, _settings.DestinationDirectory));

                Directory.Delete(_settings.DestinationDirectory, true);

                foreach (var dirPath in Directory.GetDirectories(_settings.SourceDirectory, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(_settings.SourceDirectory, _settings.DestinationDirectory));
                }

                foreach (var newPath in Directory.GetFiles(_settings.SourceDirectory, "*.*", SearchOption.AllDirectories))
                {
                    File.Copy(newPath, newPath.Replace(_settings.SourceDirectory, _settings.DestinationDirectory), true);
                }

                _log.Info(Resources.SyncComplete);
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

                    _log.Info(string.Format(Resources.EventLog, Environment.NewLine, DateTime.Now.ToString("G"), Environment.NewLine, part, eventArgs.ChangeType));

                    TryExecute(() =>
                               {
                                   File.Delete(Path.Combine(_settings.DestinationDirectory, part));
                                   _log.Info(Resources.SyncComplete);
                               });
                }
            }
            else
            {
                var part = eventArgs.FullPath.ToPartPath(_settings.SourceDirectory);

                _log.Info(string.Format(Resources.EventLog, Environment.NewLine, DateTime.Now.ToString("G"), Environment.NewLine, part, eventArgs.ChangeType));

                TryExecute(() =>
                           {
                               Directory.Delete(Path.Combine(_settings.DestinationDirectory, part), true);
                               _log.Info(Resources.SyncComplete);
                           });
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

                    _log.Info(string.Format(Resources.EventLog, Environment.NewLine, DateTime.Now.ToString("G"), Environment.NewLine, part, eventArgs.ChangeType));

                    TryExecute(() => { File.Copy(eventArgs.FullPath, Path.Combine(_settings.DestinationDirectory, part), true); });

                    _log.Info(Resources.SyncComplete);
                }
            }
            else
            {
                var part = eventArgs.FullPath.ToPartPath(_settings.SourceDirectory);

                _log.Info(string.Format(Resources.EventLog, Environment.NewLine, DateTime.Now.ToString("G"), Environment.NewLine, part, eventArgs.ChangeType));

                TryExecute(() => { Directory.CreateDirectory(Path.Combine(_settings.DestinationDirectory, part)); });

                _log.Info(Resources.SyncComplete);
            }
        }

        private bool CheckSettings(WatcherSettings settings)
        {
            var isCorrectSettings = true;

            var n = Environment.NewLine;

            if (string.IsNullOrEmpty(settings.SourceDirectory))
            {
                _log.Error(Resources.SourceDictionaryCannotBeEmpty);
                isCorrectSettings = false;
            }

            if (!Directory.Exists(settings.SourceDirectory))
            {
                _log.Error($"Directory {settings.SourceDirectory} does not exist.");
                isCorrectSettings = false;
            }

            if (string.IsNullOrEmpty(settings.DestinationDirectory))
            {
                _log.Error(Resources.DestinationDirectoryCannotBeEmpty);
                isCorrectSettings = false;
            }

            if (!Directory.Exists(settings.DestinationDirectory))
            {
                _log.Error($"Directory {settings.DestinationDirectory} does not exist.");
                isCorrectSettings = false;
            }

            if (!isCorrectSettings)
            {
                var helpMessage = $"{n}Add watcher setting to AppExtentions.json configuration file:" +
                                  $"{n}  /* Настройки наблюдателя */" +
                                  $"{n}  \"watcher\": {{" +
                                  $"{n}      /* Директория источника метаданных */" +
                                  $"{n}      \"SourceDirectory\": <path>," +
                                  $"{n}      /* Директория для синхнонизации */" +
                                  $"{n}      \"DestinationDirectory\": <path>," +
                                  $"{n}      /* Расширения синхронизируемых файлов */" +
                                  $"{n}      \"WatchingFileExtensions\": [" +
                                  $"{n}          \".json\"" +
                                  $"{n}      ]" +
                                  $"{n}  }}";

                _log.Info(helpMessage);
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

                    _log.Info(string.Format(Resources.EventLog, Environment.NewLine, DateTime.Now.ToString("G"), Environment.NewLine, part, eventArgs.ChangeType));

                    TryExecute(() => { File.Copy(eventArgs.FullPath, Path.Combine(_settings.DestinationDirectory, part), true); });

                    _log.Info(Resources.SyncComplete);
                }
            }
        }

        private void TryExecute(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
        }
    }
}