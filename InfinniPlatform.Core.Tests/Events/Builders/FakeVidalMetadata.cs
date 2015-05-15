using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Core.Tests.Events.Builders.Entities;
using InfinniPlatform.Core.Tests.Events.Builders.Extensibility;
using InfinniPlatform.Core.Tests.Events.Builders.Extensibility.Actions;
using InfinniPlatform.Core.Tests.Events.Builders.Extensibility.ChoicesMetadata;
using InfinniPlatform.Core.Tests.Events.Builders.Extensibility.FormsMetadata;
using InfinniPlatform.Core.Tests.Events.Builders.Extensibility.RestMetadata;
using InfinniPlatform.Core.Tests.Events.Builders.Models;

namespace InfinniPlatform.Core.Tests.Events.Builders
{
	public static class FakeVidalMetadata
	{
		public static IEnumerable<FieldMetadataRecord> GetClPhPointerMetadata()
		{
			#region
			var parentMetadata = new ObjectMetadataRecord()
			{
				Id = 111
			};

			var fieldCode = new FieldRecord()
			{
				Id = 10111,
				Value = "REF_CLPHPOINTER",
				FieldMetadata = new FieldMetadataRecord()
				{
					Id = 100291,
					IsEditable = false,
					MetadataDataType = new MetadataDataType()
					{
						MetadataIdentifier = "string",
						MetadataTypeKind = "SimpleType"
					},

					MetadataId = "OBJECT_CODE",
					MetadataName = "Идентификатор объекта",
					Parent = parentMetadata
				}
			};
			var fieldName = new FieldRecord()
			{
				Id = 10112,
				Value = "Клинико-фармакологический указатель",
				FieldMetadata = new FieldMetadataRecord()
				{
					Id = 10030,
					IsEditable = false,
					MetadataDataType = new MetadataDataType()
					{
						MetadataIdentifier = "string",
						MetadataTypeKind = "SimpleType"
					},

					MetadataId = "OBJECT_NAME",
					MetadataName = "Наименование объекта",
					Parent = parentMetadata
				}
			};
			parentMetadata.FieldCode = fieldCode;
			parentMetadata.FieldName = fieldName;

			var fieldsMetadata = new List<FieldMetadataRecord>();
			var id = new FieldMetadataRecord()
			{
				Id = 500,
				IsIdentifier = true,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "ID",
				MetadataName = "Идентификатор пункта КФУ",
				Parent = parentMetadata,
				DataFieldName = "Id"
			};
			var name = new FieldMetadataRecord()
			{
				Id = 501,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "Name",
				MetadataName = "Наименование пункта КФУ",
				Parent = parentMetadata,
				DataFieldName = "Name"
			};
			var code = new FieldMetadataRecord()
			{
				Id = 501,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "Code",
				MetadataName = "Код КФУ",
				Parent = parentMetadata,
				DataFieldName = "Code",
				IdTree = true
			};
			var codeParent = new FieldMetadataRecord()
			{
				Id = 501,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "ParentCode",
				MetadataName = "Код родительского элемента КФУ",
				Parent = parentMetadata,
				DataFieldName = "ParentCode",
				IdTreeParent = true
			};
			fieldsMetadata.Add(id);
			fieldsMetadata.Add(name);
			fieldsMetadata.Add(code);
			fieldsMetadata.Add(codeParent);



			return fieldsMetadata;
			#endregion
		}

		public static IEnumerable<FieldMetadataRecord> GetClPhGroupMetadata()
		{
			#region
			var parentMetadata = new ObjectMetadataRecord()
			{
				Id = 111
			};

			var fieldCode = new FieldRecord()
			{
				Id = 10111,
				Value = "REF_CLPHGROUP",
				FieldMetadata = new FieldMetadataRecord()
				{
					Id = 100291,
					IsEditable = false,
					MetadataDataType = new MetadataDataType()
					{
						MetadataIdentifier = "string",
						MetadataTypeKind = "SimpleType"
					},

					MetadataId = "OBJECT_CODE",
					MetadataName = "Идентификатор объекта",
					Parent = parentMetadata
				}
			};
			var fieldName = new FieldRecord()
			{
				Id = 10112,
				Value = "Клинико-фармакологическая группа",
				FieldMetadata = new FieldMetadataRecord()
				{
					Id = 10030,
					IsEditable = false,
					MetadataDataType = new MetadataDataType()
					{
						MetadataIdentifier = "string",
						MetadataTypeKind = "SimpleType"
					},

					MetadataId = "OBJECT_NAME",
					MetadataName = "Наименование объекта",
					Parent = parentMetadata
				}
			};
			parentMetadata.FieldCode = fieldCode;
			parentMetadata.FieldName = fieldName;

			var fieldsMetadata = new List<FieldMetadataRecord>();
			var id = new FieldMetadataRecord()
			{
				Id = 500,
				IsIdentifier = true,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "ID",
				MetadataName = "Идентификатор группы",
				Parent = parentMetadata,
				DataFieldName = "Id"
			};
			var name = new FieldMetadataRecord()
			{
				Id = 501,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "Name",
				MetadataName = "Наименование клинико-фармакологической группы",
				Parent = parentMetadata,
				DataFieldName = "Name"
			};
			var code = new FieldMetadataRecord()
			{
				Id = 501,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "Code",
				MetadataName = "Код клинико-фармакологической группы",
				Parent = parentMetadata,
				DataFieldName = "Code"
			};
			fieldsMetadata.Add(id);
			fieldsMetadata.Add(name);
			fieldsMetadata.Add(code);


			return fieldsMetadata;
			#endregion
		}

		public static IEnumerable<FieldMetadataRecord> GetPhThGroupMetadata()
		{
			#region
			var parentMetadata = new ObjectMetadataRecord()
			{
				Id = 11
			};

			var fieldCode = new FieldRecord()
			{
				Id = 1011,
				Value = "REF_PHTHGROUP",
				FieldMetadata = new FieldMetadataRecord()
				{
					Id = 10029,
					IsEditable = false,
					MetadataDataType = new MetadataDataType()
					{
						MetadataIdentifier = "string",
						MetadataTypeKind = "SimpleType"
					},

					MetadataId = "OBJECT_CODE",
					MetadataName = "Идентификатор объекта",
					Parent = parentMetadata
				}
			};
			var fieldName = new FieldRecord()
			{
				Id = 1011,
				Value = "Фармако-терапевтическая группа",
				FieldMetadata = new FieldMetadataRecord()
				{
					Id = 10030,
					IsEditable = false,
					MetadataDataType = new MetadataDataType()
					{
						MetadataIdentifier = "string",
						MetadataTypeKind = "SimpleType"
					},

					MetadataId = "OBJECT_NAME",
					MetadataName = "Наименование объекта",
					Parent = parentMetadata
				}
			};
			parentMetadata.FieldCode = fieldCode;
			parentMetadata.FieldName = fieldName;

			var fieldsMetadata = new List<FieldMetadataRecord>();
			var id = new FieldMetadataRecord()
			{
				Id = 500,
				IsIdentifier = true,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "ID",
				MetadataName = "Идентификатор группы",
				Parent = parentMetadata,
				DataFieldName = "Id"
			};
			var name = new FieldMetadataRecord()
			{
				Id = 501,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},
				MetadataId = "Name",
				MetadataName = "Наименование фармако-терапевтической группы",
				Parent = parentMetadata,
				DataFieldName = "Name"
			};
			fieldsMetadata.Add(id);
			fieldsMetadata.Add(name);

			return fieldsMetadata;
			#endregion
		}

		public static IEnumerable<FieldMetadataRecord> GetAThClassifyMetadata()
		{
			#region metadata
			var parentMetadata = new ObjectMetadataRecord()
			{
				Id = 12
			};

			var fieldCode = new FieldRecord()
			{
				Id = 1015,
				Value = "REF_ATHCLASSIFY",
				FieldMetadata = new FieldMetadataRecord()
				{
					Id = 10039,
					IsEditable = false,
					MetadataDataType = new MetadataDataType()
					{
						MetadataIdentifier = "string",
						MetadataTypeKind = "SimpleType"
					},

					MetadataId = "OBJECT_CODE",
					MetadataName = "Идентификатор объекта",
					Parent = parentMetadata
				}
			};
			var fieldName = new FieldRecord()
			{
				Id = 1015,
				Value = "Анатомо-терапевтическая группа",
				FieldMetadata = new FieldMetadataRecord()
				{
					Id = 10040,
					IsEditable = false,
					MetadataDataType = new MetadataDataType()
					{
						MetadataIdentifier = "string",
						MetadataTypeKind = "SimpleType"
					},

					MetadataId = "OBJECT_NAME",
					MetadataName = "Наименование объекта",
					Parent = parentMetadata
				}
			};
			parentMetadata.FieldCode = fieldCode;
			parentMetadata.FieldName = fieldName;

			var fieldsMetadata = new List<FieldMetadataRecord>();
			var id = new FieldMetadataRecord()
			{
				Id = 5000,
				IsIdentifier = true,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "ATCCode",
				MetadataName = "Код группы",
				Parent = parentMetadata,
				DataFieldName = "Id"
			};
			var rusName = new FieldMetadataRecord()
			{
				Id = 5002,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "RusName",
				MetadataName = "Наименование анатомо-терапевтической группы (RUS)",
				Parent = parentMetadata,
				DataFieldName = "RusName"
			};
			var engName = new FieldMetadataRecord()
			{
				Id = 5003,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "EngName",
				MetadataName = "Наименование анатомо-терапевтической группы (ENG)",
				Parent = parentMetadata,
				DataFieldName = "EngName"
			};
			var parentAtcCode = new FieldMetadataRecord()
			{
				Id = 5004,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "ParentATCCode",
				MetadataName = "Родительская группа",
				Parent = parentMetadata,
				DataFieldName = "ParentATCCode"
			};


			fieldsMetadata.Add(id);
			fieldsMetadata.Add(rusName);
			fieldsMetadata.Add(engName);
			fieldsMetadata.Add(parentAtcCode);


			return fieldsMetadata;
			#endregion
		}

		public static IEnumerable<FieldMetadataRecord> GetActiveIngredientMetadata()
		{
			#region metadata
			var parentMetadata = new ObjectMetadataRecord()
			{
				Id = 120
			};

			var fieldCode = new FieldRecord()
			{
				Id = 1055,
				Value = "REF_ACTIVEINGREDIENT",
				FieldMetadata = new FieldMetadataRecord()
				{
					Id = 10089,
					IsEditable = false,
					MetadataDataType = new MetadataDataType()
					{
						MetadataIdentifier = "string",
						MetadataTypeKind = "SimpleType"
					},

					MetadataId = "OBJECT_CODE",
					MetadataName = "Идентификатор объекта",
					Parent = parentMetadata
				}
			};
			var fieldName = new FieldRecord()
			{
				Id = 1015,
				Value = "Действующее вещество",
				FieldMetadata = new FieldMetadataRecord()
				{
					Id = 10040,
					IsEditable = false,
					MetadataDataType = new MetadataDataType()
					{
						MetadataIdentifier = "string",
						MetadataTypeKind = "SimpleType"
					},

					MetadataId = "OBJECT_NAME",
					MetadataName = "Наименование объекта",
					Parent = parentMetadata
				}
			};
			parentMetadata.FieldCode = fieldCode;
			parentMetadata.FieldName = fieldName;

			var fieldsMetadata = new List<FieldMetadataRecord>();
			var id = new FieldMetadataRecord()
			{
				Id = 5500,
				IsIdentifier = true,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "MoleculeId",
				MetadataName = "Идентификатор молекулы",
				Parent = parentMetadata,
				DataFieldName = "MoleculeId"
			};
			var rusName = new FieldMetadataRecord()
			{
				Id = 5502,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "MoleculeRusName",
				MetadataName = "Наименование действующего вещества",
				Parent = parentMetadata,
				DataFieldName = "MoleculeRusName"
			};
			var engName = new FieldMetadataRecord()
			{
				Id = 5503,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "MoleculeEngName",
				MetadataName = "Иностранное наименование действующего вещества",
				Parent = parentMetadata,
				DataFieldName = "MoleculeEngName"
			};

			fieldsMetadata.Add(id);
			fieldsMetadata.Add(rusName);
			fieldsMetadata.Add(engName);

			return fieldsMetadata;
			#endregion
		}

		public static IEnumerable<FieldMetadataRecord> GetDrugFormMetadata()
		{
			#region metadata
			var parentMetadata = new ObjectMetadataRecord()
			{
				Id = 320
			};

			var fieldCode = new FieldRecord()
			{
				Id = 1095,
				Value = "REF_DRUGFORM",
				FieldMetadata = new FieldMetadataRecord()
				{
					Id = 10189,
					IsEditable = false,
					MetadataDataType = new MetadataDataType()
					{
						MetadataIdentifier = "string",
						MetadataTypeKind = "SimpleType"
					},

					MetadataId = "OBJECT_CODE",
					MetadataName = "Идентификатор объекта",
					Parent = parentMetadata
				}
			};
			var fieldName = new FieldRecord()
			{
				Id = 1115,
				Value = "Лекарственная форма",
				FieldMetadata = new FieldMetadataRecord()
				{
					Id = 10140,
					IsEditable = false,
					MetadataDataType = new MetadataDataType()
					{
						MetadataIdentifier = "string",
						MetadataTypeKind = "SimpleType"
					},

					MetadataId = "OBJECT_NAME",
					MetadataName = "Наименование объекта",
					Parent = parentMetadata
				}
			};
			parentMetadata.FieldCode = fieldCode;
			parentMetadata.FieldName = fieldName;

			var fieldsMetadata = new List<FieldMetadataRecord>();
			var id = new FieldMetadataRecord()
			{
				Id = 5510,
				IsIdentifier = true,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "Id",
				MetadataName = "Идентификатор формы выпуска",
				Parent = parentMetadata,
				DataFieldName = "Id"
			};
			//var shortName = new FieldMetadataRecord()
			//{
			//	Id = 5512,
			//	IsEditable = false,
			//	MetadataDataType = new MetadataDataType()
			//	{
			//		MetadataIdentifier = "string",
			//		MetadataTypeKind = "SimpleType"
			//	},

			//	MetadataId = "FormShortName",
			//	MetadataName = "Краткое наименование лекарственной формы",
			//	Parent = parentMetadata,
			//	DataFieldName = "FormShortName"
			//};
			var rusName = new FieldMetadataRecord()
			{
				Id = 5513,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "FormRusName",
				MetadataName = "Наименование лекарственной формы",
				Parent = parentMetadata,
				DataFieldName = "FormRusName"
			};
			var engName = new FieldMetadataRecord()
			{
				Id = 5514,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "FormEngName",
				MetadataName = "Наименование лекарственной формы",
				Parent = parentMetadata,
				DataFieldName = "FormEngName"
			};
			var descriptionForm = new FieldMetadataRecord()
			{
				Id = 5515,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "DescriptionForm",
				MetadataName = "Описание",
				Parent = parentMetadata,
				DataFieldName = "DescriptionForm"
			};

			fieldsMetadata.Add(id);
			//fieldsMetadata.Add(shortName);
			fieldsMetadata.Add(rusName);
			fieldsMetadata.Add(engName);
			fieldsMetadata.Add(descriptionForm);

			return fieldsMetadata;
			#endregion
		}


		public static FormMetadata GetVidalRefFormOneRecordMetadata()
		{
			var mainDataProvider = new RestContainer(MetadataIdentifiers.ContainerTypeData, "REF_VIDAL_ONERECORD", "GET", "DrugsVidal",
													 new List<string>() { "string id" });
			var dataProviderList = new List<RestContainer>() {
				mainDataProvider
			};

			var controlsMetadata = new List<object>();
			controlsMetadata.Add(new ControlMetadata()
			{
				ControlIdentifier = "TabControl1",
				ControlType = "TabControl",
				Metadata = new
				{
					Orientation = "Horizontal",
					Alignment = "Left",
					Style = "Page"
				},
				InnerControls = new List<ControlMetadata>() {
					new ControlMetadata() {
						ControlIdentifier = "TabPageDrugDescription",
						ControlType = "TabPage",
						Metadata = new {
							Caption = "Описание лекарственного средства"
						},
						
						InnerControls = new List<ControlMetadata>() {
							#region innercontrols
							new ControlMetadata() {
								ControlIdentifier = "TabControlSections",
								ControlType = "TabControl",
								Metadata = new {
									Orientation = "Vertical",
									Alignment = "Left",	
									Style = "Section"
								},
								InnerControls = new List<ControlMetadata>() {
									new ControlMetadata() {
										ControlIdentifier = "TabPageSectionSpecialPointers",
										ControlType = "TabPage",
										Metadata = new {
											Caption = "Специальные указатели"
										},											
										InnerControls = new List<ControlMetadata>() {
											#region Специальные указатели
										new ControlMetadata() {
											ControlIdentifier = "GroupBoxAThClassify",
											ControlType = "GroupBox",
											Metadata = new {
												Caption = "Анатомо-терапевтическая химическая классификация",
											},
											#region innerControls
											InnerControls = new List<ControlMetadata>() {
												new ControlMetadata() {
													ControlIdentifier = "TextAThCode",
													ControlType = "Text",
													Metadata = new {
														Caption = "Код",
														DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId == "AThClassify"),
														FieldName = "Id"
													}
												},
												new ControlMetadata() {
													ControlIdentifier = "TextRusName",
													ControlType = "Text",
													Metadata = new {
														Caption = "Наименование",
														DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId =="AThClassify"),
														FieldName = "RusName"
													}
												}

											}
											#endregion
										},
										new ControlMetadata() {
											ControlIdentifier = "GroupBoxPhThGroup",
											ControlType = "GroupBox",
											Metadata = new {
												Caption = "Клинико-фармакологическая группа"
											},
											#region innerControls
											InnerControls = new List<ControlMetadata>() {
												new ControlMetadata() {
													ControlIdentifier = "TextPhThName",
													ControlType = "Text",
													Metadata = new {
														Caption = "Наименование",
														DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId == "FTH_CLASSIFY"),
														FieldName = "Name"
													}
												}
											}
										#endregion
										},
										new ControlMetadata() {
											ControlIdentifier = "GroupBoxClPhPointer",
											ControlType = "GroupBox",
											Metadata = new {
												Caption = "Клинико-фармакологический указатель"
											},
											#region innerControls
											InnerControls = new List<ControlMetadata>() {
												new ControlMetadata() {
													ControlIdentifier = "ClPhPointerCode",
													ControlType = "Text",
													Metadata = new {
														Caption = "Код",
														DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId == "ClPhPointer"),
														FieldName= "Code"
													}
												},
												new ControlMetadata() {
													ControlIdentifier = "ClPhPointerName",
													ControlType = "Text",
													Metadata = new {
														Caption = "Наименование",
														DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId == "ClPhPointer"),
														FieldName = "Name"
													}
												}
											}
										#endregion
										},
										new ControlMetadata() {
											ControlIdentifier = "GroupBoxClPhGroup",
											ControlType = "GroupBox",
											Metadata = new {
												Caption = "Клинико-фармакологическая группа"
											},
											#region innerControls
											InnerControls = new List<ControlMetadata>() {
												new ControlMetadata() {
													ControlIdentifier = "ClPhGroupCode",
													ControlType = "Text",
													Metadata = new {
														Caption = "Код группы",
														DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId == "CLPHGROUP"),
														FieldName = "Code"
													}
												},
												new ControlMetadata() {
													ControlIdentifier = "ClPhGroupName",
													ControlType = "Text",
													Metadata = new {
														Caption = "Наименование группы",
														DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId == "CLPHGROUP"),
														FieldName = "Name"
													}
												}
												#endregion
											}
											
										},
										new ControlMetadata() {
											ControlIdentifier = "GroupBoxNozology",
											ControlType = "GroupBox",
											Metadata = new {
												Caption = "Классификация по МКБ-10"
											},
											#region innerControls
											InnerControls = new List<ControlMetadata>() {
												new ControlMetadata() {
													ControlIdentifier = "NozologyCode",
													ControlType = "Text",
													Metadata = new {
														Caption = "Код группы",
														DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "NOZOLOGYCODE"),
														FieldName = "NozologyCode"
													}
												},
												new ControlMetadata() {
													ControlIdentifier = "NozologyName",
													ControlType = "Text",
													Metadata = new {
														Caption = "Наименование группы",
														DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "NOZOLOGYNAME"),
														FieldName = "NozologyName"
													}
												}
											}
											#endregion
										}
									}
									#endregion		
									},
									new ControlMetadata() {
										ControlIdentifier = "TabPageSectionIngredients",
										ControlType = "TabPage",
										Metadata = new {
											Caption = "Формы выпуска и действующие вещества"
										},
										InnerControls = new List<ControlMetadata>() {
											#region Формы выпуска и действующие вещества
											new ControlMetadata() {
												  ControlIdentifier = "GroupBoxDrugForm",
												  ControlType = "GroupBox",
												  Metadata = new {
													  Caption = "Лекарственная форма"
												  },
												  #region innerControls
												  InnerControls = new List<ControlMetadata>() {
													new ControlMetadata() {
														ControlIdentifier = "FormShortName",
														ControlType = "Text",
														Metadata = new {
															Caption = "Краткое наименование лекарственной формы",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "DRUG_FORM"),
															FieldName = "FormShortName"
														},
													},
													new ControlMetadata() {
														ControlIdentifier = "FormRusName",
														ControlType = "Text",
														Metadata = new {
															Caption = "Наименование лекарственной формы на русском языке",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "DRUG_FORM"),
															FieldName = "FormRusName"
														},
													},
													new ControlMetadata() {
														ControlIdentifier = "FormEngName",
														ControlType = "Text",
														Metadata = new {
															Caption = "Иностранное наименование лекарственной формы",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "DRUG_FORM"),
															FieldName = "FormEngName"
														},
													},
												}
				#endregion
											},
											new ControlMetadata() {
												 ControlIdentifier = "GroupBoxActiveIngredient",
												 ControlType = "GroupBox",
							 					 Metadata = new {
													  Caption = "Действующее вещество"
												 },
												#region innerControls
												InnerControls = new List<ControlMetadata>() {
													new ControlMetadata() {
														ControlIdentifier = "GridActiveIngredient",
														ControlType = "Grid",
														Metadata = new {
															Caption = "Список действующих веществ",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "ACTIVE_INGREDIENT"),
															GridColumnsMetadata = new List<GridColumnMetadata>() {
																new GridColumnMetadata()
																{																	
																	FieldName = "MoleculeRusName",
																	Caption = "Наименование",
																	ColumnName = "ColumnMoleculeRusName",
																	EditorType = "Text",
																	IsEditable = false,
																	IsVisible = true,
																	Position = 0,
																	Width = 45
																},
																new GridColumnMetadata()
																{																	
																	Caption = "Иностранное наименование",
																	ColumnName = "ColumnMoleculeEngName",
																	FieldName = "MoleculeEngName",
																	EditorType = "Text",
																	IsEditable = false,
																	IsVisible = true,
																	Position = 1,
																	Width = 250
																},
															}
														}
													},
	
													new ControlMetadata() {
														//ControlIdentifier = "ActiveIngridientList",
														//ControlType = "List",
														//Metadata = new
														//	{
														//		DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "PRODUCTINFOEXTENDED"),
														//		Caption = "Содержание действующих веществ",
														//		FieldName = "ActiveIngredientsInfo",
														//		GridColumnsMetadata = new List<object>() {
														//		new
														//		{	
														//			FieldName = "DescriptionItem",
														//			Caption = "Описание действующего вещества",
														//			ColumnName = "ColumnDescriptionItem",																	
														//			EditorType = "Text",
														//			IsEditable = false,
														//			IsVisible = true,
														//			Position = 0,
														//			Width = 250
														//		},
														//		new
														//		{																	
														//			FieldName = "ActiveIngredient",
														//			Caption = "Наименование",
														//			ColumnName = "ColumnActiveIngredient",
														//			EditorType = "Text",
														//			IsEditable = false,
														//			IsVisible = true,
														//			Position = 1,
														//			Width = 45
														//		}
														//	}
														//}
														ControlIdentifier = "GridCompositionActiveIngredient",
														ControlType = "Grid",														
														Metadata = new {
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "PRODUCTINFOEXTENDED"),
															FieldName = "ActiveIngredientsInfo",
															Caption = "Содержание действующих веществ",
															GridColumnsMetadata = new List<GridColumnMetadata>() {
																new GridColumnMetadata()
																{
																	
																	Caption = "Описание действующего вещества",
																	ColumnName = "ColumnActiveIngredientDescriptionItem",
																	FieldName = "DescriptionItem",
																	EditorType = "Text",
																	IsEditable = false,
																	IsVisible = true,
																	Position = 1,
																	Width = 250
																},
																new GridColumnMetadata()
																{
																	Caption = "Содержание действующего вещества",
																	ColumnName = "ColumnActiveIngredientComposition",
																	FieldName = "ActiveIngredient",
																	EditorType = "Text",
																	IsEditable = false,
																	IsVisible = true,
																	Position = 1,
																	Width = 250
																},
															}
														}
													}	

												}
												#endregion
											},
											new ControlMetadata() {
												ControlIdentifier = "GroupBoxInactiveIngredient",
												ControlType = "GroupBox",
												Metadata = new {
													Caption = "Вспомогательное вещество"
												},
												#region innercontrols
												InnerControls = new List<ControlMetadata>() {
													new ControlMetadata() {
														ControlIdentifier = "GridInactiveIngredient",
														ControlType = "Grid",
														LayoutName = "MainDataGridLayout",														
														Metadata = new {
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "INACTIVE_INGREDIENT"),
															GridColumnsMetadata = new List<GridColumnMetadata>() {
																new GridColumnMetadata()
																{
																	
																	FieldName = "RusName",
																	Caption = "Наименование",
																	ColumnName = "ColumnInactiveRusName",
																	EditorType = "Text",
																	IsEditable = false,
																	IsVisible = true,
																	Position = 0,
																	Width = 45
																},
																new GridColumnMetadata()
																{																	
																	Caption = "Иностранное наименование",
																	ColumnName = "ColumnInactiveEngName",
																	FieldName = "EngName",
																	EditorType = "Text",
																	IsEditable = false,
																	IsVisible = true,
																	Position = 1,
																	Width = 250
																},
																new GridColumnMetadata()
																{
																	Caption = "Описание",
																	ColumnName = "ColumnInactiveIngredientDescription",
																	FieldName = "Description",
																	EditorType = "Text",
																	IsEditable = false,
																	IsVisible = true,
																	Position = 1,
																	Width = 250
																},
															}
														}
													}	
												}
												#endregion
											},
											#endregion				
										}
									},
									new ControlMetadata() {
										ControlIdentifier = "TabPageSectionPharmacology",
										ControlType = "TabPage",
										Metadata = new {
											Caption = "Фармакологическое действие"
										},
										InnerControls = new List<ControlMetadata>() {
											#region Фармакологическое действие
							
											new ControlMetadata() {
												ControlIdentifier = "GroupBoxPharmacology",
												ControlType = "GroupBox",
												Metadata = new {
													Caption = "Фармакология"
												},
												#region innercontrols
												InnerControls = new List<ControlMetadata>() {
													new ControlMetadata() {
														ControlIdentifier = "PhInfluence",
														ControlType = "Text",
														Metadata = new {
															Caption = "Фармакологическое действие",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "PRODUCTINFOEXTENDED"),
															FieldName = "PhInfluence"
														}
													},
													new ControlMetadata() {
														ControlIdentifier = "PhKinetiks",
														ControlType = "Text",
														Metadata = new {
															Caption = "Фармакокинетика",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "PRODUCTINFOEXTENDED"),
															FieldName = "PhKinetiks"
														}
													},

												#endregion
												}
											},
											#endregion
										}
									},
									new ControlMetadata() {
										ControlIdentifier = "TabPageSectionIndication",
										ControlType = "TabPage",
										Metadata = new {
											Caption = "Показания и противопоказания"
										},
										InnerControls = new List<ControlMetadata>() {
											#region Показания и противопоказания
											new ControlMetadata() {
												ControlIdentifier = "GroupBoxIndication",
												ControlType = "GroupBox",
												Metadata = new {
													Caption = "Показания"
												},
												#region innercontrols
												InnerControls = new List<ControlMetadata>() {
													new ControlMetadata() {
														ControlIdentifier = "Indication",
														ControlType = "Text",
														Metadata = new {
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "PRODUCTINFOEXTENDED"),
															FieldName = "Indication"
														}
													},
												#endregion
												}
											},
											new ControlMetadata() {
												ControlIdentifier = "GroupBoxContraIndication",
												ControlType = "GroupBox",
												Metadata = new {
													Caption = "Противопоказания"
												},
												#region innercontrols
												InnerControls = new List<ControlMetadata>() {
													new ControlMetadata() {
														ControlIdentifier = "ContraIndication",
														ControlType = "Text",
														Metadata = new {
															Caption = "",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "PRODUCTINFOEXTENDED"),
															FieldName = "ContraIndication"
														}
													},
												}
												#endregion
											},
											#endregion				
										}
									},
									new ControlMetadata() {
										ControlIdentifier = "TabPageSectionUsings",
										ControlType = "TabPage",
										Metadata = new {
											Caption = "Предосторожности"
										},
										InnerControls = new List<ControlMetadata>() {
											#region Предосторожности 
											new ControlMetadata() {
												ControlIdentifier = "GroupBoxUsing",
												ControlType = "GroupBox",
												Metadata = new {
													Caption = "Предосторожности"
												},
												#region innercontrols
												InnerControls = new List<ControlMetadata>() {
													new ControlMetadata() {
														ControlIdentifier = "ChildUsing",
														ControlType = "Image",
														Metadata = new {
															Caption = "Применение для детей",
															Style = "ImageAndCaption",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "CHILD_USING"),											
														}
													},
													new ControlMetadata() {
														ControlIdentifier = "ElderlyUsing",
														ControlType = "Image",
														Metadata = new {
															Caption = "Применение для пожилых",
															Style = "ImageAndCaption",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "ELDERLY_USING"),											
														}
													},
													new ControlMetadata() {
														ControlIdentifier = "RenalUsing",
														ControlType = "Image",
														Metadata = new {
															Caption = "Применение в случаях заболеваний почек",
															Style = "ImageAndCaption",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "RENAL_USING"),											
														}
													},
													new ControlMetadata() {
														ControlIdentifier = "HepatoUsing",
														ControlType = "Image",
														Metadata = new {
															Caption = "Применение в случаях заболеваний печени",
															Style = "ImageAndCaption",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "HEPATO_USING"),											
														}
													},
													new ControlMetadata() {
														ControlIdentifier = "NursingUsing",
														ControlType = "Image",
														Metadata = new {
															Caption = "Применение в период лактации",
															Style = "ImageAndCaption",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "NURSING_USING"),											
														}
													},
													new ControlMetadata() {
														ControlIdentifier = "PregnancyUsing",
														ControlType = "Image",
														Metadata = new {
															Caption = "Применение в период беременности",
															Style = "ImageAndCaption",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "PREGNANCY_USING"),											
														}
													},
												#endregion
												}
											},
											#endregion
										}
									},
									new ControlMetadata() {
										ControlIdentifier = "TabPageSectionLactation",
										ControlType = "TabPage",
										Metadata = new {
											Caption = "Применение при беременности и лактации"
										},
										InnerControls = new List<ControlMetadata>() {
											#region Применение при беременности и лактации
											new ControlMetadata() {
												ControlIdentifier = "GroupBoxLactation",
												ControlType = "GroupBox",
												Metadata = new {
													Caption = "Применение при беременности и лактации"
												},
												#region innercontrols
												InnerControls = new List<ControlMetadata>() {
													new ControlMetadata() {
														ControlIdentifier = "Lactation",
														ControlType = "Text",
														Metadata = new {
															Caption = "",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "PRODUCTINFOEXTENDED"),
															FieldName = "Lactation"
														}
													},
												}
												#endregion
											},
											#endregion
										}
									},
									new ControlMetadata() {
										ControlIdentifier = "TabPageSectionSideEffects",
										ControlType = "TabPage",
										Metadata = new {
											Caption = "Побочные эффекты"
										},
										InnerControls = new List<ControlMetadata>() {
											#region Побочные эффекты
											new ControlMetadata() {
												ControlIdentifier = "GroupBoxSideEffects",
												ControlType = "GroupBox",
												Metadata = new {
													Caption = "Побочные эффекты"
												},
												#region innercontrols
												InnerControls = new List<ControlMetadata>() {
													new ControlMetadata() {
														ControlIdentifier = "SideEffects",
														ControlType = "Text",
														Metadata = new {
															Caption = "",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "PRODUCTINFOEXTENDED"),
															FieldName = "SideEffects"
														}
													},
												}
												#endregion
											},
											#endregion
										}
									},
									new ControlMetadata() {
										ControlIdentifier = "TabPageSectionInteraction",
										ControlType = "TabPage",
										Metadata = new {
											Caption = "Лекарственное взаимодействие"
										},
										InnerControls = new List<ControlMetadata>() {
											#region Лекарственное взаимодействие
											new ControlMetadata() {
												ControlIdentifier = "GroupBoxInteraction",
												ControlType = "GroupBox",
												Metadata = new {
													Caption = "Лекарственное взаимодействие"
												},
												#region innercontrols
												InnerControls = new List<ControlMetadata>() {
													new ControlMetadata() {
														ControlIdentifier = "Interaction",
														ControlType = "Text",
														Metadata = new {
															Caption = "",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "PRODUCTINFOEXTENDED"),
															FieldName = "Interaction"
														}
													},
												}
												#endregion
											},
											#endregion
										}
									},
									new ControlMetadata() {
										ControlIdentifier = "TabPageSectionMethod",
										ControlType = "TabPage",
										Metadata = new {
											Caption = "Способ применения и дозы"
										},
										InnerControls = new List<ControlMetadata>() {											
											#region Применение и дозы
											new ControlMetadata() {
												ControlIdentifier = "GroupBoxDosage",
												ControlType = "GroupBox",
												Metadata = new {
													Caption = "Способ применения и дозы"
												},
												#region innercontrols
												InnerControls = new List<ControlMetadata>() {
													new ControlMetadata() {
														ControlIdentifier = "Dosage",
														ControlType = "Text",
														Metadata = new {
															Caption = "",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "PRODUCTINFOEXTENDED"),
															FieldName = "Dosage"
														}
													},
												}
												#endregion
											},

											new ControlMetadata() {
												ControlIdentifier = "GroupBoxOverDosage",
												ControlType = "GroupBox",
												Metadata = new {
													Caption = "Передозировка"
												},
												#region innercontrols
												InnerControls = new List<ControlMetadata>() {
													new ControlMetadata() {
														ControlIdentifier = "OverDosage",
														ControlType = "Text",
														Metadata = new {
															Caption = "",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "PRODUCTINFOEXTENDED"),
															FieldName = "OverDosage"
														}
													},
												}
												#endregion
											},
											#endregion										
										}
									},
									new ControlMetadata() {
										ControlIdentifier = "TabPageSpecialInstructions",
										ControlType = "TabPage",
										Metadata = new {
											Caption = "Особые указания"
										},
										InnerControls = new List<ControlMetadata>() {											
											#region Особые указания
											new ControlMetadata() {
												ControlIdentifier = "GroupBoxSpecialInstructions",
												ControlType = "GroupBox",
												Metadata = new {
													Caption = "Особые указания"
												},
												#region innercontrols
												InnerControls = new List<ControlMetadata>() {
													new ControlMetadata() {
														ControlIdentifier = "SpecialInstruction",
														ControlType = "Text",
														Metadata = new {
															Caption = "",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "PRODUCTINFOEXTENDED"),
															FieldName = "SpecialInstruction"
														}
													},
												}
												#endregion
											},
											#endregion
										}
									},
									new ControlMetadata() {
										ControlIdentifier = "TabPagePharmDelivery",
										ControlType = "TabPage",
										Metadata = new {
											Caption = "Условия отпуска из аптек"
										},
										InnerControls = new List<ControlMetadata>() {											
											#region Условия отпуска из аптек
											new ControlMetadata() {
												ControlIdentifier = "GroupBoxPharmDelivery",
												ControlType = "GroupBox",
												Metadata = new {
													Caption = "Условия отпуска из аптек"
												},
												#region innercontrols
												InnerControls = new List<ControlMetadata>() {
													new ControlMetadata() {
														ControlIdentifier = "PharmDelivery",
														ControlType = "Text",
														Metadata = new {
															Caption = "",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "PRODUCTINFOEXTENDED"),
															FieldName = "PharmDelivery"
														}
													},
												}
												#endregion
											},
											#endregion
										}
									},
									new ControlMetadata() {
										ControlIdentifier = "TabPageStorageCondition",
										ControlType = "TabPage",
										Metadata = new {
											Caption = "Условия хранения и сроки годности"
										},
										InnerControls = new List<ControlMetadata>() {											
											#region Условия хранения и сроки годности
											new ControlMetadata() {
												ControlIdentifier = "GroupBoxStorageCondition",
												ControlType = "GroupBox",
												Metadata = new {
													Caption = "Условия хранения и сроки годности"
												},
												#region innercontrols
												InnerControls = new List<ControlMetadata>() {
													new ControlMetadata() {
														ControlIdentifier = "StorageCondition",
														ControlType = "Text",
														Metadata = new {
															Caption = "",
															DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "PRODUCTINFOEXTENDED"),
															FieldName = "StorageCondition"
														}
													},
												}
												#endregion
											}
											#endregion	
										}
									},
								},
							},
							#endregion	
						}
						
					},
					new ControlMetadata() {
						ControlIdentifier = "TabPageRegInfo",
						ControlType = "TabPage",
						Metadata = new {
							Caption = "Регистрационное удостоверение"
						},
						InnerControls = new List<ControlMetadata>() {
							#region innercontrols
							new ControlMetadata() {
								ControlIdentifier = "TabControlSectionsRegInfo",
								ControlType = "TabControl",
								Metadata = new {
									Orientation = "Vertical",
									Alignment = "Left",	
									Style = "Section"
								},
								InnerControls = new List<ControlMetadata>() {
									new ControlMetadata() {
										ControlIdentifier = "TabPageSectionRegInfo",
										ControlType = "TabPage",
										Metadata = new {
											Caption = "Основные атрибуты регистрационного удостоверения"
										},											
										InnerControls = new List<ControlMetadata>() {
											#region Основные атрибуты регистрационного удостоверения
											new ControlMetadata() {
												ControlIdentifier = "RegistrationOwner",
												ControlType = "Text",
												Metadata = new {
													Caption = "Владелец регистрационного удостоверения",
													DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "REG_INFO"),
													FieldName = "RegistrationOwner"
												}
											},
											new ControlMetadata() {
												ControlIdentifier = "RegistrationNumber",
												ControlType = "Text",
												Metadata = new {
													Caption = "Номер регистрационного удостоверения",
													DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "REG_INFO"),
													FieldName = "RegistrationNumber"
												}
											},
											new ControlMetadata() {
												ControlIdentifier = "RegistrationDate",
												ControlType = "Text",
												Metadata = new {
													Caption = "Дата регистрации",
													DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "REG_INFO"),
													FieldName = "RegistrationDate"
												}
											},
											new ControlMetadata() {
												ControlIdentifier = "DateOfCloseRegistration",
												ControlType = "Text",
												Metadata = new {
													Caption = "Дата окончания действия регистрационного удостоверения",
													DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "REG_INFO"),
													FieldName = "DateOfCloseRegistration"
												}
											},

											#endregion
										}	
									},
									new ControlMetadata() {
										ControlIdentifier = "TabPageSectionRegInfo",
										ControlType = "TabPage",
										Metadata = new {
											Caption = "Информация о производителе"
										},											
										InnerControls = new List<ControlMetadata>() {
											#region Информация о производителе
											new ControlMetadata() {
												ControlIdentifier = "RusName",
												ControlType = "Text",
												Metadata = new {
													Caption = "",
													DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "REG_INFO"),
													FieldName = "RusName"
												}
											},
											new ControlMetadata() {
												ControlIdentifier = "EngName",
												ControlType = "Text",
												Metadata = new {
													Caption = "",
													DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "REG_INFO"),
													FieldName = "EngName"
												}
											},
											new ControlMetadata() {
												ControlIdentifier = "RusAddress",
												ControlType = "Text",
												Metadata = new {
													Caption = "",
													DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "REG_INFO"),
													FieldName = "RusAddress"
												}
											},
											new ControlMetadata() {
												ControlIdentifier = "EngAddress",
												ControlType = "Text",
												Metadata = new {
													Caption = "",
													DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "REG_INFO"),
													FieldName = "EngAddress"
												}
											},
											new ControlMetadata() {
												ControlIdentifier = "PhoneNumber",
												ControlType = "Text",
												Metadata = new {
													Caption = "",
													DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "REG_INFO"),
													FieldName = "PhoneNumber"
												}
											},
											new ControlMetadata() {
												ControlIdentifier = "Fax",
												ControlType = "Text",
												Metadata = new {
													Caption = "",
													DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "REG_INFO"),
													FieldName = "Fax"
												}
											}
											#endregion
										}	
									},

								}
							}
							#endregion
						}					
					}
				}
			});


			var formMetadata = new FormMetadata()
			{
				MetadataId = "REF_VIDAL_FORM_ONERECORD",
				ObjectMetadata = GetVidalRefObjectMetadata(),
				MainDataProvider = mainDataProvider,
				DataProviders = dataProviderList,
				ControlsMetadata = controlsMetadata,
				ChoiceMetadata = new List<ChoiceMetadata>()
			};


			return formMetadata;
		}



		public static FormMetadata GetVidalRefFormJournalMetadata()
		{

			var mainDataProvider = new RestContainer(MetadataIdentifiers.ContainerTypeData, "REF_VIDAL_JOURNAL", "GET", "DrugsVidal",
													 new List<string>() { "string searchString", "int pageNumber", "int pageSize" });
			var dataProviderList = new List<RestContainer>() {
				mainDataProvider
			};


			var formMetadata = new FormMetadata()
			{
				MetadataId = "REF_VIDAL_FORM_JOURNAL",
				ObjectMetadata = GetVidalRefObjectMetadata(),
				MainDataProvider = mainDataProvider,
				DataProviders = dataProviderList,
				ChoiceMetadata = GetVidalRefChoiceMetadata()
			};

			var controlsMetadata = new List<object>();
			var columnsMetadata = new List<GridColumnMetadata>();

			#region Метаданные столбцов грида
			columnsMetadata.Add(new GridColumnMetadata()
			{
				DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "ID"),
				ColumnName = "ColumnId",
				EditorType = "Text",
				IsEditable = false,
				IsVisible = false,
				Position = 0,
				Width = 80
			});
			columnsMetadata.Add(new GridColumnMetadata()
			{
				DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "REG_INFO"),
				ColumnName = "ColumnRegInfo",
				EditorType = "Text",
				IsEditable = false,
				IsVisible = true,
				Position = 0,
				Width = 80
			});
			columnsMetadata.Add(new GridColumnMetadata()
			{
				DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "RUSNAME"),
				ColumnName = "ColumnRusName",
				Caption = "Наименование (RUS)",
				EditorType = "Text",
				IsEditable = false,
				IsVisible = true,
				SortOrder = "asc",
				Position = 1,
				Width = 120
			});
			columnsMetadata.Add(new GridColumnMetadata()
			{
				DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "ACTIVE_INGREDIENT"),
				ColumnName = "ColumnActiveIngredient",
				EditorType = "Text",
				IsEditable = false,
				IsVisible = true,
				Position = 2,
				Width = 120
			});
			columnsMetadata.Add(new GridColumnMetadata()
			{
				DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "DRUG_FORM"),
				ColumnName = "ColumnDrugForm",
				EditorType = "Text",
				IsEditable = false,
				IsVisible = true,
				Position = 3,
				Width = 90
			});
			columnsMetadata.Add(new GridColumnMetadata()
			{
				DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "ATHCLASSIFY"),
				ColumnName = "ColumnAThClassify",
				EditorType = "Text",
				IsEditable = false,
				IsVisible = true,
				Position = 4,
				Width = 90
			});
			columnsMetadata.Add(new GridColumnMetadata()
			{
				DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "FTH_CLASSIFY"),
				ColumnName = "ColumnPhThGroup",
				EditorType = "Text",
				IsEditable = false,
				IsVisible = true,
				Position = 5,
				Width = 90
			});
			columnsMetadata.Add(new GridColumnMetadata()
			{
				DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "CLPHGROUP"),
				ColumnName = "ColumnClPhGroup",
				EditorType = "Text",
				IsEditable = false,
				IsVisible = true,
				Position = 6,
				Width = 90
			});
			columnsMetadata.Add(new GridColumnMetadata()
			{
				DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "CLPHPOINTER"),
				ColumnName = "ColumnClPhPointer",
				EditorType = "Text",
				IsEditable = false,
				IsVisible = true,
				Position = 7,
				Width = 90
			});
			columnsMetadata.Add(new GridColumnMetadata()
			{
				DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "NOZOLOGYCODE"),
				ColumnName = "ColumnNozologyCode",
				EditorType = "Text",
				IsEditable = false,
				IsVisible = true,
				Position = 8,
				Width = 45
			});
			columnsMetadata.Add(new GridColumnMetadata()
			{
				DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "NOZOLOGYNAME"),
				ColumnName = "ColumnNozologyName",
				EditorType = "Text",
				IsEditable = false,
				IsVisible = true,
				Position = 9,
				Width = 90
			});
			columnsMetadata.Add(new GridColumnMetadata()
			{
				DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "HEPATO_USING"),
				ColumnName = "ColumnHepatoUsing",
				EditorType = "Text",
				IsEditable = false,
				IsVisible = true,
				Position = 10,
				Width = 45
			});
			columnsMetadata.Add(new GridColumnMetadata()
			{
				DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "RENAL_USING"),
				ColumnName = "ColumnRenalUsing",
				EditorType = "Text",
				IsEditable = false,
				IsVisible = true,
				Position = 11,
				Width = 45
			});
			columnsMetadata.Add(new GridColumnMetadata()
			{
				DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "NURSING_USING"),
				ColumnName = "ColumnNursingUsing",
				EditorType = "Text",
				IsEditable = false,
				IsVisible = true,
				Position = 12,
				Width = 45
			});
			columnsMetadata.Add(new GridColumnMetadata()
			{
				DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "CHILD_USING"),
				ColumnName = "ColumnChildUsing",
				EditorType = "Text",
				IsEditable = false,
				IsVisible = true,
				Position = 13,
				Width = 45
			});
			columnsMetadata.Add(new GridColumnMetadata()
			{
				DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "ELDERLY_USING"),
				ColumnName = "ColumnElderlyUsing",
				EditorType = "Text",
				IsEditable = false,
				IsVisible = true,
				Position = 14,
				Width = 45
			});
			columnsMetadata.Add(new GridColumnMetadata()
			{
				DataField = GetVidalRefFieldsMetadata().FirstOrDefault(f => f.MetadataId.ToUpperInvariant() == "PREGNANCY_USING"),
				ColumnName = "ColumnPregnancyUsing",
				EditorType = "Text",
				IsEditable = false,
				IsVisible = true,
				Position = 15,
				Width = 45
			});
			#endregion


			controlsMetadata.Add(new ControlMetadata()
			{
				ControlIdentifier = "Grid1",
				ControlType = "Grid",
				LayoutName = "MainDataGridLayout",
				Metadata = new
				{
					GridColumnsMetadata = columnsMetadata
				}
			});
			controlsMetadata.Add(new ControlMetadata()
			{
				ControlIdentifier = "Filter1",
				ControlType = "Filter",
				LayoutName = "MainFilterLayout",
				Metadata = new
				{
					DataProviderName = "REF_VIDAL_JOURNAL",
					FieldsMetadata = GetVidalRefFieldsMetadata()
				}
			});
			controlsMetadata.Add(new ControlMetadata()
			{
				ControlIdentifier = "CommandManager1",
				ControlType = "CommandManager",
				LayoutName = "BottomForm",
				Metadata = new
				{
					Target = new List<string>() { "Grid1" },
					Toolbar = true,
					ContextMenu = true,
					#region actions list
					Actions = new List<ActionMetadata> {
						new ActionMetadata() {
							ActionId = MetadataIdentifiers.ActionView,
							Arguments = new List<string>() {"string id"},
							ActionName = "Открыть",
							MethodType = "GET",
							ControllerName = "DrugsVidal",
							ActionType = "Base"
						},
						new ActionMetadata() {
							ActionId = MetadataIdentifiers.ActionEdit,
							Arguments = new List<string>() {"string id"},
							ActionName = "Редактировать",
							MethodType = "GET",
							ControllerName = "DrugsVidal",
							ActionType = "Base"
						},
						new ActionMetadata() {
							ActionId = MetadataIdentifiers.ActionDelete,
							Arguments = new List<string>() {"string id"},
							ActionName = "Удалить",
							MethodType = "DELETE",
							ControllerName = "DrugsVidal",
							ActionType = "Base"
						},
						new ActionMetadata() {
				            ActionId = MetadataIdentifiers.ActionSelectMany,
							Arguments = new List<string>() {"array[string] id"},
							ActionName = "Подбор",
							MethodType = "GET",
							ControllerName = "DrugsVidal",
							ActionType = "SelectMany"
						}

					}
					#endregion
				}
			});


			formMetadata.ControlsMetadata = controlsMetadata;
			return formMetadata;
		}

		private static IEnumerable<ChoiceMetadata> GetVidalRefChoiceMetadata()
		{
			return new List<ChoiceMetadata>() {
				new ChoiceMetadata() {
					ChoiceIdentifier = "CHOICE_INTERACTION",
					IsMain = true,
					VisualTemplate = new VisualTemplate() {
						Template = "Торговое наименование: <%=NAME%>",
						TemplateParams = new List<TemplateParam>() {
							new TemplateParam() {
								ParamName = "NAME",
								ParamValue = "RusName"
							}
						}
					}
				}
			};
		}


		private static ObjectMetadataRecord GetVidalRefObjectMetadata()
		{


			var result = new ObjectMetadataRecord()
			{
				Id = 10
			};
			#region main object fields metadata
			var fieldCode = new FieldRecord()
			{
				Id = 1010,
				Value = "REF_VIDAL",
				FieldMetadata = new FieldMetadataRecord()
				{
					Id = 10010,
					IsEditable = false,
					MetadataDataType = new MetadataDataType()
					{
						MetadataIdentifier = "string",
						MetadataTypeKind = "SimpleType"
					},

					MetadataId = "OBJECT_CODE",
					MetadataName = "Идентификатор объекта",
					Parent = result
				}
			};
			var fieldName = new FieldRecord()
			{
				Id = 1010,
				Value = "Реестр лекарственных средств",
				FieldMetadata = new FieldMetadataRecord()
				{
					Id = 10011,
					IsEditable = false,
					MetadataDataType = new MetadataDataType()
					{
						MetadataIdentifier = "string",
						MetadataTypeKind = "SimpleType"
					},

					MetadataId = "OBJECT_NAME",
					MetadataName = "Наименование объекта",
					Parent = result
				}
			};
			#endregion main object fields metadata
			result.FieldCode = fieldCode;
			result.FieldName = fieldName;
			return result;
		}

		public static IEnumerable<FieldMetadataRecord> GetVidalRefFieldsMetadata()
		{
			var parentMetadata = GetVidalRefObjectMetadata();
			var fieldsMetadata = new List<FieldMetadataRecord>();
			#region fieldsMetadata
			var documentId = new FieldMetadataRecord()
			{
				Id = 500,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "DOCUMENT_ID",
				MetadataName = "Идентификатор документа",
				Parent = parentMetadata,
				DataFieldName = "DocumentId"
			};
			var elderlyUsing = new FieldMetadataRecord()
			{
				Id = 501,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "enum",
					MetadataValues = new List<object>()
						                 {
							                 new {
													Value = ProductUsingType.CanUse,
    												Caption = "Разрешено к применению"
								                 },
										     new
											     {
													Value = ProductUsingType.NotAllowed,
    												Caption = "Не разрешено к применению"												     
											     },
										     new
											     {
													Value = ProductUsingType.Unknown,
    												Caption = "Указания о предосторожностях отсуствуют"												     
											     },
										     new
											     {
													Value = ProductUsingType.UseWithCare,
    												Caption = "Применять с осторожностью"												     
											     },
						                 },
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "ELDERLY_USING",
				MetadataName = "Использование для пожилых",
				Parent = parentMetadata,
				DataFieldName = "ElderlyUsing",
				IsFilterable = false,
				FilterControlType = "IMAGEDROPDOWN"
			};
			var childUsing = new FieldMetadataRecord()
			{
				Id = 501,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "enum",
					MetadataValues = new List<object>()
						                 {
							                 new {
													Value = ProductUsingType.CanUse,
    												Caption = "Разрешено к применению"
								                 },
										     new
											     {
													Value = ProductUsingType.NotAllowed,
    												Caption = "Не разрешено к применению"												     
											     },
										     new
											     {
													Value = ProductUsingType.Unknown,
    												Caption = "Указания о предосторожностях отсуствуют"												     
											     },
										     new
											     {
													Value = ProductUsingType.UseWithCare,
    												Caption = "Применять с осторожностью"												     
											     },
						                 },
					MetadataTypeKind = "SimpleType"
				},
				MetadataId = "CHILD_USING",
				MetadataName = "Использование для детей",
				Parent = parentMetadata,
				DataFieldName = "ChildUsing",
				IsFilterable = false,
				FilterControlType = "IMAGEDROPDOWN"
			};
			var nursingUsing = new FieldMetadataRecord()
			{
				Id = 502,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "enum",
					MetadataValues = new List<object>()
						                 {
							                 new {
													Value = ProductUsingType.CanUse,
    												Caption = "Разрешено к применению"
								                 },
										     new
											     {
													Value = ProductUsingType.NotAllowed,
    												Caption = "Не разрешено к применению"												     
											     },
										     new
											     {
													Value = ProductUsingType.Unknown,
    												Caption = "Указания о предосторожностях отсуствуют"												     
											     },
										     new
											     {
													Value = ProductUsingType.UseWithCare,
    												Caption = "Применять с осторожностью"												     
											     },
						                 },
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "NURSING_USING",
				MetadataName = "Использование при лактации",
				Parent = parentMetadata,
				DataFieldName = "NursingUsing",
				FilterControlType = "IMAGEDROPDOWN"
			};
			var hepatoUsing = new FieldMetadataRecord()
			{
				Id = 503,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "enum",
					MetadataValues = new List<object>()
						                 {
							                 new {
													Value = ProductUsingType.CanUse,
    												Caption = "Разрешено к применению"
								                 },
										     new
											     {
													Value = ProductUsingType.NotAllowed,
    												Caption = "Не разрешено к применению"												     
											     },
										     new
											     {
													Value = ProductUsingType.Unknown,
    												Caption = "Указания о предосторожностях отсуствуют"												     
											     },
										     new
											     {
													Value = ProductUsingType.UseWithCare,
    												Caption = "Применять с осторожностью"												     
											     },
						                 },
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "HEPATO_USING",
				MetadataName = "Использование при болезнях печени",
				Parent = parentMetadata,
				DataFieldName = "HepatoUsing",
				FilterControlType = "IMAGEDROPDOWN"
			};
			var renalUsing = new FieldMetadataRecord()
			{
				Id = 504,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "enum",
					MetadataValues = new List<object>()
						                 {
							                 new {
													Value = ProductUsingType.CanUse,
    												Caption = "Разрешено к применению"
								                 },
										     new
											     {
													Value = ProductUsingType.NotAllowed,
    												Caption = "Не разрешено к применению"												     
											     },
										     new
											     {
													Value = ProductUsingType.Unknown,
    												Caption = "Указания о предосторожностях отсуствуют"												     
											     },
										     new
											     {
													Value = ProductUsingType.UseWithCare,
    												Caption = "Применять с осторожностью"												     
											     },
						                 },
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "RENAL_USING",
				MetadataName = "Использование при болезнях почек",
				Parent = parentMetadata,
				DataFieldName = "RenalUsing",
				FilterControlType = "IMAGEDROPDOWN"
			};

			var regInfo = new FieldMetadataRecord()
			{
				Id = 505,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "object",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "REG_INFO",
				MetadataName = "Регистрационный номер",
				Parent = parentMetadata,
				DataFieldName = "RegistrationInfo",
				VisualTemplate = new VisualTemplate()
				{
					Template = "<% if (arguments[0].REGNUMBER) { %>" +
								  "<%=REGNUMBER%>" +
								"<% }; %>" +
								"<% if (arguments[0].REGDATE) { %>" +
								  " от <%=REGDATE%>" +
								"<% }; %>" +
								"<% if (arguments[0].REGOWNER) { %>" +
								  " (<%=REGOWNER%>)" +
								"<% }; %>",
					TemplateParams = new List<TemplateParam>() {
						new TemplateParam() {
							ParamName = "REGNUMBER",
							ParamValue = "RegistrationNumber"
						},
						new TemplateParam() {
							ParamName = "REGDATE",
							ParamValue = "RegistrationDate"
						},
						new TemplateParam() {
							ParamName = "REGOWNER",
							ParamValue = "RegistrationOwner"
						}
					}
				},
			};
			var rusName = new FieldMetadataRecord()
			{
				Id = 506,
				IsEditable = false,
				IsFilterable = true,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},
				MetadataId = "RUSNAME",
				MetadataName = "Торговое наименование",
				Parent = parentMetadata,
				DataFieldName = "RusName",
				FilterControlType = "TEXT"
			};
			var activeIngredient = new FieldMetadataRecord()
			{
				Id = 507,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "REF_ACTIVEINGREDIENT",//"array",
					MetadataTypeKind = "ReferenceType"//"SimpleType"
				},
				IsFilterable = true,
				VisualTemplate = new VisualTemplate()
				{
					Template = "<%=RUSNAME%> <% if (arguments[0].ENGNAME) { %>" +
								  "(<%=ENGNAME%>)" +
								"<% }; %>",
					TemplateParams = new List<TemplateParam>() {
						new TemplateParam() {
							ParamName = "RUSNAME",
							ParamValue = "MoleculeRusName"
						},
						new TemplateParam() {
							ParamName = "ENGNAME",
							ParamValue = "MoleculeEngName"
						}
					}
				},
				FilterControlType = "SELECT",
				MetadataId = "ACTIVE_INGREDIENT",
				MetadataName = "Активное вещество",
				Parent = parentMetadata,
				DataFieldName = "ActiveIngredient"
			};
			var inactiveIngredient = new FieldMetadataRecord()
			{
				Id = 507,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "array",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "INACTIVE_INGREDIENT",
				MetadataName = "Вспомогательное вещество",
				Parent = parentMetadata,
				DataFieldName = "InactiveIngredient"
			};
			var athClassify = new FieldMetadataRecord()
			{

				Id = 508,
				IsFilterable = true,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "REF_ATHCLASSIFY",
					MetadataTypeKind = "ReferenceType"
				},

				MetadataId = "AThClassify",
				MetadataName = "АТХ классификация",
				Parent = parentMetadata,
				DataFieldName = "AThClassify",
				VisualTemplate = new VisualTemplate()
				{
					Template = "<%=CODE%> <%=NAME%>",
					TemplateParams = new List<TemplateParam>() {
						new TemplateParam() {
							ParamName = "CODE",
							ParamValue = "Id"
						},
						new TemplateParam() {
							ParamName = "NAME",
							ParamValue = "RusName"
						}
					}
				},
				FilterControlType = "SELECT"
			};

			var fthClassify = new FieldMetadataRecord()
			{
				Id = 509,
				IsFilterable = true,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "REF_PHTHGROUP",
					MetadataTypeKind = "ReferenceType"
				},
				MetadataId = "FTH_CLASSIFY",
				MetadataName = "ФТГ",
				Parent = parentMetadata,
				DataFieldName = "PhThGroup",
				FilterControlType = "DROPDOWN",
				VisualTemplate = new VisualTemplate()
				{
					Template = "<%=ID%> <%=NAME%>",
					TemplateParams = new List<TemplateParam>() {
						new TemplateParam() {
							ParamName = "NAME",
							ParamValue = "Name"
						},
						new TemplateParam() {
							ParamName = "ID",
							ParamValue = "Id"
						}
					}
				},
			};

			var drugForm = new FieldMetadataRecord()
			{
				Id = 510,
				IsEditable = false,
				Parent = parentMetadata,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "REF_DRUGFORM",
					MetadataTypeKind = "ReferenceType"
				},
				IsFilterable = true,
				VisualTemplate = new VisualTemplate()
				{
					Template = "<%=RUSNAME%>",
					TemplateParams = new List<TemplateParam>() {
						new TemplateParam() {
							ParamName = "RUSNAME",
							ParamValue = "FormRusName"
						}
					}
				},
				FilterControlType = "SELECT",
				MetadataId = "DRUG_FORM",
				MetadataName = "Лекарственная форма",
				DataFieldName = "DrugForm"
			};

			var company = new FieldMetadataRecord()
			{
				Id = 511,
				IsEditable = false,
				Parent = parentMetadata,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "object",
					MetadataTypeKind = "SimpleType"
				},
				MetadataId = "COMPANY",
				MetadataName = "Компания-производитель",
				DataFieldName = "Company"
			};

			var clphgroup = new FieldMetadataRecord()
			{
				Id = 512,
				IsEditable = false,
				IsFilterable = true,
				Parent = parentMetadata,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "REF_CLPHGROUP",
					MetadataTypeKind = "ReferenceType"
				},
				MetadataId = "CLPHGROUP",
				MetadataName = "КФГ",
				DataFieldName = "ClPhGroup",
				FilterControlType = "SELECT",
				VisualTemplate = new VisualTemplate()
				{
					Template = "<%=CODE%> <%=NAME%>",
					TemplateParams = new List<TemplateParam>() {
						new TemplateParam() {
							ParamName = "CODE",
							ParamValue = "Code"
						},
						new TemplateParam() {
							ParamName = "NAME",
							ParamValue = "Name"
						}
					}
				},
			};

			var clphpointer = new FieldMetadataRecord()
			{
				Id = 513,
				IsEditable = false,
				IsFilterable = true,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "REF_CLPHPOINTER",
					MetadataTypeKind = "ReferenceType"
				},
				Parent = parentMetadata,
				MetadataName = "КФУ",
				MetadataId = "ClPhPointer",
				DataFieldName = "ClPhPointer",
				FilterControlType = "SELECTTREE",
				VisualTemplate = new VisualTemplate()
				{
					Template = "<%=CODE%> <%=NAME%>",
					TemplateParams = new List<TemplateParam>() {
						new TemplateParam() {
							ParamName = "CODE",
							ParamValue = "Code"
						},
						new TemplateParam() {
							ParamName = "NAME",
							ParamValue = "Name"
						}
					}
				},
			};

			var extendedInfoMetadata = new FieldMetadataRecord()
			{
				Id = 543,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "object",
					MetadataTypeKind = "SimpleType"
				},
				Parent = parentMetadata,
				FilterControlType = "TEXT",
				MetadataName = "Основная информация",
				MetadataId = "ProductInfoExtended",
				DataFieldName = "ProductInfoExtended"
			};
			var zipInfo = new FieldMetadataRecord()
			{
				Id = 549,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},
				Parent = parentMetadata,
				FilterControlType = "TEXT",
				MetadataName = "Упаковка",
				MetadataId = "ZipInfo",
				DataFieldName = "ZipInfo"
			};

			var marketRusName = new FieldMetadataRecord()
			{
				Id = 514,
				IsEditable = false,
				IsFilterable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "ReferenceType",
					MetadataTypeKind = "REF_MARKETNAME"
				},
				Parent = parentMetadata,
				MetadataName = "",
				DataFieldName = "MarketRusName",
				MetadataId = "MarketRusName",
				FilterControlType = "DROPDOWN"
			};
			var nozologyCode = new FieldMetadataRecord()
			{
				Id = 515,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "table",
					MetadataTypeKind = "SimpleType"
				},
				Parent = parentMetadata,
				MetadataName = "Код МКБ-10",
				MetadataId = "NozologyCode",
				DataFieldName = "NozologyCode"
			};
			var nozologyName = new FieldMetadataRecord()
			{
				Id = 516,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "table",
					MetadataTypeKind = "SimpleType"
				},
				Parent = parentMetadata,
				MetadataName = "Наименование МКБ-10",
				MetadataId = "NozologyName",
				DataFieldName = "NozologyName"
			};
			var pregnancyUsing = new FieldMetadataRecord()
			{
				Id = 517,
				IsEditable = false,
				IsFilterable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "enum",
					MetadataValues = new List<object>()
						                 {
							                 new {
													Value = ProductUsingType.CanUse,
    												Caption = "Разрешено к применению"
								                 },
										     new
											     {
													Value = ProductUsingType.NotAllowed,
    												Caption = "Не разрешено к применению"												     
											     },
										     new
											     {
													Value = ProductUsingType.Unknown,
    												Caption = "Указания о предосторожностях отсуствуют"												     
											     },
										     new
											     {
													Value = ProductUsingType.UseWithCare,
    												Caption = "Применять с осторожностью"												     
											     },
						                 },
					MetadataTypeKind = "SimpleType"
				},
				MetadataId = "PREGNANCY_USING",
				MetadataName = "Использование при беременности",
				Parent = parentMetadata,
				DataFieldName = "PregnancyUsing",
				FilterControlType = "IMAGEDROPDOWN"
			};
			var id = new FieldMetadataRecord()
			{
				Id = 599,
				IsEditable = false,
				MetadataDataType = new MetadataDataType()
				{
					MetadataIdentifier = "string",
					MetadataTypeKind = "SimpleType"
				},

				MetadataId = "Id",
				MetadataName = "Идентификатор записи",
				IsIdentifier = true,
				Parent = parentMetadata,
				DataFieldName = "Id"
			};
			#endregion

			fieldsMetadata.Add(documentId);
			fieldsMetadata.Add(regInfo);
			fieldsMetadata.Add(rusName);
			fieldsMetadata.Add(activeIngredient);
			fieldsMetadata.Add(inactiveIngredient);
			fieldsMetadata.Add(athClassify);
			fieldsMetadata.Add(elderlyUsing);
			fieldsMetadata.Add(childUsing);
			fieldsMetadata.Add(nursingUsing);
			fieldsMetadata.Add(hepatoUsing);
			fieldsMetadata.Add(renalUsing);
			fieldsMetadata.Add(pregnancyUsing);
			fieldsMetadata.Add(fthClassify);
			fieldsMetadata.Add(drugForm);
			fieldsMetadata.Add(company);
			fieldsMetadata.Add(clphgroup);
			fieldsMetadata.Add(clphpointer);
			fieldsMetadata.Add(marketRusName);
			fieldsMetadata.Add(nozologyCode);
			fieldsMetadata.Add(nozologyName);
			fieldsMetadata.Add(id);
			fieldsMetadata.Add(extendedInfoMetadata);
			fieldsMetadata.Add(zipInfo);
			return fieldsMetadata;

		}

	}
}