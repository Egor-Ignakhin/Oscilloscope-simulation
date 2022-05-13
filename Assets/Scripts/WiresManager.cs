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
        private bool allWiresAreVisible = true;
        [SerializeField] private Transform wiresParent;
             
        internal Wire ActiveWire { get; private set; }

        [SerializeField] private PlayerInteractive playerInteractive;

        private void Start()
        {
            int numberOfWiresBeingCreated = 100;

            for (int i = 0; i < numberOfWiresBeingCreated; i++)
            {
                Wire wire = Instantiate(Resources.Load<Wire>("Wire"), wiresParent);
                wire.transform.localScale = Vector3.one;
                wire.Initialize(playerInteractive, this);
                allWires.Add(wire);
                wire.gameObject.SetActive(false);
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
                wire.gameObject.SetActive(true);

                return ActiveWire;
            }
        }

        private Wire FindFreeWire()
        {
            foreach (var wire_V2 in allWires)
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
            wire.gameObject.SetActive(false);

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