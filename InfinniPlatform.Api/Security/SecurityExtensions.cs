using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

using InfinniPlatform.Api.Serialization;

namespace InfinniPlatform.Api.Security
{
	/// <summary>
	/// Вспомогательные методы для работы с подсистемой безопасности.
	/// </summary>
	public static class SecurityExtensions
	{
		private static readonly JsonObjectSerializer JsonObjectSerializer = JsonObjectSerializer.Default;


		// IIdentity and Claims

		/// <summary>
		/// Возвращает первое утверждение заданного типа.
		/// </summary>
		/// <param name="identity">Объект идентификации.</param>
		/// <param name="claimType">Тип утверждения (например, <see cref="ClaimTypes"/> или <see cref="ApplicationClaimTypes"/>).</param>
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
		/// <param name="claimType">Тип утверждения (например, <see cref="ClaimTypes"/> или <see cref="ApplicationClaimTypes"/>).</param>
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
		/// <param name="claimType">Тип утверждения (например, <see cref="ClaimTypes"/> или <see cref="ApplicationClaimTypes"/>).</param>
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
		/// <param name="claimType">Тип утверждения (например, <see cref="ClaimTypes"/> или <see cref="ApplicationClaimTypes"/>).</param>
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
		/// <param name="claimType">Тип утверждения (например, <see cref="ClaimTypes"/> или <see cref="ApplicationClaimTypes"/>).</param>
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
		/// Добавляет или обновляет утверждения заданного типа.
		/// </summary>
		/// <param name="identity">Объект идентификации.</param>
		/// <param name="claimType">Тип утверждения (например, <see cref="ClaimTypes"/> или <see cref="ApplicationClaimTypes"/>).</param>
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
		/// Устанавливает утверждение заданного типа.
		/// </summary>
		/// <param name="identity">Объект идентификации.</param>
		/// <param name="claimType">Тип утверждения (например, <see cref="ClaimTypes"/> или <see cref="ApplicationClaimTypes"/>).</param>
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
		/// Удаляет утверждения заданного типа.
		/// </summary>
		/// <param name="identity">Объект идентификации.</param>
		/// <param name="claimType">Тип утверждения (например, <see cref="ClaimTypes"/> или <see cref="ApplicationClaimTypes"/>).</param>
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

		private static TResult AsClaimsIdentity<TResult>(this IIdentity identity, string claimType, Func<ClaimsIdentity, string, TResult> action)
		{
			var claimsIdentity = identity as ClaimsIdentity;

			if (claimsIdentity != null && !string.IsNullOrEmpty(claimType))
			{
				action(claimsIdentity, claimType);
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
			if (claim != null && !string.IsNullOrEmpty(claim.Value))
			{
				var converter = TypeDescriptor.GetConverter(typeof(T));
				return (T)converter.ConvertFromInvariantString(claim.Value);
			}

			return default(T);
		}

		private static string GetClaimValue(Claim claim)
		{
			if (claim != null)
			{
				return claim.Value;
			}

			return null;
		}


		// Security Model

		/// <summary>
		/// Привести объект к типу <see cref="ApplicationUser"/>.
		/// </summary>
		public static ApplicationUser ToApplicationUser(this object value)
		{
			return (ApplicationUser)JsonObjectSerializer.ConvertFromDynamic(value, typeof(ApplicationUser));
		}

		/// <summary>
		/// Привести объект к типу <see cref="ApplicationRole"/>.
		/// </summary>
		public static ApplicationRole ToApplicationRole(this object value)
		{
			return (ApplicationRole)JsonObjectSerializer.ConvertFromDynamic(value, typeof(ApplicationRole));
		}

		/// <summary>
		/// Привести объект к типу <see cref="ApplicationClaimType"/>.
		/// </summary>
		public static ApplicationClaimType ToApplicationClaimType(this object value)
		{
			return (ApplicationClaimType)JsonObjectSerializer.ConvertFromDynamic(value, typeof(ApplicationClaimType));
		}
	}
}