
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
        private Wire connectedWire;

        /// <summary>
        /// �������� ��������
        /// </summary>
        private WiresManager wiresManager;

        [SerializeField] private TMPro.TextMeshPro valueText;

        public bool Value
        {
            get => value; set
            {
                this.value = value;
                ChangeValueEvent?.Invoke(value);

                //������� �����, ������������ ��������� �������� ���. ����������
                valueText.SetText(value ? "1" : "0");
            }
        }

        [SerializeField] private ToggleSwitchInteractable toggleSwitch;
        private bool value;

        private void Start()
        {
            //������� ��� �������� ����� �������� ��������
            wiresManager = FindObjectOfType<WiresManager>();
            Value = false;

            //���� ����� ��������� � ��������            
            if (toggleSwitch)
            {
                //������������� �� ��������� ��� ���������
                toggleSwitch.ChangeValueEvent += (bool v)=>
                {
                    Value = v;
                };
            }
        }

        internal override void Interact()
        {
            //���� � ����� ��� ��������� ������
            if (connectedWire)
            {
                //���� ������ ��������� ������ � ���� �����
                if (wiresManager.ActiveWire == connectedWire)
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
            connectedWire = wiresManager.ConnectWire(this);

            //���� ������ ��������� ������ �������
            if (connectedWire.Connector_1 && connectedWire.Connector_2)
            {
                // ���� � ����� ��������� 2 ���������                    
                if (connectedWire.Connector_2 == this)
                {
                    // ���� ��������� ����� �� ��������� � ��������
                    if (!toggleSwitch)
                    {
                        // �������� ������ = �������� ���������� 1
                        Value = connectedWire.Connector_1.Value;
                    }
                    else
                    {
                        // ���� ��������� ����� ��������� � ��������
                        //������ ���������� �������
                        connectedWire.SwapConnectors();
                    }

                    connectedWire.Connector_1.ChangeValueEvent += OnBehindSocketValueUpdate;
                }
            }
        }

        /// <summary>
        /// ����� ������� ���������� ������� �� �������
        /// </summary>
        public void DisconnectWireAbs()
        {
            connectedWire.Connector_1.ChangeValueEvent -= OnBehindSocketValueUpdate;
            wiresManager.DisconnectWireAbs(connectedWire);
            connectedWire = null;
        }

        /// <summary>
        /// ����� ���������� ������� �� ���������� ������
        /// </summary>
        private void DisconnectWireFromThisPoint()
        {
            connectedWire.Connector_1.ChangeValueEvent -= OnBehindSocketValueUpdate;
            Value = false;
            wiresManager.DisconnectWireFromPoint(this, connectedWire);
            connectedWire = null;
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
            Value = connectedWire.Connector_1.Value;
        }
    }
}