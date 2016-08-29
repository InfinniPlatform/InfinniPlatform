using System;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// ������������� ����� ������� ��� ����������� ������� ��������� ������� � ����� ��� ������.
    /// </summary>
    public interface ICronExpressionDayOfWeekBuilder
    {
        /// <summary>
        /// ������ ���� ������.
        /// </summary>
        /// <remarks>
        /// � CRON-��������� '*'.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder Every();

        /// <summary>
        /// ������ ��������� ���� ������.
        /// </summary>
        /// <param name="dayOfWeek">���� ������.</param>
        /// <remarks>
        /// � CRON-��������� 'D', ��� D - ���� ������ <paramref name="dayOfWeek"/> �� 1 (�����������) �� 7 (�������).
        /// ���� �������� <paramref name="dayOfWeek"/> ����� <see cref="DayOfWeek.Friday"/>, �� ������� ������ �����������
        /// ������ �������.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder Each(DayOfWeek dayOfWeek);

        /// <summary>
        /// ������ ��������� ���� ������ � ����� �������� �������� ����� ����.
        /// </summary>
        /// <param name="dayOfWeek">���� ������.</param>
        /// <param name="interval">�������� � ����.</param>
        /// <remarks>
        /// � CRON-��������� 'D/I', ��� D - ���� ������ <paramref name="dayOfWeek"/> �� 1 (�����������) �� 7 (�������), I - �������� � ����
        /// <paramref name="interval"/>. ���� �������� <paramref name="dayOfWeek"/> ����� <see cref="DayOfWeek.Tuesday"/>, � ��������
        /// <paramref name="interval"/> ����� 2, �� ������� ������ ����������� �� �������, ������� � �������.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder Each(DayOfWeek dayOfWeek, int interval);

        /// <summary>
        /// ������ ���� ������ �� ���������� ������.
        /// </summary>
        /// <param name="daysOfWeek">������ ���� ������.</param>
        /// <remarks>
        /// � CRON-��������� 'D1,D2,D3,...,Dn', ��� D1, D2, D3, ..., Dn - ��� ������ ������ <paramref name="daysOfWeek"/>. ���� ��������
        /// <paramref name="daysOfWeek"/> ������������ �������� <c>new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday }</c>,
        /// �� ������� ������ ����������� � �����������, ������� � �����.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder EachOfSet(params DayOfWeek[] daysOfWeek);

        /// <summary>
        /// ������ ���� ������ �� ���������� ���������.
        /// </summary>
        /// <param name="dayOfWeekFrom">������ ��������� ���� ������.</param>
        /// <param name="dayOfWeekTo">����� ��������� ���� ������.</param>
        /// <remarks>
        /// � CRON-��������� 'D1-D2', ��� D1 � D2 - �������������� ������ <paramref name="dayOfWeekFrom"/> � ����� <paramref name="dayOfWeekTo"/>
        /// ��������� ���� ������. ���� �������� <paramref name="dayOfWeekFrom"/> ����� <see cref="DayOfWeek.Monday"/>, � ��������
        /// <paramref name="dayOfWeekTo"/> ����� <see cref="DayOfWeek.Wednesday"/>, �� ������� ������ ����������� � �����������,
        /// ������� � �����.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder EachOfRange(DayOfWeek dayOfWeekFrom, DayOfWeek dayOfWeekTo);

        /// <summary>
        /// ������ ��������� ��������� ���� ������ � ������.
        /// </summary>
        /// <param name="dayOfWeek">���� ������.</param>
        /// <remarks>
        /// � CRON-��������� 'DL', ��� D - ���� ������ <paramref name="dayOfWeek"/> �� 1 (�����������) �� 7 (�������).
        /// ���� �������� <paramref name="dayOfWeek"/> ����� <see cref="DayOfWeek.Friday"/>, �� ������� ������
        /// ����������� � ��������� ������� ������.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder EachLast(DayOfWeek dayOfWeek);

        /// <summary>
        /// ������ N-� ��������� ���� ������ � ������.
        /// </summary>
        /// <param name="dayOfWeek">���� ������.</param>
        /// <param name="orderNumber">���������� ����� ��� ������ � ������ (������� � 1).</param>
        /// <remarks>
        /// � CRON-��������� 'D#n', ��� D - ���� ������ <paramref name="dayOfWeek"/> �� 1 (�����������) �� 7 (�������),
        /// n - ����� ��� ������ � ������ <paramref name="orderNumber"/>. ���� �������� <paramref name="dayOfWeek"/>
        /// ����� <see cref="DayOfWeek.Friday"/>, � �������� <paramref name="orderNumber"/> ����� 1, �� ������� ������
        /// ����������� � ������ ������� ������.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder EachNth(DayOfWeek dayOfWeek, int orderNumber);
    }
}