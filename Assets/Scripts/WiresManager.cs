using OscilloscopeSimulation.InteractableObjects;

using System.Collections.Generic;

using UnityEngine;

namespace OscilloscopeSimulation
{
    /// <summary>
    /// ћенеджер проводов, соедин€ющих сокеты стенда
    /// </summary>
    internal sealed class WiresManager : MonoBehaviour
    {
        private readonly List<Wire> allWires = new List<Wire>();
        private readonly List<Wire_V2> allWires_V2 = new List<Wire_V2>();
        private bool allWiresAreVisible = true;
        [SerializeField] private Transform wiresParent;
             
        internal Wire ActiveWire { get; private set; }
        internal Wire_V2 ActiveWire_V2 { get; private set; }

        [SerializeField] private PlayerInteractive playerInteractive;

        private void Start()
        {
            int numberOfWiresBeingCreated = 100;

            for (int i = 0; i < numberOfWiresBeingCreated; i++)
            {
                Wire wire = Instantiate(Resources.Load<Wire>("Wire"), transform);
                wire.Initialize(playerInteractive, this);
                allWires.Add(wire);

                Wire_V2 wire_v2 = Instantiate(Resources.Load<Wire_V2>("Wire_V2"), wiresParent);
                wire_v2.transform.localScale = Vector3.one;
                wire_v2.Initialize(playerInteractive, this);
                allWires_V2.Add(wire_v2);

            }
        }

        internal Wire ConnectWire(WireSocketInteractable socket)
        {
            if (ActiveWire)
            {
                ActiveWire.InsertWire(socket);

                Wire bufferWire = ActiveWire;
                ActiveWire = null;

                return bufferWire;
            }
            else
            {
                Wire wire = FindFreeWire();

                wire.InsertWire(socket);
                ActiveWire = wire;

                return ActiveWire;
            }
        }

        internal Wire_V2 ConnectWire_V2(WireSocketInteractable socket)
        {
            if (ActiveWire_V2)
            {
                ActiveWire_V2.InsertWire(socket);

                Wire_V2 bufferWire = ActiveWire_V2;
                ActiveWire_V2 = null;

                return bufferWire;
            }
            else
            {
                Wire_V2 wire = FindFreeWire_V2();

                wire.InsertWire(socket);
                ActiveWire_V2 = wire;

                return ActiveWire_V2;
            }
        }

        private Wire FindFreeWire()
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
        private Wire_V2 FindFreeWire_V2()
        {
            foreach (var wire_V2 in allWires_V2)
            {
                if (wire_V2.IsAvailable())
                {
                    wire_V2.SetAvailability(false);

                    return wire_V2;
                }
            }

            return null;
        }

        internal void ReleaseWire(Wire wire)
        {
            wire.SetAvailability(true);

            ActiveWire = null;
        }

        internal void SetActiveWire(Wire wire)
        {
            ActiveWire = wire;
        }

        internal void SetActiveWire_V2(Wire_V2 wire_V2)
        {
            ActiveWire_V2 = wire_V2;
        }

        internal void SetVisibilityToWires()
        {
            allWiresAreVisible = !allWiresAreVisible;
            foreach (var w in allWires)
            {
                w.SetVisible(allWiresAreVisible);
            }
        }

        internal void SetVisibilityToWires_V2()
        {
            allWiresAreVisible = !allWiresAreVisible;
            foreach (var w_v2 in allWires_V2)
            {
                w_v2.SetVisible(allWiresAreVisible);
            }
        }
    }
}