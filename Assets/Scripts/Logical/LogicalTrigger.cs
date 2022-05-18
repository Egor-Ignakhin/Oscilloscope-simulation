using OscilloscopeSimulation.Logical.LOPS;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace OscilloscopeSimulation
{
    internal sealed class LogicalTrigger : MonoBehaviour, ILogicalValue
    {
        private bool logicalValue;
        public Action<bool> ChangeValueEvent { get; set; }

        [SerializeField] private GameObject SInGM;
        private ILogicalValue SIn;

        [SerializeField] private GameObject RInGM;
        private ILogicalValue RIn;

        [SerializeField] private GameObject QOutGM;
        private ILogicalValue QOut;

        [SerializeField] private GameObject invertedQOutGM;
        private ILogicalValue invertedQOut;

        private readonly Dictionary<(bool S, bool R, bool Qlast), bool?> truthTable
            = new Dictionary<(bool S, bool R, bool Qlast), bool?>()
            {
                //Хранение
                {(false, false, false), false},
                {(false, false, true), true},

                //Установка 1
                {(false, true, false), true},
                {(false, true, true), true},

                //Установка 0
                {(true, false, false), false},
                {(true, false, true), false},

                //Запрет
                {(true, true, false), null},
                {(true, true, true), null},
            };        

        private void Start()
        {
            SIn = SInGM.GetComponent<LogicalOperationsProcessingSystem>();
            RIn = RInGM.GetComponent<LogicalOperationsProcessingSystem>();
            QOut = QOutGM.GetComponent<LogicalOperationsProcessingSystem>();
            invertedQOut = invertedQOutGM.GetComponent<LogicalOperationsProcessingSystem>();


            logicalValue = false;
        }
        private void Update()
        {
            bool? truthTableV = truthTable[(SIn.GetLogicalValue(), RIn.GetLogicalValue(), logicalValue)];

            if (truthTableV == null)
            {
            }
            else if (truthTableV == true)
            {
                logicalValue = true;
            }
            else if (truthTableV == false)
            {
                logicalValue = false;
            }

            ChangeValueEvent?.Invoke(logicalValue);
        }

        public bool GetLogicalValue()
        {
            return logicalValue;
        }

        public void SetLogicalValue(bool value)
        {
            logicalValue = value;
        }
    }
}
