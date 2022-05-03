
using System;
using System.Collections.Generic;

using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;

namespace OscilloscopeSimulation
{
    internal sealed class LogicalOperationsProcessingSystem : MonoBehaviour, ILogicalValue
    {
        public Action<bool> ChangeValueEvent { get; set; }
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

        [SerializeField] private List<WireSocketInteractable> behindSockets = new List<WireSocketInteractable>();
        [SerializeField] private List<LogicalOperationsProcessingSystem> behindLOPS = new List<LogicalOperationsProcessingSystem>();

        private void Start()
        {
            //��� �������� ����� ���������� ������������� ���������
            //(����, �����)- ���������� ���. ���������
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
        /// ����� ��������� ���������� ��������� ���������
        /// </summary>
        /// <param name="sysOperator"></param>
        /// <returns></returns>
        private bool OperateBehindLogicalValues(Operators sysOperator)
        {
            switch (sysOperator)
            {
                case Operators.And:
                    {
                        foreach (ILogicalValue blv in behindLogicalValues)
                        {
                            if (!blv.Value)
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
                            if (blv.Value)
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
        /// � ������ ������ ���� ���������� �������� �������� �� 
        /// ���������� ���. ��������� ��������� ���������� � ����� 
        /// ���������� ����������� ���������� ������������ ���. ��������
        /// </summary>
        private void LateUpdate()
        {
            Value = OperateBehindLogicalValues(systemOperator);

            foreach (ILogicalValue alv in aheadLogicalValues)
            {
                alv.Value = Value;
            }
        }
        /// <summary>
        /// ����� ���������� ������, ���� ���� � �� � ������ �� ���������� ������� �������� ������
        /// </summary>
        /// <returns></returns>
        internal bool BehindSocketsHasAConnectedWire()
        {
            //���� ��� ������� ����������, �� ��������� � ������� ����������� ������ ������
            foreach(var bs in behindSockets)
            {
                if (bs.ConnectedWire)
                    return true;
            }
            //���� ��� ���������� � ����, ������� ������ 0+, �� �������� ��������� �-� ����������
            foreach (var bLOPs in behindLOPS)
            {
                if (bLOPs.BehindSocketsHasAConnectedWire())
                    return true;
            }

            return false;
        }
    }
}