using System.Collections.Generic;

using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;

namespace OscilloscopeSimulation
{
    /// <summary>
    /// ћенеджер проводов, соедин€ющих сокеты стенда
    /// </summary>
    internal sealed class WiresManager : MonoBehaviour
    {
        private readonly List<Wire> allWires = new List<Wire>();
        private bool allWiresAreVisible = true;

        internal Wire ActiveWire { get; private set; }

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

        internal void ReleaseWire(Wire wire)
        {
            wire.SetAvailability(true);

            ActiveWire = null;
        }

        internal void SetActiveWire(Wire wire)
        {
            ActiveWire = wire;
        }

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