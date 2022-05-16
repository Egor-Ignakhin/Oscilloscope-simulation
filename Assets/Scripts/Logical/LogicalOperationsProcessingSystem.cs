
using OscilloscopeSimulation.InteractableObjects;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace OscilloscopeSimulation
{
    internal class LogicalOperationsProcessingSystem : MonoBehaviour, ILogicalValue
    {
        private bool logicalValue;
        public Action<bool> ChangeValueEvent { get; set; }

        protected enum Operations { And, Or, NOR, NAND, Add }
        [SerializeField] protected Operations selectedOperator;

        [SerializeField] private List<GameObject> behindLogicalValuesGM = new List<GameObject>();
        private readonly List<ILogicalValue> behindLogicalHolders = new List<ILogicalValue>();

        [SerializeField] protected List<GameObject> aheadLogicalValuesGM = new List<GameObject>();
        protected readonly List<ILogicalValue> aheadLogicalHolders = new List<ILogicalValue>();

        [SerializeField]
        protected List<WireSocketInteractable> behindSockets =
            new List<WireSocketInteractable>();

        [SerializeField]
        protected List<LogicalOperationsProcessingSystem> behindLOPS
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
            logicalValue = OperateBehindLogicalValues(selectedOperator);

            foreach (ILogicalValue aheadLogicalHolder in aheadLogicalHolders)
            {
                aheadLogicalHolder.SetLogicalValue(logicalValue);
            }
        }

        protected bool OperateBehindLogicalValues(Operations sOperator)
        {
            switch (sOperator)
            {
                case Operations.And:
                    {
                        foreach (ILogicalValue blv in behindLogicalHolders)
                        {
                            if (!blv.GetLogicalValue())
                            {
                                return false;
                            }
                        }
                        return true;
                    }

                case Operations.Or:
                    {
                        foreach (ILogicalValue blv in behindLogicalHolders)
                        {
                            if (blv.GetLogicalValue())
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                case Operations.NOR:
                    {
                        return !OperateBehindLogicalValues(Operations.Or);
                    }
                case Operations.NAND:
                    {
                        return !OperateBehindLogicalValues(Operations.And);
                    }
                default:
                    {
                        throw new Exception("");
                    }
            }
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