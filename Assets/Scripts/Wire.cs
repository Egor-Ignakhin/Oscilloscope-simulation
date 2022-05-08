
using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;
namespace OscilloscopeSimulation
{
    /// <summary>
    /// ������, ����������� ������ ���������
    /// </summary>
    internal sealed class Wire : MonoBehaviour
    {
        /// <summary>
        /// ���������� ������������ �������
        /// </summary>
        [SerializeField] private LineRenderer lineRenderer;

        private WireSocketInteractable connector_1;
        private WireSocketInteractable connector_2;

        private PlayerInteractive playerInteractive;
        private WiresManager wiresManager;
        private bool IsAvailable = true;

        internal void Initialize(PlayerInteractive playerInteractive, WiresManager wiresManager)
        {
            //������������� ����������� ���-�� ������ �������
            lineRenderer.positionCount = 2;

            this.playerInteractive = playerInteractive;
            this.wiresManager = wiresManager;
        }

        /// <summary>
        /// ����� ��������� ������� ����������
        /// </summary>
        /// <param name="positionForWireConnector"></param>
        internal void SetConnectorPosition(WireSocketInteractable positionForWireConnector)
        {
            //���� ��������� 1 ��� ��������� � ������
            if (connector_1)
            {
                //��������� 2 ������������ � ������
                connector_2 = positionForWireConnector;
            }
            else
            {
                //��������� 1 ������������ � ������
                connector_1 = positionForWireConnector;
            }
        }

        /// <summary>
        /// ����� ��������������� �������
        /// </summary>
        private void Reconnect()
        {
            //���� ��������� 1 �� ���������
            if (!connector_1)
            {
                //���������� ������� ������� ������������� � ������ ���������
                lineRenderer.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });

                return;
            }

            //���� ������������ ��������������� � ��������
            if (wiresManager.ActiveWire == this)
            {
                //������������� �������:
                // - �������� 1
                // - ���������� ����� ��������������� ���� ��������� � ���. ��������� �����
                lineRenderer.SetPositions(
                    new Vector3[] { connector_1.GetPositionForWireConnector(), playerInteractive.LastHitPoint });
            }
            else
            {
                //������������� �������:
                // - �������� 1
                // - �������� 2
                lineRenderer.SetPositions(
                    new Vector3[] { connector_1.GetPositionForWireConnector(), connector_2.GetPositionForWireConnector() });
            }
        }

        private void LateUpdate()
        {
            Reconnect();
        }

        /// <summary>
        /// ����� ������� ���������� ������� �� �������
        /// </summary>
        internal void Disconnect()
        {
            connector_1 = null;
            connector_2 = null;
        }

        /// <summary>
        /// ����� ���������� ���������� ������� �� ������. �������� - �����
        /// </summary>
        /// <param name="point"></param>
        internal void DisconnectFromPoint(WireSocketInteractable point)
        {
            //���� �������� 1 - ��� �����
            if (connector_1.Equals(point))
            {
                // �������� ��������� 1
                connector_1 = null;
            }
            else
            {
                // �������� ��������� 2
                connector_2 = null;
            }

            //���� �������� 1 - ����
            if (connector_1 == null)
            {
                //������������� ��������� 2 �� ��������� 1
                connector_1 = connector_2;
                //�������� 2 �������
                connector_2 = null;
            }
        }

        /// <summary>
        /// ����� ����� ����������� ���������
        /// </summary>
        internal void SwapConnectors()
        {
            Extenions<WireSocketInteractable>.Swap(ref connector_1, ref connector_2);
        }

        /// <summary>
        /// ��������� ��������� �������
        /// </summary>
        /// <param name="allWiresIsVisible"></param>
        internal void SetVisible(bool allWiresIsVisible)
        {
            lineRenderer.enabled = allWiresIsVisible;
        }
        internal void SetAvailability(bool value)
        {
            IsAvailable = value;
        }
        internal bool GetIsAvailable()
        {
            return IsAvailable;
        }
        internal WireSocketInteractable GetConnector_1()
        {
            return connector_1;
        }
        internal WireSocketInteractable GetConnector_2()
        {
            return connector_2;
        }
    }
}