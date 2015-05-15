using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Api.RestApi.AuthApi
{
	/// <summary>
	///    API для работы с Access control list
	/// </summary>
	public sealed class AclApi
	{
		/// <summary>
		///   Предоставить доступ пользователю
		/// </summary>
		/// <param name="configuration">Конфигурация, к которой предоставляется доступ</param>
		/// <param name="metadata">Метаданные, к которым предоставляется доступ</param>
		/// <param name="action">Действие, для которого предоставляется доступ</param>
		/// <param name="recordId">Экземпляр метаданных, к которым предоставлен доступ</param>
		/// <param name="userName">Пользователь, которому предоставляется доступ</param>
		public dynamic GrantAccess(string userName, string configuration, string metadata = null, string action = null, string recordId = null)
		{
		    return GrantAccessTo(userName, configuration, metadata, action, recordId);
		}

		/// <summary>
		///   Предоставить доступ пользователю
		/// </summary>
		/// <param name="configuration">Конфигурация, к которой предоставляется доступ</param>
		/// <param name="metadata">Метаданные, к которым предоставляется доступ</param>
		/// <param name="action">Действие, для которого предоставляется доступ</param>
		/// <param name="recordId">Экземпляр метаданных, к которым предоставлен доступ</param>
		public dynamic GrantAccessAll(string configuration, string metadata = null, string action = null, string recordId = null)
		{
			return GrantAccessTo(AuthorizationStorageExtensions.Default, configuration, metadata, action, recordId);
		}

	    private static dynamic GrantAccessTo(string accessObject, string configuration, string metadata, string action, string recordId)
	    {
	        return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "applyaccess", null, new
	            {
	                Configuration = configuration,
	                Metadata = metadata,
	                Action = action,
	                RecordId = recordId,
	                UserName = accessObject,
	                Result = true
	            }).ToDynamic();
	    }

	    /// <summary>
        ///   Предоставить доступ пользователю
        /// </summary>
        /// <param name="configuration">Конфигурация, к которой предоставляется доступ</param>
        /// <param name="metadata">Метаданные, к которым предоставляется доступ</param>
        /// <param name="action">Действие, для которого предоставляется доступ</param>
        /// <param name="recordId">Экземпляр метаданных, к которым предоставлен доступ</param>
        /// <param name="roleName">Роль, которой предоставляется доступ</param>
		public dynamic GrantAccessRole(string roleName, string configuration, string metadata = null, string action = null, string recordId = null)
        {
            return GrantAccessTo(roleName, configuration, metadata, action, recordId);
        }

		/// <summary>
		///   Заблокировать доступ пользователю 
		/// </summary>
		/// <param name="configuration">Конфигурация, к которой заблокирован доступ</param>
		/// <param name="metadata">Метаданные, к которым заблокирован доступ</param>
		/// <param name="action">Действие, к которому заблокирован доступ</param>
		/// <param name="recordId">Запись, к которой заблокирован доступ</param>
		/// <param name="userName">Пользователь, которому заблокирован доступ</param>
		public dynamic DenyAccess(string userName, string configuration, string metadata = null, string action = null, string recordId = null)
		{
		    return DenyAccessTo(userName, configuration, metadata, action, recordId);
		}

		private static dynamic DenyAccessTo(string accessObject, string configuration, string metadata, string action, string recordId)
	    {
	        return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "applyaccess", null, new
	            {
	                Configuration = configuration,
	                Metadata = metadata,
	                Action = action,
	                RecordId = recordId,
	                UserName = accessObject,
	                Result = false
	            }).ToDynamic();
	    }

	    /// <summary>
        ///   Заблокировать доступ пользователю 
        /// </summary>
        /// <param name="configuration">Конфигурация, к которой заблокирован доступ</param>
        /// <param name="metadata">Метаданные, к которым заблокирован доступ</param>
        /// <param name="action">Действие, к которому заблокирован доступ</param>
        /// <param name="recordId">Запись, к которой заблокирован доступ</param>
        /// <param name="roleName">Роль, которой заблокирован доступ</param>
		public dynamic DenyAccessRole(string roleName, string configuration, string metadata = null, string action = null, string recordId = null)
        {
            return DenyAccessTo(roleName, configuration, metadata, action, recordId);
        }

		/// <summary>
		///   Заблокировать доступ всем пользователям
		/// </summary>
		/// <param name="configuration">Конфигурация, к которой заблокирован доступ</param>
		/// <param name="metadata">Метаданные, к которым заблокирован доступ</param>
		/// <param name="action">Действие, к которому заблокирован доступ</param>
		/// <param name="recordId">Запись, к которой заблокирован доступ</param>
		public dynamic DenyAccessAll(string configuration, string metadata = null, string action = null, string recordId = null)
		{
            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "applyaccess", null, new
			{
				Configuration = configuration,
				Metadata = metadata,
				Action = action,
				RecordId = recordId,
                UserName = AuthorizationStorageExtensions.Default,
                Result = false
			}).ToDynamic();
		}

		/// <summary>
		///   Добавить пользователя во внутреннее хранилище
		/// </summary>
		/// <param name="userName">Пользователь</param>
		/// <param name="password">пароль</param>
		public dynamic AddUser(string userName, string password)
		{
			return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "adduser", null, new
			{
				UserName = userName,
				Password = password
			}).ToDynamic();		
		}

        /// <summary>
        ///   Установить значение утверждения для пользователя
        /// </summary>
        /// <param name="userName">Пользователь</param>
        /// <param name="claimType">Тип утверждения</param>
        /// <param name="claimValue">Значение утверждения</param>
        public void AddClaim(string userName, string claimType, string claimValue)
        {
            RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "addclaim", null, new
                {
                    UserName = userName,
                    ClaimType = claimType,
                    ClaimValue = claimValue
                });
        }

	    /// <summary>
	    ///   Добавить роль
	    /// </summary>
	    /// <param name="roleName">Роль</param>
	    /// <param name="roleCaption"></param>
	    /// <param name="roleDescription"></param>
		public dynamic AddRole(string roleName, string roleCaption, string roleDescription)
        {
            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "addrole", null, new
            {
                Name = roleName,
                Caption = roleCaption,
                Description = roleDescription
            }).ToDynamic();
        }

        /// <summary>
        ///   Добавить пользователю роль
        /// </summary>
        /// <param name="userName">Пользователь</param>
        /// <param name="roleName">Роль</param>
		public dynamic AddUserToRole(string userName, string roleName)
        {
            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "adduserrole", null, new
            {
                UserName = userName,
                RoleName = roleName
            }).ToDynamic();	
        }

		/// <summary>
		///   Удалить пользователя из внутреннего хранилища
		/// </summary>
		/// <param name="userName">Пользователь</param>
		public dynamic RemoveUser(string userName)
		{
			return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "removeuser", null, new
			{
				UserName = userName,
			}).ToDynamic();
		}


		/// <summary>
		///   Удалить роль
		/// </summary>
		/// <param name="roleName">Роль</param>
		public dynamic RemoveRole(string roleName)
		{
			return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "removerole", null, new
			{
				RoleName = roleName,
			}).ToDynamic();
		}

		/// <summary>
		///   Удалить роль пользователя
		/// </summary>
		/// <param name="userName">Пользователь</param>
		/// <param name="roleName">Роль</param>
		public dynamic RemoveUserRole(string userName, string roleName)
		{
			return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "removeuserrole", null, new
			{
				UserName = userName,
				RoleName = roleName
			}).ToDynamic();
		}

		/// <summary>
		///   Удалить ACL правило
		/// </summary>
		/// <param name="id">Идентификатор ACL</param>
		public dynamic RemoveAcl(string id)
		{
			return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "removeacl", null,new
			{
				AclId = id
			}).ToDynamic();	
		}

        /// <summary>
        ///   Получить список доступных ролей пользователей
        /// </summary>
        /// <returns>Список ролей пользователей</returns>
	    public IEnumerable<dynamic> GetRoles()
	    {
            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "getroles", null, null).ToDynamicList();	
	    }

		/// <summary>
		///   Получить список связанных с пользователями ролей
		/// </summary>
		/// <returns>Список связок</returns>
		public IEnumerable<dynamic> GetUserRoles()
		{
			return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "getuserroles", null, null).ToDynamicList();
		}

		/// <summary>
		///   Получить список прав доступа для всех пользователей
		/// </summary>
		/// <returns></returns>
	    public IEnumerable<dynamic> GetAcl()
	    {
            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "getacl", null, null).ToDynamicList();	
	    }

		/// <summary>
		///   Получить список зарегистрированных пользователей
		/// </summary>
		/// <returns></returns>
		public IEnumerable<dynamic> GetUsers()
		{
			return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "getusers", null, null).ToDynamicList();
		}

        /// <summary>
        ///   Обновить список доступных ролей пользователей на сервере
        /// </summary>
        /// <returns>Список ролей пользователей</returns>
        public void UpdateRoles()
        {
            RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "updateroles", null, null);
        }

        /// <summary>
        ///   Обновить ACL на сервере
        /// </summary>
        /// <returns></returns>
        public void UpdateAcl()
        {
            RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "updateacl", null, null);
        }

		/// <summary>
		///   Обновить утверждения относительно пользователей на сервере
		/// </summary>
		public void UpdateUserClaims()
		{
			RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "updateuserclaims", null, null);
		}

	}
}
