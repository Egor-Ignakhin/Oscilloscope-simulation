
using OscilloscopeSimulation.InteractableObjects;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace OscilloscopeSimulation.Logical.LOPS
{
    internal abstract class LogicalOperationsProcessingSystem : MonoBehaviour, ILogicalValue
    {
        private bool logicalValue;
        public Action<bool> ChangeValueEvent { get; set; }

        [SerializeField] private List<GameObject> behindLogicalValuesGM = new List<GameObject>();
        protected readonly List<ILogicalValue> behindLogicalHolders = new List<ILogicalValue>();

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
            logicalValue = OperateBehindLogicalValues();

            foreach (ILogicalValue aheadLogicalHolder in aheadLogicalHolders)
            {
                aheadLogicalHolder.SetLogicalValue(logicalValue);
            }
        }

        protected abstract bool OperateBehindLogicalValues();

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