
using UnityEngine;

namespace OscilloscopeSimulation
{
    public class LogicalOperationsProcessingSystem : MonoBehaviour, LogicalValue
    {
        private enum Operators { And, Or, Not }

        [SerializeField] private Operators systemOperator;


        [SerializeField] private GameObject behindLogicalValue_1GM;
        [SerializeField] private GameObject behindLogicalValue_2GM;
        private LogicalValue behindLogicalValue_1;
        private LogicalValue behindLogicalValue_2;

        [SerializeField] private GameObject aheadLogicalValueGM;
        private LogicalValue aheadLogicalValue;

        [SerializeField] private LogicalOperationsProcessingSystem behindSystem;

        private bool tempValue;

        public bool Value { get; set; }

        private void Start()
        {
            aheadLogicalValue = aheadLogicalValueGM.GetComponent<LogicalValue>();
            behindLogicalValue_1 = behindLogicalValue_1GM.GetComponent<LogicalValue>();
            behindLogicalValue_2 = behindLogicalValue_2GM.GetComponent<LogicalValue>();
        }

        private void Update()
        {
            switch (systemOperator)
            {
                case Operators.And:
                    tempValue = behindLogicalValue_1.Value && behindLogicalValue_2.Value;

                    aheadLogicalValue.Value = tempValue;
                    break;

                case Operators.Or:
                    tempValue = behindLogicalValue_1.Value || behindLogicalValue_2.Value;

                    aheadLogicalValue.Value = tempValue;
                    break;

                case Operators.Not:
                    tempValue = !behindSystem.GetValue();

                    aheadLogicalValue.Value = tempValue;
                    break;
            }
        }

        private bool GetValue()
        {
            return tempValue;
        }
    }
}