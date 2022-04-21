using System;

using UnityEngine;
namespace OscilloscopeSimulation
{
    internal sealed class PlayerInteractive : MonoBehaviour
    {
        [SerializeField] private Camera mcamera;
        private Vector3 lastHitPoint;
        private void Update()
        {
            Ray ray = mcamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, ~0, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.TryGetComponent(out Interactable interactable))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        interactable.Interact();
                    }
                }
                lastHitPoint = hit.point;
            }
        }

        internal Vector3 GetLastHitPoint() => lastHitPoint;
    }
}