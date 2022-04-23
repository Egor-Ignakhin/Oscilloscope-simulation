using System;
/// <summary>
/// Наследуемая сущность хранит логическое значение, 
/// а также свойства для доступа к нему
/// </summary>
public interface ILogicalValue
{
    public bool Value { get; set; }

    /// <summary>
    /// Событие вызываемое при каждом изменении лог. значения
    /// </summary>
    public Action<bool> ChangeValueEvent { get; set; }
}
