
using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;
namespace OscilloscopeSimulation
{
    /// <summary>
    /// Провод, соединяющий сокеты установки
    /// </summary>
    internal sealed class Wire : MonoBehaviour
    {
        /// <summary>
        /// Визуальная составляющая провода
        /// </summary>
        [SerializeField] private LineRenderer lineRenderer;

        public WireSocketInteractable Connector_1 { get; private set; }
        public WireSocketInteractable Connector_2 { get; private set; }

        private PlayerInteractive playerInteractive;
        private WiresManager wiresManager;
        private bool IsAvailable;

        /// <summary>
        /// Метод, вызываемый перед первым вызовом метода Update
        /// </summary>
        private void Start()
        {
            //Устанавливаем необходимое кол-во вершин провода
            lineRenderer.positionCount = 2;

            //Находим на сцене оператор взаимодействия
            playerInteractive = FindObjectOfType<PlayerInteractive>();
            wiresManager = FindObjectOfType<WiresManager>();
        }

        /// <summary>
        /// Метод установки позиции коннектора
        /// </summary>
        /// <param name="positionForWireConnector"></param>
        internal void SetConnectorPosition(WireSocketInteractable positionForWireConnector)
        {
            //Если коннектор 1 не подключен
            if (!Connector_1)
            {
                //Коннектор 1 подключается к сокету
                Connector_1 = positionForWireConnector;
            }
            else
            {
                //Коннектор 1 подключается к сокету
                Connector_2 = positionForWireConnector;
            }
        }

        /// <summary>
        /// Метод переподключения провода
        /// </summary>
        private void Reconnect()
        {
            //Если коннектор 1 не подключен
            if (!Connector_1)
            {
                //Заставляем вершины провода переместиться в начало мировых координат
                lineRenderer.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });

                //Выходим из метода
                return;
            }

            //Если пользователь взаимодействует с проводом
            if (wiresManager.ActiveWire == this)
            {
                //Устанавливаем вершины:
                // - коннетор 1
                // - координаты точки соприкосновения луча оператора с физ. объектами сцены
                lineRenderer.SetPositions(
                    new Vector3[] { Connector_1.GetPositionForWireConnector(), playerInteractive.LastHitPoint });
            }
            else
            {
                //Устанавливаем вершины:
                // - коннетор 1
                // - коннетор 2
                lineRenderer.SetPositions(
                    new Vector3[] { Connector_1.GetPositionForWireConnector(), Connector_2.GetPositionForWireConnector() });
            }
        }

        private void LateUpdate()
        {
            Reconnect();
        }

        /// <summary>
        /// Метод полного отключения провода от сокетов
        /// </summary>
        internal void Disconnect()
        {
            Connector_1 = null;
            Connector_2 = null;
        }

        /// <summary>
        /// Метод частичного отключения провода от сокета. Параметр - сокет
        /// </summary>
        /// <param name="point"></param>
        internal void DisconnectFromPoint(WireSocketInteractable point)
        {
            //Если коннетор 1 - это сокет
            if (Connector_1.Equals(point))
            {
                // Обнуляем коннектор 1
                Connector_1 = null;
            }
            else
            {
                // Обнуляем коннектор 2
                Connector_2 = null;
            }

            //Если коннетор 1 - пуст
            if (Connector_1 == null)
            {
                //Переназначаем коннектор 2 на коннектор 1
                Connector_1 = Connector_2;
                //Коннетор 2 очищаем
                Connector_2 = null;
            }
        }

        /// <summary>
        /// Метод смены коннекторов индексами
        /// </summary>
        internal void SwapConnectors()
        {
            WireSocketInteractable connector_1Buffer = Connector_1;

            Connector_1 = Connector_2;
            Connector_2 = connector_1Buffer;
        }

        /// <summary>
        /// Установка видимости провода
        /// </summary>
        /// <param name="allWiresIsVisible"></param>
        internal void SetVisible(bool allWiresIsVisible)
        {
            lineRenderer.enabled = allWiresIsVisible;
        }
        internal void SetAvailability(bool value)
        {
            IsAvailable = value;
        }
        internal bool GetIsAvailable()
        {
            return IsAvailable;
        }
    }
}