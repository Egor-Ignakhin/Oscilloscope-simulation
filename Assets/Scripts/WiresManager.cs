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
        private bool allWiresAreVisible = true;
        [SerializeField] private PlayerInteractive playerInteractive;
        private readonly List<Wire> allWires = new List<Wire>();
        /// <summary>
        /// �������� ������ - ��� ������, 
        /// ��� ��������� ������ � 1 �����, � 2 ����� ������� �� ����������
        /// </summary>
        public Wire ActiveWire { get; private set; }


        private void Start()
        {
            int numberOfWiresBeingCreated = 100;

            for (int i = 0; i < numberOfWiresBeingCreated; i++)
            {
                Wire wire = Instantiate(Resources.Load<Wire>("Wire"), transform);
                wire.Initialize(playerInteractive, this);
                allWires.Add(wire);
            }
        }
        /// <summary>
        /// ������� ����������� ������� � ������, ������������� ����������.
        /// ���������� ������������ ������
        /// </summary>
        /// <param name="socket"></param>
        /// 
        /// <returns></returns>
        internal Wire ConnectWire(WireSocketInteractable socket)
        {
            //���� � ������ ������ ��� ��������� �������
            if (!ActiveWire)
            {
                //"�������" ��������� ������ �� �����
                Wire wire = GetFreeWire();

                //���������� ���� ������ � ������
                wire.InsertWireInTheSocket(socket);
                //��������� �������� �������� ������ ��� ���������
                ActiveWire = wire;

                //���������� �������� ������
                return ActiveWire;
            }
            else
            {
                //� ������, ���� ��� ���� �������� ������
                // ���������� �������� ������ � ������������� ���������� ������
                ActiveWire.InsertWireInTheSocket(socket);

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
            currentWire.SetAvailability(true);

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
            currentWire.DisconnectFromTheSocket(socket);

            //������������� �������� �������� ���, ������� ��� ���������� ��������
            ActiveWire = currentWire;
        }

        /// <summary>
        /// ��������� ��������� ���� �������� �� �����
        /// </summary>
        /// <param name="button"></param>
        internal void SetVisibilityToWires()
        {
            allWiresAreVisible = !allWiresAreVisible;
            foreach (var w in allWires)
            {
                w.SetVisible(allWiresAreVisible);
            }
        }
        private Wire GetFreeWire()
        {
            foreach (var wire in allWires)
            {
                if (wire.IsAvailable())
                {
                    wire.SetAvailability(false);

                    return wire;
                }
            }

            return null;
        }
    }
}