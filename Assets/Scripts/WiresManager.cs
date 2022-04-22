using System.Collections.Generic;

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
        internal Wire ConnectWire(Transform positionForWireConnector)
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
        internal void DisconnectAbs(Wire currentWire)
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
        internal void DisconnectFromPoint(Transform point, Wire currentWire)
        {
            currentWire.DisconnectFromPoint(point);

            //������������� �������� �������� ���, ������� ��� ���������� ��������
            ActiveWire = currentWire;
        }
    }
}