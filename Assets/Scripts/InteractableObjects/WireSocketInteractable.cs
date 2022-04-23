
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
        private Wire connectedWire;

        /// <summary>
        /// Менеджер проводов
        /// </summary>
        private WiresManager wiresManager;

        [SerializeField] private TMPro.TextMeshPro valueText;

        public bool Value
        {
            get => value; set
            {
                this.value = value;
                ChangeValueEvent?.Invoke(value);

                //Выводим текст, отображающий настоящее значение лог. переменной
                valueText.SetText(value ? "1" : "0");
            }
        }

        [SerializeField] private ToggleSwitchInteractable toggleSwitch;
        private bool value;

        private void Start()
        {
            //Находим при загрузке сцены менеджер проводов
            wiresManager = FindObjectOfType<WiresManager>();
            Value = false;

            //Если сокет подключен к тумблеру            
            if (toggleSwitch)
            {
                //Подписываемся на изменения его состояния
                toggleSwitch.ChangeValueEvent += (bool v)=>
                {
                    Value = v;
                };
            }
        }

        internal override void Interact()
        {
            //Если в сокет уже подключен провод
            if (connectedWire)
            {
                //Если провод подключен только в этот сокет
                if (wiresManager.ActiveWire == connectedWire)
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
            connectedWire = wiresManager.ConnectWire(this);

            //Если провод подключен обоими концами
            if (connectedWire.Connector_1 && connectedWire.Connector_2)
            {
                // Если в сокет подключен 2 коннектор                    
                if (connectedWire.Connector_2 == this)
                {
                    // Если настоящий сокет не подключен к тумблеру
                    if (!toggleSwitch)
                    {
                        // Значение сокета = значение коннектора 1
                        Value = connectedWire.Connector_1.Value;
                    }
                    else
                    {
                        // Если настоящий сокет подключен к тумблеру
                        //Меняем коннекторы местами
                        connectedWire.SwapConnectors();
                    }

                    connectedWire.Connector_1.ChangeValueEvent += OnBehindSocketValueUpdate;
                }
            }
        }

        /// <summary>
        /// Метод полного отключения провода от сокетов
        /// </summary>
        public void DisconnectWireAbs()
        {
            connectedWire.Connector_1.ChangeValueEvent -= OnBehindSocketValueUpdate;
            wiresManager.DisconnectWireAbs(connectedWire);
            connectedWire = null;
        }

        /// <summary>
        /// Метод отключения провода от настоящего сокета
        /// </summary>
        private void DisconnectWireFromThisPoint()
        {
            connectedWire.Connector_1.ChangeValueEvent -= OnBehindSocketValueUpdate;
            Value = false;
            wiresManager.DisconnectWireFromPoint(this, connectedWire);
            connectedWire = null;
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
            Value = connectedWire.Connector_1.Value;
        }
    }
}