using System.Collections.Generic;

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
        
        private Transform connector_1;
        private Transform connector_2;

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
        internal void SetConnectorPosition(Transform positionForWireConnector)
        {
            //���� ��������� 1 �� ���������
            if (!connector_1)
            {
                //��������� 1 ������������ � ������
                connector_1 = positionForWireConnector;

                //������ ������ � �������� ��������� ��������������
                UserInteractingWithTheWire = true;
            }
            else
            {
                //��������� 1 ������������ � ������
                connector_2 = positionForWireConnector;

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
            if (!connector_1)
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
                    new Vector3[] { connector_1.position, playerInteractive.LastHitPoint });
            }
            else
            {
                //������������� �������:
                // - �������� 1
                // - �������� 2
                lineRenderer.SetPositions(
                    new Vector3[] { connector_1.position, connector_2.position });
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
            UserInteractingWithTheWire = false;
        }

        /// <summary>
        /// ����� ���������� ���������� ������� �� ������. �������� - �����
        /// </summary>
        /// <param name="point"></param>
        internal void DisconnectFromPoint(Transform point)
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

            //������ ������������ ��������������� � ��������
            UserInteractingWithTheWire = true;
        }
    }
}