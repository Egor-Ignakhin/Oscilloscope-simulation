
using System;

using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    /// <summary>
    /// ����� ��� ����������� �������
    /// </summary>
    internal sealed class WireSocketInteractable : Interactable, ILogicalValue
    {
        public Action<bool> ChangeValueEvent { get; set; }
        /// <summary>
        /// ����� ��������� ������� �������
        /// </summary>
        [SerializeField] private Transform positionForWireConnector;

        /// <summary>
        /// ������������ ������
        /// </summary>
        internal Wire ConnectedWire { get; private set; }

        /// <summary>
        /// �������� ��������
        /// </summary>
        private WiresManager wiresManager;

        [SerializeField] private TMPro.TextMeshPro valueText;

        public bool Value
        {
            get => value; set
            {
                if (!isInvertedSignal)
                    this.value = value;
                else
                    this.value = !value;

                ChangeValueEvent?.Invoke(this.value);

                //������� �����, ������������ ��������� �������� ���. ����������
                valueText.SetText(this.value ? "1" : "0");

                if (toggleSwitch || ConnectedWire ||
                    (behindLOPS && behindLOPS.BehindSocketsHasAConnectedWire()))
                {
                    valueText.SetText(this.value ? "1" : "0");
                }
                else
                {
                    valueText.SetText("");
                }
            }
        }

        [SerializeField] private ToggleSwitchInteractable toggleSwitch;
        [SerializeField] private LogicalOperationsProcessingSystem behindLOPS;
        [SerializeField] private bool isInvertedSignal;
        private bool value;

        private void Start()
        {
            //dev
            GetComponent<MeshRenderer>().enabled = false;
            //dev

            //������� ��� �������� ����� �������� ��������
            wiresManager = FindObjectOfType<WiresManager>();
            Value = false;

            //���� ����� ��������� � ��������            
            if (toggleSwitch)
            {
                //������������� �� ��������� ��� ���������
                toggleSwitch.ChangeValueEvent += (bool v) =>
                {
                    Value = v;
                };
            }
        }

        internal override void Interact()
        {
            //���� � ����� ��� ��������� ������
            if (ConnectedWire)
            {
                //���� ������ ��������� ������ � ���� �����
                if (wiresManager.ActiveWire == ConnectedWire)
                {
                    //��������� ��������� ������ �� �������
                    DisconnectWireAbs();
                }
                else
                {
                    //���� ������ ��������� �� ������ � ���� �����

                    // ��������� ������ �� ���������� ������
                    DisconnectWireFromThisPoint();
                }
            }
            else
            {
                // ���� � ����� ��� �� ��������� ������ - 
                // ���������� ������
                ConnectWire();
            }
        }



        /// <summary>
        /// ����� ����������� ������� � ������
        /// </summary>
        public void ConnectWire()
        {
            ConnectedWire = wiresManager.ConnectWire(this);

            //���� ������ ��������� ������ �������
            if (ConnectedWire.Connector_1 && ConnectedWire.Connector_2)
            {
                // ���� � ����� ��������� 2 ���������                    
                if (ConnectedWire.Connector_2 == this)
                {
                    // ���� ��������� ����� �� ��������� � ��������
                    if (!toggleSwitch)
                    {
                        // �������� ������ = �������� ���������� 1
                        Value = ConnectedWire.Connector_1.Value;
                    }
                    else
                    {
                        // ���� ��������� ����� ��������� � ��������
                        //������ ���������� �������
                        ConnectedWire.SwapConnectors();
                    }
                    if (ConnectedWire.Connector_2 == this)
                    {
                        ConnectedWire.Connector_1.ChangeValueEvent += OnBehindSocketValueUpdate;
                    }
                    else
                    {
                        ChangeValueEvent += ConnectedWire.Connector_2.OnBehindSocketValueUpdate;

                    }
                }
            }
        }

        /// <summary>
        /// ����� ������� ���������� ������� �� �������
        /// </summary>
        public void DisconnectWireAbs()
        {
            if (ConnectedWire.Connector_2 == this)
            {
                ConnectedWire.Connector_1.ChangeValueEvent = null;
            }
            else
            {
                ChangeValueEvent = null;
            }
            wiresManager.DisconnectWireAbs(ConnectedWire);
            ConnectedWire = null;

            if (toggleSwitch)
                return;
            Value = false;
        }

        /// <summary>
        /// ����� ���������� ������� �� ���������� ������
        /// </summary>
        private void DisconnectWireFromThisPoint()
        {
            if (ConnectedWire.Connector_2 == this)
            {
                ConnectedWire.Connector_1.ChangeValueEvent = null;
            }
            else
            {
                ChangeValueEvent = null;
            }
            wiresManager.DisconnectWireFromPoint(this, ConnectedWire);
            ConnectedWire = null;

            if (toggleSwitch)
                return;
            Value = false;
        }

        /// <summary>
        /// ������ �������, ������� ��������� 
        /// ������ ��� ����������� � �����
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPositionForWireConnector()
        {
            return positionForWireConnector.position;
        }

        private void OnBehindSocketValueUpdate(bool v)
        {
            // �������� ������ = �������� ���������� 1
            Value = v;
        }
        internal LogicalOperationsProcessingSystem GetBehindLOPS()
        {
            return behindLOPS;
        }
    }
}