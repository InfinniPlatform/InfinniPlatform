using System;

using FastReport.Data;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Data
{
	static class TestReportDataSource
	{
		public static readonly DataSourceBase Instance
			= new BusinessObjectDataSource
				  {
					  Name = "Document",

					  Columns =
						  {
							  new Column { Name = "Id", DataType = typeof(string) },
							  new Column { Name = "Title", DataType = typeof(string) },
							  new Column
								  {
									  Name = "Patient",
									  DataType = typeof(object),

									  Columns =
										  {
											  new Column { Name = "FirstName", DataType = typeof(string) },
											  new Column { Name = "LastName", DataType = typeof(string) },
											  new Column
												  {
													  Name = "Passport",
													  DataType = typeof(object),

													  Columns =
														  {
															  new Column { Name = "Series", DataType = typeof(string) },
															  new Column { Name = "Number", DataType = typeof(string) },
															  new Column { Name = "IssueDate", DataType = typeof(DateTime) }
														  }
												  },
											  new BusinessObjectDataSource
												  {
													  Name = "Addresses",

													  Columns =
														  {
															  new Column { Name = "City", DataType = typeof(string) },
															  new Column { Name = "Street", DataType = typeof(string) },
															  new Column { Name = "Home", DataType = typeof(string) }
														  }
												  }
										  }
								  },
							  new BusinessObjectDataSource
								  {
									  Name = "Authors",

									  Columns =
										  {
											  new Column { Name = "Type", DataType = typeof(string) },
											  new Column
												  {
													  Name = "Employee",
													  DataType = typeof(object),

													  Columns =
														  {
															  new Column { Name = "FirstName", DataType = typeof(string) },
															  new Column { Name = "LastName", DataType = typeof(string) }
														  }
												  },
											  new BusinessObjectDataSource
												  {
													  Name = "Contacts",

													  Columns =
														  {
															  new Column { Name = "Caption", DataType = typeof(string) },
															  new Column { Name = "Value", DataType = typeof(string) }
														  }
												  }
										  }
								  }
						  }
				  };
	}
}