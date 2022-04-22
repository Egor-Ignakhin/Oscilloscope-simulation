
using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    /// <summary>
    /// ����� ��� ����������� �������
    /// </summary>
    internal sealed class WireSocketInteractable : Interactable, LogicalValue
    {
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

        public bool Value { get; set; }

        [SerializeField] private ToggleSwitchInteractable toggleSwitch;
        private void Start()
        {
            //������� ��� �������� ����� �������� ��������
            wiresManager = FindObjectOfType<WiresManager>();
        }

        internal override void Interact()
        {
            //���� � ����� ��� ��������� ������
            if (connectedWire)
            {
                //���� ������ ��������� ������ � ���� �����
                if (connectedWire.UserInteractingWithTheWire)
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
        }

        /// <summary>
        /// ����� ������� ���������� ������� �� �������
        /// </summary>
        public void DisconnectWireAbs()
        {
            wiresManager.DisconnectWireAbs(connectedWire);
            connectedWire = null;
        }

        /// <summary>
        /// ����� ���������� ������� �� ���������� ������
        /// </summary>
        private void DisconnectWireFromThisPoint()
        {
            wiresManager.DisconnectWireFromPoint(this, connectedWire);
            connectedWire = null;
        }

        private void Update()
        {
            if (toggleSwitch)
            {
                Value = toggleSwitch.Value;
            }

            if (connectedWire)
            {
                if (connectedWire.Connector_1 && connectedWire.Connector_2)
                {
                    if (connectedWire.Connector_1.Equals(this))
                    {
                        Value = connectedWire.Connector_2.Value;
                    }
                    else
                    {
                        Value = connectedWire.Connector_1.Value;
                    }
                }
            }

            valueText.SetText(Value ? "1" : "0");
        }

        public Vector3 GetPositionForWireConnector()
        {
            return positionForWireConnector.position;
        }
    }
}