using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using InfinniPlatform.Sdk.Api;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
    //[Ignore("Тесты SDK не выполняют запуск сервера InfinniPlatform. Необходимо существование уже запущенного сервера на localhost : 9900")]
    [TestFixture]
    public class DocumentApiTest
    {
        private const string InfinniSessionPort = "9900";
        private const string InfinniSessionServer = "localhost";
        private const string InfinniSessionVersion = "1";
        private InfinniDocumentApi _api;

        [TestFixtureSetUp]
        public void SetupApi()
        {
            _api = new InfinniDocumentApi(InfinniSessionServer, InfinniSessionPort, InfinniSessionVersion);
        }

        [Test]
        public void ShouldGetDocument()
        {
            _api.GetDocument("gameshop", "catalogue",
                 f => f.AddCriteria(cr => cr.Property("Name").IsContains("gta")), 0, 100,
                 s => s.AddSorting("Price", "descending"));
        }

        [Test]
        public void ShouldSetDocument()
        {
            var documentObject = new
            {
                Name = "gta vice city",
                Price = 100.50
            };

            var result = _api.SetDocument("gameshop", "catalogue", Guid.NewGuid().ToString(), documentObject);
            Assert.True(!string.IsNullOrEmpty(result));
        }

        [Test]
        public void ShouldSetDocumentWithIncorrectMapping()
        {
            var documentObject = new
            {
                Name = "gta vice city",
                Price = "someStringValueThatNotConvertToFloat"  //string value but float in schema
            };

            var ex = Assert.Throws<ArgumentException>(() => _api.SetDocument("gameshop", "catalogue", Guid.NewGuid().ToString(), documentObject));
            Assert.AreEqual(ex.Message, "Fail to set document with exception: There an business logic error on request execution./r/nAdditional info: ﻿{\"Error\":\"Fail to commit transaction: \\r\\nExpected value for field 'Price' should have Float type, but value has System.String type ('someStringValueThatNotConvertToFloat')\"}");
        }

        [Test]
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

            var result = _api.SetDocument("gameshop", "catalogue", Guid.NewGuid().ToString(), documentObject);

            dynamic document = _api.GetDocument("gameshop", "catalogue", null, 0, 1).FirstOrDefault();

            Assert.IsNotNull(document);

            var changesObject = new
            {
                Name = "gta V ultimate edition",
                Availability = new
                {
                    Available = true,
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

            var result = _api.SetDocument("gameshop", "catalogue", Guid.NewGuid().ToString(), documentObject);

            IEnumerable<dynamic> docs = _api.GetDocument("gameshop", "catalogue",
                f => f.AddCriteria(cr => cr.Property("Id").IsEquals(result)), 0, 1);

            Assert.True(docs.Any());

            dynamic deleteResult = JsonConvert.DeserializeObject<ExpandoObject>(_api.DeleteDocument("gameshop", "catalogue", result).Content.ToString());

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

            var result = _api.SetDocument("gameshop", "catalogue", Guid.NewGuid().ToString(), documentObject);

            dynamic persistentDocument = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetDocumentById("gameshop", "catalogue", result).ToString());

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
        public void ShouldUseSession()
        {
            var session = _api.CreateSession();

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

            dynamic attachResult = JsonConvert.DeserializeObject<ExpandoObject>(_api.Attach(session, "gameshop", "catalogue",documentObject.Id, documentObject).Content.ToString());

            Assert.AreEqual(attachResult.IsValid, true);

            dynamic sessionItems = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetSession(session).Content.ToString()).Items;

            Assert.AreEqual(sessionItems.Count, 1);

            _api.Attach(session, "gameshop", "catalogue",additionalObject.Id, additionalObject);
            _api.Attach(session, "gameshop", "catalogue",failObject.Id, failObject);

            _api.Detach(session, failObject.Id);

            sessionItems = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetSession(session).Content.ToString()).Items;
            Assert.AreEqual(sessionItems.Count, 2);

            _api.SaveSession(session);

            sessionItems = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetSession(session).Content.ToString()).Items;
            Assert.AreEqual(sessionItems.Count, 0);

            var savedObject1 = _api.GetDocumentById("gameshop", "catalogue", documentObject.Id);
            var savedObject2 = _api.GetDocumentById("gameshop", "catalogue", additionalObject.Id);
            var unsavedObject3 = _api.GetDocumentById("gameshop", "catalogue", failObject.Id);

            Assert.IsNotNull(savedObject1);
            Assert.IsNotNull(savedObject2);
            Assert.IsNull(unsavedObject3);
        }

        [Test]
        public void ShouldRemoveTransaction()
        {
            var session = _api.CreateSession();

            var documentObject = new
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Homeworld remastered",
                Price = 400
            };

            _api.Attach(session, "gameshop", "catalogue", documentObject.Id, documentObject);

            _api.RemoveSession(session);

            var sessionItems = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetSession(session).Content.ToString()).Items;

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

            string id = _api.SetDocument("gameshop", "catalogue", Guid.NewGuid().ToString(), game);

            dynamic review = new
            {
                Game = new
                {
                    Id = id,
                    DisplayName = "X-Com:Enemy Within"
                }
            };

            string reviewId = _api.SetDocument("gameshop", "review", Guid.NewGuid().ToString(), review);

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

            string rateId = _api.SetDocument("gameshop", "gamerating", Guid.NewGuid().ToString(), rate);


            dynamic review = new
            {
                Rate = new
                {
                    Id = rateId,
                    DisplayName = "UserRate:90, ReviewRate:95"
                }
            };

            string reviewId = _api.SetDocument("gameshop", "review", Guid.NewGuid().ToString(), review);

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

            string gameId = _api.SetDocument("gameshop", "catalogue", Guid.NewGuid().ToString(), game);

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


            string reviewId = _api.SetDocument("gameshop", "review", Guid.NewGuid().ToString(), review);

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

            string reviewId = _api.SetDocument("gameshop", "review", Guid.NewGuid().ToString(), review);

            //When
            dynamic item = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetDocumentById("gameshop", "review", reviewId).ToString());

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

            string rootCommentId = _api.SetDocument("gameshop", "comment", Guid.NewGuid().ToString(), rootComment);

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

            string childCommentId = _api.SetDocument("gameshop", "comment", Guid.NewGuid().ToString(), childComment);

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

            string childOfChildCommentId = _api.SetDocument("gameshop", "comment", Guid.NewGuid().ToString(), childOfChildComment);

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

            string reviewId = _api.SetDocument("gameshop", "review", Guid.NewGuid().ToString(), review);

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

            string rootCommentId = _api.SetDocument("gameshop", "comment", Guid.NewGuid().ToString(), rootComment);

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

            string childCommentId = _api.SetDocument("gameshop", "comment", Guid.NewGuid().ToString(), childComment);

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

            string childOfChildCommentId = _api.SetDocument("gameshop", "comment", Guid.NewGuid().ToString(), childOfChildComment);

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

            _api.SetDocument("gameshop", "review", Guid.NewGuid().ToString(), review);

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

            string reviewId = _api.SetDocument("gameshop", "review", Guid.NewGuid().ToString(), review);
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

            string lotteryId = _api.SetDocument("gameshop", "lottery", Guid.NewGuid().ToString(), lottery);
            //When
            dynamic item = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetDocumentById("gameshop", "lottery", lotteryId).ToString());
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
                SimilarGames = new[] {
                    new {
                       Id = game.Id,
                       DisplayName = game.Name
                    }
                }
            };


            //When
            _api.SetDocument("gameshop", "catalogue", game.Id, game);
            _api.SetDocument("gameshop", "catalogue", game1.Id, game1);

            dynamic item = JsonConvert.DeserializeObject<ExpandoObject>(_api.GetDocumentById("gameshop", "catalogue", game1.Id).ToString());
            //Then
            Assert.IsNotNull(item);
            Assert.AreEqual(item.SimilarGames.Count, 1);
            Assert.AreEqual(item.SimilarGames[0].Id, game.Id);
            Assert.AreEqual(item.SimilarGames[0].DisplayName, game.Name);
        }


    }


}
