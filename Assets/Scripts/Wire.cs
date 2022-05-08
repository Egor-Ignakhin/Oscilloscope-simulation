
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

        private WireSocketInteractable connector_1;
        private WireSocketInteractable connector_2;

        private PlayerInteractive playerInteractive;
        private WiresManager wiresManager;
        private bool IsAvailable = true;

        internal void Initialize(PlayerInteractive playerInteractive, WiresManager wiresManager)
        {
            //Устанавливаем необходимое кол-во вершин провода
            lineRenderer.positionCount = 2;

            this.playerInteractive = playerInteractive;
            this.wiresManager = wiresManager;
        }

        /// <summary>
        /// Метод установки позиции коннектора
        /// </summary>
        /// <param name="positionForWireConnector"></param>
        internal void SetConnectorPosition(WireSocketInteractable positionForWireConnector)
        {
            //Если коннектор 1 уже подключен к сокету
            if (connector_1)
            {
                //Коннектор 2 подключается к сокету
                connector_2 = positionForWireConnector;
            }
            else
            {
                //Коннектор 1 подключается к сокету
                connector_1 = positionForWireConnector;
            }
        }

        /// <summary>
        /// Метод переподключения провода
        /// </summary>
        private void Reconnect()
        {
            //Если коннектор 1 не подключен
            if (!connector_1)
            {
                //Заставляем вершины провода переместиться в начало координат
                lineRenderer.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });

                return;
            }

            //Если пользователь взаимодействует с проводом
            if (wiresManager.ActiveWire == this)
            {
                //Устанавливаем вершины:
                // - коннетор 1
                // - координаты точки соприкосновения луча оператора с физ. объектами сцены
                lineRenderer.SetPositions(
                    new Vector3[] { connector_1.GetPositionForWireConnector(), playerInteractive.LastHitPoint });
            }
            else
            {
                //Устанавливаем вершины:
                // - коннетор 1
                // - коннетор 2
                lineRenderer.SetPositions(
                    new Vector3[] { connector_1.GetPositionForWireConnector(), connector_2.GetPositionForWireConnector() });
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
            connector_1 = null;
            connector_2 = null;
        }

        /// <summary>
        /// Метод частичного отключения провода от сокета. Параметр - сокет
        /// </summary>
        /// <param name="point"></param>
        internal void DisconnectFromPoint(WireSocketInteractable point)
        {
            //Если коннетор 1 - это сокет
            if (connector_1.Equals(point))
            {
                // Обнуляем коннектор 1
                connector_1 = null;
            }
            else
            {
                // Обнуляем коннектор 2
                connector_2 = null;
            }

            //Если коннетор 1 - пуст
            if (connector_1 == null)
            {
                //Переназначаем коннектор 2 на коннектор 1
                connector_1 = connector_2;
                //Коннетор 2 очищаем
                connector_2 = null;
            }
        }

        /// <summary>
        /// Метод смены коннекторов индексами
        /// </summary>
        internal void SwapConnectors()
        {
            Extenions<WireSocketInteractable>.Swap(ref connector_1, ref connector_2);
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
        internal WireSocketInteractable GetConnector_1()
        {
            return connector_1;
        }
        internal WireSocketInteractable GetConnector_2()
        {
            return connector_2;
        }
    }
}