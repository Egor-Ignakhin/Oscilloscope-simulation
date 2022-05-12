
using System;

using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    /// <summary>
    /// Сокет для подключения провода
    /// </summary>
    internal sealed class WireSocketInteractable : Interactable, ILogicalValue
    {
        public bool LogicalValue
        {
            get => logicalValue;
            set
            {
                logicalValue = isInvertedSignal ? !value : value;

                ChangeValueEvent?.Invoke(logicalValue);

                WriteLogicalValueInText();
            }
        }
        private bool logicalValue;
        public Action<bool> ChangeValueEvent { get; set; }

        [SerializeField] private Transform wireConnectorSetupPlace;

        internal Wire ConnectedWire { get; private set; }
        internal Wire_V2 ConnectedWire_V2 { get; private set; }

        [SerializeField] private WiresManager wiresManager;

        [SerializeField] private TMPro.TextMeshPro valueText;

        [SerializeField] private ToggleSwitchInteractable toggleSwitch;

        [SerializeField] private LogicalOperationsProcessingSystem behindLOPS;

        [SerializeField] private bool isInvertedSignal;

        private void WriteLogicalValueInText()
        {
            if (HasAToggleSwitch() || ConnectedWire || behindLOPS)
            {
                valueText.SetText(logicalValue ? "1" : "0");
            }
            else
            {
                valueText.SetText("");
            }
        }

        private void Start()
        {
            //dev
            GetComponent<MeshRenderer>().enabled = false;
            //dev

            LogicalValue = false;

            if (HasAToggleSwitch())
            {
                toggleSwitch.ChangeValueEvent += (bool v) =>
                {
                    LogicalValue = v;
                };
            }
        }

        internal override void Interact()
        {
            if (ConnectedWire)
            {
                DisconnectWire();
                DisconnectWire_V2();
            }
            else
            {
                ConnectWire();
                ConnectWire_V2();
            }
        }

        private void DisconnectWire()
        {
            if (ConnectedWire.GetSocket_2() == this)
            {
                ConnectedWire.GetSocket_1().ChangeValueEvent = null;
            }
            else
            {
                ChangeValueEvent = null;
            }
            ConnectedWire.DisconnectWire(this);
            ConnectedWire = null;

            if (HasAToggleSwitch())
            {
                return;
            }

            LogicalValue = false;
        }
        private void DisconnectWire_V2()
        {
            if (ConnectedWire_V2.GetSocket_2() == this)
            {
                ConnectedWire_V2.GetSocket_1().ChangeValueEvent = null;
            }
            else
            {
                ChangeValueEvent = null;
            }
            ConnectedWire_V2.DisconnectWire(this);
            ConnectedWire_V2 = null;

            if (HasAToggleSwitch())
            {
                return;
            }

            LogicalValue = false;
        }

        public void ConnectWire()
        {
            ConnectedWire = wiresManager.ConnectWire(this);

            if (!ConnectedWire.IsFullyConnected())
            {
                return;
            }
            if (ConnectedWire.GetSocket_2() != this)
            {
                return;
            }
            if (!HasAToggleSwitch())
            {
                LogicalValue = ConnectedWire.GetSocket_1().LogicalValue;
            }
            else
            {
                ConnectedWire.SwapSocketReferences();
            }
            if (ConnectedWire.GetSocket_2() == this)
            {
                ConnectedWire.GetSocket_1().
                    ChangeValueEvent += OnBehindSocketValueUpdate;
            }
            else
            {
                ChangeValueEvent += ConnectedWire.
                    GetSocket_2().OnBehindSocketValueUpdate;
            }
        }
        public void ConnectWire_V2()
        {
            ConnectedWire_V2 = wiresManager.ConnectWire_V2(this);

            if (!ConnectedWire_V2.IsFullyConnected())
            {
                return;
            }
            if (ConnectedWire_V2.GetSocket_2() != this)
            {
                return;
            }
            if (!HasAToggleSwitch())
            {
                LogicalValue = ConnectedWire_V2.GetSocket_1().LogicalValue;
            }
            else
            {
                ConnectedWire_V2.SwapSocketReferences();
            }
            if (ConnectedWire_V2.GetSocket_2() == this)
            {
                ConnectedWire_V2.GetSocket_1().
                    ChangeValueEvent += OnBehindSocketValueUpdate;
            }
            else
            {
                ChangeValueEvent += ConnectedWire_V2.
                    GetSocket_2().OnBehindSocketValueUpdate;
            }
        }

        public Vector3 GetWireConnectorSetupPosition()
        {
            return wireConnectorSetupPlace.position;
        }

        private void OnBehindSocketValueUpdate(bool v)
        {
            LogicalValue = v;
        }

        internal LogicalOperationsProcessingSystem GetBehindLOPS()
        {
            return behindLOPS;
        }

        private bool HasAToggleSwitch()
        {
            return toggleSwitch;
        }
    }
}