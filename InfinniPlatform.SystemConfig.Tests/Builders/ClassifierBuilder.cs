using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniConfiguration.SystemConfig.Tests.Builders
{
	public static class ClassifierBuilder
	{
		public static object BuildSampleMetadataClassifier()
		{
			return new
			{
				Id = "2f7bc2f3-ff6d-45aa-a3fa-0f791cfd58df",
				Name = "1.2.643.5.1.13.2.7.1.55",
				Caption = "Аллергены",
				Description = "Аллергены",
				CodeSystem = "1.2.643.5.1.13.2.7.1.55",
				CodeSystemName = "Аллергены",
				CodeSystemVersion = "1.0",
				CodePropertyRef = "CODE",
				DisplayNamePropertyRef = "NAME",
				Status = "Published",
				Model = new
				{
					Name = "ClassifierModel",
					Caption = "Модель полей справочника",
					Properties = new[]
								                            {
									                            new
										                            {
											                            Type = "String",
											                            Name = "ID",
											                            Caption = "ID",
										                            },
									                            new
										                            {
											                            Type = "String",
											                            Name = "CODE",
											                            Caption = "Код",
										                            },
									                            new
										                            {
											                            Type = "String",
											                            Name = "NAME",
											                            Caption = "Наименование",
										                            },
									                            new
										                            {
											                            Type = "String",
											                            Name = "PARENT_ID",
											                            Caption = "Родитель",
										                            }
								                            }
				}

			};


		}
	}
}
