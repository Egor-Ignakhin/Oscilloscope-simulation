using System;

using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    /// <summary>
    /// Интерактивный тумблер
    /// </summary>
    internal sealed class ToggleSwitchInteractable : Interactable, ILogicalValue
    {
        public bool LogicalValue { get; set; } = false;
        public Action<bool> ChangeValueEvent { get; set; }

        [SerializeField] private Vector3 angleOfRotationDuringOperation;
        [SerializeField] private Vector3 angleOfRotationDuringRest;

        private void Start()
        {
            Switch(false);
        }

        internal override void Interact()
        {
            //Переключаем значение тумблера на противоположное,
            //при взаимодействии с ним
            Switch(!LogicalValue);
        }

        private void Switch(bool state)
        {
            LogicalValue = state;

            //Поворачиваем тумблер в зависимости от его состояния
            transform.localEulerAngles = LogicalValue ? angleOfRotationDuringOperation : angleOfRotationDuringRest;

            ChangeValueEvent?.Invoke(LogicalValue);
        }
    }
}