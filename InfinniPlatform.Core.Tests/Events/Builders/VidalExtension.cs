using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Tests.Events.Builders.Models;

namespace InfinniPlatform.Core.Tests.Events.Builders
{
	public static class VidalExtension
	{
		private static readonly Dictionary<string,ProductUsingType> _productUsingTypes = new Dictionary<string, ProductUsingType>() {
			{"Qwes",ProductUsingType.Unknown},
			{"Care",ProductUsingType.UseWithCare},
			{"Can",ProductUsingType.CanUse},
			{"Not",ProductUsingType.NotAllowed}
		};

		private static readonly Dictionary<DrugCategoryUsing, string> _drugCategories = new Dictionary<DrugCategoryUsing, string>() {
			{DrugCategoryUsing.ChildUsing, "для детей,детям,детский,детское,детские"},
			{DrugCategoryUsing.ElderlyUsing, "пожилых,пожилым,пожилой,пожилые,пенсионер,старик"},
			{DrugCategoryUsing.HepatoUsing, "печень,печенью,печени"},
			{DrugCategoryUsing.NursingUsing, "лактация,кормление,грудью,вскармливание"},
			{DrugCategoryUsing.PregnancyUsing, "беременности,беременным,беременные"},
			{DrugCategoryUsing.RenalUsing, "почки,почечным,почечная"}
		};


        public static bool IsNullOrEmpty(dynamic value)
        {
            return DBNull.Value.Equals(value) || string.IsNullOrEmpty(value.ToString());
        }


	}
}