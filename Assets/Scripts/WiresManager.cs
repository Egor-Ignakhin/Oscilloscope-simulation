using System.Collections.Generic;

using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;
namespace OscilloscopeSimulation
{
    /// <summary>
    /// �������� ������� - ��������, ����������� ������ �������
    /// </summary>
    internal sealed class WiresManager : MonoBehaviour
    {
        /// <summary>
        /// ���� ��������� �������� (�� ����������� ��� ������������)
        /// </summary>
        private readonly Stack<Wire> freeWires = new Stack<Wire>();

        /// <summary>
        /// �������� ������ - ��� ������, 
        /// ��� ��������� ������ � 1 �����, � 2 ����� ������� �� ����������
        /// </summary>
        public Wire ActiveWire { get; private set; }


        private void Start()
        {
            //����������� ���������� �������� ��� �������. ��������� ����� ��������� �����.
            int countOfLoadableWires = 100;

            for (int i = 0; i < countOfLoadableWires; i++)
            {
                Wire wire = Instantiate(Resources.Load<Wire>("Wire"), transform);
                freeWires.Push(wire);
            }

        }
        /// <summary>
        /// ������� ����������� ������� � ������, ������������� ����������.
        /// ���������� ������������ ������
        /// </summary>
        /// <param name="positionForWireConnector"></param>
        /// 
        /// <returns></returns>
        internal Wire ConnectWire(WireSocketInteractable positionForWireConnector)
        {
            //���� � ������ ������ ��� ��������� �������
            if (!ActiveWire)
            {
                //"�������" ��������� ������ �� �����
                Wire wire = freeWires.Pop();

                //���������� ���� ������ � ������
                wire.SetConnectorPosition(positionForWireConnector);
                //��������� �������� �������� ������ ��� ���������
                ActiveWire = wire;

                //���������� �������� ������
                return ActiveWire;
            }
            else
            {
                //� ������, ���� ��� ���� �������� ������
                // ���������� �������� ������ � ������������� ���������� ������
                ActiveWire.SetConnectorPosition(positionForWireConnector);

                //������� � ����������� ��������� ������� ������ �� ��������
                //��� ����, ����� ���������� ��� �������
                Wire bufferWire = ActiveWire;

                //������� �������� ������
                ActiveWire = null;

                //���������� ��������(������ �������� ������)
                return bufferWire;
            }
        }

        /// <summary>
        /// ����� ������� ���������� ��������� ������� �� �������
        /// </summary>
        /// <param name="currentWire"></param>
        internal void DisconnectWireAbs(Wire currentWire)
        {
            currentWire.Disconnect();

            //���������� � ���� ��������� �������� ��������������
            freeWires.Push(currentWire);

            //������� �������� ������
            ActiveWire = null;
        }

        /// <summary>
        /// ����� ���������� ������� �� ������, ������������� ����������
        /// </summary>
        /// <param name="point"></param>
        /// <param name="currentWire"></param>
        internal void DisconnectWireFromPoint(WireSocketInteractable socket, Wire currentWire)
        {
            currentWire.DisconnectFromPoint(socket);

            //������������� �������� �������� ���, ������� ��� ���������� ��������
            ActiveWire = currentWire;
        }
    }
}