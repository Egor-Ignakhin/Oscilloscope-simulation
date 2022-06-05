using OscilloscopeSimulation.InteractableObjects;

using System.Collections.Generic;

using UnityEngine;

namespace OscilloscopeSimulation
{
    /// <summary>
    /// �������� ��������, ����������� ������ ������
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
            foreach (var wire in wiresInfo.GetWires())
            {
                if (wire.IsAvailableToUse())
                {
                    wire.SetUsageAvailability(false);

                    return wire;
                }
            }

            return null;
        }

        internal void ReleaseWire(Wire wire)
        {
            wire.SetUsageAvailability(true);

            wiresInfo.SetActiveWire(null);
        }

        internal bool HasActiveWire()
        {
            return wiresInfo.GetActiveWire() != null;
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

        internal Wire GetWireByParticleIndex(int index)
        {
            var wires = wiresInfo.GetWires();            
            foreach(var wire in wires)
            {
                WireRope wireRope = wire.GetWireRope();
                if (!wireRope.ContainsSolverIndices(index))
                {
                    continue;
                }

                return wire;
            }

            return null;
        }

        internal void DeleteActiveWire()
        {
            Wire activeWire = wiresInfo.GetActiveWire();
            activeWire.DeleteWire();
            wiresInfo.SetActiveWire(null);
        }

        internal Wire GetActiveWire()
        {
            return wiresInfo.GetActiveWire();
        }
    }
}