using System;

using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    /// <summary>
    /// ������������� �������
    /// </summary>
    internal sealed class ToggleSwitchInteractable : Interactable, ILogicalValue
    {
        public Action<bool> ChangeValueEvent { get; set; }

        /// <summary>
        /// ��������� �������� �� ���������� � 
        /// ����������� ���������� ��������������
        /// </summary>
        [SerializeField] private Vector3 enabledLocalEulersState;
        [SerializeField] private Vector3 disabledLocalEulersState;

        public bool Value { get; set; } = false;

        private void Start()
        {
            //����������� ��������� �������� ������������� ��� FALSE
            Switch(Value);
        }

        internal override void Interact()
        {
            //����������� �������� �������� �� ���������������,
            //��� �������������� � ���
            Switch(!Value);
        }

        /// <summary>
        /// ����� ������������ �������� ��������: {FALSE, TRUE}
        /// </summary>
        /// <param name="nextState"></param>
        private void Switch(bool nextState)
        {
            //������������� � ������ ������� ����� �������� ��������� ��������
            Value = nextState;

            //������������ ������� � ����������� �� ��� ���������
            transform.localEulerAngles = Value ? enabledLocalEulersState : disabledLocalEulersState;

            ChangeValueEvent?.Invoke(Value);
        }
    }
}