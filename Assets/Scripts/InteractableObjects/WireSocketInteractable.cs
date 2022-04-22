
using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    /// <summary>
    /// Сокет для подключения провода
    /// </summary>
    internal sealed class WireSocketInteractable : Interactable
    {
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

        private void Start()
        {
            //Находим при загрузке сцены менеджер проводов
            wiresManager = FindObjectOfType<WiresManager>();
        }

        internal override void Interact()
        {
            //Если в сокет уже подключен провод
            if (connectedWire)
            {
                //Если провод подключен только в этот сокет
                if (connectedWire.UserInteractingWithTheWire)
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
            connectedWire = wiresManager.ConnectWire(positionForWireConnector);
        }

        /// <summary>
        /// Метод полного отключения провода от сокетов
        /// </summary>
        public void DisconnectWireAbs()
        {
            wiresManager.DisconnectAbs(connectedWire);
            connectedWire = null;
        }

        /// <summary>
        /// Метод отключения провода от настоящего сокета
        /// </summary>
        private void DisconnectWireFromThisPoint()
        {
            wiresManager.DisconnectFromPoint(positionForWireConnector, connectedWire);
            connectedWire = null;
        }
    }
}