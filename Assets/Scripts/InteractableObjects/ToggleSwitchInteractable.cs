using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    /// <summary>
    /// ������������� �������
    /// </summary>
    internal sealed class ToggleSwitchInteractable : Interactable
    {
        /// <summary>
        /// ������� �������?
        /// </summary>
        private bool isEnabled = false;
        /// <summary>
        /// ��������� �������� �� ���������� � 
        /// ����������� ���������� ��������������
        /// </summary>
        [SerializeField] private Vector3 enabledLocalEulersState;
        [SerializeField] private Vector3 disabledLocalEulersState;

        private void Start()
        {
            //����������� ��������� �������� ������������� ��� FALSE
            Switch(isEnabled);
        }

        internal override void Interact()
        {
            //����������� �������� �������� �� ���������������,
            //��� �������������� � ���
            Switch(!isEnabled);
        }

        /// <summary>
        /// ����� ������������ �������� ��������: {FALSE, TRUE}
        /// </summary>
        /// <param name="nextState"></param>
        private void Switch(bool nextState)
        {
            //������������� � ������ ������� ����� �������� ��������� ��������
            isEnabled = nextState;

            //������������ ������� � ����������� �� ��� ���������
            transform.localEulerAngles = isEnabled ? enabledLocalEulersState : disabledLocalEulersState;
        }
    }
}