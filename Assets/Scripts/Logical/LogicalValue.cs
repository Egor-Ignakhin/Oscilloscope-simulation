using System;
/// <summary>
/// Наследуемая сущность хранит логическое значение, 
/// а также свойства для доступа к нему
/// </summary>
public interface ILogicalValue
{
    bool GetLogicalValue();    
    void SetLogicalValue(bool value);

    public Action<bool> ChangeValueEvent { get; set; }
}
