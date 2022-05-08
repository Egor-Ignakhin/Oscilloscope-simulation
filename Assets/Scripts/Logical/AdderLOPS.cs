using System;
using System.Collections.Generic;

using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;

namespace OscilloscopeSimulation
{
    //Сумматор - обработчик
    internal sealed class AdderLOPS : LogicalOperationsProcessingSystem
    {
        [SerializeField] private List<WireSocketInteractable> sumSockets = new List<WireSocketInteractable>();

        [SerializeField] private List<WireSocketInteractable> digitSockets = new List<WireSocketInteractable>();

        [SerializeField] private WireSocketInteractable invertedDigitSocket;
        private void CalculateOutputValue()
        {
            int sumValue = 0;

            //Проходимся по пред. сокетами и складываем их значения
            foreach (var bs in behindSockets)
            {
                sumValue += bs.LogicalValue ? 1 : 0;
            }

            //Преобразуем полученое десятичное значение в двоичное
            string binaryCode = Convert.ToString(sumValue, 2);

            if (binaryCode.Length == 1)
            {
                //Записываем в сумм-сокеты сумму
                foreach (var sS in sumSockets)
                {
                    sS.LogicalValue = binaryCode[0] == '0' ? false : true;
                }

                //Записываем в рязряд-сокеты нуль
                foreach (var dS in digitSockets)
                {
                    dS.LogicalValue = false;
                }
            }
            else if (binaryCode.Length == 2)
            {
                //Записываем в сумм-сокеты сумму
                foreach (var sS in sumSockets)
                {
                    sS.LogicalValue = binaryCode[1] == '0' ? false : true;
                }

                //Записываем в рязряд-сокеты разряд
                foreach (var dS in digitSockets)
                {
                    dS.LogicalValue = binaryCode[0] == '0' ? false : true;
                }
            }

            invertedDigitSocket.LogicalValue = !digitSockets[0].LogicalValue;
        }
        protected override void LateUpdate()
        {
            CalculateOutputValue();
        }
    }
}