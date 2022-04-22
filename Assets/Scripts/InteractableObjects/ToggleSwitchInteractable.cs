using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    /// <summary>
    /// Интерактивный тумблер
    /// </summary>
    internal sealed class ToggleSwitchInteractable : Interactable
    {
        /// <summary>
        /// Тумблер включен?
        /// </summary>
        private bool isEnabled = false;
        /// <summary>
        /// Положение тумблера во включенном и 
        /// выключенных состояниих соответственно
        /// </summary>
        [SerializeField] private Vector3 enabledLocalEulersState;
        [SerializeField] private Vector3 disabledLocalEulersState;

        private void Start()
        {
            //Изначальное положение тумблера устанавливаем как FALSE
            Switch(isEnabled);
        }

        internal override void Interact()
        {
            //Переключаем значение тумблера на противоположное,
            //при взаимодействии с ним
            Switch(!isEnabled);
        }

        /// <summary>
        /// Метод переключения значения тумблера: {FALSE, TRUE}
        /// </summary>
        /// <param name="nextState"></param>
        private void Switch(bool nextState)
        {
            //Устанавливаем в память объекта новое значение состояния тумблера
            isEnabled = nextState;

            //Поворачиваем тумблер в зависимости от его состояния
            transform.localEulerAngles = isEnabled ? enabledLocalEulersState : disabledLocalEulersState;
        }
    }
}