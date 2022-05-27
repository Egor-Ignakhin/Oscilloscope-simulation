using System;

using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    /// <summary>
    /// Socket for wire connection
    /// </summary>
    internal sealed class WireSocketInteractable : Interactable, ILogicalValue
    {
        internal Wire ConnectedWire { get; private set; }

        private bool logicalValue;
        public Action<bool> ChangeValueEvent { get; set; }

        [SerializeField] private Transform wireConnectorSetupPlace;

        [SerializeField] private WiresManager wiresManager;

        [SerializeField] private WireSocketText socketText;

        [SerializeField] private ToggleSwitchInteractable toggleSwitch;

        [SerializeField] private bool isInvertedSignal;

        [SerializeField] private bool itsOutSocket;

        private void WriteLogicalValueInText()
        {
            socketText.Write(logicalValue ? "1" : "0");
        }

        private void Start()
        {
            GetComponent<MeshRenderer>().enabled = false;

            SetLogicalValue(false);

            if (toggleSwitch)
            {
                toggleSwitch.ChangeValueEvent += (bool v) =>
                {
                    SetLogicalValue(v);
                };
            }

            socketText.Initialize(itsOutSocket || toggleSwitch);
        }

        internal override void Interact()
        {
            if (ConnectedWire)
            {
                DisconnectWire();
            }
            else
            {
                ConnectWire();
            }
        }

        internal void DisconnectWire()
        {
            if (ConnectedWire.GetSocketEnd() == this)
            {
                ConnectedWire.GetSocketStart().ChangeValueEvent = null;
            }
            else
            {
                ChangeValueEvent = null;
            }
            ConnectedWire.DisconnectWireFromSocket(this);
            ConnectedWire = null;

            if (toggleSwitch)
            {
                return;
            }

            SetLogicalValue(false);
        }
        public void ConnectWire()
        {
            ConnectedWire = wiresManager.ConnectWire(this);

            if (!ConnectedWire.IsFullyConnected())
            {
                return;
            }
            if (ConnectedWire.GetSocketEnd() != this)
            {
                return;
            }
            if (toggleSwitch)
            {
                ConnectedWire.SwapSocketReferences();
            }
            else
            {
                SetLogicalValue(ConnectedWire.GetSocketStart().GetLogicalValue());
            }
            if (ConnectedWire.GetSocketEnd() == this)
            {
                ConnectedWire.GetSocketStart().
                    ChangeValueEvent += OnBehindSocketValueUpdate;
            }
            else
            {
                ChangeValueEvent += ConnectedWire.
                    GetSocketEnd().OnBehindSocketValueUpdate;
            }
        }

        public Vector3 GetWireConnectorSetupPosition()
        {
            return wireConnectorSetupPlace.position;
        }

        private void OnBehindSocketValueUpdate(bool value)
        {
            SetLogicalValue(value);
        }

        public bool GetLogicalValue()
        {
            return logicalValue;
        }
        public void SetLogicalValue(bool value)
        {
            logicalValue = isInvertedSignal ? !value : value;

            ChangeValueEvent?.Invoke(logicalValue);
            
            WriteLogicalValueInText();
        }
    }
}