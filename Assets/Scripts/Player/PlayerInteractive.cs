using Obi;

using UnityEngine;

namespace OscilloscopeSimulation.Player
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

        private WiredParticleMotionOperator wiresParticleMotionOperator;

        internal enum WireInteractiveModes
        {
            Inserting,
            Moving
        }
        private WireInteractiveModes mode;

        private void Start()
        {
            wiresParticleMotionOperator = new WiredParticleMotionOperator(obiSolver, obiParticlePicker, wiresManager, wireInteractiveCursor, this);
        }

        private void LateUpdate()
        {
            mode = Input.GetKey(KeyCode.LeftControl) ? WireInteractiveModes.Moving : WireInteractiveModes.Inserting;

            if (mode == WireInteractiveModes.Inserting)
            {
                ThrowRayFromMouseAndTryToInteract();
            }

            wiresParticleMotionOperator.Update();

            mousePositionInBehindFrame = Input.mousePosition;

            if (Input.GetMouseButtonDown(1))
            {
                DeleteWireIfPossible();
            }
        }

        private void ThrowRayFromMouseAndTryToInteract()
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
            return wiresParticleMotionOperator.DoesTheBeamIntersectTheParticle();
        }

        private void DeleteWireUnderCursor()
        {
            int particleIndexUnderCursor = wiresParticleMotionOperator.GetParticleIndexUnderCursor();
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

        internal WireInteractiveModes GetWireInteractiveMode()
        {
            return mode;
        }
    }
}