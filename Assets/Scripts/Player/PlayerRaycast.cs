using UnityEngine;

namespace OscilloscopeSimulation.Player
{
    [System.Serializable]
    internal sealed class PlayerRaycast
    {
        private static Vector3 lastRaycastPointPosition;

        [SerializeField] private Camera techCamera;

        internal void Update()
        {
            ThrowRayFromMouseAndTryToInteract();
        }

        private void ThrowRayFromMouseAndTryToInteract()
        {
            Ray ray = techCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit,
                float.PositiveInfinity, ~0, QueryTriggerInteraction.Ignore))
            {
                return;
            }
            if (hit.transform.TryGetComponent(out Interactable hitInteractable))
            {
                //Если нажата левая кнопка мыши
                if (Input.GetMouseButtonDown(0))
                {
                    hitInteractable.Interact();
                }
            }
            lastRaycastPointPosition = hit.point;
        }

        internal static Vector3 GetLastHitPosition()
        {
            return lastRaycastPointPosition;
        }
    }
}