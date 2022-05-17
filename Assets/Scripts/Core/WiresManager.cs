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
        [SerializeField]
        private WiresInfo wiresInfo = new WiresInfo();

        private void Start()
        {
            WiresBuilder wiresBuilder = new WiresBuilder(wiresInfo.GetWiresParent(), this);
            List<Wire> wires = wiresBuilder.Build(100);
            wiresInfo.SetWires(wires);
        }

        internal Wire ConnectWire(WireSocketInteractable socket)
        {
            Wire activeWire = wiresInfo.GetActiveWire();
            if (activeWire)
            {
                activeWire.InsertWire(socket);

                wiresInfo.SetActiveWire(null);
            }
            else
            {
                Wire wire = FindFreeWire();

                wire.InsertWire(socket);
                activeWire = wire;

                wiresInfo.SetActiveWire(wire);
            }
            return activeWire;
        }

        private Wire FindFreeWire()
        {
            foreach (var wire_V2 in wiresInfo.GetWires())
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

            wiresInfo.SetActiveWire(null);
        }

        internal void SetActiveWire(Wire wire)
        {
            wiresInfo.SetActiveWire(wire);
        }

        internal void SetVisibilityToWires()
        {
            foreach (var wire in wiresInfo.GetWires())
            {
                WireRenderer wireRenderer = wire.GetWireRenderer();
                wireRenderer.SetVisible(!wireRenderer.IsVisible());
            }
        }

        internal List<Wire> GetWires()
        {
            return wiresInfo.GetWires();
        }

        internal bool EqualsWithActiveWire(Wire wire)
        {
            return wiresInfo.GetActiveWire() == wire;
        }
    }
}