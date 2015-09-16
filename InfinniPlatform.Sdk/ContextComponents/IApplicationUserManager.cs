using System.Collections.Generic;

namespace InfinniPlatform.Sdk.ContextComponents
{
	/// <summary>
	/// ������������� ������ ���������� ����������� �������� ������������.
	/// </summary>
	public interface IApplicationUserManager
	{
		/// <summary>
		/// ���������� �������� � ������������.
		/// </summary>
		object GetCurrentUser();


		/// <summary>
		/// ���������, ��� ������������ ����� ������.
		/// </summary>
		bool HasPassword();

		/// <summary>
		/// ��������� ������������ ������.
		/// </summary>
		void AddPassword(string password);

		/// <summary>
		/// ������� � ������������ ������.
		/// </summary>
		void RemovePassword();

		/// <summary>
		/// �������� ������������ ������.
		/// </summary>
		void ChangePassword(string currentPassword, string newPassword);


		/// <summary>
		/// ���������� ����� ��������� �������� ������������.
		/// </summary>
		string GetSecurityStamp();

		/// <summary>
		/// ��������� ����� ��������� �������� ������������.
		/// </summary>
		void UpdateSecurityStamp();


		/// <summary>
		/// ���������� ����������� ����� ������������.
		/// </summary>
		string GetEmail();

		/// <summary>
		/// ������������� ����������� ����� ������������.
		/// </summary>
		void SetEmail(string email);

		/// <summary>
		/// ���������, ��� ����������� ����� ������������ ������������.
		/// </summary>
		bool IsEmailConfirmed();


		/// <summary>
		/// ���������� ����� �������� ������������.
		/// </summary>
		string GetPhoneNumber();

		/// <summary>
		/// ������������� ����� �������� ������������.
		/// </summary>
		void SetPhoneNumber(string phoneNumber);

		/// <summary>
		/// ���������, ��� ����� �������� ������������ �����������.
		/// </summary>
		bool IsPhoneNumberConfirmed();


		/// <summary>
		/// ��������� ��� ����� ������������ ��� �������� ����������.
		/// </summary>
		void AddLogin(string loginProvider, string providerKey);

		/// <summary>
		/// ������� ��� ����� ������������ ��� �������� ����������.
		/// </summary>
		void RemoveLogin(string loginProvider, string providerKey);


		/// <summary>
		/// ��������� ����������� ������������.
		/// </summary>
		void AddClaim(string claimType, string claimValue);

		/// <summary>
		/// ������� ����������� ������������.
		/// </summary>
		void RemoveClaim(string claimType, string claimValue);


		/// <summary>
		/// ���������, ��� ������������ ������ � ����.
		/// </summary>
		bool IsInRole(string role);

		/// <summary>
		/// ��������� ������������ � ����.
		/// </summary>
		void AddToRole(string role);

		/// <summary>
		/// ��������� ������������ � ����.
		/// </summary>
		void AddToRoles(params string[] roles);

		/// <summary>
		/// ��������� ������������ � ����.
		/// </summary>
		void AddToRoles(IEnumerable<string> roles);

		/// <summary>
		/// ������� ������������ �� ����.
		/// </summary>
		void RemoveFromRole(string role);

		/// <summary>
		/// ������� ������������ �� �����.
		/// </summary>
		void RemoveFromRoles(params string[] roles);

		/// <summary>
		/// ������� ������������ �� �����.
		/// </summary>
		void RemoveFromRoles(IEnumerable<string> roles);
	}
}