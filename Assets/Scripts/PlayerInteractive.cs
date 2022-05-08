
using UnityEngine;
namespace OscilloscopeSimulation
{
    /// <summary>
    /// �������� �������������� ����������������� �����
    /// </summary>
    internal sealed class PlayerInteractive : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        /// <summary>
        /// ���������� ����� ��������������� � ���. ��������� �����
        /// </summary>
        public Vector3 LastHitPoint { get; private set; }

        /// <summary>
        /// �����, ���������� ������ ����
        /// </summary>
        private void Update()
        {
            //������� ��� �� ������
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, ~0, QueryTriggerInteraction.Ignore))
            {
                //���� � ������ ���������� �������� �������������
                if (hit.transform.TryGetComponent(out Interactable interactable))
                {
                    //���� ������ ����� ������ ����
                    if (Input.GetMouseButtonDown(0))
                    {
                        //�������� �-� �������������� � ��������
                        interactable.Interact();
                    }
                }

                //����������� ����� ��������������� �� ���� � ������
                LastHitPoint = hit.point;
            }
        }
    }
}