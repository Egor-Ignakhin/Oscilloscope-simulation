using System;
/// <summary>
/// Наследуемая сущность хранит логическое значение, 
/// а также свойства для доступа к нему
/// </summary>
public interface ILogicalValue
{
    public bool LogicalValue { get; set; }

    public Action<bool> ChangeValueEvent { get; set; }
}
