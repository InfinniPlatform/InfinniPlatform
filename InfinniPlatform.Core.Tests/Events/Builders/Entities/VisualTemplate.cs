using System.Collections.Generic;

namespace InfinniPlatform.Core.Tests.Events.Builders.Entities
{
	public class VisualTemplate
	{

		public string Template { get; set; }

		public IEnumerable<TemplateParam> TemplateParams { get; set; } 
	}
}