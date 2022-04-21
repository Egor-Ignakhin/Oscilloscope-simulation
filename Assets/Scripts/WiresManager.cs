using System;
using System.Collections.Generic;

using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;
namespace OscilloscopeSimulation
{
    internal sealed class WiresManager : MonoBehaviour
    {
        private readonly Stack<Wire> freeWires = new Stack<Wire>();
        public bool LastWireInActive { get; private set; }
        private Wire activeWire;
        private void Start()
        {
            int countOfLoadableWires = 100;

            for (int i = 0; i < countOfLoadableWires; i++)
            {
                Wire wire = Instantiate(Resources.Load<Wire>("Wire"), transform);
                freeWires.Push(wire);
            }

        }
        internal Wire ConnectWire(WireSocketInteractable wireSocket, Transform positionForWireConnector)
        {
            if (!LastWireInActive)
            {
                Wire wire = freeWires.Pop();

                wire.SetConnectorPosition(positionForWireConnector);
                activeWire = wire;
                LastWireInActive = true;

                return activeWire;
            }
            else
            {
                activeWire.SetConnectorPosition(positionForWireConnector);
                Wire bufferWire = activeWire;

                activeWire = null;
                LastWireInActive = false;

                return bufferWire;
            }            
        }

        internal void Disconnect(Wire currentWire)
        {
            currentWire.Disconnect();
            activeWire = null;
            LastWireInActive = false;
        }

        internal void DisconnectFromPoint(Transform point, Wire currentWire)
        {
            currentWire.DisconnectFromPoint(point);
            activeWire = currentWire;
            LastWireInActive = true;
        }
    }
}