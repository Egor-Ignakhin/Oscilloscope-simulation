
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

        private InteractiveWiredParticleMotionOperator wireParticlesMover;

        private void Start()
        {
            wireParticlesMover = new InteractiveWiredParticleMotionOperator(obiSolver, obiParticlePicker, wiresManager, wireInteractiveCursor);            
        }

        private void LateUpdate()
        {
            ThrowRayFromMouse();

            wireParticlesMover.Update();
            
            mousePositionInBehindFrame = Input.mousePosition;
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