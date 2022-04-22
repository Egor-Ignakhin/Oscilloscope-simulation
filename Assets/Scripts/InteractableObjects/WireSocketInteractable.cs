
using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    /// <summary>
    /// ����� ��� ����������� �������
    /// </summary>
    internal sealed class WireSocketInteractable : Interactable
    {
        /// <summary>
        /// ����� ��������� ������� �������
        /// </summary>
        [SerializeField] private Transform positionForWireConnector;

        /// <summary>
        /// ������������ ������
        /// </summary>
        private Wire connectedWire;

        /// <summary>
        /// �������� ��������
        /// </summary>
        private WiresManager wiresManager;

        private void Start()
        {
            //������� ��� �������� ����� �������� ��������
            wiresManager = FindObjectOfType<WiresManager>();
        }

        internal override void Interact()
        {
            //���� � ����� ��� ��������� ������
            if (connectedWire)
            {
                //���� ������ ��������� ������ � ���� �����
                if (connectedWire.UserInteractingWithTheWire)
                {
                    //��������� ��������� ������ �� �������
                    DisconnectWireAbs();
                }
                else
                {
                    //���� ������ ��������� �� ������ � ���� �����
                    // ��������� ������ �� ���������� ������
                    DisconnectWireFromThisPoint();
                }
            }
            else
            {
                // ���� � ����� ��� �� ��������� ������ - 
                // ���������� ������
                ConnectWire();
            }
        }



        /// <summary>
        /// ����� ����������� ������� � ������
        /// </summary>
        public void ConnectWire()
        {
            connectedWire = wiresManager.ConnectWire(positionForWireConnector);
        }

        /// <summary>
        /// ����� ������� ���������� ������� �� �������
        /// </summary>
        public void DisconnectWireAbs()
        {
            wiresManager.DisconnectAbs(connectedWire);
            connectedWire = null;
        }

        /// <summary>
        /// ����� ���������� ������� �� ���������� ������
        /// </summary>
        private void DisconnectWireFromThisPoint()
        {
            wiresManager.DisconnectFromPoint(positionForWireConnector, connectedWire);
            connectedWire = null;
        }
    }
}