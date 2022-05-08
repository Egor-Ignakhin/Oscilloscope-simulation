using System;
using System.Collections.Generic;

using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;

namespace OscilloscopeSimulation
{
    //�������� - ����������
    internal sealed class AdderLOPS : LogicalOperationsProcessingSystem
    {
        [SerializeField] private List<WireSocketInteractable> sumSockets = new List<WireSocketInteractable>();

        [SerializeField] private List<WireSocketInteractable> digitSockets = new List<WireSocketInteractable>();

        [SerializeField] private WireSocketInteractable invertedDigitSocket;
        private void CalculateOutputValue()
        {
            int sumValue = 0;

            //���������� �� ����. �������� � ���������� �� ��������
            foreach (var bs in behindSockets)
            {
                sumValue += bs.LogicalValue ? 1 : 0;
            }

            //����������� ��������� ���������� �������� � ��������
            string binaryCode = Convert.ToString(sumValue, 2);

            if (binaryCode.Length == 1)
            {
                //���������� � ����-������ �����
                foreach (var sS in sumSockets)
                {
                    sS.LogicalValue = binaryCode[0] == '0' ? false : true;
                }

                //���������� � ������-������ ����
                foreach (var dS in digitSockets)
                {
                    dS.LogicalValue = false;
                }
            }
            else if (binaryCode.Length == 2)
            {
                //���������� � ����-������ �����
                foreach (var sS in sumSockets)
                {
                    sS.LogicalValue = binaryCode[1] == '0' ? false : true;
                }

                //���������� � ������-������ ������
                foreach (var dS in digitSockets)
                {
                    dS.LogicalValue = binaryCode[0] == '0' ? false : true;
                }
            }

            invertedDigitSocket.LogicalValue = !digitSockets[0].LogicalValue;
        }
        protected override void LateUpdate()
        {
            CalculateOutputValue();
        }
    }
}