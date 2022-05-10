
using UnityEngine;
namespace OscilloscopeSimulation
{
    /// <summary>
    /// �������� �������������� ����������������� �����
    /// </summary>
    internal sealed class PlayerInteractive : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        public Vector3 LastRaycastPointPosition { get; private set; }

        private void Update()
        {
            //������� ��� �� ������
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, ~0, QueryTriggerInteraction.Ignore))
            {
                return;
            }
            if (hit.transform.TryGetComponent(out Interactable hitInteractable))
            {
                //���� ������ ����� ������ ����
                if (Input.GetMouseButtonDown(0))
                {
                    hitInteractable.Interact();
                }
            }

            LastRaycastPointPosition = hit.point;
        }
    }
}