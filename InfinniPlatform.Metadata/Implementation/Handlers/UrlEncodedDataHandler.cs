﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Hosting;

namespace InfinniPlatform.Metadata.Implementation.Handlers
{
	public sealed class UrlEncodedDataHandler : IWebRoutingHandler
	{
		private readonly IGlobalContext _globalContext;

		public IConfigRequestProvider ConfigRequestProvider { get; set; }

		public UrlEncodedDataHandler(IGlobalContext globalContext)
		{
			_globalContext = globalContext;
		}

		public dynamic Process(dynamic parameters)
		{
			string config = ConfigRequestProvider.GetConfiguration();
			string metadata = ConfigRequestProvider.GetMetadataIdentifier();
			
			var target = new UrlEncodedDataContext()
			{
				IsValid = true,
				FormData = parameters,
				Configuration = config,
				Metadata = metadata,
				Context = _globalContext
			};

            var metadataConfig = _globalContext.GetComponent<IMetadataConfigurationProvider>(ConfigRequestProvider.GetVersion()).GetMetadataConfiguration(ConfigRequestProvider.GetVersion(), ConfigRequestProvider.GetConfiguration());

			metadataConfig.MoveWorkflow(metadata, metadataConfig.GetExtensionPointValue(ConfigRequestProvider, "ProcessUrlEncodedData"), target);

			if (target.IsValid)
			{
				dynamic item = new DynamicWrapper();
				item.IsValid = true;
				item.ValidationMessage = Resources.UrlEncodedDataProcessingComplete;


				dynamic result = new DynamicWrapper();
				if (target.Result != null)
				{
					result.Data = target.Result.Data;
					result.Info = target.Result.Info;
				}
				else
				{
					item.ValidationMessage = "Content not found.";
					item.IsValid = false;
				}

				item.Result = result;
				return AggregateExtensions.PrepareResultAggregate(item);
			}
			return AggregateExtensions.PrepareInvalidResultAggregate(target);
		}
	}
}
