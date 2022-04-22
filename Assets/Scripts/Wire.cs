using System.Collections.Generic;

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
        
        private Transform connector_1;
        private Transform connector_2;

        /// <summary>
        /// Пользователь взаимодействует с проводом?
        /// </summary>
        public bool UserInteractingWithTheWire { get; private set; }

        private PlayerInteractive playerInteractive;

        /// <summary>
        /// Метод, вызываемый перед первым вызовом метода Update
        /// </summary>
        private void Start()
        {
            //Устанавливаем необходимое кол-во вершин провода
            lineRenderer.positionCount = 2;

            //Находим на сцене оператор взаимодействия
            playerInteractive = FindObjectOfType<PlayerInteractive>();
        }

        /// <summary>
        /// Метод установки позиции коннектора
        /// </summary>
        /// <param name="positionForWireConnector"></param>
        internal void SetConnectorPosition(Transform positionForWireConnector)
        {
            //Если коннектор 1 не подключен
            if (!connector_1)
            {
                //Коннектор 1 подключается к сокету
                connector_1 = positionForWireConnector;

                //Провод входит в активное состояния взаимодействия
                UserInteractingWithTheWire = true;
            }
            else
            {
                //Коннектор 1 подключается к сокету
                connector_2 = positionForWireConnector;

                //Провод выходит из активного состояния взаимодействия
                UserInteractingWithTheWire = false;
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
                //Заставляем вершины провода переместиться в начало мировых координат
                lineRenderer.SetPositions(new Vector3[]{Vector3.zero, Vector3.zero});

                //Выходим из метода
                return;
            }

            //Если пользователь взаимодействует с проводом
            if (UserInteractingWithTheWire)
            {
                //Устанавливаем вершины:
                // - коннетор 1
                // - координаты точки соприкосновения луча оператора с физ. объектами сцены
                lineRenderer.SetPositions(
                    new Vector3[] { connector_1.position, playerInteractive.LastHitPoint });
            }
            else
            {
                //Устанавливаем вершины:
                // - коннетор 1
                // - коннетор 2
                lineRenderer.SetPositions(
                    new Vector3[] { connector_1.position, connector_2.position });
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
            UserInteractingWithTheWire = false;
        }

        /// <summary>
        /// Метод частичного отключения провода от сокета. Параметр - сокет
        /// </summary>
        /// <param name="point"></param>
        internal void DisconnectFromPoint(Transform point)
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

            //Теперь пользователь взаимодействует с проводом
            UserInteractingWithTheWire = true;
        }
    }
}