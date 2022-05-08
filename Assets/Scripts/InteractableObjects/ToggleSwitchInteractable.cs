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

        [SerializeField] private Vector3 angleOfRotationDuringOperation;
        [SerializeField] private Vector3 angleOfRotationDuringRest;

        public bool LogicalValue { get; set; } = false;

        private void Start()
        {
            //����������� ��������� �������� ������������� ��� FALSE
            Switch(LogicalValue);
        }

        internal override void Interact()
        {
            //����������� �������� �������� �� ���������������,
            //��� �������������� � ���
            Switch(!LogicalValue);
        }

        /// <summary>
        /// ����� ������������ �������� ��������: {FALSE, TRUE}
        /// </summary>
        /// <param name="nextState"></param>
        private void Switch(bool nextState)
        {
            //������������� � ������ ������� ����� �������� ��������� ��������
            LogicalValue = nextState;

            //������������ ������� � ����������� �� ��� ���������
            transform.localEulerAngles = LogicalValue ? angleOfRotationDuringOperation : angleOfRotationDuringRest;

            ChangeValueEvent?.Invoke(LogicalValue);
        }
    }
}