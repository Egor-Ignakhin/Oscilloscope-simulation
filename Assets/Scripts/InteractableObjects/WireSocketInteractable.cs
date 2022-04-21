using System;

using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    internal sealed class WireSocketInteractable : Interactable
    {
        private bool wireIsConnected;
        [SerializeField] private Transform positionForWireConnector;
        private Wire currentWire;
        private WiresManager wiresManager;
        private void Start()
        {
            wiresManager = FindObjectOfType<WiresManager>();
        }
        internal override void Interact()
        {
            if (wireIsConnected)
            {
                if (currentWire.WireIsActive)
                {
                    DisconnectWireAbs();                    
                }
                else
                {
                    DisconnectWireFromThisPoint();
                    // Сюда надо поместить действо отключения от точки, на которую нажали,
                    // и вернуть линии ход свободный, но привязанный к 1 точке. 
                }
            }
            else
            {
                ConnectWire();
            }
        }

        

        public void ConnectWire()
        {
            currentWire = wiresManager.ConnectWire(this, positionForWireConnector);

            wireIsConnected = true;
        }
        public void DisconnectWireAbs()
        {
            wiresManager.Disconnect(currentWire);
            currentWire = null;
            wireIsConnected = false;
        }
        private void DisconnectWireFromThisPoint()
        {
            wiresManager.DisconnectFromPoint(positionForWireConnector, currentWire);
            currentWire = null;
            wireIsConnected = false;
        }
    }
}