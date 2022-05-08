
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

        [SerializeField] private Transform positionForWireConnector;

        internal Wire ConnectedWire { get; private set; }

        [SerializeField] private WiresManager wiresManager;

        [SerializeField] private TMPro.TextMeshPro valueText;

        public bool LogicalValue
        {
            get => logicalValue; set
            {
                if (!isInvertedSignal)
                {
                    logicalValue = value;
                }
                else
                {
                    logicalValue = !value;
                }

                ChangeValueEvent?.Invoke(logicalValue);

                if (toggleSwitch || ConnectedWire ||
                    behindLOPS /*&& behindLOPS.BehindSocketsHasAConnectedWire())*/)
                {
                    valueText.SetText(logicalValue ? "1" : "0");
                }
                else
                {
                    valueText.SetText("");
                }
            }
        }
        private bool logicalValue;

        [SerializeField] private ToggleSwitchInteractable toggleSwitch;
        [SerializeField] private LogicalOperationsProcessingSystem behindLOPS;
        [SerializeField] private bool isInvertedSignal;

        private void Start()
        {
            //dev
            GetComponent<MeshRenderer>().enabled = false;
            //dev

            LogicalValue = false;

            //���� ����� ��������� � ��������            
            if (toggleSwitch)
            {
                //������������� �� ��������� ��� ���������
                toggleSwitch.ChangeValueEvent += (bool v) =>
                {
                    LogicalValue = v;
                };
            }
        }

        internal override void Interact()
        {
            //���� � ����� ��������� ������
            if (ConnectedWire)
            {
                //���� ������ ��������� ������ � ���� �����
                if ((ConnectedWire.GetSocket_1() == this) &&
                    (ConnectedWire.GetSocket_2() == null))
                {
                    //��������� ����������� ������
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
            if (ConnectedWire.GetSocket_1() 
                && ConnectedWire.GetSocket_2())
            {
                // ���� ������ ��������� � ��������� ����� 2 ������
                if (ConnectedWire.GetSocket_2() != this)
                {
                    return;
                }
                // ���� ��������� ����� �� ��������� � ��������
                if (!toggleSwitch)
                {
                    // �������� ������ = �������� �������.������_1
                    LogicalValue = ConnectedWire.GetSocket_1().LogicalValue;
                }
                else
                {
                    // ���� ��������� ����� ��������� � ��������
                    //������ ������ ������� �������
                    ConnectedWire.SwapSockets();
                }
                if (ConnectedWire.GetSocket_2() == this)
                {
                    ConnectedWire.GetSocket_1().ChangeValueEvent += OnBehindSocketValueUpdate;
                }
                else
                {
                    ChangeValueEvent += ConnectedWire.GetSocket_2().OnBehindSocketValueUpdate;
                }
            }
        }

        /// <summary>
        /// ����� ������� ���������� ������� �� �������
        /// </summary>
        public void DisconnectWireAbs()
        {
            if (ConnectedWire.GetSocket_2() == this)
            {
                ConnectedWire.GetSocket_1().ChangeValueEvent = null;
            }
            else
            {
                ChangeValueEvent = null;
            }
            wiresManager.DisconnectWireAbs(ConnectedWire);
            ConnectedWire = null;

            if (toggleSwitch)
            {
                return;
            }

            LogicalValue = false;
        }

        /// <summary>
        /// ����� ���������� ������� �� ���������� ������
        /// </summary>
        private void DisconnectWireFromThisPoint()
        {
            if (ConnectedWire.GetSocket_2() == this)
            {
                ConnectedWire.GetSocket_1().ChangeValueEvent = null;
            }
            else
            {
                ChangeValueEvent = null;
            }
            wiresManager.DisconnectWireFromPoint(this, ConnectedWire);
            ConnectedWire = null;

            if (toggleSwitch)
            {
                return;
            }

            LogicalValue = false;
        }

        /// <summary>
        /// �������, ������� ��������� 
        /// ������ ��� ����������� � �����
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPositionForWireConnector()
        {
            return positionForWireConnector.position;
        }

        private void OnBehindSocketValueUpdate(bool v)
        {
            LogicalValue = v;
        }
        internal LogicalOperationsProcessingSystem GetBehindLOPS()
        {
            return behindLOPS;
        }
    }
}