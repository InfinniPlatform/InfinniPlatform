using System;
using System.Reflection;

using InfinniPlatform.Dynamic;

using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Предоставляет интерфейс сериализации и десериализации <see cref="DynamicWrapper"/> для MongoDB.
    /// </summary>
    internal class MongoDynamicWrapperBsonSerializationProvider : IBsonSerializationProvider
    {
        public static readonly MongoDynamicWrapperBsonSerializationProvider Default = new MongoDynamicWrapperBsonSerializationProvider();


        public IBsonSerializer GetSerializer(Type type)
        {
            if (typeof(DynamicWrapper).GetTypeInfo().IsAssignableFrom(type))
            {
                return MongoDynamicWrapperBsonSerializer.Default;
            }

            return null;
        }
    }
}