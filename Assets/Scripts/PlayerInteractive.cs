using Obi;

using UnityEngine;
namespace OscilloscopeSimulation
{
    /// <summary>
    /// Оператор взаимодействия пользовательского ввода
    /// </summary>
    internal sealed class PlayerInteractive : MonoBehaviour
    {
        private static Vector3 lastRaycastPointPosition;
        private static Vector3 mousePositionInBehindFrame;

        [SerializeField] private Camera mainCamera;

        [SerializeField] private Texture2D wireInteractiveCursor;

        [SerializeField] private ObiSolver obiSolver;
        [SerializeField] private ObiParticlePicker obiParticlePicker;

        [SerializeField] private WiresManager wiresManager;

        private InteractiveWiredParticleMotionOperator interactiveWiresParticleMotionOperator;

        private void Start()
        {
            interactiveWiresParticleMotionOperator = new InteractiveWiredParticleMotionOperator(obiSolver, obiParticlePicker, wiresManager, wireInteractiveCursor);
        }

        private void LateUpdate()
        {
            ThrowRayFromMouse();

            interactiveWiresParticleMotionOperator.Update();

            mousePositionInBehindFrame = Input.mousePosition;

            if (Input.GetMouseButtonDown(1))
            {
                DeleteWireIfPossible();
            }
        }

        private void ThrowRayFromMouse()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
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

        private void DeleteWireIfPossible()
        {
            if (HasWireUnderCursor())
            {
                DeleteWireIfItIsUnderCursor();
            }
            else if (wiresManager.HasActiveWire())
            {
                wiresManager.DeleteActiveWire();
            }
        }

        private void DeleteWireIfItIsUnderCursor()
        {
            if (HasWireUnderCursor())
            {
                DeleteWireUnderCursor();
            }
        }

        private bool HasWireUnderCursor()
        {
            return interactiveWiresParticleMotionOperator.DoesTheBeamIntersectTheParticle();
        }

        private void DeleteWireUnderCursor()
        {
            int particleIndexUnderCursor = interactiveWiresParticleMotionOperator.GetParticleIndexUnderCursor();
            Wire wire = wiresManager.GetWireByParticleIndex(particleIndexUnderCursor);
            wire.DeleteWire();
        }

        internal static Vector3 GetLastRaycastPointPosition()
        {
            return lastRaycastPointPosition;
        }

        internal static Vector3 GetMousePositionInBehindFrame()
        {
            return mousePositionInBehindFrame;
        }
    }
}