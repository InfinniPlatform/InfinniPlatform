﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using InfinniPlatform.Cache.Properties;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace InfinniPlatform.Cache
{
    /// <summary>
    /// Фабрика подключений к Redis.
    /// </summary>
    public class RedisConnectionFactory
    {
        public RedisConnectionFactory(RedisSharedCacheOptions options, ILogger<RedisConnectionFactory> logger)
        {
            var redisHost = string.IsNullOrEmpty(options.Host)
                                ? RedisSharedCacheOptions.Default.Host
                                : options.Host;

            var redisPort = options.Port <= 0
                                ? RedisSharedCacheOptions.Default.Port
                                : options.Port;

            var writeBufferSize = options.WriteBufferSize <= 0
                                      ? RedisSharedCacheOptions.Default.WriteBufferSize
                                      : options.WriteBufferSize;

            var connectionTimeout = options.ConnectionTimeout < 0
                                        ? RedisSharedCacheOptions.Default.ConnectionTimeout
                                        : options.ConnectionTimeout;

            var maxReconnectRetries = options.MaxReconnectRetries <= 0
                                          ? RedisSharedCacheOptions.Default.MaxReconnectRetries
                                          : options.MaxReconnectRetries;

            var configurationOptions = new ConfigurationOptions
                                       {
                                           EndPoints = { { TryResolveIPv4(redisHost), redisPort } },
                                           ConnectTimeout = connectionTimeout,
                                           ConnectRetry = maxReconnectRetries,
                                           AbortOnConnectFail = false,
                                           WriteBuffer = writeBufferSize,
                                           Password = options.Password,
                                           AllowAdmin = true
                                       };
            RedisClient = new Lazy<ConnectionMultiplexer>(()=> Connect(logger, configurationOptions, redisHost, redisPort));
        }

        private static ConnectionMultiplexer Connect(ILogger<RedisConnectionFactory> logger, ConfigurationOptions configurationOptions, string redisHost, int redisPort)
        {
            ConnectionMultiplexer redisClient = ConnectionMultiplexer.Connect(configurationOptions);

            if (redisClient.IsConnected)
            {
                Func<Dictionary<string, object>> logContext = () => new Dictionary<string, object>
                {
                    { "status", redisClient.GetStatus()},
                    {"endPoint", $"{redisHost}:{redisPort}"}
                };

                logger.LogError(Resources.RedisConnectionFailed, logContext);
            }

            

            return redisClient;
        }

        private static string TryResolveIPv4(string host)
        {
            IPAddress hostIPv4 = null;

            if (!IsIPv4Address(host))
            {
                var hostEntry = Dns.GetHostEntryAsync(host).GetAwaiter().GetResult();

                hostIPv4 = hostEntry.AddressList.FirstOrDefault(i => IsIPv4Address(i.ToString()));
            }

            return (hostIPv4 != null) ? hostIPv4.ToString() : host;
        }

        private static bool IsIPv4Address(string address)
        {
            return Regex.IsMatch(address, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
        }


        public Lazy<ConnectionMultiplexer> RedisClient { get; }
    }
}