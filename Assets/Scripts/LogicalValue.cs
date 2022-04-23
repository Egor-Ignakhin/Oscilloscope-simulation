using System;
/// <summary>
/// ����������� �������� ������ ���������� ��������, 
/// � ����� �������� ��� ������� � ����
/// </summary>
public interface ILogicalValue
{
    public bool Value { get; set; }

    /// <summary>
    /// ������� ���������� ��� ������ ��������� ���. ��������
    /// </summary>
    public Action<bool> ChangeValueEvent { get; set; }
}
