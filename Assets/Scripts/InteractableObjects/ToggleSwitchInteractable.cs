using System;

using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    /// <summary>
    /// Интерактивный тумблер
    /// </summary>
    internal sealed class ToggleSwitchInteractable : Interactable, ILogicalValue
    {
        public Action<bool> ChangeValueEvent { get; set; }

        /// <summary>
        /// Положение тумблера во включенном и 
        /// выключенных состояниих соответственно
        /// </summary>
        [SerializeField] private Vector3 enabledLocalEulersState;
        [SerializeField] private Vector3 disabledLocalEulersState;

        public bool Value { get; set; } = false;

        private void Start()
        {
            //Изначальное положение тумблера устанавливаем как FALSE
            Switch(Value);
        }

        internal override void Interact()
        {
            //Переключаем значение тумблера на противоположное,
            //при взаимодействии с ним
            Switch(!Value);
        }

        /// <summary>
        /// Метод переключения значения тумблера: {FALSE, TRUE}
        /// </summary>
        /// <param name="nextState"></param>
        private void Switch(bool nextState)
        {
            //Устанавливаем в память объекта новое значение состояния тумблера
            Value = nextState;

            //Поворачиваем тумблер в зависимости от его состояния
            transform.localEulerAngles = Value ? enabledLocalEulersState : disabledLocalEulersState;

            ChangeValueEvent?.Invoke(Value);
        }
    }
}