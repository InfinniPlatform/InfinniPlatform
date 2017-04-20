using System;
using System.Linq.Expressions;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage
{
    internal static class MongoHelpers
    {
        public static string GetPropertyName<T>(Expression<Func<T, object>> property)
        {
            return ((MemberExpression)property.Body).Member.Name;
        }


        public static ProjectionDefinition<TSource, TProjection> Combine<TSource, TProjection>(params ProjectionDefinition<TSource, TProjection>[] projections)
        {
            if (projections.Length > 1)
            {
                var combineProjectionDocument = new BsonDocument();
                var sourceSerializer = BsonSerializer.LookupSerializer<TSource>();
                var serializerRegistry = BsonSerializer.SerializerRegistry;

                foreach (var projectionDefinition in projections)
                {
                    foreach (var element in projectionDefinition.Render(sourceSerializer, serializerRegistry).Document.Elements)
                    {
                        combineProjectionDocument.Remove(element.Name);
                        combineProjectionDocument.Add(element);
                    }
                }

                return new BsonDocumentProjectionDefinition<TSource, TProjection>(combineProjectionDocument);
            }

            return projections[0];
        }


        public static DocumentUpdateResult CreateUpdateResult(UpdateResult mongoResult, bool insertIfNotExists)
        {
            var modifiedCount = (mongoResult.IsModifiedCountAvailable && mongoResult.ModifiedCount > 0)
                ? mongoResult.ModifiedCount
                : 0;

            var updateStatus = (insertIfNotExists && mongoResult.UpsertedId != null)
                ? DocumentUpdateStatus.Inserted
                : (modifiedCount > 0)
                    ? DocumentUpdateStatus.Updated
                    : DocumentUpdateStatus.None;

            return new DocumentUpdateResult(mongoResult.MatchedCount, modifiedCount, updateStatus);
        }

        public static DocumentUpdateResult CreateReplaceResult(ReplaceOneResult mongoResult, bool insertIfNotExists)
        {
            var modifiedCount = (mongoResult.IsModifiedCountAvailable && mongoResult.ModifiedCount > 0)
                ? mongoResult.ModifiedCount
                : 0;

            var updateStatus = (insertIfNotExists && mongoResult.UpsertedId != null)
                ? DocumentUpdateStatus.Inserted
                : (modifiedCount > 0)
                    ? DocumentUpdateStatus.Updated
                    : DocumentUpdateStatus.None;

            return new DocumentUpdateResult(mongoResult.MatchedCount, modifiedCount, updateStatus);
        }
    }
}