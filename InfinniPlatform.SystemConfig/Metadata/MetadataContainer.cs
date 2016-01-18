using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Index;

namespace InfinniPlatform.SystemConfig.Metadata
{
    /// <summary>
    ///     Каркас контейнера метаданных объекта
    /// </summary>
    public sealed class MetadataContainer
    {
        private readonly List<dynamic> _generators = new List<dynamic>();
        private readonly List<dynamic> _printViews = new List<dynamic>();
        private readonly List<dynamic> _processes = new List<dynamic>();
        private readonly List<dynamic> _scenario = new List<dynamic>();
        private readonly List<dynamic> _services = new List<dynamic>();
        private readonly List<dynamic> _statuses = new List<dynamic>();
        private readonly List<dynamic> _validationErrors = new List<dynamic>();
        private readonly List<dynamic> _validationWarnings = new List<dynamic>();
        private readonly List<dynamic> _views = new List<dynamic>();

        public MetadataContainer(string metadataIndexType)
        {
            MetadataIndexType = metadataIndexType;
        }

        public string ContainerId { get; set; }

        /// <summary>
        ///     Поисковые возможности контейнера
        /// </summary>
        public SearchAbilityType SearchAbility { get; private set; }

        /// <summary>
        ///     Тип в индексе, в котором будет храниться документ
        /// </summary>
        public string MetadataIndexType { get; set; }

        /// <summary>
        ///     Схема данных документа
        /// </summary>
        public dynamic Schema { get; set; }

        /// <summary>
        ///     Установить возможности поиска хранимых документов
        /// </summary>
        /// <param name="searchAbility">Возможности поиска</param>
        public void UpdateSearchAbilityType(SearchAbilityType searchAbility)
        {
            SearchAbility = searchAbility;
        }

        public void RegisterProcess(dynamic process)
        {
            var existingProcess =
                _processes.FirstOrDefault(v => v.Name.ToLowerInvariant() == process.Name.ToLowerInvariant());
            if (existingProcess != null)
            {
                _processes.Remove(existingProcess);
            }
            _processes.Add(process);
        }

        public void RegisterScenario(dynamic scenario)
        {
            var existingScenario =
                _scenario.FirstOrDefault(v => v.Name.ToLowerInvariant() == scenario.Name.ToLowerInvariant());
            if (existingScenario != null)
            {
                _scenario.Remove(existingScenario);
            }
            _scenario.Add(scenario);
        }

        public void RegisterService(dynamic service)
        {
            var existingService =
                _services.FirstOrDefault(v => v.Name.ToLowerInvariant() == service.Name.ToLowerInvariant());
            if (existingService != null)
            {
                _services.Remove(existingService);
            }
            _services.Add(service);
        }

        public void RegisterGenerator(dynamic generator)
        {
            var existingGenerator =
                _generators.FirstOrDefault(v => v.Name.ToLowerInvariant() == generator.Name.ToLowerInvariant());
            if (existingGenerator != null)
            {
                _generators.Remove(existingGenerator);
            }
            _generators.Add(generator);
        }

        public void RegisterValidationWarning(dynamic warning)
        {
            var existingWarning =
                _validationWarnings.FirstOrDefault(v => v.Name.ToLowerInvariant() == warning.Name.ToLowerInvariant());
            if (existingWarning != null)
            {
                _validationWarnings.Remove(existingWarning);
            }
            _validationWarnings.Add(warning);
        }

        public void RegisterValidationError(dynamic error)
        {
            var existingError =
                _validationErrors.FirstOrDefault(v => v.Name.ToLowerInvariant() == error.Name.ToLowerInvariant());
            if (existingError != null)
            {
                _validationErrors.Remove(existingError);
            }
            _validationErrors.Add(error);
        }

        public void RegisterStatus(dynamic status)
        {
            var existingStatus =
                _statuses.FirstOrDefault(v => v.Name.ToLowerInvariant() == status.Name.ToLowerInvariant());
            if (existingStatus != null)
            {
                _statuses.Remove(existingStatus);
            }
            _statuses.Add(status);
        }

        public void RegisterView(dynamic view)
        {
            var existingView = _views.FirstOrDefault(v => v.Name.ToLowerInvariant() == view.Name.ToLowerInvariant());
            if (existingView != null)
            {
                _views.Remove(existingView);
            }
            _views.Add(view);
        }

        public void RegisterPrintView(dynamic printView)
        {
            var existingPrintView =
                _printViews.FirstOrDefault(
                    v => string.Equals(v.Name, printView.Name, StringComparison.OrdinalIgnoreCase));

            if (existingPrintView != null)
            {
                _printViews.Remove(existingPrintView);
            }

            _printViews.Add(printView);
        }

        public void DeleteProcess(string processName)
        {
            var existingProcess =
                _processes.FirstOrDefault(v => v.Name.ToLowerInvariant() == processName.ToLowerInvariant());
            if (existingProcess != null)
            {
                _processes.Remove(existingProcess);
            }
        }

        public void DeleteScenario(string scenarioName)
        {
            var existingScenario =
                _scenario.FirstOrDefault(v => v.Name.ToLowerInvariant() == scenarioName.ToLowerInvariant());
            if (existingScenario != null)
            {
                _scenario.Remove(existingScenario);
            }
        }

        public void DeleteService(string serviceName)
        {
            var existingService =
                _services.FirstOrDefault(v => v.Name.ToLowerInvariant() == serviceName.ToLowerInvariant());
            if (existingService != null)
            {
                _services.Remove(existingService);
            }
        }

        public void DeleteGenerator(string generatorName)
        {
            var existingGenerator =
                _generators.FirstOrDefault(v => v.Name.ToLowerInvariant() == generatorName.ToLowerInvariant());
            if (existingGenerator != null)
            {
                _generators.Remove(existingGenerator);
            }
        }

        public void DeleteValidationWarning(string warningName)
        {
            var existingWarning =
                _validationWarnings.FirstOrDefault(v => v.Name.ToLowerInvariant() == warningName.ToLowerInvariant());
            if (existingWarning != null)
            {
                _validationWarnings.Remove(existingWarning);
            }
        }

        public void DeleteValidationError(string errorName)
        {
            var existingError =
                _validationErrors.FirstOrDefault(v => v.Name.ToLowerInvariant() == errorName.ToLowerInvariant());
            if (existingError != null)
            {
                _validationErrors.Remove(existingError);
            }
        }

        public void DeleteStatus(string statusName)
        {
            var existingStatus =
                _statuses.FirstOrDefault(v => v.Name.ToLowerInvariant() == statusName.ToLowerInvariant());
            if (existingStatus != null)
            {
                _statuses.Remove(existingStatus);
            }
        }

        public void DeleteView(string viewName)
        {
            var existingView = _views.FirstOrDefault(v => v.Name.ToLowerInvariant() == viewName.ToLowerInvariant());
            if (existingView != null)
            {
                _views.Remove(existingView);
            }
        }

        public void DeletePrintView(string printViewName)
        {
            var existingPrintView =
                _printViews.FirstOrDefault(v => string.Equals(v.Name, printViewName, StringComparison.OrdinalIgnoreCase));

            if (existingPrintView != null)
            {
                _printViews.Remove(existingPrintView);
            }
        }

        public dynamic GetProcess(string processName)
        {
            return _processes.FirstOrDefault(p => p.Name == processName);
        }

        public dynamic GetProcess(Func<dynamic, bool> processSelector)
        {
            return _processes.FirstOrDefault(processSelector);
        }

        public dynamic GetScenario(string scenarioName)
        {
            return _scenario.FirstOrDefault(sc => sc.Name == scenarioName);
        }

        public dynamic GetScenario(Func<dynamic, bool> scenarioSelector)
        {
            return _scenario.FirstOrDefault(scenarioSelector);
        }

        public dynamic GetService(string serviceName)
        {
            return _services.FirstOrDefault(s => s.Name == serviceName);
        }

        public dynamic GetService(Func<dynamic, bool> serviceSelector)
        {
            return _services.FirstOrDefault(serviceSelector);
        }

        public dynamic GetGenerator(Func<dynamic, bool> generatorSelector)
        {
            return _generators.FirstOrDefault(g => generatorSelector(g));
        }

        public dynamic GetView(Func<dynamic, bool> viewSelector)
        {
            return _views.FirstOrDefault(v => viewSelector(v));
        }

        public dynamic GetPrintView(Func<dynamic, bool> selector)
        {
            return _printViews.FirstOrDefault(v => selector(v));
        }

        public dynamic GetValidationWarning(string warningName)
        {
            return _validationWarnings.FirstOrDefault(sc => sc.Name == warningName);
        }

        public dynamic GetValidationError(string errorName)
        {
            return _validationErrors.FirstOrDefault(sc => sc.Name == errorName);
        }

        public dynamic GetValidationWarning(Func<dynamic, bool> selector)
        {
            return _validationWarnings.FirstOrDefault(selector);
        }

        public dynamic GetValidationError(Func<dynamic, bool> selector)
        {
            return _validationErrors.FirstOrDefault(selector);
        }

        public dynamic GetStatus(string statusName)
        {
            return _statuses.FirstOrDefault(sc => sc.Name == statusName);
        }

        public IEnumerable<dynamic> GetViews()
        {
            return _views;
        }

        public IEnumerable<dynamic> GetPrintViews()
        {
            return _printViews;
        }

        public IEnumerable<dynamic> GetGenerators()
        {
            return _generators;
        }

        public IEnumerable<dynamic> GetScenarios()
        {
            return _scenario;
        }

        public IEnumerable<dynamic> GetProcesses()
        {
            return _processes;
        }

        public IEnumerable<dynamic> GetServices()
        {
            return _services;
        }

        public IEnumerable<dynamic> GetValidationErrors()
        {
            return _validationErrors;
        }

        public IEnumerable<dynamic> GetValidationWarnings()
        {
            return _validationWarnings;
        }
    }
}