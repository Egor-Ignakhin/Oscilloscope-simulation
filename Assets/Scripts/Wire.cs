
using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;
namespace OscilloscopeSimulation
{
    /// <summary>
    /// Провод, соединяющий сокеты установки
    /// </summary>
    internal sealed class Wire : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRendererOfWire;

        private WireSocketInteractable socket_1;
        private WireSocketInteractable socket_2;

        private PlayerInteractive playerInteractive;
        private WiresManager wiresManager;
        private bool available = true;

        internal void Initialize(PlayerInteractive playerInteractive, WiresManager wiresManager)
        {
            //Устанавливаем необходимое кол-во вершин провода
            lineRendererOfWire.positionCount = 2;

            this.playerInteractive = playerInteractive;
            this.wiresManager = wiresManager;
        }

        /// <summary>
        /// Метод вставки провода в сокет
        /// </summary>
        /// <param name="socket"></param>
        internal void InsertWireInTheSocket(WireSocketInteractable socket)
        {
            //Если провод уже подключен в сокет 1
            if (socket_1)
            {
                //Провод подключается в сокет 2 концом
                socket_2 = socket;
            }
            else
            {
                //Провод подключается в сокет 1 концом
                socket_1 = socket;
            }
        }

        /// <summary>
        /// Метод переподключения провода
        /// </summary>
        private void Reconnect()
        {
            //Если сокет 1 не подключен
            if (!socket_1)
            {
                //Заставляем вершины провода переместиться в начало координат
                lineRendererOfWire.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });

                return;
            }

            //Если пользователь взаимодействует с настоящим проводом
            //Устанавливаем вершины:
            if (wiresManager.ActiveWire == this)
            {
                lineRendererOfWire.SetPositions(
                    new Vector3[] { socket_1.GetPositionForWireConnector(),
                        playerInteractive.LastRaycastPointPosition });
            }
            else
            {
                lineRendererOfWire.SetPositions(
                    new Vector3[] { socket_1.GetPositionForWireConnector(),
                        socket_2.GetPositionForWireConnector() });
            }
        }

        private void LateUpdate()
        {
            Reconnect();
        }

        /// <summary>
        /// Метод отключения провода от сокетов
        /// </summary>
        internal void Disconnect()
        {
            socket_1 = null;
            socket_2 = null;
        }

        /// <summary>
        /// Метод отключения провода от сокета
        /// </summary>
        /// <param name="socket"></param>
        internal void DisconnectFromTheSocket(WireSocketInteractable socket)
        {
            //Если сокет 1 - это параметр
            if (socket_1.Equals(socket))
            {
                // Обнуляем сокет 1
                socket_1 = null;
            }
            else
            {
                // Обнуляем сокет 2
                socket_2 = null;
            }

            //Если сокет 1 пуст
            if (socket_1 == null)
            {
                //Переназначаем сокет 2 на сокет 1
                socket_1 = socket_2;
                //Сокет 2 очищаем
                socket_2 = null;
            }
        }

        internal void SwapSockets()
        {
            Extenions<WireSocketInteractable>.Swap(ref socket_1, ref socket_2);
        }

        /// <summary>
        /// Установка видимости провода
        /// </summary>
        /// <param name="allWiresIsVisible"></param>
        internal void SetVisible(bool allWiresIsVisible)
        {
            lineRendererOfWire.enabled = allWiresIsVisible;
        }
        internal void SetAvailability(bool value)
        {
            available = value;
        }
        internal bool IsAvailable()
        {
            return available;
        }
        internal WireSocketInteractable GetSocket_1()
        {
            return socket_1;
        }
        internal WireSocketInteractable GetSocket_2()
        {
            return socket_2;
        }
    }
}