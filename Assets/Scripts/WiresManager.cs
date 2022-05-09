using System.Collections.Generic;

using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;

namespace OscilloscopeSimulation
{
    /// <summary>
    /// Менеджер активов - проводов, соеднияющих сокеты прибора
    /// </summary>
    internal sealed class WiresManager : MonoBehaviour
    {
        private readonly List<Wire> allWires = new List<Wire>();
        private bool allWiresAreVisible = true;

        /// <summary>
        /// Активный провод - тот провод, 
        /// что подключен только в 1 сокет, и 2 конец тянется за указателем
        /// </summary>
        public Wire ActiveWire { get; private set; }

        [SerializeField] private PlayerInteractive playerInteractive;

        private void Start()
        {
            int numberOfWiresBeingCreated = 100;

            for (int i = 0; i < numberOfWiresBeingCreated; i++)
            {
                Wire wire = Instantiate(Resources.Load<Wire>("Wire"), transform);
                wire.Initialize(playerInteractive, this);
                allWires.Add(wire);
            }
        }

        internal Wire ConnectWire(WireSocketInteractable socket)
        {
            //Если в данный момент нет активного провода
            if (!ActiveWire)
            {
                Wire wire = GetFreeWire();

                //Подключаем свободный провод к сокету
                wire.InsertWire(socket);
                //Назначаем активным проводом только что достанный
                ActiveWire = wire;

                return ActiveWire;
            }
            else
            {
                //В случае, если уже есть активный провод
                // Подключаем активный провод к передаваемому аргументом сокету
                ActiveWire.InsertWire(socket);

                //Создаем и присваиваем буферному проводу ссылку на активный
                //для того, чтобы оптимально его вернуть
                Wire bufferWire = ActiveWire;

                //Очищаем активный провод
                ActiveWire = null;

                return bufferWire;
            }
        }

        private Wire GetFreeWire()
        {
            foreach (var wire in allWires)
            {
                if (wire.IsAvailable())
                {
                    wire.SetAvailability(false);

                    return wire;
                }
            }

            return null;
        }

        /// <summary>
        /// Метод полного отключения провода от сокетов
        /// </summary>
        /// <param name="wire"></param>
        internal void DisconnectWireAbs(Wire wire)
        {
            wire.DisconnectAbs();

            // Устанавливаем проводу положительную доступность
            wire.SetAvailability(true);

            //Очищаем активный провод
            ActiveWire = null;
        }

        internal void SetActiveWire(Wire wire)
        {
            ActiveWire = wire;
        }

        /// <summary>
        /// Установка видимости всем проводам на сцене
        /// </summary>
        /// <param name="button"></param>
        internal void SetVisibilityToWires()
        {
            allWiresAreVisible = !allWiresAreVisible;
            foreach (var w in allWires)
            {
                w.SetVisible(allWiresAreVisible);
            }
        }
    }
}