using System;

using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    /// <summary>
    /// ������������� �������
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
            //����������� �������� �������� �� ���������������,
            //��� �������������� � ���
            Switch(!LogicalValue);
        }

        private void Switch(bool state)
        {
            LogicalValue = state;

            //������������ ������� � ����������� �� ��� ���������
            transform.localEulerAngles = LogicalValue ? angleOfRotationDuringOperation : angleOfRotationDuringRest;

            ChangeValueEvent?.Invoke(LogicalValue);
        }
    }
}