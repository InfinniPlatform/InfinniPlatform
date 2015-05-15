using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Registers;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfinniPlatform.MigrationsAndVerifications.Migrations
{
    /// <summary>
    /// Миграция позволяет рассчитать итоги для регистров накопления на текущую дату
    /// </summary>
    public sealed class CalculateTotalsForRegisters : IConfigurationMigration
    {
        /// <summary>
        /// Конфигурация, к которой применяется миграция
        /// </summary>
        private string _activeConfiguration;
        
        /// <summary>
        /// Текстовое описание миграции
        /// </summary>
        public string Description
        {
            get { return "Migration calculates totals for all configuration registers"; }
        }

        /// <summary>
        /// Идентификатор конфигурации, к которой применима миграция.
        /// В том случае, если идентификатор не указан (null or empty string), 
        /// миграция применима ко всем конфигурациям
        /// </summary>
        public string ConfigurationId
        {
            get { return ""; }
        }

        /// <summary>
        /// Версия конфигурации, к которой применима миграция.
        /// В том случае, если версия не указана (null or empty string), 
        /// миграция применима к любой версии конфигурации
        /// </summary>
        public string ConfigVersion
        {
            get { return ""; }
        }

        /// <summary>
        /// Признак того, что миграцию можно откатить
        /// </summary>
        public bool IsUndoable
        {
            get { return false; }
        }

        /// <summary>
        /// Выполнить миграцию
        /// </summary>
        /// <param name="message">Информативное сообщение с результатом выполнения действия</param>
        /// <param name="parameters"></param>
        public void Up(out string message, object[] parameters)
        {
            var resultMessage = new StringBuilder();

            if (IndexApi.IndexExists(_activeConfiguration, _activeConfiguration + RegisterConstants.RegistersCommonInfo))
            {
                var registersInfo = new DocumentApi().GetDocument(_activeConfiguration,
                    _activeConfiguration + RegisterConstants.RegistersCommonInfo, null, 0, 1000);

                foreach (var registerInfo in registersInfo)
                {
                    var registerId = registerInfo.Id;

                    var tempDate = DateTime.Now;
                    
                    var calculationDate = new DateTime(
                        tempDate.Year,
                        tempDate.Month,
                        tempDate.Day,
                        tempDate.Hour,
                        tempDate.Minute,
                        tempDate.Second);

                    var aggregatedData =
                        RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "GetRegisterValuesByDate", null,
                            new
                            {
                                Configuration = _activeConfiguration,
                                Register = registerId,
                                Date = calculationDate
                            }).ToDynamicList();

                    foreach (var item in aggregatedData)
                    {
                        item.Id = Guid.NewGuid().ToString();
                        item[RegisterConstants.DocumentDateProperty] = calculationDate;
                        new DocumentApi().SetDocument(_activeConfiguration,
                            RegisterConstants.RegisterTotalNamePrefix + registerId, item);
                    }
                }
            }
            else
            {
                resultMessage.AppendLine("Nothing to calculate (registers not found).");
            }

            resultMessage.AppendLine();
            resultMessage.AppendFormat("Migration completed.");
            
            message = resultMessage.ToString();
        }

        /// <summary>
        /// Отменить миграцию
        /// </summary>
        /// <param name="message">Информативное сообщение с результатом выполнения действия</param>
        /// <param name="parameters">Параметры миграции</param>
        public void Down(out string message, object[] parameters)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Устанавливает активную конфигурацию для миграции
        /// </summary>
        public void AssignActiveConfiguration(string configurationId, IGlobalContext context)
        {
            _activeConfiguration = configurationId;
        }

        /// <summary>
        /// Возвращает параметры миграции
        /// </summary>
        public IEnumerable<MigrationParameter> Parameters
        {
            get { return new MigrationParameter[0]; }
        }
    }
}
