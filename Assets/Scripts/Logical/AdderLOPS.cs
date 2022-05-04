using System;
using System.Collections.Generic;

using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;

namespace OscilloscopeSimulation
{
    internal sealed class AdderLOPS : LogicalOperationsProcessingSystem
    {
        [SerializeField] private List<WireSocketInteractable> sumSockets = new List<WireSocketInteractable>();
        [SerializeField] private List<WireSocketInteractable> digitSockets = new List<WireSocketInteractable>();
        [SerializeField] private WireSocketInteractable invertedDigitSocket;
        private int SValue;
        protected override bool OperateBehindLogicalValues(Operators sysOperator)
        {
            switch (sysOperator)
            {
                case Operators.Add:
                    {
                        return false;
                    }
                default:
                    {
                        throw new Exception("");
                    }
            }
        }
        private void Add()
        {
            //Сначала обнуляем значение суммы
            SValue = 0;

            //Проходимся по пред. сокетами и складываем их значения
            foreach (var bs in behindSockets)
            {
                SValue += bs.Value ? 1 : 0;
            }

            //Преобразуем полученое десятичное значение в двоичное
            string binaryCode = Convert.ToString(SValue, 2);

            //Если длина строки - 1
            if (binaryCode.Length == 1)
            {
                //Записываем в сумм-сокеты сумму
                foreach(var ss in sumSockets)
                    ss.Value = binaryCode == "0" ? false : true;

                //Записываем в рязряд-сокеты нуль
                foreach(var ds in digitSockets)
                    ds.Value = false;
            }
            //Если же длина строки - 2, появился новый разряд
            else if(binaryCode.Length == 2)
            {
                //Записываем в сумм-сокеты сумму
                foreach (var ss in sumSockets)
                    ss.Value = binaryCode[1] == '0' ? false : true;

                //Записываем в рязряд-сокеты разряд
                foreach (var ds in digitSockets)
                    ds.Value = binaryCode[0] == '0' ? false : true;
            }

            invertedDigitSocket.Value = !digitSockets[0].Value;
        }
        protected override void LateUpdate()
        {
            Add();
        }
    }
}