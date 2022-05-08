
using System;
using System.Collections.Generic;

using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;

namespace OscilloscopeSimulation
{
    internal class LogicalOperationsProcessingSystem : MonoBehaviour, ILogicalValue
    {
        public Action<bool> ChangeValueEvent { get; set; }
        /// <summary>
        /// Типы операций, которые может осуществлять система
        /// </summary>
        protected enum Operators { And, Or, NOR, NAND, Add }

        /// <summary>
        /// Выбранная операция из доступных для системы
        /// </summary>
        [SerializeField] protected Operators systemOperator;

        //Предыдущие лог. носители
        [SerializeField] private List<GameObject> behindLogicalValuesGM = new List<GameObject>();
        private readonly List<ILogicalValue> behindLogicalValues = new List<ILogicalValue>();

        //Последущие лог. носители
        [SerializeField] protected List<GameObject> aheadLogicalValuesGM = new List<GameObject>();
        protected readonly List<ILogicalValue> aheadLogicalValues = new List<ILogicalValue>();

        public bool LogicalValue { get; set; }

        [SerializeField] protected List<WireSocketInteractable> behindSockets = new List<WireSocketInteractable>();
        [SerializeField] protected List<LogicalOperationsProcessingSystem> behindLOPS = new List<LogicalOperationsProcessingSystem>();

        private void Start()
        {
            for (int i = 0; i < behindLogicalValuesGM.Count; i++)
            {
                behindLogicalValues.Add(behindLogicalValuesGM[i].GetComponent<ILogicalValue>());

                if (behindLogicalValues[i] == null)
                {
                    throw new Exception("crashed link");
                }
            }

            for (int i = 0; i < aheadLogicalValuesGM.Count; i++)
            {
                aheadLogicalValues.Add(aheadLogicalValuesGM[i].GetComponent<ILogicalValue>());

                if (aheadLogicalValues[i] == null)
                {
                    throw new Exception("crashed link");
                }
            }
        }

        /// <summary>
        /// Метод обработки предыдущих логических носителей
        /// </summary>
        /// <param name="sysOperator"></param>
        /// <returns></returns>
        protected bool OperateBehindLogicalValues(Operators sysOperator)
        {
            switch (sysOperator)
            {
                case Operators.And:
                    {
                        foreach (ILogicalValue blv in behindLogicalValues)
                        {
                            if (!blv.LogicalValue)
                            {
                                return false;
                            }
                        }
                        return true;
                    }

                case Operators.Or:
                    {
                        foreach (ILogicalValue blv in behindLogicalValues)
                        {
                            if (blv.LogicalValue)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                case Operators.NOR:
                    {
                        bool orOp = OperateBehindLogicalValues(Operators.Or);
                        return !orOp;
                    }
                case Operators.NAND:
                    {
                        bool andOp = OperateBehindLogicalValues(Operators.And);
                        return !andOp;
                    }
                default:
                    {
                        throw new Exception("");
                    }
            }
        }

        /// <summary>
        /// В методе каждый кадр происходит пересчет значения из 
        /// предыдущих лог. носителей выбранным оператором и затем 
        /// присвоение полученного результата последующему лог. носителю
        /// </summary>
        protected virtual void LateUpdate()
        {
            LogicalValue = OperateBehindLogicalValues(systemOperator);

            foreach (ILogicalValue alv in aheadLogicalValues)
            {
                alv.LogicalValue = LogicalValue;
            }
        }
        /// <summary>
        ///  Метод возвращает правду, если хотя у бы в одного из предыдущих сокетов вставлен провод
        /// </summary>
        /// <param name="behindCalledlops"></param>
        /// <returns></returns>
        internal bool BehindSocketsHasAConnectedWire(LogicalOperationsProcessingSystem behindCalledlops = null)
        {
            //Если это обычный обработчик, то проверяем у каждого предыдущего сокета провод
            foreach (var bs in behindSockets)
            {
                if (bs.ConnectedWire)
                {
                    return true;
                }
            }
            //Проверяем, нет ли у каждого предущего сокета
            //заполненного поля "предыдущий обработчик"
            foreach (var bs2 in behindSockets)
            {
                //Если нашелся обработчик
                if (bs2.GetBehindLOPS() != null)
                {
                    //Если найденный обработчик не указывает на обработчик предыдущий
                    if (bs2.GetBehindLOPS() != behindCalledlops)
                    {
                        //Если поле есть - вызываем эту же ф-ю рекурсивно в найденном обработчике
                        if (bs2.GetBehindLOPS().BehindSocketsHasAConnectedWire(this))
                        {
                            return true;
                        }
                    }
                }
            }
            //Если это обработчик в цепи, имеющий индекс 0+, то вызываем настоящую ф-ю рекурсивно
            foreach (var bLOPs in behindLOPS)
            {
                if (bLOPs.BehindSocketsHasAConnectedWire())
                {
                    return true;
                }
            }

            return false;
        }
    }
}