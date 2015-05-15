﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Properties;

namespace InfinniPlatform.ContextTypes
{
	public static class ContextTypeProvider
	{
		private static readonly Dictionary<Type, ContextTypeKind> ContextTypeKinds = new Dictionary<Type, ContextTypeKind>();

		private static readonly Dictionary<Type, string> ContextTypeNames = new Dictionary<Type, string>(); 

		static ContextTypeProvider()
		{
			ContextTypeKinds.Add(typeof(IApplyContext),ContextTypeKind.ApplyMove);
			ContextTypeKinds.Add(typeof(IApplyResultContext), ContextTypeKind.ApplyResult);
			ContextTypeKinds.Add(typeof(IFilterEventContext), ContextTypeKind.ApplyFilter);
			ContextTypeKinds.Add(typeof(ISearchContext), ContextTypeKind.SearchContext);
			ContextTypeKinds.Add(typeof(IUploadContext), ContextTypeKind.Upload);

			ContextTypeNames.Add(typeof(IApplyContext),Resources.DocumentMoveContextType);
			ContextTypeNames.Add(typeof(IApplyResultContext), Resources.DocumentMoveResultContext);
			ContextTypeNames.Add(typeof(IFilterEventContext), Resources.DocumentFilterEventContext);
			ContextTypeNames.Add(typeof(ISearchContext), Resources.DocumentSearchContext);
			ContextTypeNames.Add(typeof(IUploadContext), Resources.FileUploadContext);

		}


		public static ContextTypeKind GetContextTypeKind(this Type type)
		{
			ContextTypeKind result;
			if (!ContextTypeKinds.TryGetValue(type, out result))
			{
				result = ContextTypeKind.None;				
			}
			return result;
		}

		public static string GetContextTypeVisual(this Type contextType)
		{
			string result;
			if (!ContextTypeNames.TryGetValue(contextType, out result))
			{
				result = "Unknown context type";
			}
			return result;					
		}

		public static string GetContextTypeDisplayByKind(this ContextTypeKind contextTypeKind)
		{
			var contextType = ContextTypeKinds.Where(v => v.Value.Equals(contextTypeKind)).Select(v => v.Key).FirstOrDefault();
			if (contextType == null)
			{
				return "Unknown context type";
			}
			return ContextTypeNames[contextType];
		}
	}
}
