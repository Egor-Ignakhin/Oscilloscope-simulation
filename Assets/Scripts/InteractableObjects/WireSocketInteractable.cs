
using System;

using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    /// <summary>
    /// Сокет для подключения провода
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

            //Если сокет подключен к тумблеру            
            if (toggleSwitch)
            {
                //Подписываемся на изменения его состояния
                toggleSwitch.ChangeValueEvent += (bool v) =>
                {
                    LogicalValue = v;
                };
            }
        }

        internal override void Interact()
        {
            //Если в сокет подключен провод
            if (ConnectedWire)
            {
                //Если провод подключен только в этот сокет
                if ((ConnectedWire.GetSocket_1() == this) &&
                    (ConnectedWire.GetSocket_2() == null))
                {
                    //Полностью отсоединяем провод
                    DisconnectWireAbs();
                }
                else
                {
                    //Если провод подключен не только в этот сокет

                    // Отключаем провод из настоящего сокета
                    DisconnectWireFromThisPoint();
                }
            }
            else
            {
                // Если в сокет еще не подключен провод - 
                // Подключаем провод
                ConnectWire();
            }
        }



        /// <summary>
        /// Метод подключения провода к сокету
        /// </summary>
        public void ConnectWire()
        {
            ConnectedWire = wiresManager.ConnectWire(this);

            //Если провод подключен обоими концами
            if (ConnectedWire.GetSocket_1() 
                && ConnectedWire.GetSocket_2())
            {
                // Если провод подключен в настоящий сокет 2 концом
                if (ConnectedWire.GetSocket_2() != this)
                {
                    return;
                }
                // Если настоящий сокет не подключен к тумблеру
                if (!toggleSwitch)
                {
                    // Значение сокета = значение провода.сокета_1
                    LogicalValue = ConnectedWire.GetSocket_1().LogicalValue;
                }
                else
                {
                    // Если настоящий сокет подключен к тумблеру
                    //Меняем сокеты провода местами
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
        /// Метод полного отключения провода от сокетов
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
        /// Метод отключения провода от настоящего сокета
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
        /// Позиция, которую принимает 
        /// провод при подключении в сокет
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