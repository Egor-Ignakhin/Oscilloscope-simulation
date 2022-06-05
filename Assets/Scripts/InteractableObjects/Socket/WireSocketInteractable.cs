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

            socketText.Initialize();
            socketText.SetCanVisibility(itsOutSocket || toggleSwitch);
        }

        internal override void Interact()
        {
            if (ConnectedWire)
            {
                DisconnectWire();
            }
            else
            {
                if (CanConnectWire())
                {
                    ConnectWire();
                }
            }
        }

        internal void DisconnectWire()
        {
            socketText.SetCanVisibility(itsOutSocket || toggleSwitch);
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
        private bool CanConnectWire()
        {
            Wire activeWire = wiresManager.GetActiveWire();

            if(activeWire != null)
            {
                var socketStart = activeWire.GetSocketStart();
                if (socketStart.HaveToggleSwitch())
                {
                    return false;
                }
            }

            return true;
        }
        public void ConnectWire()
        {
            ConnectedWire = wiresManager.ConnectWire(this);
            socketText.SetCanVisibility(true);

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

        internal bool HaveToggleSwitch()
        {
            return toggleSwitch != null;
        }
    }
}