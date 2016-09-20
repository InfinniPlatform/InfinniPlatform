using System.IO;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.PrintView.Contract
{
    /// <summary>
    /// ������������� ������ ��� �������� �������� �������������.
    /// </summary>
    public interface IPrintViewBuilder
    {
        /// <summary>
        /// ������� ���� ��������� �������������.
        /// </summary>
        /// <param name="printViewTemplate">������ ��������� �������������.</param>
        /// <param name="printViewSource">������ ��������� �������������.</param>
        /// <param name="printViewFormat">������ ��������� �������������.</param>
        /// <returns>���� ��������� �������������.</returns>
        byte[] Build(Stream printViewTemplate, object printViewSource, PrintViewFileFormat printViewFormat = PrintViewFileFormat.Pdf);

        /// <summary>
        /// ������� ���� ��������� �������������.
        /// </summary>
        /// <param name="printViewTemplate">������ ��������� �������������.</param>
        /// <param name="printViewSource">������ ��������� �������������.</param>
        /// <param name="printViewFormat">������ ��������� �������������.</param>
        /// <returns>���� ��������� �������������.</returns>
        byte[] Build(DynamicWrapper printViewTemplate, object printViewSource, PrintViewFileFormat printViewFormat = PrintViewFileFormat.Pdf);
    }
}