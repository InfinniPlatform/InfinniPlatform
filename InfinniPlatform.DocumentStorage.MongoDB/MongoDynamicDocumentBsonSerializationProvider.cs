using System;
using System.Reflection;

using InfinniPlatform.Dynamic;

using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Предоставляет интерфейс сериализации и десериализации <see cref="DynamicDocument"/> для MongoDB.
    /// </summary>
    internal class MongoDynamicDocumentBsonSerializationProvider : IBsonSerializationProvider
    {
        public static readonly MongoDynamicDocumentBsonSerializationProvider Default = new MongoDynamicDocumentBsonSerializationProvider();


        public IBsonSerializer GetSerializer(Type type)
        {
            if (typeof(DynamicDocument).GetTypeInfo().IsAssignableFrom(type))
            {
                return MongoDynamicDocumentBsonSerializer.Default;
            }

            return null;
        }
    }
}