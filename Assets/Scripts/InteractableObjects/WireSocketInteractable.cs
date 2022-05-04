
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
        /// <summary>
        /// Место установки штекера провода
        /// </summary>
        [SerializeField] private Transform positionForWireConnector;

        /// <summary>
        /// Подключенный провод
        /// </summary>
        internal Wire ConnectedWire { get; private set; }

        /// <summary>
        /// Менеджер проводов
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

                //Выводим текст, отображающий настоящее значение лог. переменной
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

            //Находим при загрузке сцены менеджер проводов
            wiresManager = FindObjectOfType<WiresManager>();
            Value = false;

            //Если сокет подключен к тумблеру            
            if (toggleSwitch)
            {
                //Подписываемся на изменения его состояния
                toggleSwitch.ChangeValueEvent += (bool v) =>
                {
                    Value = v;
                };
            }
        }

        internal override void Interact()
        {
            //Если в сокет уже подключен провод
            if (ConnectedWire)
            {
                //Если провод подключен только в этот сокет
                if (wiresManager.ActiveWire == ConnectedWire)
                {
                    //Полностью отключаем провод от сокетов
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
            if (ConnectedWire.Connector_1 && ConnectedWire.Connector_2)
            {
                // Если в сокет подключен 2 коннектор                    
                if (ConnectedWire.Connector_2 == this)
                {
                    // Если настоящий сокет не подключен к тумблеру
                    if (!toggleSwitch)
                    {
                        // Значение сокета = значение коннектора 1
                        Value = ConnectedWire.Connector_1.Value;
                    }
                    else
                    {
                        // Если настоящий сокет подключен к тумблеру
                        //Меняем коннекторы местами
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
        /// Метод полного отключения провода от сокетов
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
        /// Метод отключения провода от настоящего сокета
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
        /// Геттер позиции, которую принимает 
        /// провод при подключении в сокет
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPositionForWireConnector()
        {
            return positionForWireConnector.position;
        }

        private void OnBehindSocketValueUpdate(bool v)
        {
            // Значение сокета = значение коннектора 1
            Value = v;
        }
        internal LogicalOperationsProcessingSystem GetBehindLOPS()
        {
            return behindLOPS;
        }
    }
}