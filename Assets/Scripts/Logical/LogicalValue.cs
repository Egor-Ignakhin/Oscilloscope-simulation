using System;
/// <summary>
/// ����������� �������� ������ ���������� ��������, 
/// � ����� �������� ��� ������� � ����
/// </summary>
public interface ILogicalValue
{
    bool GetLogicalValue();    
    void SetLogicalValue(bool value);

    public Action<bool> ChangeValueEvent { get; set; }
}
