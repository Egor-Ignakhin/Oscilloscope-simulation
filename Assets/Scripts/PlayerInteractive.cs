
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
        public bool RayIsAcrossingTheParticle
        {
            get => rayIsAcrossingTheParticle; set
            {
                rayIsAcrossingTheParticle = value;
                if (rayIsAcrossingTheParticle)
                {
                    SetWireInteractiveCursor();
                }
                else
                {
                    SetDefaultCursor();
                }
            }
        }
        private bool rayIsAcrossingTheParticle;
        private Vector3 mousePositionInBehindFrame;

        private void LateUpdate()
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

            if (pickedParticleIndex == -1)
            {
                return;
            }

            if (Input.GetMouseButton(0))
            {
                MovePickedParticle();
            }
            else
            {
                obiSolver.invMasses[pickedParticleIndex] = 100000;
                pickedParticleIndex = -1;
            }
            mousePositionInBehindFrame = Input.mousePosition;
        }

        private void SetDefaultCursor()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        private void SetWireInteractiveCursor()
        {
            Cursor.SetCursor(wireInteractiveHand, Vector2.zero, CursorMode.Auto);
        }

        public void OnWireParticleAcross()
        {
            ObiRope wireRope = null;
            foreach (var wire in wiresManager.GetWires())
            {
                wireRope = wire.GetObiRope();
                if (wireRope.solverIndices.Contains(obiParticlePicker.GetPickedParticleIndex()))
                {
                    if ((wire.GetSocket_2() == null))
                    {
                        return;
                    }
                    break;
                }
            }

            //Сейчас мы должны проверить, является ли выделенная вершмина  - точкой start или end отрезка провода
            var ppi = obiParticlePicker.GetPickedParticleIndex();
            var firstParticleElement = wireRope.GetElementAt(0, out float elementMu);
            var endParticleElement = wireRope.GetElementAt(wireRope.particleCount - 1, out float elementMu2);
            int firstParticleIndex = firstParticleElement.particle1; // first particle in the rope
            int endParticleIndex = endParticleElement.particle2;
            bool particleBelongsToInnerInterval = (ppi != firstParticleIndex) &&
                (ppi != endParticleIndex);

            if (!particleBelongsToInnerInterval)
            {
                return;
            }
            RayIsAcrossingTheParticle = true;

            if (Input.GetMouseButtonDown(0))
            {
                pickedParticleIndex = obiParticlePicker.GetPickedParticleIndex();
                obiSolver.invMasses[pickedParticleIndex] = 0;
                obiSolver.velocities[pickedParticleIndex] = Vector3.zero;
            }
        }

        private void MovePickedParticle()
        {
            //Мы сдвигаем вершину на вектор изменения курсора
            Vector3 vChange = Input.mousePosition - mousePositionInBehindFrame;

            obiSolver.positions[pickedParticleIndex] += Mathf.Sin(Mathf.PI/2 - Mathf.Deg2Rad * mainCamera.transform.localEulerAngles.x) * Time.deltaTime * new Vector4(vChange.y, 0, -vChange.x, 0);
        }

        public void OnParticleNone()
        {
            RayIsAcrossingTheParticle = false;
        }
    }
}