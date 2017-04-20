using System.Collections.Generic;
using System.Threading.Tasks;

using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Указатель на список документов MongoDB.
    /// </summary>
    internal class MongoDocumentCursor<TDocument> : IDocumentCursor<TDocument>
    {
        protected MongoDocumentCursor()
        {
        }

        public MongoDocumentCursor(IAsyncCursor<TDocument> cursor)
        {
            Cursor = cursor;
        }


        protected virtual IAsyncCursor<TDocument> Cursor { get; }


        public bool Any()
        {
            return Cursor.Any();
        }

        public Task<bool> AnyAsync()
        {
            return Cursor.AnyAsync();
        }


        public TDocument First()
        {
            return Cursor.First();
        }

        public Task<TDocument> FirstAsync()
        {
            return Cursor.FirstAsync();
        }


        public TDocument FirstOrDefault()
        {
            return Cursor.FirstOrDefault();
        }

        public Task<TDocument> FirstOrDefaultAsync()
        {
            return Cursor.FirstOrDefaultAsync();
        }


        public List<TDocument> ToList()
        {
            return Cursor.ToList();
        }

        public Task<List<TDocument>> ToListAsync()
        {
            return Cursor.ToListAsync();
        }
    }
}