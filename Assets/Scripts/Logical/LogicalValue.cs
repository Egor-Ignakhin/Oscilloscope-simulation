using System;
/// <summary>
/// ����������� �������� ������ ���������� ��������, 
/// � ����� �������� ��� ������� � ����
/// </summary>
public interface ILogicalValue
{
    public bool Value { get; set; }

    public Action<bool> ChangeValueEvent { get; set; }
}
