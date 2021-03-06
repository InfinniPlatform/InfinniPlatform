﻿using System;

using InfinniPlatform.Types;

using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Предоставляет интерфейс сериализации и десериализации <see cref="Date"/> для MongoDB.
    /// </summary>
    internal class MongoDateBsonSerializationProvider : IBsonSerializationProvider
    {
        public static readonly MongoDateBsonSerializationProvider Default = new MongoDateBsonSerializationProvider();


        public IBsonSerializer GetSerializer(Type type)
        {
            if (type == typeof(Date))
            {
                return MongoDateBsonSerializer.Default;
            }

            return null;
        }
    }
}