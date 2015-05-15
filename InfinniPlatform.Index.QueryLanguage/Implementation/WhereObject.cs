using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.SearchOptions;

namespace InfinniPlatform.Index.QueryLanguage.Implementation
{
	public sealed class WhereObject
	{
		public string Property { get; set; }

		public string RawProperty { get; set; }

		public object Value { get; set; }

		public CriteriaType CriteriaType { get; set; }

		public Criteria ToCriteria()
		{
			return new Criteria()
				       {
					       CriteriaType = this.CriteriaType,
					       Value = this.Value,
					       Property = this.Property
				       };
		}
	}
}
