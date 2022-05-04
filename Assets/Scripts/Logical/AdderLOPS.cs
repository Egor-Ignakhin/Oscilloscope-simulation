using System;
using System.Collections.Generic;

using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;

namespace OscilloscopeSimulation
{
    //�������� - ����������
    internal sealed class AdderLOPS : LogicalOperationsProcessingSystem
    {
        /// <summary>
        /// ����-������
        /// </summary>
        [SerializeField] private List<WireSocketInteractable> sumSockets = new List<WireSocketInteractable>();

        /// <summary>
        /// ������-������
        /// </summary>
        [SerializeField] private List<WireSocketInteractable> digitSockets = new List<WireSocketInteractable>();

        /// <summary>
        /// ��������������� ������-�����
        /// </summary>
        [SerializeField] private WireSocketInteractable invertedDigitSocket;
        private void Add()
        {
            //������� �������� �����
            int SValue = 0;

            //���������� �� ����. �������� � ���������� �� ��������
            foreach (var bs in behindSockets)
            {
                SValue += bs.Value ? 1 : 0;
            }

            //����������� ��������� ���������� �������� � ��������
            string binaryCode = Convert.ToString(SValue, 2);

            //���� ����� ������ - 1
            if (binaryCode.Length == 1)
            {
                //���������� � ����-������ �����
                foreach(var ss in sumSockets)
                    ss.Value = binaryCode[0] == '0' ? false : true;

                //���������� � ������-������ ����
                foreach(var ds in digitSockets)
                    ds.Value = false;
            }
            //���� �� ����� ������ - 2
            else if(binaryCode.Length == 2)
            {
                //���������� � ����-������ �����
                foreach (var ss in sumSockets)
                    ss.Value = binaryCode[1] == '0' ? false : true;

                //���������� � ������-������ ������
                foreach (var ds in digitSockets)
                    ds.Value = binaryCode[0] == '0' ? false : true;
            }

            //���������� � ��������������� ������-����� ��������
            invertedDigitSocket.Value = !digitSockets[0].Value;
        }
        protected override void LateUpdate()
        {
            Add();
        }
    }
}