using System;
/// <summary>
/// ����������� �������� ������ ���������� ��������, 
/// � ����� �������� ��� ������� � ����
/// </summary>
public interface ILogicalValue
{
    public bool LogicalValue { get; set; }

    public Action<bool> ChangeValueEvent { get; set; }
}
