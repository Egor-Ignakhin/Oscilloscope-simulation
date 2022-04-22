using System.Collections.Generic;

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
        
        public WireSocketInteractable Connector_1 { get; private set; }
        public WireSocketInteractable Connector_2 { get; private set; }

        /// <summary>
        /// ������������ ��������������� � ��������?
        /// </summary>
        public bool UserInteractingWithTheWire { get; private set; }

        private PlayerInteractive playerInteractive;

        /// <summary>
        /// �����, ���������� ����� ������ ������� ������ Update
        /// </summary>
        private void Start()
        {
            //������������� ����������� ���-�� ������ �������
            lineRenderer.positionCount = 2;

            //������� �� ����� �������� ��������������
            playerInteractive = FindObjectOfType<PlayerInteractive>();
        }

        /// <summary>
        /// ����� ��������� ������� ����������
        /// </summary>
        /// <param name="positionForWireConnector"></param>
        internal void SetConnectorPosition(WireSocketInteractable positionForWireConnector)
        {
            //���� ��������� 1 �� ���������
            if (!Connector_1)
            {
                //��������� 1 ������������ � ������
                Connector_1 = positionForWireConnector;

                //������ ������ � �������� ��������� ��������������
                UserInteractingWithTheWire = true;
            }
            else
            {
                //��������� 1 ������������ � ������
                Connector_2 = positionForWireConnector;

                //������ ������� �� ��������� ��������� ��������������
                UserInteractingWithTheWire = false;
            }
        }

        /// <summary>
        /// ����� ��������������� �������
        /// </summary>
        private void Reconnect()
        {
            //���� ��������� 1 �� ���������
            if (!Connector_1)
            {
                //���������� ������� ������� ������������� � ������ ������� ���������
                lineRenderer.SetPositions(new Vector3[]{Vector3.zero, Vector3.zero});

                //������� �� ������
                return;
            }

            //���� ������������ ��������������� � ��������
            if (UserInteractingWithTheWire)
            {
                //������������� �������:
                // - �������� 1
                // - ���������� ����� ��������������� ���� ��������� � ���. ��������� �����
                lineRenderer.SetPositions(
                    new Vector3[] { Connector_1.GetPositionForWireConnector(), playerInteractive.LastHitPoint });
            }
            else
            {
                //������������� �������:
                // - �������� 1
                // - �������� 2
                lineRenderer.SetPositions(
                    new Vector3[] { Connector_1.GetPositionForWireConnector(), Connector_2.GetPositionForWireConnector() });
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
            Connector_1 = null;
            Connector_2 = null;
            UserInteractingWithTheWire = false;
        }

        /// <summary>
        /// ����� ���������� ���������� ������� �� ������. �������� - �����
        /// </summary>
        /// <param name="point"></param>
        internal void DisconnectFromPoint(WireSocketInteractable point)
        {
            //���� �������� 1 - ��� �����
            if (Connector_1.Equals(point))
            {
                // �������� ��������� 1
                Connector_1 = null;
            }
            else
            {
                // �������� ��������� 2
                Connector_2 = null;
            }

            //���� �������� 1 - ����
            if (Connector_1 == null)
            {
                //������������� ��������� 2 �� ��������� 1
                Connector_1 = Connector_2;
                //�������� 2 �������
                Connector_2 = null;
            }

            //������ ������������ ��������������� � ��������
            UserInteractingWithTheWire = true;
        }        
    }
}