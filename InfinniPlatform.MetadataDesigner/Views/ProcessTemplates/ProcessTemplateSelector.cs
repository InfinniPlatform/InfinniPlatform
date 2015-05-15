using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InfinniPlatform.MetadataDesigner.Views.ProcessTemplates
{
	public sealed class ProcessTemplateSelector
	{
		private readonly Dictionary<string, Func<Control>> _templateDictionary = new Dictionary<string, Func<Control>>(); 

		public ProcessTemplateSelector()
		{
			TemplateDictionary.Add("Default",() => new DefaultProcessTemplate());
			TemplateDictionary.Add("Custom", () => new CustomProcessTemplate());
		}


		public Dictionary<string, Func<Control>> TemplateDictionary
		{
			get { return _templateDictionary; }
		}

		public Control GetTemplate(string templateName)
		{
			if (_templateDictionary.ContainsKey(templateName))
			{
				return _templateDictionary[templateName].Invoke();
			}
			return null;
		}

		public IEnumerable<string> GetTemplateList()
		{
			return _templateDictionary.Select(r => r.Key).ToList();
		}

		public string GetDefaultTemplate()
		{
			return "Custom";
		}
	}
}
