using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.Validation;

namespace InfinniPlatform.RestfulApi.Utils
{
    public sealed class AuthUtils
    {
	    private readonly ISecurityComponent _securityComponent;
	    private readonly string _userName;
	    private readonly IEnumerable<dynamic> _filteredRoles;

	    public AuthUtils(ISecurityComponent securityComponent, string userName, IEnumerable<dynamic> filteredRoles)
	    {
		    _securityComponent = securityComponent;
		    _userName = userName;
		    _filteredRoles = filteredRoles;
	    }

	    public ValidationResult CheckDocumentAccess(string configId, string documentId, string action, string recordId)
        {
	        if (_userName == AuthorizationStorageExtensions.AdminRole ||
	            _userName == AuthorizationStorageExtensions.AdminUser)
	        {
                return new ValidationResult()
                {
                    IsValid = true
                };
	        }

            IEnumerable<dynamic> roles = _securityComponent.Roles ?? new List<dynamic>();
			//роли по умолчанию
			var defaultRoles = roles.Where(r => r.UserName == "Default").ToList();

			//роли, назначенные пользователям
			var userRoles = _filteredRoles ??  roles.Where(r => r.UserName == _userName).ToList();

		    var havingRoles = defaultRoles.Concat(userRoles);  //roles.Where(r => r.UserName == _target.UserName || r.UserName == "Default").ToList();

            IEnumerable<dynamic> allAcl = _securityComponent.Acl ?? new List<dynamic>();

            //получаем правила авторизации для всех пользователей и конкретного указанного пользователя
			var userAcl = allAcl.Where(a => a.UserName == _userName || a.UserName == "Default").ToList();

            foreach (dynamic havingRole in havingRoles)
            {
                userAcl.AddRange(allAcl.Where(a => a.UserName == havingRole.RoleName));
            }

            //проверка наличия запретов на конкретного пользователя
            var denyUser = CheckAcl(userAcl.ToArray(), () => new ValidationResult(), configId, documentId, action, recordId);

            //проверка наличия разрешения для конкретного пользователя
			var enableUser = CheckAcl(userAcl.ToArray(), () => new ValidationResult(false)
				                                                   {
					                                                   Items = new List<dynamic>()
						                                                           {
							                                                          FormatAuthMessage(_userName, configId, documentId, action, recordId)
						                                                           }
				                                                   }, configId, documentId, action, recordId);

            //модель безопасности следующая:
            /*
             * 1. Если не установлено ни разрешений ни запретов - доступ всем пользователям разрешен
             * 2. Если установлен запрет для всех пользователей и нет разрешения для конкретного пользователя - доступ запрещен
             * 3. Если установлен запрет для конкретного пользователя и установлено разрешение для конкретного пользователя - доступ разрешен
             * 4. Если установлен запрет для всех пользователей и установлено разрешение для конкретного пользователя - доступ разрешен
             * 5. Если установлено разрешение для всех пользователей и установлен запрет для конкретного пользователя - доступ запрещен
             */

		    var validationResult = new ValidationResult();
            //если присутствует запрет для пользователей по умолчанию или запрет на конкретного пользователя и при этом не существует разрешения для конкретного пользователя			
            if (!denyUser.IsValid)
            {
                if (!enableUser.IsValid)
                {
                    validationResult.IsValid = false;
                    validationResult.Items.Add(denyUser.Items.FirstOrDefault());
                }
            }
		    return validationResult;
        }


        private ValidationResult CheckAcl(dynamic[] useraclList, Func<ValidationResult> defaultResult, string configId, string documentId, string action, string recordId)
        {

	        configId = string.IsNullOrEmpty(configId) ? null : configId.ToLowerInvariant();
			documentId = string.IsNullOrEmpty(documentId) ? null : documentId.ToLowerInvariant();
			action = string.IsNullOrEmpty(action) ? null : action.ToLowerInvariant();
			recordId = string.IsNullOrEmpty(recordId) ? null : recordId.ToLowerInvariant();

            var result = defaultResult();

            if (!string.IsNullOrEmpty(configId))
            {
				var aclList = useraclList.Where(
					uac => !string.IsNullOrEmpty(uac.Configuration) && uac.Configuration.ToLowerInvariant() == configId && 
						string.IsNullOrEmpty(uac.Metadata)).ToList();

                //проверяем доступ к конфигурации
                var authConfiguration = aclList.Any(item => item.Result) || !aclList.Any();

                if (!authConfiguration)
                {
                    result.IsValid = false;
                    result.Items.Clear();
					result.Items.Add(FormatAuthMessage(_userName, configId, documentId, action, recordId));

                    if (string.IsNullOrEmpty(documentId))
                    {
                        return result;
                    }

                }


                //проверяем доступ для документа
                if (!string.IsNullOrEmpty(documentId))
                {
	                aclList = useraclList.Where(uac =>
	                                            !string.IsNullOrEmpty(uac.Configuration) &&
	                                            uac.Configuration.ToLowerInvariant() == configId &&
	                                            !string.IsNullOrEmpty(uac.Metadata) &&
	                                            uac.Metadata.ToLowerInvariant() == documentId &&
	                                            string.IsNullOrEmpty(uac.Action)).ToList();

	                //если существует хотя бы одно правило в acl, отменяем предыдущий result
	                if (aclList.Any())
	                {
		                result = defaultResult();
	                }

	                var authMetadata = aclList.Any(item => item.Result) || !aclList.Any();

	                if (!authMetadata)
	                {
		                result.IsValid = false;
		                result.Items.Clear();
		                result.Items.Add(
			                FormatAuthMessage(_userName, configId, documentId, action, recordId));

		                if (string.IsNullOrEmpty(action))
		                {
			                return result;
		                }
	                }


	                //проверяем выполняемое действие
	                if (action != null)
	                {
		                aclList = useraclList.Where(
			                uac =>
			                !string.IsNullOrEmpty(uac.Configuration) && uac.Configuration.ToLowerInvariant() == configId &&
			                !string.IsNullOrEmpty(uac.Metadata) && uac.Metadata.ToLowerInvariant() == documentId &&
			                !string.IsNullOrEmpty(uac.Action) && uac.Action.ToLowerInvariant() == action &&
			                string.IsNullOrEmpty(uac.RecordId)).ToList();

		                //если существует хотя бы одно правило в acl, отменяем предыдущий result
		                if (aclList.Any())
		                {
			                result = defaultResult();
		                }

		                var authAction = aclList.Any(item => item.Result) || !aclList.Any();

		                if (!authAction)
		                {
			                result.IsValid = false;
			                result.Items.Clear();
			                result.Items.Add(FormatAuthMessage(_userName, configId, documentId, action, recordId));
			                if (string.IsNullOrEmpty(recordId))
			                {
				                return result;
			                }
		                }


		                //проверяем конкретную запись
		                if (recordId != null)
		                {
			                aclList = useraclList.Where(uac =>
			                                            !string.IsNullOrEmpty(uac.Configuration) &&
			                                            uac.Configuration.ToLowerInvariant() == configId &&
			                                            !string.IsNullOrEmpty(uac.Metadata) &&
			                                            uac.Metadata.ToLowerInvariant() == documentId &&
			                                            !string.IsNullOrEmpty(uac.Action) &&
			                                            uac.Action.ToLowerInvariant() == action &&
			                                            !string.IsNullOrEmpty(uac.RecordId) &&
			                                            uac.RecordId.ToLowerInvariant() == recordId).ToList();

			                //если существует хотя бы одно правило в acl, отменяем предыдущий result
			                if (aclList.Any())
			                {
				                result = defaultResult();
			                }

			                if (!aclList.Any())
			                {
				                return result;
			                }


			                var authRecord = aclList.Any(item => item.Result) || !aclList.Any();

			                if (!authRecord)
			                {
				                result.IsValid = false;
				                result.Items.Clear();
				                result.Items.Add(
					                FormatAuthMessage(_userName, configId, documentId, action, recordId));
				                return result;
			                }
							else if (aclList.Any(item => item.Result))
							{
								result.IsValid = true;
							}
		                }
						else if (aclList.Any(item => item.Result))
						{
							result.IsValid = true;
						}
	                }
					else if (aclList.Any(item => item.Result))
					{
						result.IsValid = true;
					}
                }
				else if (aclList.Any(item => item.Result))
				{
					result.IsValid = true;
				}
            }
            return result;
        }

		private string FormatAuthMessage(string userName, string configId, string documentId, string action, string recordId)
        {
            if (!string.IsNullOrEmpty(configId) &&
                string.IsNullOrEmpty(documentId) &&
                string.IsNullOrEmpty(action) &&
                string.IsNullOrEmpty(recordId))
            {
                return string.Format(Resources.AccessDeniedToConfiguration, userName, configId);
            }
            if (!string.IsNullOrEmpty(configId) &&
                !string.IsNullOrEmpty(documentId) &&
                string.IsNullOrEmpty(action) &&
                string.IsNullOrEmpty(recordId)
                )
            {
                return string.Format(Resources.AccessDeniedToDocument, userName, configId, documentId);
            }
            if (!string.IsNullOrEmpty(configId) &&
                !string.IsNullOrEmpty(documentId) &&
                !string.IsNullOrEmpty(action) &&
                string.IsNullOrEmpty(recordId)
                )
            {
                return string.Format(Resources.AccessDeniedToAction, userName, configId, documentId, action);
            }
            if (!string.IsNullOrEmpty(configId) &&
                !string.IsNullOrEmpty(documentId) &&
                !string.IsNullOrEmpty(action) &&
                !string.IsNullOrEmpty(recordId))
            {
                return string.Format(
                    Resources.AccessDeniedToDocumentInstance,
                    _userName, configId, documentId, action,
                    recordId);
            }
            return string.Empty;
        }

    }
}
