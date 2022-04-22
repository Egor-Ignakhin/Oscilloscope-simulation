using System.Collections.Generic;

using UnityEngine;
namespace OscilloscopeSimulation
{
    /// <summary>
    /// Менеджер активов - проводов, соеднияющих сокеты прибора
    /// </summary>
    internal sealed class WiresManager : MonoBehaviour
    {
        /// <summary>
        /// Стек свободных проводов (не учитываются уже подключенные)
        /// </summary>
        private readonly Stack<Wire> freeWires = new Stack<Wire>();

        /// <summary>
        /// Активный провод - тот провод, 
        /// что подключен только в 1 сокет, и 2 конец тянется за указателем
        /// </summary>
        public Wire ActiveWire { get; private set; }


        private void Start()
        {
            //Необходимое количество проводов для прибора. Создаются перед загрузкой сцены.
            int countOfLoadableWires = 100;

            for (int i = 0; i < countOfLoadableWires; i++)
            {
                Wire wire = Instantiate(Resources.Load<Wire>("Wire"), transform);
                freeWires.Push(wire);
            }

        }
        /// <summary>
        /// Функция подключения провода к сокету, передаваемому аргументом.
        /// Возвращает подключаемый провод
        /// </summary>
        /// <param name="positionForWireConnector"></param>
        /// 
        /// <returns></returns>
        internal Wire ConnectWire(Transform positionForWireConnector)
        {
            //Если в данный момент нет активного провода
            if (!ActiveWire)
            {
                //"Достаем" свободный провод из стека
                Wire wire = freeWires.Pop();

                //Подключаем этот провод к сокету
                wire.SetConnectorPosition(positionForWireConnector);
                //Назначаем активным проводом только что достанный
                ActiveWire = wire;

                //Возвращаем активный провод
                return ActiveWire;
            }
            else
            {
                //В случае, если уже есть активный провод
                // Подключаем активный провод к передаваемому аргументом сокету
                ActiveWire.SetConnectorPosition(positionForWireConnector);

                //Создаем и присваиваем буферному проводу ссылку на активный
                //для того, чтобы оптимально его вернуть
                Wire bufferWire = ActiveWire;

                //Очищаем активный провод
                ActiveWire = null;

                //Возвращаем буферный(прежде активный провод)
                return bufferWire;
            }
        }

        /// <summary>
        /// Метод полного отключения активного провода от сокетов
        /// </summary>
        /// <param name="currentWire"></param>
        internal void DisconnectAbs(Wire currentWire)
        {
            currentWire.Disconnect();

            //Возвращаем в стек доступных проводов освободившийся
            freeWires.Push(currentWire);

            //Очищаем активный провод
            ActiveWire = null;
        }

        /// <summary>
        /// Метод отключения провода от сокета, передаваемого параметром
        /// </summary>
        /// <param name="point"></param>
        /// <param name="currentWire"></param>
        internal void DisconnectFromPoint(Transform point, Wire currentWire)
        {
            currentWire.DisconnectFromPoint(point);

            //Устанавливаем активным проводом тот, который был наполовину отключен
            ActiveWire = currentWire;
        }
    }
}