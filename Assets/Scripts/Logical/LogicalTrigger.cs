using System;
using System.Collections.Generic;

using UnityEngine;

namespace OscilloscopeSimulation
{
    internal sealed class LogicalTrigger : MonoBehaviour, ILogicalValue
    {

        [SerializeField] private GameObject SInGM;
        private ILogicalValue SIn;

        [SerializeField] private GameObject RInGM;
        private ILogicalValue RIn;

        [SerializeField] private GameObject QOutGM;
        private ILogicalValue QOut;

        [SerializeField] private GameObject invertedQOutGM;
        private ILogicalValue invertedQOut;

        [SerializeField] private TMPro.TextMeshPro text;
        [SerializeField]
        private List<LogicalOperationsProcessingSystem> behindLOPSs
            = new List<LogicalOperationsProcessingSystem>();
        private Dictionary<(bool S, bool R, bool Qlast), bool?> truthTable
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

        public bool LogicalValue
        {
            get => logicalValue; set
            {
                logicalValue = value;

                ChangeValueEvent?.Invoke(logicalValue);

                //Выводим текст, отображающий настоящее значение лог. переменной
                text.SetText(logicalValue ? "1" : "0");
            }
        }
        private bool logicalValue;
        public Action<bool> ChangeValueEvent { get; set; }

        private void Start()
        {
            SIn = SInGM.GetComponent<LogicalOperationsProcessingSystem>();
            RIn = RInGM.GetComponent<LogicalOperationsProcessingSystem>();
            QOut = QOutGM.GetComponent<LogicalOperationsProcessingSystem>();
            invertedQOut = invertedQOutGM.GetComponent<LogicalOperationsProcessingSystem>();

            LogicalValue = false;
        }
        private void Update()
        {
            bool? truthTableV = truthTable[(SIn.LogicalValue, RIn.LogicalValue, LogicalValue)];

            if (truthTableV == null)
            {
                text.SetText("Crash");
                throw new Exception("Crash");
            }
            if (truthTableV == true)
            {
                LogicalValue = true;
            }
            if (truthTableV == false)
            {
                LogicalValue = false;
            }
        }
    }
}
