using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace InfinniPlatform.Sdk.Security
{
    public static class SecurityExtensions
    {
        /// <summary>
        /// Возвращает уникальный идентификатор пользователя.
        /// </summary>
        /// <param name="identity">Объект идентификации.</param>
        public static string GetUserId(this IIdentity identity)
        {
            return (identity != null && identity.IsAuthenticated) ? FindFirstClaim(identity, ClaimTypes.NameIdentifier) : null;
        }


        /// <summary>
        /// Проверяет наличие заданного типа утверждения.
        /// </summary>
        /// <param name="identity">Объект идентификации.</param>
        /// <param name="claimType">Тип утверждения (например, <see cref="ClaimTypes" />).</param>
        public static bool HasClaim(this IIdentity identity, string claimType)
        {
            return identity.AsClaimsIdentity(claimType, (ci, ct) => ci.FindFirst(ct) != null);
        }

        /// <summary>
        /// Проверяет наличие заданного типа утверждения.
        /// </summary>
        /// <param name="identity">Объект идентификации.</param>
        /// <param name="claimValue">Значение утверждения.</param>
        public static bool HasClaim(this IIdentity identity, string claimType, string claimValue)
        {
            return identity.AsClaimsIdentity(claimType, (ci, ct) => ci.HasClaim(ct, claimValue));
        }

        /// <summary>
        /// Проверяет наличие всех заданных типов утверждений.
        /// </summary>
        /// <param name="identity">Объект идентификации.</param>
        /// <param name="claimTypes">Типы утверждений (например, <see cref="ClaimTypes" />).</param>
        public static bool HasAllClaims(this IIdentity identity, IEnumerable<string> claimTypes)
        {
            return identity.AsClaimsIdentity(claimTypes, (ci, ct) => (ct == null) || ct.All(t => ci.FindFirst(t) != null));
        }

        /// <summary>
        /// Проверяет наличие любого заданного типа утверждения.
        /// </summary>
        /// <param name="identity">Объект идентификации.</param>
        /// <param name="claimTypes">Типы утверждений (например, <see cref="ClaimTypes" />).</param>
        public static bool HasAnyClaims(this IIdentity identity, IEnumerable<string> claimTypes)
        {
            return identity.AsClaimsIdentity(claimTypes, (ci, ct) => (ct != null) && ct.Any(t => ci.FindFirst(t) != null));
        }


        /// <summary>
        /// Возвращает первое утверждение заданного типа.
        /// </summary>
        /// <param name="identity">Объект идентификации.</param>
        /// <param name="claimType">Тип утверждения (например, <see cref="ClaimTypes" />).</param>
        /// <returns>Значение утверждения.</returns>
        public static string FindFirstClaim(this IIdentity identity, string claimType)
        {
            return identity.AsClaimsIdentity(claimType, (ci, ct) =>
                                                        {
                                                            var claim = ci.FindFirst(ct);
                                                            return GetClaimValue(claim);
                                                        });
        }


        /// <summary>
        /// Возвращает первое утверждение заданного типа.
        /// </summary>
        /// <typeparam name="T">Тип значения утверждения.</typeparam>
        /// <param name="identity">Объект идентификации.</param>
        /// <param name="claimType">Тип утверждения (например, <see cref="ClaimTypes" />).</param>
        /// <returns>Значение утверждения.</returns>
        public static T FindFirstClaim<T>(this IIdentity identity, string claimType)
        {
            return identity.AsClaimsIdentity(claimType, (ci, ct) =>
                                                        {
                                                            var claim = ci.FindFirst(ct);
                                                            return GetClaimValue<T>(claim);
                                                        });
        }

        /// <summary>
        /// Возвращает все утверждения заданного типа.
        /// </summary>
        /// <param name="identity">Объект идентификации.</param>
        /// <param name="claimType">Тип утверждения (например, <see cref="ClaimTypes" />).</param>
        /// <returns>Значения утверждений.</returns>
        public static IEnumerable<string> FindAllClaims(this IIdentity identity, string claimType)
        {
            return identity.AsClaimsIdentity(claimType, (ci, ct) =>
                                                        {
                                                            var claims = ci.FindAll(ct);
                                                            return claims.Select(GetClaimValue);
                                                        });
        }

        /// <summary>
        /// Возвращает все утверждения заданного типа.
        /// </summary>
        /// <typeparam name="T">Тип значения утверждения.</typeparam>
        /// <param name="identity">Объект идентификации.</param>
        /// <param name="claimType">Тип утверждения (например, <see cref="ClaimTypes" />).</param>
        /// <returns>Значения утверждений.</returns>
        public static IEnumerable<T> FindAllClaims<T>(this IIdentity identity, string claimType)
        {
            return identity.AsClaimsIdentity(claimType, (ci, ct) =>
                                                        {
                                                            var claims = ci.FindAll(ct);
                                                            return claims.Select(GetClaimValue<T>);
                                                        });
        }


        /// <summary>
        /// Добавляет утверждение заданного типа.
        /// </summary>
        /// <param name="identity">Объект идентификации.</param>
        /// <param name="claimType">Тип утверждения (например, <see cref="ClaimTypes" />).</param>
        /// <param name="claimValue">Значение утверждения.</param>
        public static void AddClaim(this IIdentity identity, string claimType, string claimValue)
        {
            identity.AsClaimsIdentity(claimType, claimValue, (ci, ct, cv) =>
                                                             {
                                                                 if (cv != null)
                                                                 {
                                                                     ci.AddClaim(new Claim(ct, cv));
                                                                 }
                                                             });
        }

        /// <summary>
        /// Заменяет все утверждения заданного типа.
        /// </summary>
        /// <param name="identity">Объект идентификации.</param>
        /// <param name="claimType">Тип утверждения (например, <see cref="ClaimTypes" />).</param>
        /// <param name="claimValue">Значение утверждения.</param>
        public static void SetClaim(this IIdentity identity, string claimType, string claimValue)
        {
            identity.AsClaimsIdentity(claimType, claimValue, (ci, ct, cv) =>
                                                             {
                                                                 var claims = ci.FindAll(ct);

                                                                 foreach (var c in claims)
                                                                 {
                                                                     ci.TryRemoveClaim(c);
                                                                 }

                                                                 if (cv != null)
                                                                 {
                                                                     ci.AddClaim(new Claim(ct, cv));
                                                                 }
                                                             });
        }

        /// <summary>
        /// Добавляет или обновляет утверждения заданного типа.
        /// </summary>
        /// <param name="identity">Объект идентификации.</param>
        /// <param name="claimType">Тип утверждения (например, <see cref="ClaimTypes" />).</param>
        /// <param name="claimValue">Значение утверждения.</param>
        public static void AddOrUpdateClaim(this IIdentity identity, string claimType, string claimValue)
        {
            identity.AsClaimsIdentity(claimType, claimValue, (ci, ct, cv) =>
                                                             {
                                                                 if (cv != null)
                                                                 {
                                                                     var claims = ci.FindAll(ct);

                                                                     if (!claims.Any(c => string.Equals(c.Value, cv)))
                                                                     {
                                                                         ci.AddClaim(new Claim(ct, cv));
                                                                     }
                                                                 }
                                                             });
        }

        /// <summary>
        /// Удаляет утверждения заданного типа.
        /// </summary>
        /// <param name="identity">Объект идентификации.</param>
        /// <param name="claimType">Тип утверждения (например, <see cref="ClaimTypes" />).</param>
        /// <param name="claimValueMatch">Функция выборки утверждений, которые следует удалить.</param>
        public static void RemoveClaims(this IIdentity identity, string claimType, Func<string, bool> claimValueMatch = null)
        {
            identity.AsClaimsIdentity(claimType, null, (ci, ct, cv) =>
                                                       {
                                                           var claims = ci.FindAll(ct);

                                                           foreach (var c in claims)
                                                           {
                                                               if (claimValueMatch == null || claimValueMatch(c.Value))
                                                               {
                                                                   ci.TryRemoveClaim(c);
                                                               }
                                                           }
                                                       });
        }


        private static TResult AsClaimsIdentity<T, TResult>(this IIdentity identity, T parameter, Func<ClaimsIdentity, T, TResult> action)
        {
            var claimsIdentity = identity as ClaimsIdentity;

            if (claimsIdentity != null && !Equals(parameter, null) && !Equals(parameter, ""))
            {
                return action(claimsIdentity, parameter);
            }

            return default(TResult);
        }

        private static void AsClaimsIdentity(this IIdentity identity, string claimType, string claimValue, Action<ClaimsIdentity, string, string> action)
        {
            var claimsIdentity = identity as ClaimsIdentity;

            if (claimsIdentity != null && !string.IsNullOrEmpty(claimType))
            {
                action(claimsIdentity, claimType, claimValue);
            }
        }

        private static T GetClaimValue<T>(Claim claim)
        {
            var claimValue = GetClaimValue(claim);

            if (!string.IsNullOrEmpty(claimValue))
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                return (T)converter.ConvertFromInvariantString(claimValue);
            }

            return default(T);
        }

        private static string GetClaimValue(Claim claim)
        {
            return (claim != null) ? claim.Value : null;
        }
    }
}