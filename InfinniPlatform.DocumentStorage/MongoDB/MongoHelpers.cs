using InfinniPlatform.Sdk.Documents;

using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    internal static class MongoHelpers
    {
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