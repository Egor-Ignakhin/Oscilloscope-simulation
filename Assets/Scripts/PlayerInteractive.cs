
using Obi;

using System.Linq;

using UnityEngine;
namespace OscilloscopeSimulation
{
    /// <summary>
    /// Оператор взаимодействия пользовательского ввода
    /// </summary>
    internal sealed class PlayerInteractive : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        [SerializeField] private Texture2D wireInteractiveHand;
        public Vector3 LastRaycastPointPosition { get; private set; }

        [SerializeField] private Obi.ObiSolver obiSolver;
        int pickedParticleIndex = -1;
        [SerializeField] private WiresManager wiresManager;
        [SerializeField] private ObiParticlePicker obiParticlePicker;

        private void Update()
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
            LastRaycastPointPosition = hit.point;

            if (pickedParticleIndex != -1)
            {
                if (!Input.GetMouseButton(0))
                {
                    obiSolver.invMasses[pickedParticleIndex] = 1;
                    pickedParticleIndex = -1;

                    return;
                }
                MovePickedParticle();
            }
            else
                SetDefaultCursor();
        }

        public void SetDefaultCursor()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        public void OnObiParticleAcross()
        {
            foreach (var wire in wiresManager.GetWires())
            {
                if (wire.GetObiRope().solverIndices.Contains(obiParticlePicker.GetPickedParticleIndex()))
                {
                    if (wire.GetSocket_2() == null)
                        return;

                    break;
                }
            }

            Cursor.SetCursor(wireInteractiveHand, Vector2.zero, CursorMode.Auto);

            if (Input.GetMouseButtonDown(0))
            {
                pickedParticleIndex = obiParticlePicker.GetPickedParticleIndex();
            }
        }

        private void MovePickedParticle()
        {
            obiSolver.velocities[pickedParticleIndex] = Vector4.zero;
            obiSolver.invMasses[pickedParticleIndex] = 0;
            obiSolver.positions[pickedParticleIndex] = LastRaycastPointPosition;
        }
    }
}