using System;

using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    /// <summary>
    /// ������������� �������
    /// </summary>
    internal sealed class ToggleSwitchInteractable : Interactable, ILogicalValue
    {
        private bool logicalValue = false;
        public Action<bool> ChangeValueEvent { get; set; }

        [SerializeField] private Vector3 angleOfRotationDuringOperation;
        [SerializeField] private Vector3 angleOfRotationDuringRest;

        private void Start()
        {
            Switch(false);
        }

        internal override void Interact()
        {
            Switch(!logicalValue);
        }

        private void Switch(bool state)
        {
            logicalValue = state;

            //������������ ������� � ����������� �� ��� ���������
            transform.localEulerAngles = logicalValue ?
                angleOfRotationDuringOperation : angleOfRotationDuringRest;

            ChangeValueEvent?.Invoke(logicalValue);
        }

        bool ILogicalValue.GetLogicalValue()
        {
            return logicalValue;
        }

        public void SetLogicalValue(bool value)
        {
            logicalValue = value;
        }
    }
}