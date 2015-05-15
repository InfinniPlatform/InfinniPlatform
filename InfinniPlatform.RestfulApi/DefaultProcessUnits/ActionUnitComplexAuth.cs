﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Runtime;

namespace InfinniPlatform.RestfulApi.DefaultProcessUnits
{
	/// <summary>
	///   Сложная авторизация при обработке бизнес-процесса
	/// </summary>
	public sealed class ActionUnitComplexAuth
	{
		public void Action(IApplyContext target)
		{
			dynamic defaultBusinessProcess = null;

			//TODO Игнорировать системные конфигурации при валидации. Пока непонятно, как переделать
			if (target.Item.Configuration.ToLowerInvariant() != "systemconfig" &&
				target.Item.Configuration.ToLowerInvariant() != "update" &&
				target.Item.Configuration.ToLowerInvariant() != "restfulapi")
			{
				//ищем метаданные бизнес-процесса по умолчанию документа 

				defaultBusinessProcess = target.Context.GetComponent<IMetadataComponent>().GetMetadata(target.Item.Configuration, target.Item.Metadata, MetadataType.Process, "Default");
			}
			else
			{
				return;
			}

			if (defaultBusinessProcess != null && defaultBusinessProcess.Transitions[0].ComplexAuthorizationPoint != null)
			{
				var scriptArguments = new ApplyContext();
				scriptArguments.CopyPropertiesFrom(target);
				scriptArguments.Item = target.Item.Document;
				scriptArguments.Item.Configuration = target.Item.Configuration;
				scriptArguments.Item.Metadata = target.Item.Metadata;

				target.Context.GetComponent<IScriptRunnerComponent>().GetScriptRunner(target.Item.Configuration).InvokeScript(defaultBusinessProcess.Transitions[0].ComplexAuthorizationPoint.ScenarioId, scriptArguments);
			}
		}
	}
}
