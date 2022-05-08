
using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;
namespace OscilloscopeSimulation
{
    /// <summary>
    /// ������, ����������� ������ ���������
    /// </summary>
    internal sealed class Wire : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRendererOfWire;

        private WireSocketInteractable socket_1;
        private WireSocketInteractable socket_2;

        private PlayerInteractive playerInteractive;
        private WiresManager wiresManager;
        private bool available = true;

        internal void Initialize(PlayerInteractive playerInteractive, WiresManager wiresManager)
        {
            //������������� ����������� ���-�� ������ �������
            lineRendererOfWire.positionCount = 2;

            this.playerInteractive = playerInteractive;
            this.wiresManager = wiresManager;
        }

        /// <summary>
        /// ����� ������� ������� � �����
        /// </summary>
        /// <param name="socket"></param>
        internal void InsertWireInTheSocket(WireSocketInteractable socket)
        {
            //���� ������ ��� ��������� � ����� 1
            if (socket_1)
            {
                //������ ������������ � ����� 2 ������
                socket_2 = socket;
            }
            else
            {
                //������ ������������ � ����� 1 ������
                socket_1 = socket;
            }
        }

        /// <summary>
        /// ����� ��������������� �������
        /// </summary>
        private void Reconnect()
        {
            //���� ����� 1 �� ���������
            if (!socket_1)
            {
                //���������� ������� ������� ������������� � ������ ���������
                lineRendererOfWire.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });

                return;
            }

            //���� ������������ ��������������� � ��������� ��������
            //������������� �������:
            if (wiresManager.ActiveWire == this)
            {
                lineRendererOfWire.SetPositions(
                    new Vector3[] { socket_1.GetPositionForWireConnector(),
                        playerInteractive.LastRaycastPointPosition });
            }
            else
            {
                lineRendererOfWire.SetPositions(
                    new Vector3[] { socket_1.GetPositionForWireConnector(),
                        socket_2.GetPositionForWireConnector() });
            }
        }

        private void LateUpdate()
        {
            Reconnect();
        }

        /// <summary>
        /// ����� ���������� ������� �� �������
        /// </summary>
        internal void Disconnect()
        {
            socket_1 = null;
            socket_2 = null;
        }

        /// <summary>
        /// ����� ���������� ������� �� ������
        /// </summary>
        /// <param name="socket"></param>
        internal void DisconnectFromTheSocket(WireSocketInteractable socket)
        {
            //���� ����� 1 - ��� ��������
            if (socket_1.Equals(socket))
            {
                // �������� ����� 1
                socket_1 = null;
            }
            else
            {
                // �������� ����� 2
                socket_2 = null;
            }

            //���� ����� 1 ����
            if (socket_1 == null)
            {
                //������������� ����� 2 �� ����� 1
                socket_1 = socket_2;
                //����� 2 �������
                socket_2 = null;
            }
        }

        internal void SwapSockets()
        {
            Extenions<WireSocketInteractable>.Swap(ref socket_1, ref socket_2);
        }

        /// <summary>
        /// ��������� ��������� �������
        /// </summary>
        /// <param name="allWiresIsVisible"></param>
        internal void SetVisible(bool allWiresIsVisible)
        {
            lineRendererOfWire.enabled = allWiresIsVisible;
        }
        internal void SetAvailability(bool value)
        {
            available = value;
        }
        internal bool IsAvailable()
        {
            return available;
        }
        internal WireSocketInteractable GetSocket_1()
        {
            return socket_1;
        }
        internal WireSocketInteractable GetSocket_2()
        {
            return socket_2;
        }
    }
}