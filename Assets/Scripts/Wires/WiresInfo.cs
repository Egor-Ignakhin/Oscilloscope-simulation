using System.Collections.Generic;

using UnityEngine;

namespace OscilloscopeSimulation.Wires
{
    [System.Serializable]
    public sealed class WiresInfo
    {
        private List<Wire> wires;

        private Wire activeWire;

        [SerializeField] private Transform wiresParent;

        public List<Wire> GetWires()
        {
            return wires;
        }
        internal void SetWires(List<Wire> wires)
        {
            this.wires = wires;
        }

        internal Wire GetActiveWire()
        {
            return activeWire;
        }
        internal void SetActiveWire(Wire wire)
        {
            activeWire = wire;
        }

        internal Transform GetWiresParent()
        {
            return wiresParent;
        }

        internal void AddWire(Wire wire)
        {
            wires.Add(wire);
        }
    }
}