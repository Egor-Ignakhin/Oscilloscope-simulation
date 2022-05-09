
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
        private void LateUpdate()
        {
            SetupWireVertexPositions();
        }

        /// <summary>
        /// ����� ������� ������� � �����
        /// </summary>
        /// <param name="socket"></param>
        internal void InsertWire(WireSocketInteractable socket)
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

        private void SetupWireVertexPositions()
        {
            //���� ����� 1 �� ���������
            if (!socket_1)
            {
                //���������� ������� ������� ������������� � ������ ���������
                lineRendererOfWire.SetPositions
                    (new Vector3[] { Vector3.zero, Vector3.zero });

                return;
            }

            //���� ������������ ��������������� � ��������� ��������
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

        /// <summary>
        /// ����� ���������� ������� �� �������
        /// </summary>
        internal void DisconnectAbs()
        {
            socket_1 = null;
            socket_2 = null;
        }

        /// <summary>
        /// ����� ���������� ������� �� ������
        /// </summary>
        /// <param name="socket"></param>
        internal void DisconnectWire(WireSocketInteractable socket)
        {
            //���� ����� 1 - ��� ��������
            if (socket_1.Equals(socket))
            {
                socket_1 = null;
            }
            else
            {
                socket_2 = null;
            }

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
        /// <param name="visibility"></param>
        internal void SetVisible(bool visibility)
        {
            lineRendererOfWire.enabled = visibility;
        }

        internal void SetAvailability(bool availability)
        {
            available = availability;
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