using OscilloscopeSimulation.InteractableObjects;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace OscilloscopeSimulation
{
    //�������� - ����������
    internal sealed class AdderLOPS : LogicalOperationsProcessingSystem
    {
        [SerializeField] private List<WireSocketInteractable> sumSockets =
            new List<WireSocketInteractable>();

        [SerializeField] private List<WireSocketInteractable> digitSockets =
            new List<WireSocketInteractable>();

        [SerializeField] private WireSocketInteractable invertedDigitSocket;

        protected override void LateUpdate()
        {
            string binarySumValue = ConvertSumValueFromDegToBin();
            bool sumLogicalValue = CalculateSumLogicalValue(binarySumValue);
            bool digitLogicalValue = CalculateDigitLogicalValue(binarySumValue);


            foreach (var sS in sumSockets)
            {
                sS.LogicalValue = sumLogicalValue;
            }

            foreach (var dS in digitSockets)
            {
                dS.LogicalValue = digitLogicalValue;
            }
            invertedDigitSocket.LogicalValue = !digitLogicalValue;
        }

        private string ConvertSumValueFromDegToBin()
        {
            int sumValue = 0;

            //���������� �� ����. �������� � ���������� �� ��������
            foreach (var behindSocket in behindSockets)
            {
                sumValue += behindSocket.LogicalValue ? 1 : 0;
            }

            //����������� ��������� ���������� �������� � ��������
            return Convert.ToString(sumValue, 2);
        }

        private bool CalculateSumLogicalValue(string binarySumValue)
        {
            bool sumLogicalValue = false;

            if (binarySumValue.Length == 1)
            {
                if (binarySumValue[0] == '1')
                {
                    sumLogicalValue = true;
                }
            }
            else if (binarySumValue.Length == 2)
            {
                if (binarySumValue[1] == '1')
                {
                    sumLogicalValue = true;
                }
            }
            return sumLogicalValue;
        }

        private bool CalculateDigitLogicalValue(string binarySumValue)
        {
            bool digitLogicalValue = false;

            if (binarySumValue.Length == 2)
            {
                if (binarySumValue[0] == '1')
                {
                    digitLogicalValue = true;
                }
            }
            return digitLogicalValue;
        }
    }
}