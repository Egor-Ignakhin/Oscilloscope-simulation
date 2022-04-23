
using System.Collections.Generic;

using UnityEngine;

namespace OscilloscopeSimulation
{
    internal sealed class LogicalOperationsProcessingSystem : MonoBehaviour, ILogicalValue
    {
        /// <summary>
        /// ���� ��������, ������� ����� ������������ �������
        /// </summary>
        private enum Operators { And, Or, NOR, NAND }

        /// <summary>
        /// ��������� �������� �� ��������� ��� �������
        /// </summary>
        [SerializeField] private Operators systemOperator;

        //���������� ���. ��������
        [SerializeField] private List<GameObject> behindLogicalValuesGM = new List<GameObject>();
        private readonly List<ILogicalValue> behindLogicalValues = new List<ILogicalValue>();

        //���������� ���. ��������
        [SerializeField] private List<GameObject> aheadLogicalValuesGM = new List<GameObject>();
        private readonly List<ILogicalValue> aheadLogicalValues = new List<ILogicalValue>();

        public bool Value { get; set; }

        private void Start()
        {
            //��� �������� ����� ���������� ������������� ���������
            //(����, �����)- ���������� ���. ���������
            for (int i = 0; i < behindLogicalValuesGM.Count; i++)
            {
                behindLogicalValues.Add(behindLogicalValuesGM[i].GetComponent<ILogicalValue>());
            }

            for (int i = 0; i < aheadLogicalValuesGM.Count; i++)
            {
                aheadLogicalValues.Add(aheadLogicalValuesGM[i].GetComponent<ILogicalValue>());
            }
        }

        /// <summary>
        /// ����� ��������� ���������� ��������� ���������
        /// </summary>
        /// <param name="sysOperator"></param>
        /// <returns></returns>
        private bool OperateBehindLogicalValues(Operators sysOperator)
        {
            switch (sysOperator)
            {
                case Operators.And:

                    foreach (ILogicalValue blv in behindLogicalValues)
                    {
                        if (!blv.Value)
                        {
                            return false;
                        }
                    }

                    return true;

                case Operators.Or:

                    foreach (ILogicalValue blv in behindLogicalValues)
                    {
                        if (blv.Value)
                        {
                            return true;
                        }
                    }

                    return false;

                case Operators.NOR:
                    bool orOp = OperateBehindLogicalValues(Operators.Or);
                    return !orOp;

                case Operators.NAND:
                    bool andOp = OperateBehindLogicalValues(Operators.And);
                    return !andOp;

                default:
                    throw new System.Exception("");
            }
        }

        /// <summary>
        /// � ������ ������ ���� ���������� �������� �������� �� 
        /// ���������� ���. ��������� ��������� ���������� � ����� 
        /// ���������� ����������� ���������� ������������ ���. ��������
        /// </summary>
        private void Update()
        {
            Value = OperateBehindLogicalValues(systemOperator);

            foreach (ILogicalValue alv in aheadLogicalValues)
            {
                alv.Value = Value;
            }
        }
    }
}