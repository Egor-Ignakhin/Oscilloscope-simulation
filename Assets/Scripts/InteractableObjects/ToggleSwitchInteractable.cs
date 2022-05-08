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

        [SerializeField] private Vector3 angleOfRotationDuringOperation;
        [SerializeField] private Vector3 angleOfRotationDuringRest;

        public bool LogicalValue { get; set; } = false;

        private void Start()
        {
            //Изначальное положение тумблера устанавливаем как FALSE
            Switch(LogicalValue);
        }

        internal override void Interact()
        {
            //Переключаем значение тумблера на противоположное,
            //при взаимодействии с ним
            Switch(!LogicalValue);
        }

        /// <summary>
        /// Метод переключения значения тумблера: {FALSE, TRUE}
        /// </summary>
        /// <param name="nextState"></param>
        private void Switch(bool nextState)
        {
            //Устанавливаем в память объекта новое значение состояния тумблера
            LogicalValue = nextState;

            //Поворачиваем тумблер в зависимости от его состояния
            transform.localEulerAngles = LogicalValue ? angleOfRotationDuringOperation : angleOfRotationDuringRest;

            ChangeValueEvent?.Invoke(LogicalValue);
        }
    }
}