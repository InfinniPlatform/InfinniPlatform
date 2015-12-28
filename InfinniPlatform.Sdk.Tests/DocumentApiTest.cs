using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Dynamic;

using Newtonsoft.Json;

using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
	[TestFixture]
	[Category(TestCategories.AcceptanceTest)]
	public class DocumentApiTest
	{
		private const string Route = "1";

		private IDisposable _server;
		private InfinniDocumentApi _api;

		[TestFixtureSetUp]
		public void SetUp()
		{
			_server = InfinniPlatformInprocessHost.Start();
			_api = new InfinniDocumentApi(HostingConfig.Default.Name, HostingConfig.Default.Port);
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			_server.Dispose();
		}

		[Test]
		public void ShouldGetDocument()
		{
			var documentObject = new
								 {
									 Name = "gta vice city",
									 Price = 100.50
								 };

			var result = _api.SetDocument("gameshop", "catalogue", documentObject).Id.ToString();

			var resultDoc = _api.GetDocument("gameshop", "catalogue",
				f => f.AddCriteria(cr => cr.Property("Id").IsEquals(result)), 0, 100,
				s => s.AddSorting("Price", "descending"));

			Assert.AreEqual(1, resultDoc.Count());
		}

		[Test]
		public void ShouldGetDocumentMoreThanPrice()
		{
			var documentObject = new
								 {
									 Name = "gta vice city",
									 Price = 1000.50
								 };

			var result = _api.SetDocument("gameshop", "catalogue", documentObject).Id.ToString();

			var resultDoc = _api.GetDocument("gameshop", "catalogue",
				f => f
					.AddCriteria(cr => cr.Property("Id").IsEquals(result))
					.AddCriteria(cr => cr.Property("Price").IsMoreThanOrEquals(1000)), 0, 100
				);

			Assert.AreEqual(1, resultDoc.Count());
		}

		[Test]
		public void ShouldGetDocumentByDateTime()
		{
			dynamic documentObject = new DynamicWrapper();
			documentObject.Availability = new DynamicWrapper();
			documentObject.Availability.SaleStartDate = DateTime.Now;

			var result = _api.SetDocument("gameshop", "catalogue", documentObject).Id.ToString();

			var resultDoc = _api.GetDocument("gameshop", "catalogue",
				f => f.AddCriteria(cr => cr.Property("Id").IsEquals(result))
					  .AddCriteria(cr => cr.Property("Availability.SaleStartDate").IsMoreThanOrEquals(DateTime.Now.Date))
					  .AddCriteria(cr => cr.Property("Availability.SaleStartDate").IsLessThanOrEquals(DateTime.Now.Date.AddDays(1))), 0, 100);
			Assert.AreEqual(1, resultDoc.Count(r => r.Id == result));
		}

		[Test]
		public void ShouldGetNumberOfDocuments()
		{
			var documentObject = new
								 {
									 Name = "gta vice city",
									 Price = 1000.50
								 };

			var result = _api.SetDocument("gameshop", "catalogue", documentObject).Id.ToString();

			var docCount = _api.GetNumberOfDocuments("gameshop", "catalogue",
				f => f.AddCriteria(cr => cr.Property("Id").IsEquals(result)));

			Assert.AreEqual(1, docCount);
		}

		[Test]
		public void ShouldGetDocumentLessThanPrice()
		{
			var documentObject = new
								 {
									 Name = "gta vice city",
									 Price = 1000.50
								 };

			var result = _api.SetDocument("gameshop", "catalogue", documentObject).Id.ToString();

			var resultDoc = _api.GetDocument("gameshop", "catalogue",
				f => f
					.AddCriteria(cr => cr.Property("Id").IsEquals(result))
					.AddCriteria(cr => cr.Property("Price").IsMoreThanOrEquals(1000))
					.AddCriteria(cr => cr.Property("Price").IsLessThanOrEquals(10000)), 0, 1000
				);

			Assert.AreEqual(1, resultDoc.Count(r => r.Id == result));
		}

		[Test]
		public void ShouldGetDocumentWithSorting()
		{
			var documentObject1 = new
								  {
									  Id = Guid.NewGuid().ToString(),
									  Name = "Gta3",
									  Price = 100.50
								  };

			var documentObject2 = new
								  {
									  Id = Guid.NewGuid().ToString(),
									  Name = "Gta4",
									  Price = 120.50
								  };

			var documentObject3 = new
								  {
									  Id = Guid.NewGuid().ToString(),
									  Name = "Gta5",
									  Price = 1400
								  };


			_api.SetDocument("gameshop", "catalogue", documentObject1);
			_api.SetDocument("gameshop", "catalogue", documentObject2);
			_api.SetDocument("gameshop", "catalogue", documentObject3);

			var resultDoc = _api.GetDocument("gameshop", "catalogue",
				f => f.AddCriteria(cr => cr.Property("Id").IsIdIn(new List<string> { documentObject1.Id, documentObject2.Id, documentObject3.Id })), 0, 100,
				s => s.AddSorting("Name", "descending")).ToArray();

			Assert.AreEqual(3, resultDoc.Length);
			Assert.AreEqual(resultDoc[0].Name, "Gta5");
			Assert.AreEqual(resultDoc[1].Name, "Gta4");
			Assert.AreEqual(resultDoc[2].Name, "Gta3");
		}

		[Test]
		public void ShouldGetDocumentByIdIn()
		{
			var documentObject = new
								 {
									 Name = "gta vice city",
									 Price = 100.50
								 };

			var result = _api.SetDocument("gameshop", "catalogue", documentObject).Id.ToString();

			var resultDoc = _api.GetDocument("gameshop", "catalogue",
				f => f.AddCriteria(cr => cr.Property("Id").IsIdIn(new List<string> { result })), 0, 100,
				s => s.AddSorting("Price", "descending"));

			Assert.AreEqual(1, resultDoc.Count());
		}

		[Test]
		public void ShouldGetDocumentByIdInEmpty()
		{
			var resultDoc = _api.GetDocument("gameshop", "catalogue",
				f => f.AddCriteria(cr => cr.Property("Id").IsIdIn(new List<string>())), 0, 100,
				s => s.AddSorting("Price", "descending"));

			Assert.AreNotEqual(0, resultDoc.Count());
		}

		[Test]
		public void ShouldGetDocumentByIn()
		{
			var documentObject = new
								 {
									 Name = "gta vice city",
									 Price = 100.50
								 };

			var result = _api.SetDocument("gameshop", "catalogue", documentObject).Id.ToString();

			var resultDoc = _api.GetDocument("gameshop", "catalogue",
				f => f.AddCriteria(cr => cr.Property("Id").IsIn(result)), 0, 100,
				s => s.AddSorting("Price", "descending"));

			Assert.AreEqual(1, resultDoc.Count());
		}


		[Test]
		public void ShouldGetDocumentByIsNotEmpty()
		{
			var documentObject = new
								 {
									 Name = "gta vice city",
									 Price = 100.50
								 };

			_api.SetDocument("gameshop", "catalogue", documentObject).Id.ToString();

			var resultDoc = _api.GetDocument("gameshop", "catalogue",
				f => f.AddCriteria(cr => cr.Property("Id").IsNotEmpty()), 0, 1,
				s => s.AddSorting("Price", "descending"));

			Assert.AreEqual(1, resultDoc.Count());
		}

		[Test]
		public void ShouldGetDocumentByIsEmpty()
		{
			var documentObject = new
								 {
									 Price = 100.50
								 };

			var result = _api.SetDocument("gameshop", "catalogue", documentObject).Id.ToString();

			var resultDoc = _api.GetDocument("gameshop", "catalogue",
				f => f.AddCriteria(cr => cr.Property("Name").IsEmpty()), 0, 10000,
				s => s.AddSorting("Price", "descending"));

			Assert.AreEqual(1, resultDoc.Count(r => r.Id == result));
		}

		[Test]
		public void ShouldSetDocument()
		{
			var documentObject = new
								 {
									 Name = "gta vice city",
									 Price = 100.50
								 };

			var result = _api.SetDocument("gameshop", "catalogue", documentObject).Id.ToString();
			Assert.True(!string.IsNullOrEmpty(result));
		}

		[Test]
		public void ShouldSetDocumentOnSuccessAction()
		{
			//Given
			dynamic comment = new
							  {
								  Text = "some text"
							  };

			//When
		    var document = _api.SetDocument("Gameshop", "Comment", comment);
		    string docId = document.Id.ToString();

			//Then
			dynamic documentResult = _api.GetDocumentById("Gameshop", "Comment", docId);
			Assert.AreEqual(documentResult.Text, "some text123");
		}

		[Test]
		public void ShouldSetDocumentWithNullId()
		{
			var documentObject = new
								 {
									 Name = "gta vice city",
									 Price = 100.50
								 };

			var result = _api.SetDocument("gameshop", "catalogue", documentObject).Id.ToString();

			var resultGet = _api.GetDocumentById("gameshop", "catalogue", result);
			Assert.IsNotNull(resultGet);
		}

		[Test]
		public void ShouldMakeGetRequestWithNullValueInFilter()
		{
			var result = _api.GetDocument("gameshop", "catalogue",
				f => f.AddCriteria(cr => cr.Property("Id").IsEquals(null)), 0, 1);

			Assert.IsNotNull(result);
		}

		public void ShouldUpdateDocument()
		{
			var documentObject = new
								 {
									 Name = "gta V",
									 Price = 2000,
									 Availability = new
													{
														Available = false,
														SaleStartDate = new DateTime(2014, 04, 14)
													}
								 };

			var result = _api.SetDocument("gameshop", "catalogue", documentObject).Id.ToString();

			dynamic document = _api.GetDocument("gameshop", "catalogue", null, 0, 1).FirstOrDefault();

			Assert.IsNotNull(document);

			var changesObject = new
								{
									Name = "gta V ultimate edition",
									Availability = new
												   {
													   Available = true
												   }
								};

			_api.UpdateDocument("gameshop", "catalogue", result, changesObject);

			document = _api.GetDocument("gameshop", "catalogue",
				f => f.AddCriteria(cr => cr.Property("Id").IsEquals(result)), 0, 1).FirstOrDefault();

			Assert.IsNotNull(document);

			document = JsonConvert.DeserializeObject<ExpandoObject>(document.ToString());
			Assert.AreEqual(document.Availability.Available, true);
		}

		[Test]
		public void ShouldDeleteDocument()
		{
			var documentObject = new
								 {
									 Name = "Deus Ex:Human Revolution",
									 Price = 1999
								 };

		    var document = _api.SetDocument("gameshop", "catalogue", documentObject);
		    var result = document.Id.ToString();

			var docs = _api.GetDocument("gameshop", "catalogue",
				f => f.AddCriteria(cr => cr.Property("Id").IsEquals(result)), 0, 1);

			Assert.True(docs.Any());

			dynamic deleteResult = _api.DeleteDocument("gameshop", "catalogue", result);

			Assert.AreEqual(deleteResult.IsValid, true);
			Assert.AreEqual(deleteResult.ValidationMessage, string.Format("Document with identifier \"{0}\" deleted successfully", result));

			docs = _api.GetDocument("gameshop", "catalogue",
				f => f.AddCriteria(cr => cr.Property("Id").IsEquals(result)), 0, 1);

			Assert.False(docs.Any());
		}

		[Test]
		public void ShouldGetDocumentById()
		{
			var documentObject = new
								 {
									 Name = "Bioshock:Infinite",
									 Price = 1499
								 };

			var result = _api.SetDocument("gameshop", "catalogue", documentObject).Id.ToString();

			dynamic persistentDocument = _api.GetDocumentById("gameshop", "catalogue", result);

			Assert.IsNotNull(persistentDocument);
			Assert.AreEqual(persistentDocument.Name, "Bioshock:Infinite");
		}

		[Test]
		public void ShouldNotGetDocumentByNonExistingId()
		{
			dynamic persistentDocument = _api.GetDocumentById("gameshop", "catalogue", Guid.NewGuid().ToString());

			Assert.IsNull(persistentDocument);
		}

		[Test]
        [Ignore("Transaction implementation is obsolete")]
		public void ShouldUseSession()
		{
			var session = _api.CreateSession().SessionId;

			Assert.IsNotNull(session);

			var documentObject = new
								 {
									 Id = Guid.NewGuid().ToString(),
									 Name = "Diablo III",
									 Price = 1800
								 };

			var additionalObject = new
								   {
									   Id = Guid.NewGuid().ToString(),
									   Name = "Wolfenstein:New Order",
									   Price = 999
								   };

			var failObject = new
							 {
								 Id = Guid.NewGuid().ToString(),
								 Name = "Fail object",
								 Price = -1
							 };

			dynamic attachResult = _api.Attach(session, "gameshop", "catalogue", documentObject.Id, documentObject);

			Assert.AreEqual(attachResult.IsValid, true);

			dynamic sessionItems = _api.GetSession(session).Items;

			Assert.AreEqual(sessionItems.Count, 1);

			_api.Attach(session, "gameshop", "catalogue", additionalObject.Id, additionalObject);
			_api.Attach(session, "gameshop", "catalogue", failObject.Id, failObject);

			_api.Detach(session, failObject.Id);

			sessionItems = _api.GetSession(session).Items;
			Assert.AreEqual(sessionItems.Count, 2);

			_api.SaveSession(session);

			sessionItems = _api.GetSession(session).Items;
			Assert.AreEqual(sessionItems.Count, 0);

			var savedObject1 = _api.GetDocumentById("gameshop", "catalogue", documentObject.Id);
			var savedObject2 = _api.GetDocumentById("gameshop", "catalogue", additionalObject.Id);
			var unsavedObject3 = _api.GetDocumentById("gameshop", "catalogue", failObject.Id);

			Assert.IsNotNull(savedObject1);
			Assert.IsNotNull(savedObject2);
			Assert.IsNull(unsavedObject3);
		}

		[Test]
        [Ignore("Transaction implementation is obsolete")]
        public void ShouldRemoveTransaction()
		{
			var session = _api.CreateSession().SessionId.ToString();

			var documentObject = new
								 {
									 Id = Guid.NewGuid().ToString(),
									 Name = "Homeworld remastered",
									 Price = 400
								 };

			_api.Attach(session, "gameshop", "catalogue", documentObject.Id, documentObject);

			_api.RemoveSession(session);

			var sessionItems = _api.GetSession(session).Items;

			Assert.AreEqual(sessionItems.Count, 0);
		}


		[Test]
		public void ShouldSetDocuments()
		{
			var documentObject1 = new
								  {
									  Id = Guid.NewGuid().ToString(),
									  Name = "Homeworld remastered",
									  Price = 400
								  };

			var documentObject2 = new
								  {
									  Id = Guid.NewGuid().ToString(),
									  Name = "Homeworld: Cataclysm",
									  Price = 400
								  };

			_api.SetDocuments("gameshop", "catalogue", new[] { documentObject1, documentObject2 });

			var savedObject1 = _api.GetDocumentById("gameshop", "catalogue", documentObject1.Id);
			var savedObject2 = _api.GetDocumentById("gameshop", "catalogue", documentObject2.Id);


			Assert.IsNotNull(savedObject1);
			Assert.IsNotNull(savedObject2);
		}

		[Test]
		public void ShouldGetDocumentWithNotResolvedReference()
		{
			//Given
			dynamic game = new
						   {
							   Name = "X-Com:Enemy Within",
							   Price = 1900
						   };

			string id = _api.SetDocument("gameshop", "catalogue", game).Id.ToString();

			dynamic review = new
							 {
								 Game = new
										{
											Id = id,
											DisplayName = "X-Com:Enemy Within"
										}
							 };

			string reviewId = _api.SetDocument("gameshop", "review", review).Id.ToString();

			//When
			dynamic item = _api.GetDocumentById("gameshop", "review", reviewId);

			//Then
			Assert.IsNotNull(item);
			Assert.IsNotNull(item.Game);
			Assert.IsNotNull(item.Game.Id);
			Assert.IsNotNull(item.Game.DisplayName);

			Assert.IsNull(item.Game.Name);
			Assert.IsNull(item.Game.Price);
		}

		[Test]
		public void ShouldGetDocumentWithResolvedReference()
		{
			//Given
			dynamic rate = new
						   {
							   UserRate = 90,
							   ReviewRate = 95
						   };

			string rateId = _api.SetDocument("gameshop", "gamerating", rate).Id.ToString();


			dynamic review = new
							 {
								 Rate = new
										{
											Id = rateId,
											DisplayName = "UserRate:90, ReviewRate:95"
										}
							 };

			string reviewId = _api.SetDocument("gameshop", "review", review).Id.ToString();

			//When
			dynamic item = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetDocumentById("gameshop", "review", reviewId).ToString());

			//Then
			Assert.IsNotNull(item);
			Assert.IsNotNull(item.Rate);
			Assert.IsNotNull(item.Rate.Id);

			Assert.False(((IDictionary<string, object>)item.Rate).ContainsKey("DisplayName"));

			Assert.IsNotNull(item.Rate.Id);
			Assert.AreEqual(item.Rate.UserRate, 90);
			Assert.AreEqual(item.Rate.ReviewRate, 95);
		}

		[Test]
		public void ShouldRemoveNotSchemaFieldsOnSave()
		{
			//Given
			dynamic game = new
						   {
							   Name = "Divinity:Original Sin",
							   Price = 1800,
							   SomeFieldWhichMissingInSchema = 1111
						   };

			string gameId = _api.SetDocument("gameshop", "catalogue", game).Id.ToString();

			//When
			dynamic item = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetDocumentById("gameshop", "catalogue", gameId).ToString());
			//Then
			Assert.IsNotNull(item);
			Assert.False(((IDictionary<string, object>)item).ContainsKey("DisplayName"));
		}

		[Test]
		public void ShouldGetDocumentWithStrongTypeInlineReference()
		{
			//Given

			dynamic review = new
							 {
								 ReviewConfig = new
												{
													Processor = "Intel Core i7 4770K @ 3.50GHz",
													Memory = "G.Skill DDR3-2133 8 Гб",
													Videocard = "GeForce GTX 770 2048 Мб"
												}
							 };


			string reviewId = _api.SetDocument("gameshop", "review", review).Id.ToString();

			//When
			dynamic item = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetDocumentById("gameshop", "review", reviewId).ToString());

			//Then
			Assert.IsNotNull(item);
			Assert.AreEqual(item.ReviewConfig.Processor, "Intel Core i7 4770K @ 3.50GHz");
			Assert.AreEqual(item.ReviewConfig.Memory, "G.Skill DDR3-2133 8 Гб");
			Assert.AreEqual(item.ReviewConfig.Videocard, "GeForce GTX 770 2048 Мб");
		}

		[Test]
		public void ShouldGetDocumentWithAnonimousTypeInlineReference()
		{
			//Given

			dynamic review = new
							 {
								 Author = new
										  {
											  Nick = "Kolyan",
											  Name = "Super javascripter"
										  }
							 };

			string reviewId = _api.SetDocument("gameshop", "review", review).Id.ToString();

			//When
			dynamic item = _api.GetDocumentById("gameshop", "review", reviewId);

			//Then
			Assert.IsNotNull(item);
			Assert.AreEqual(item.Author.Nick, "Kolyan");
			Assert.AreEqual(item.Author.Name, "Super javascripter");
		}

		[Test]
		public void ShouldGetDocumentWithArrayOfResolvableLinks()
		{
			//Given
			dynamic rootComment = new
								  {
									  Likes = 10,
									  Text = "I am first!"
								  };

			string rootCommentId = _api.SetDocument("gameshop", "comment", rootComment).Id.ToString();

			dynamic childComment = new
								   {
									   Likes = -1,
									   Text = "Oh, you foolin!",
									   ParentComment = new
													   {
														   Id = rootCommentId,
														   DisplayName = "I am first! (10)"
													   }
								   };

			string childCommentId = _api.SetDocument("gameshop", "comment", childComment).Id.ToString();

			dynamic childOfChildComment = new
										  {
											  Likes = -199,
											  Text = "you too!",
											  ParentComment = new
															  {
																  Id = childCommentId,
																  DisplayName = "you too! (-199)"
															  }
										  };

			string childOfChildCommentId = _api.SetDocument("gameshop", "comment", childOfChildComment).Id.ToString();

			dynamic review = new
							 {
								 Comments = new[]
				                            {
					                            new
					                            {
						                            Id = rootCommentId
					                            },
					                            new
					                            {
						                            Id = childCommentId
					                            },
					                            new
					                            {
						                            Id = childOfChildCommentId
					                            }
				                            }
							 };

			string reviewId = _api.SetDocument("gameshop", "review", review).Id.ToString();

			//When
			dynamic item = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetDocumentById("gameshop", "review", reviewId).ToString());
			//Then
			Assert.IsNotNull(item);
			Assert.AreEqual(item.Comments.Count, 3);
		}


		[Test]
		public void ShouldGetDocumentWithArrayOfResolvableLinksWithFilterById()
		{
			//Given
			dynamic rootComment = new
								  {
									  Likes = 10,
									  Text = "I am first!"
								  };

			string rootCommentId = _api.SetDocument("gameshop", "comment", rootComment).Id.ToString();

			dynamic childComment = new
								   {
									   Likes = -1,
									   Text = "Oh, you foolin!",
									   ParentComment = new
													   {
														   Id = rootCommentId,
														   DisplayName = "I am first! (10)"
													   }
								   };

			string childCommentId = _api.SetDocument("gameshop", "comment", childComment).Id.ToString();

			dynamic childOfChildComment = new
										  {
											  Likes = -199,
											  Text = "you too!",
											  ParentComment = new
															  {
																  Id = childCommentId,
																  DisplayName = "Oh, you foolin! (-1)"
															  }
										  };

			string childOfChildCommentId = _api.SetDocument("gameshop", "comment", childOfChildComment).Id.ToString();

			dynamic review = new
							 {
								 Comments = new[]
				                            {
					                            new
					                            {
						                            Id = rootCommentId
					                            },
					                            new
					                            {
						                            Id = childCommentId
					                            },
					                            new
					                            {
						                            Id = childOfChildCommentId
					                            }
				                            }
							 };

			_api.SetDocument("gameshop", "review", review).Id.ToString();

			//When
			dynamic item = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetDocument("gameshop", "review", f => f.AddCriteria(cr => cr.Property("Comments.Id").IsEquals(childCommentId)), 0, 1).FirstOrDefault().ToString());
			//Then
			Assert.IsNotNull(item);
			Assert.AreEqual(item.Comments.Count, 3);
		}

		[Test]
		public void ShouldGetDocumentWithArrayOfInlineStrongTypeDocuments()
		{
			//Given
			dynamic review = new
							 {
								 AdditionalInfo = new[]
				                                  {
					                                  new
					                                  {
						                                  InfoSection = "Разработчик",
						                                  InfoContent = "MachineGames"
					                                  },
					                                  new
					                                  {
						                                  InfoSection = "Издатель",
						                                  InfoContent = "Bethesda"
					                                  }
				                                  }
							 };

			string reviewId = _api.SetDocument("gameshop", "review", review).Id.ToString();
			//When
			dynamic item = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetDocumentById("gameshop", "review", reviewId).ToString());
			//Then
			Assert.IsNotNull(item);
			Assert.AreEqual(item.AdditionalInfo.Count, 2);
			Assert.AreEqual(item.AdditionalInfo[0].InfoSection, "Разработчик");
			Assert.AreEqual(item.AdditionalInfo[0].InfoContent, "MachineGames");
			Assert.AreEqual(item.AdditionalInfo[1].InfoSection, "Издатель");
			Assert.AreEqual(item.AdditionalInfo[1].InfoContent, "Bethesda");
		}

		[Test]
		public void ShouldGetDocumentWithArrayOfInlineAnonimousTypeDocuments()
		{
			//Given
			dynamic lottery = new
							  {
								  Users = new[]
				                          {
					                          new
					                          {
						                          Id = Guid.NewGuid().ToString(),
						                          DisplayName = "SomeUser1"
					                          },
					                          new
					                          {
						                          Id = Guid.NewGuid().ToString(),
						                          DisplayName = "SomeUser2"
					                          }
				                          }
							  };

			string lotteryId = _api.SetDocument("gameshop", "lottery", lottery).Id.ToString();
			//When
			dynamic item = _api.GetDocumentById("gameshop", "lottery", lotteryId);
			//Then
			Assert.IsNotNull(item);
			Assert.AreEqual(item.Users.Count, 2);

			Assert.IsNotNull(item.Users[0].Id);
			Assert.IsNotNull(item.Users[0].DisplayName);

			Assert.IsNotNull(item.Users[1].Id);
			Assert.IsNotNull(item.Users[1].DisplayName);
		}

		[Test]
		public void ShouldGetDocumentWithCycleReferenceToStrongTypeDocument()
		{
			//Given
			dynamic game = new
						   {
							   Id = Guid.NewGuid().ToString(),
							   Name = "Bioshock Infinite"
						   };

			dynamic game1 = new
							{
								Id = Guid.NewGuid().ToString(),
								Name = "Bioshock Infinite: Burial in the Sea",
								SimilarGames = new[]
				                               {
					                               new
					                               {
						                               game.Id,
						                               DisplayName = game.Name
					                               }
				                               }
							};


			//When
			_api.SetDocument("gameshop", "catalogue", game).Id.ToString();
			_api.SetDocument("gameshop", "catalogue", game1).Id.ToString();

			dynamic item = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetDocumentById("gameshop", "catalogue", game1.Id).ToString());
			//Then
			Assert.IsNotNull(item);
			Assert.AreEqual(item.SimilarGames.Count, 1);
			Assert.AreEqual(item.SimilarGames[0].Id, game.Id);
			Assert.AreEqual(item.SimilarGames[0].DisplayName, game.Name);
		}
	}
}