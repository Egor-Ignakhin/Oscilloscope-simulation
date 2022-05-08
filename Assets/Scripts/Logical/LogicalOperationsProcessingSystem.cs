
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
        /// ���� ��������, ������� ����� ������������ �������
        /// </summary>
        protected enum Operators { And, Or, NOR, NAND, Add }

        /// <summary>
        /// ��������� �������� �� ��������� ��� �������
        /// </summary>
        [SerializeField] protected Operators systemOperator;

        //���������� ���. ��������
        [SerializeField] private List<GameObject> behindLogicalValuesGM = new List<GameObject>();
        private readonly List<ILogicalValue> behindLogicalValues = new List<ILogicalValue>();

        //���������� ���. ��������
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
        /// ����� ��������� ���������� ���������� ���������
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
        /// � ������ ������ ���� ���������� �������� �������� �� 
        /// ���������� ���. ��������� ��������� ���������� � ����� 
        /// ���������� ����������� ���������� ������������ ���. ��������
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
        ///  ����� ���������� ������, ���� ���� � �� � ������ �� ���������� ������� �������� ������
        /// </summary>
        /// <param name="behindCalledlops"></param>
        /// <returns></returns>
        internal bool BehindSocketsHasAConnectedWire(LogicalOperationsProcessingSystem behindCalledlops = null)
        {
            //���� ��� ������� ����������, �� ��������� � ������� ����������� ������ ������
            foreach (var bs in behindSockets)
            {
                if (bs.ConnectedWire)
                {
                    return true;
                }
            }
            //���������, ��� �� � ������� ��������� ������
            //������������ ���� "���������� ����������"
            foreach (var bs2 in behindSockets)
            {
                //���� ������� ����������
                if (bs2.GetBehindLOPS() != null)
                {
                    //���� ��������� ���������� �� ��������� �� ���������� ����������
                    if (bs2.GetBehindLOPS() != behindCalledlops)
                    {
                        //���� ���� ���� - �������� ��� �� �-� ���������� � ��������� �����������
                        if (bs2.GetBehindLOPS().BehindSocketsHasAConnectedWire(this))
                        {
                            return true;
                        }
                    }
                }
            }
            //���� ��� ���������� � ����, ������� ������ 0+, �� �������� ��������� �-� ����������
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