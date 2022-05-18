
using OscilloscopeSimulation.Logical.LOPS;

using System;

using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    /// <summary>
    /// Сокет для подключения провода
    /// </summary>
    internal sealed class WireSocketInteractable : Interactable, ILogicalValue
    {
        internal Wire ConnectedWire { get; private set; }

        private bool logicalValue;
        public Action<bool> ChangeValueEvent { get; set; }

        [SerializeField] private Transform wireConnectorSetupPlace;

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

            SetLogicalValue(false);

            if (HasAToggleSwitch())
            {
                toggleSwitch.ChangeValueEvent += (bool v) =>
                {
                    SetLogicalValue(v);
                };
            }
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

            SetLogicalValue(false);
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
                SetLogicalValue(ConnectedWire.GetSocket_1().GetLogicalValue());
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

        public Vector3 GetWireConnectorSetupPosition()
        {
            return wireConnectorSetupPlace.position;
        }

        private void OnBehindSocketValueUpdate(bool value)
        {
            SetLogicalValue(value);
        }

        internal LogicalOperationsProcessingSystem GetBehindLOPS()
        {
            return behindLOPS;
        }

        private bool HasAToggleSwitch()
        {
            return toggleSwitch;
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