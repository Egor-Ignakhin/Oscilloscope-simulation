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
        private readonly List<Wire> allWires = new List<Wire>();
        private bool allWiresAreVisible = true;

        /// <summary>
        /// �������� ������ - ��� ������, 
        /// ��� ��������� ������ � 1 �����, � 2 ����� ������� �� ����������
        /// </summary>
        public Wire ActiveWire { get; private set; }

        [SerializeField] private PlayerInteractive playerInteractive;

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

        internal Wire ConnectWire(WireSocketInteractable socket)
        {
            //���� � ������ ������ ��� ��������� �������
            if (!ActiveWire)
            {
                Wire wire = GetFreeWire();

                //���������� ��������� ������ � ������
                wire.InsertWire(socket);
                //��������� �������� �������� ������ ��� ���������
                ActiveWire = wire;

                return ActiveWire;
            }
            else
            {
                //� ������, ���� ��� ���� �������� ������
                // ���������� �������� ������ � ������������� ���������� ������
                ActiveWire.InsertWire(socket);

                //������� � ����������� ��������� ������� ������ �� ��������
                //��� ����, ����� ���������� ��� �������
                Wire bufferWire = ActiveWire;

                //������� �������� ������
                ActiveWire = null;

                return bufferWire;
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

        /// <summary>
        /// ����� ������� ���������� ������� �� �������
        /// </summary>
        /// <param name="wire"></param>
        internal void DisconnectWireAbs(Wire wire)
        {
            wire.DisconnectAbs();

            // ������������� ������� ������������� �����������
            wire.SetAvailability(true);

            //������� �������� ������
            ActiveWire = null;
        }

        internal void SetActiveWire(Wire wire)
        {
            ActiveWire = wire;
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
    }
}