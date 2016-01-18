using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Registers;
using InfinniPlatform.ElasticSearch.Factories;
using InfinniPlatform.ElasticSearch.Filters;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Registers;

namespace InfinniPlatform.SystemConfig.Registers
{
    /// <summary>
    /// Предоставляет методы для работы с регистрами.
    /// </summary>
    internal sealed class RegisterApi : IRegisterApi
    {
        // TODO: Нужен глубокий рефакторинг и тестирование.

        public RegisterApi(IDocumentApi documentApi, IIndexFactory indexFactory, IMetadataApi metadataApi)
        {
            _documentApi = documentApi;
            _indexFactory = indexFactory;
            _metadataApi = metadataApi;
            _filterFactory = FilterBuilderFactory.GetInstance();
        }

        private readonly IDocumentApi _documentApi;
        private readonly INestFilterBuilder _filterFactory;
        private readonly IIndexFactory _indexFactory;
        private readonly IMetadataApi _metadataApi;

        /// <summary>
        /// Создает (но не сохраняет) запись регистра.
        /// </summary>
        public dynamic CreateEntry(
            string configuration,
            string registerName,
            string documentId,
            DateTime? documentDate,
            dynamic document,
            bool isInfoRegister)
        {
            if (string.IsNullOrEmpty(configuration))
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrEmpty(registerName))
            {
                throw new ArgumentNullException(nameof(registerName));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (documentId == null)
            {
                documentId = document.Id;
            }

            if (string.IsNullOrEmpty(documentId))
            {
                throw new ArgumentNullException(nameof(documentId));
            }

            dynamic registerEntry = new DynamicWrapper();
            registerEntry[RegisterConstants.RegistrarProperty] = document.Id;
            registerEntry[RegisterConstants.RegistrarTypeProperty] = documentId;
            registerEntry[RegisterConstants.EntryTypeProperty] = RegisterEntryType.Other;

            if (documentDate == null)
            {
                // Дата документа явно не задана, используем дату из содержимого переданного документа
                dynamic documentEvents = _metadataApi.GetDocumentEvents(configuration, documentId);
                string dateFieldName = documentEvents?.RegisterPoint?.DocumentDateProperty;

                if (!string.IsNullOrEmpty(dateFieldName))
                {
                    documentDate = document[dateFieldName];
                }
            }

            var registerMetadata = _metadataApi.GetRegister(configuration, registerName);

            // Признак того, что необходимо создать запись для регистра сведений
            if (isInfoRegister && registerMetadata != null)
            {
                documentDate = RegisterPeriod.AdjustDateToPeriod(documentDate.Value, registerMetadata.Period);
            }

            registerEntry[RegisterConstants.DocumentDateProperty] = documentDate;

            return registerEntry;
        }

        /// <summary>
        /// Выполняет проведение данных документа в регистр.
        /// </summary>
        public void PostEntries(
            string configuration,
            string registerName,
            IEnumerable<dynamic> registerEntries)
        {
            if (string.IsNullOrEmpty(configuration))
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrEmpty(registerName))
            {
                throw new ArgumentNullException(nameof(registerName));
            }

            if (registerEntries == null)
            {
                throw new ArgumentNullException(nameof(registerEntries));
            }

            var registerObject = _metadataApi.GetRegister(configuration, registerName);

            if (registerObject == null)
            {
                throw new ArgumentException($"Register '{registerName} not found'");
            }

            var dimensionNames = new List<string>();

            foreach (var property in registerObject.Properties)
            {
                if (property.Type == RegisterPropertyType.Dimension)
                {
                    dimensionNames.Add(property.Property);
                }
            }

            foreach (var registerEntry in registerEntries)
            {
                // Id генерируется по следующему алгоритму:
                // формируется уникальный ключ записи по всем полям-измерениям и по полю даты,
                // далее из уникального ключа рассчитывается Guid записи

                if (registerEntry[RegisterConstants.DocumentDateProperty] == null)
                {
                    throw new InvalidOperationException("Property 'DocumentDate' should be in the registry entry");
                }

                string uniqueKey = registerEntry[RegisterConstants.DocumentDateProperty].ToString();

                foreach (var dimensionName in dimensionNames)
                {
                    if (registerEntry[dimensionName] != null)
                    {
                        uniqueKey += registerEntry[dimensionName].ToString();
                    }
                }

                using (var md5 = MD5.Create())
                {
                    var hash = md5.ComputeHash(Encoding.Default.GetBytes(uniqueKey));
                    registerEntry.Id = new Guid(hash).ToString();
                }
            }

            _documentApi.SetDocuments(configuration, RegisterConstants.RegisterNamePrefix + registerName, registerEntries);
        }

        /// <summary>
        /// Выполняет перепроведение документов до указанной даты.
        /// </summary>
        public void RecarryingEntries(
            string configuration,
            string registerName,
            DateTime aggregationDate,
            bool deteleExistingRegisterEntries = true
            )
        {
            // TODO: Механизм перепроведения нуждается в переработке!

            // Один документ может создать две записи в одном регистре в ходе проведения, 
            // однако перепровести документ нужно только один раз. Для этого будем хранить
            // список идентификаторов уже перепроведенных документов
            var recarriedDocuments = new List<string>();

            var pageNumber = 0;

            while (true)
            {
                // Получаем записи из регистра постранично
                Action<FilterBuilder> filter = f => f.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsLessThanOrEquals(aggregationDate));

                var registerEntries = _documentApi.GetDocument(
                    configuration,
                    RegisterConstants.RegisterNamePrefix + registerName,
                    filter,
                    pageNumber++, 1000).ToArray();

                if (registerEntries.Length == 0)
                {
                    break;
                }

                // Перепроводить документы нужно все сразу после удаления соответствующих
                // записей из регистра. Поэтому сначала сохраняем пары
                // <Тип документа - содержимое документа> чтобы далее выполнить SetDocument для каждого элемента
                var documentsToRecarry = new List<Tuple<string, dynamic>>();

                foreach (var registerEntry in registerEntries)
                {
                    // Получаем документ-регистратор
                    string registrarId = registerEntry.Registrar;
                    string registrarType = registerEntry.RegistrarType;

                    var documentRegistrar = _documentApi.GetDocumentById(configuration, registrarType, registrarId);

                    if (deteleExistingRegisterEntries)
                    {
                        // Удаляем запись из регистра
                        _documentApi.DeleteDocument(configuration, RegisterConstants.RegisterNamePrefix + registerName, registerEntry.Id);
                    }

                    if (documentRegistrar != null && !recarriedDocuments.Contains(registrarId))
                    {
                        documentsToRecarry.Add(new Tuple<string, dynamic>(registrarType, documentRegistrar));
                        recarriedDocuments.Add(registrarId);
                    }
                }

                foreach (var document in documentsToRecarry)
                {
                    // Перепроводка документа
                    _documentApi.SetDocument(configuration, document.Item1, document.Item2);
                }
            }

            // Удаляем значения из таблицы итогов
            Action<FilterBuilder> action = f => f.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsLessThanOrEquals(aggregationDate));

            var registerTotalEntries = _documentApi.GetDocument(
                configuration,
                RegisterConstants.RegisterTotalNamePrefix + registerName,
                action,
                0, 1000);

            foreach (var registerEntry in registerTotalEntries)
            {
                _documentApi.DeleteDocument(configuration, RegisterConstants.RegisterTotalNamePrefix + registerName, registerEntry.Id);
            }
        }

        /// <summary>
        /// Рассчитывает итоги для регистров накопления на текущую дату.
        /// </summary>
        public void RecalculateTotals(
            string configuration)
        {
            var registerName = configuration + RegisterConstants.RegistersCommonInfo;

            var registersInfo = _documentApi.GetDocument(configuration, registerName, null, 0, 1000);

            var tempDate = DateTime.Now;

            var calculationDate = new DateTime(
                tempDate.Year,
                tempDate.Month,
                tempDate.Day,
                tempDate.Hour,
                tempDate.Minute,
                tempDate.Second);

            foreach (var registerInfo in registersInfo)
            {
                dynamic registerId = registerInfo.Id;

                var aggregatedData = GetValuesByDate(configuration, registerId, calculationDate);

                foreach (var item in aggregatedData)
                {
                    item.Id = Guid.NewGuid().ToString();
                    item[RegisterConstants.DocumentDateProperty] = calculationDate;

                    _documentApi.SetDocument(configuration, RegisterConstants.RegisterTotalNamePrefix + registerId, item);
                }
            }
        }

        /// <summary>
        /// Удаляет запись регистра.
        /// </summary>
        public void DeleteEntry(
            string configuration,
            string registerName,
            string registar)
        {
            if (string.IsNullOrEmpty(configuration))
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrEmpty(registerName))
            {
                throw new ArgumentNullException(nameof(registerName));
            }

            if (string.IsNullOrEmpty(registar))
            {
                throw new ArgumentNullException(nameof(registar));
            }

            // Находим все записи в регистре, соответствующие регистратору
            var registerEntries = GetEntries(
                configuration,
                registerName,
                new[] { new FilterCriteria(RegisterConstants.RegistrarProperty, registar, CriteriaType.IsEquals) },
                0,
                1000);

            var earliestDocumentDate = DateTime.MaxValue;

            foreach (var registerEntry in registerEntries)
            {
                _documentApi.DeleteDocument(configuration, RegisterConstants.RegisterNamePrefix + registerName, registerEntry.Id);

                var documentDate = registerEntry[RegisterConstants.DocumentDateProperty];

                if (documentDate < earliestDocumentDate)
                {
                    earliestDocumentDate = documentDate;
                }
            }

            // Необходимо удалить все записи регистра после earliestDocumentDate
            var notActualRegisterEntries = GetEntries(
                configuration,
                registerName,
                new[] { new FilterCriteria(RegisterConstants.DocumentDateProperty, earliestDocumentDate, CriteriaType.IsMoreThanOrEquals) },
                0,
                1000);

            foreach (var registerEntry in notActualRegisterEntries)
            {
                _documentApi.DeleteDocument(configuration, RegisterConstants.RegisterNamePrefix + registerName, registerEntry.Id);
            }
        }

        /// <summary>
        /// Возвращает записи регистра.
        /// </summary>
        public IEnumerable<dynamic> GetEntries(
            string configuration,
            string registerName,
            IEnumerable<FilterCriteria> filter,
            int pageNumber,
            int pageSize)
        {
            return _documentApi.GetDocuments(
                configuration,
                RegisterConstants.RegisterNamePrefix + registerName,
                filter,
                pageNumber,
                pageSize);
        }

        /// <summary>
        /// Возвращает записи регистра.
        /// </summary>
        public IEnumerable<dynamic> GetValuesByDate(
            string configuration,
            string registerName,
            DateTime aggregationDate,
            IEnumerable<FilterCriteria> filter = null,
            IEnumerable<string> dimensionsProperties = null,
            IEnumerable<string> valueProperties = null,
            IEnumerable<AggregationType> aggregationTypes = null)
        {
            var registerObject = _metadataApi.GetRegister(configuration, registerName);

            if (registerObject == null)
            {
                throw new ArgumentException($"Register '{registerName} not found'");
            }

            // Сначала необходимо извлечь значения из регистра итогов
            var closestDate = GetClosestDateTimeOfTotalCalculation(configuration, registerName, aggregationDate);

            var filetrBuilder = new FilterBuilder();
            filetrBuilder.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsLessThanOrEquals(aggregationDate));

            IEnumerable<dynamic> aggregatedTotals = null;

            if (closestDate != null)
            {
                aggregatedTotals = _documentApi.GetDocument(
                    configuration,
                    RegisterConstants.RegisterTotalNamePrefix + registerName,
                    f => f.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsEquals(closestDate.Value)), 0, 1000);

                filetrBuilder.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsMoreThan(closestDate.Value));
            }

            IEnumerable<dynamic> dimensions = (dimensionsProperties == null)
                ? AggregationUtils.BuildDimensionsFromRegisterMetadata(registerObject)
                : AggregationUtils.BuildDimensionsFromProperties(dimensionsProperties);

            valueProperties = valueProperties ?? AggregationUtils.BuildValuePropertyFromRegisterMetadata(registerObject);

            var valuePropertiesCount = valueProperties.Count();

            var resultFilter = new List<FilterCriteria>();

            if (filter != null)
            {
                resultFilter.AddRange(filter);
            }

            resultFilter.AddRange(filetrBuilder.CriteriaList);

            if (aggregationTypes == null)
            {
                // По умолчанию считаем сумму значений
                aggregationTypes = AggregationUtils.BuildAggregationType(AggregationType.Sum, valuePropertiesCount);
            }

            var aggregationResult = GetAggregationDocumentResult(
                configuration,
                RegisterConstants.RegisterNamePrefix + registerName,
                resultFilter,
                dimensions,
                aggregationTypes,
                valueProperties);

            var dimensionNames = dimensions.Select(d => (string)d.FieldName).ToArray();

            // Выполняем обработку результата агрегации, чтобы представить полученные данные в табличном виде
            var denormalizedResult = AggregationUtils.ProcessBuckets(dimensionNames, valueProperties.ToArray(), aggregationResult);

            if (aggregatedTotals != null)
            {
                return AggregationUtils.MergeAggregaionResults(dimensionNames, valueProperties, denormalizedResult, aggregatedTotals);
            }

            return denormalizedResult;
        }

        /// <summary>
        /// Возвращает значения ресурсов в указанном диапазоне дат для регистра.
        /// </summary>
        public IEnumerable<dynamic> GetValuesBetweenDates(
            string configuration,
            string registerName,
            DateTime beginDate,
            DateTime endDate,
            IEnumerable<FilterCriteria> filter = null,
            IEnumerable<string> dimensionsProperties = null,
            IEnumerable<string> valueProperties = null,
            IEnumerable<AggregationType> aggregationTypes = null)
        {
            var registerObject = _metadataApi.GetRegister(configuration, registerName);

            if (registerObject == null)
            {
                throw new ArgumentException($"Register '{registerName} not found'");
            }

            IEnumerable<dynamic> dimensions = (dimensionsProperties == null)
                ? AggregationUtils.BuildDimensionsFromRegisterMetadata(registerObject)
                : AggregationUtils.BuildDimensionsFromProperties(dimensionsProperties);

            valueProperties = valueProperties ?? AggregationUtils.BuildValuePropertyFromRegisterMetadata(registerObject);

            var valuePropertiesCount = valueProperties.Count();

            var resultFilter = new List<FilterCriteria>();

            if (filter != null)
            {
                resultFilter.AddRange(filter);
            }

            resultFilter.AddRange(FilterBuilder.DateRangeCondition(RegisterConstants.DocumentDateProperty, beginDate, endDate));

            if (aggregationTypes == null)
            {
                // По умолчанию считаем сумму значений
                aggregationTypes = AggregationUtils.BuildAggregationType(AggregationType.Sum, valuePropertiesCount);
            }

            var aggregationResult = GetAggregationDocumentResult(
                configuration,
                RegisterConstants.RegisterNamePrefix + registerName,
                resultFilter,
                dimensions,
                aggregationTypes,
                valueProperties);

            var dimensionNames = dimensions.Select(d => (string)d.FieldName).ToArray();

            // Выполняем обработку результата агрегации, чтобы представить полученные данные в табличном виде
            return AggregationUtils.ProcessBuckets(dimensionNames, valueProperties.ToArray(), aggregationResult);
        }

        /// <summary>
        /// Возвращает значения ресурсов в указанном диапазоне дат c разбиением на периоды.
        /// </summary>
        public IEnumerable<dynamic> GetValuesByPeriods(
            string configuration,
            string registerName,
            DateTime beginDate,
            DateTime endDate,
            string interval,
            IEnumerable<FilterCriteria> filter,
            IEnumerable<string> dimensionsProperties = null,
            IEnumerable<string> valueProperties = null,
            string timezone = null)
        {
            var registerObject = _metadataApi.GetRegister(configuration, registerName);

            if (registerObject == null)
            {
                throw new ArgumentException($"Register '{registerName}' not found.");
            }

            // В качестве интервалов могут быть указаны следующие значения:
            // year, quarter, month, week, day, hour, minute, second

            if (!CheckInterval(interval))
            {
                throw new ArgumentException($"Specified interval '{interval}' is invalid. Supported intervals: year, quarter, month, week, day, hour, minute, second.", nameof(interval));
            }

            if (string.IsNullOrEmpty(timezone))
            {
                var hours = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalHours;
                timezone = hours > 0 ? "+" + hours.ToString("00") + ":00" : hours.ToString("00") + ":00";
            }

            if (!CheckTimezone(timezone))
            {
                throw new ArgumentException($"Specified timezone {timezone} is invalid. Valid timezone example: '+05:00'.", nameof(timezone));
            }

            var dimensions = new List<dynamic>
                             {
                                 new DynamicWrapper
                                 {
                                     ["Label"] = RegisterConstants.DocumentDateProperty + "_datehistogram",
                                     ["FieldName"] = RegisterConstants.DocumentDateProperty,
                                     ["DimensionType"] = DimensionType.DateHistogram,
                                     ["Interval"] = interval,
                                     ["TimeZone"] = timezone
                                 }
                             };

            dimensions.AddRange((dimensionsProperties == null)
                ? AggregationUtils.BuildDimensionsFromRegisterMetadata(registerObject)
                : AggregationUtils.BuildDimensionsFromProperties(dimensionsProperties));

            valueProperties = valueProperties ?? AggregationUtils.BuildValuePropertyFromRegisterMetadata(registerObject);

            var valuePropertiesCount = valueProperties.Count();

            var resultFilter = new List<FilterCriteria>();

            if (filter != null)
            {
                resultFilter.AddRange(filter);
            }

            resultFilter.AddRange(FilterBuilder.DateRangeCondition(RegisterConstants.DocumentDateProperty, beginDate, endDate));

            IEnumerable<dynamic> aggregationResult = GetAggregationDocumentResult(
                configuration,
                RegisterConstants.RegisterNamePrefix + registerName,
                resultFilter,
                dimensions,
                AggregationUtils.BuildAggregationType(AggregationType.Sum, valuePropertiesCount),
                valueProperties);

            var dimensionNames = dimensions.Select(d => (string)d.FieldName).ToArray();

            // Выполняем обработку результата агрегации, чтобы представить полученные данные в табличном виде
            return AggregationUtils.ProcessBuckets(dimensionNames, valueProperties.ToArray(), aggregationResult);
        }

        /// <summary>
        /// Получение значений ресурсов по документу-регистратору.
        /// </summary>
        public IEnumerable<dynamic> GetValuesByRegistrar(
            string configuration,
            string registerName,
            string registrar,
            IEnumerable<string> dimensionsProperties = null,
            IEnumerable<string> valueProperties = null)
        {
            var registerObject = _metadataApi.GetRegister(configuration, registerName);

            if (registerObject == null)
            {
                throw new ArgumentException($"Register '{registerName}' not found.");
            }

            IEnumerable<dynamic> dimensions = (dimensionsProperties == null)
                ? AggregationUtils.BuildDimensionsFromRegisterMetadata(registerObject)
                : AggregationUtils.BuildDimensionsFromProperties(dimensionsProperties);

            valueProperties = valueProperties ?? AggregationUtils.BuildValuePropertyFromRegisterMetadata(registerObject);

            var valuePropertiesCount = valueProperties.Count();

            var filetrBuilder = new FilterBuilder();
            filetrBuilder.AddCriteria(c => c.Property(RegisterConstants.RegistrarProperty).IsEquals(registrar));

            IEnumerable<dynamic> aggregationResult = GetAggregationDocumentResult(
                configuration,
                RegisterConstants.RegisterNamePrefix + registerName,
                filetrBuilder.CriteriaList,
                dimensions,
                AggregationUtils.BuildAggregationType(AggregationType.Sum, valuePropertiesCount),
                valueProperties.ToArray());

            var dimensionNames = dimensions.Select(d => (string)d.FieldName).ToArray();

            // Выполняем обработку результата агрегации, чтобы представить полученные данные в табличном виде
            return AggregationUtils.ProcessBuckets(dimensionNames, valueProperties.ToArray(), aggregationResult);
        }

        /// <summary>
        /// Получение значений ресурсов по типу документа-регистратора.
        /// </summary>
        public IEnumerable<dynamic> GetValuesByRegistrarType(
            string configuration,
            string registerName,
            string registrar,
            IEnumerable<string> dimensionsProperties = null,
            IEnumerable<string> valueProperties = null)
        {
            var registerObject = _metadataApi.GetRegister(configuration, registerName);

            if (registerObject == null)
            {
                throw new ArgumentException($"Register '{registerName}' not found.");
            }

            IEnumerable<dynamic> dimensions = (dimensionsProperties == null)
                ? AggregationUtils.BuildDimensionsFromRegisterMetadata(registerObject)
                : AggregationUtils.BuildDimensionsFromProperties(dimensionsProperties);

            valueProperties = valueProperties ?? AggregationUtils.BuildValuePropertyFromRegisterMetadata(registerObject);

            var valuePropertiesCount = valueProperties.Count();

            var filetrBuilder = new FilterBuilder();
            filetrBuilder.AddCriteria(c => c.Property(RegisterConstants.RegistrarTypeProperty).IsEquals(registrar));

            IEnumerable<dynamic> aggregationResult = GetAggregationDocumentResult(
                configuration,
                RegisterConstants.RegisterNamePrefix + registerName,
                filetrBuilder.CriteriaList,
                dimensions,
                AggregationUtils.BuildAggregationType(AggregationType.Sum, valuePropertiesCount),
                valueProperties.ToArray());

            var dimensionNames = dimensions.Select(d => (string)d.FieldName).ToArray();

            // Выполняем обработку результата агрегации, чтобы представить полученные данные в табличном виде
            return AggregationUtils.ProcessBuckets(dimensionNames, valueProperties.ToArray(), aggregationResult);
        }

        /// <summary>
        /// Получение значений из таблицы итогов на дату, ближайшую к заданной
        /// </summary>
        public IEnumerable<dynamic> GetTotals(
            string configuration,
            string registerName,
            DateTime aggregationDate)
        {
            var closestDate = GetClosestDateTimeOfTotalCalculation(configuration, registerName, aggregationDate);

            if (closestDate != null)
            {
                Action<FilterBuilder> action = f => f.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsEquals(aggregationDate));

                return _documentApi.GetDocument(configuration, RegisterConstants.RegisterTotalNamePrefix + registerName, action, 0, 1000);
            }

            return Enumerable.Empty<dynamic>();
        }

        /// <summary>
        /// Возвращает дату последнего подсчета итогов для регистра накоплений, ближайшей к заданной.
        /// </summary>
        public DateTime? GetClosestDateTimeOfTotalCalculation(
            string configuration,
            string registerName,
            DateTime aggregationDate)
        {
            // В таблице итогов нужно найти итог, ближайший к aggregationDate
            var dateToReturn = new DateTime();

            var min = long.MaxValue;
            var isDateFound = false;

            var page = 0;

            while (true)
            {
                // Постранично считываем данные и таблицы итогов и ищем итоги с датой, ближайшей к заданной
                Action<FilterBuilder> filter = f => f.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsLessThan(aggregationDate));

                var totals = _documentApi.GetDocument(configuration, RegisterConstants.RegisterTotalNamePrefix + registerName, filter, page++, 1000).ToArray();

                if (totals.Length == 0)
                {
                    break;
                }

                foreach (var docWithTotals in totals)
                {
                    if (docWithTotals.DocumentDate != null)
                    {
                        if (Math.Abs(aggregationDate.Ticks - docWithTotals.DocumentDate.Ticks) < min)
                        {
                            min = docWithTotals.DocumentDate.Ticks - aggregationDate.Ticks;
                            dateToReturn = docWithTotals.DocumentDate;
                            isDateFound = true;
                        }
                    }
                }
            }

            return isDateFound ? dateToReturn : (DateTime?)null;
        }

        private IEnumerable<AggregationResult> GetAggregationDocumentResult(
            string configuration,
            string registerName,
            IEnumerable<FilterCriteria> filter,
            IEnumerable<dynamic> dimensions,
            IEnumerable<AggregationType> aggregationTypes,
            IEnumerable<string> valueProperties)
        {
            var executor = _indexFactory.BuildAggregationProvider(configuration, registerName);

            var extractSearchModel = filter.ExtractSearchModel(_filterFactory);

            var result = executor.ExecuteAggregation(
                dimensions.ToArray(),
                aggregationTypes.ToArray(),
                valueProperties.ToArray(),
                extractSearchModel);

            return result;
        }

        private static bool CheckInterval(string interval)
        {
            var validIntervals = new[] { "year", "quarter", "month", "week", "day", "hour", "minute", "second" };

            return validIntervals.Contains(interval.ToLowerInvariant());
        }

        private static bool CheckTimezone(string timezone)
        {
            // Временная зона задаётся в виде "+05:00"
            return (Regex.IsMatch(timezone, "^[+-]?[0-9]*:00$"));
        }
    }
}