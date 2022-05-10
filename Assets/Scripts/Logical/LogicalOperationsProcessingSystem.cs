
using System;
using System.Collections.Generic;

using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;

namespace OscilloscopeSimulation
{
    internal class LogicalOperationsProcessingSystem : MonoBehaviour, ILogicalValue
    {
        public bool LogicalValue { get; set; }
        public Action<bool> ChangeValueEvent { get; set; }

        /// <summary>
        /// Типы операций, которые может осуществлять система
        /// </summary>
        protected enum Operators { And, Or, NOR, NAND, Add }
        /// <summary>
        /// TODO: рефакторинг
        /// </summary>
        [SerializeField] protected Operators systemOperator;

        [SerializeField] private List<GameObject> behindLogicalValuesGM = new List<GameObject>();
        private readonly List<ILogicalValue> behindLogicalHolders = new List<ILogicalValue>();

        [SerializeField] protected List<GameObject> aheadLogicalValuesGM = new List<GameObject>();
        protected readonly List<ILogicalValue> aheadLogicalHolders = new List<ILogicalValue>();

        [SerializeField] protected List<WireSocketInteractable> behindSockets =
            new List<WireSocketInteractable>();

        [SerializeField] protected List<LogicalOperationsProcessingSystem> behindLOPS
            = new List<LogicalOperationsProcessingSystem>();

        private void Start()
        {
            for (int i = 0; i < behindLogicalValuesGM.Count; i++)
            {
                behindLogicalHolders.Add(behindLogicalValuesGM[i].GetComponent<ILogicalValue>());

                if (behindLogicalHolders[i] == null)
                {
                    throw new Exception("crashed link");
                }
            }

            for (int i = 0; i < aheadLogicalValuesGM.Count; i++)
            {
                aheadLogicalHolders.Add(aheadLogicalValuesGM[i].GetComponent<ILogicalValue>());

                if (aheadLogicalHolders[i] == null)
                {
                    throw new Exception("crashed link");
                }
            }
        }

        protected virtual void LateUpdate()
        {
            LogicalValue = OperateBehindLogicalValues(systemOperator);

            foreach (ILogicalValue aheadLogicalHolder in aheadLogicalHolders)
            {
                aheadLogicalHolder.LogicalValue = LogicalValue;
            }
        }

        protected bool OperateBehindLogicalValues(Operators sysOperator)
        {
            switch (sysOperator)
            {
                case Operators.And:
                    {
                        foreach (ILogicalValue blv in behindLogicalHolders)
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
                        foreach (ILogicalValue blv in behindLogicalHolders)
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
                        return !OperateBehindLogicalValues(Operators.Or);
                    }
                case Operators.NAND:
                    {
                        return !OperateBehindLogicalValues(Operators.And);
                    }
                default:
                    {
                        throw new Exception("");
                    }
            }
        }
    }
}