using Obi;

using UnityEngine;

namespace OscilloscopeSimulation
{
    /// <summary>
    /// ќператор движени€ интерактивных частиц провода
    /// </summary>
    internal sealed class InteractiveWiredParticleMotionOperator
    {
        private readonly ObiSolver solver;

        private int pickedParticleIndex = -1;
        private bool RayIsAcrossingTheParticle;

        private readonly WiresManager wiresManager;

        private readonly Texture2D wireInteractiveCursor;

        public InteractiveWiredParticleMotionOperator(ObiSolver solver, ObiParticlePicker particlePicker,
            WiresManager wiresManager, Texture2D wireInteractiveCursor)
        {
            this.solver = solver;
            this.wiresManager = wiresManager;
            this.wireInteractiveCursor = wireInteractiveCursor;

            particlePicker.OnParticleAcrossing.AddListener(OnWireParticleAcrossing);
            particlePicker.OnParticleNone.AddListener(OnParticleNone);
        }

        internal void Update()
        {
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
                solver.invMasses[pickedParticleIndex] = 100000;
                pickedParticleIndex = -1;
            }
        }
        private void MovePickedParticle()
        {
            Vector3 mousePositionVector = Input.mousePosition - PlayerInteractive.GetMousePositionInBehindFrame();

            solver.positions[pickedParticleIndex] += 0.25f * Time.deltaTime *
                new Vector4(mousePositionVector.y, 0, -mousePositionVector.x, 0);
        }

        private void OnWireParticleAcrossing(ObiParticlePicker.ParticlePickEventArgs eventArgs)
        {
            if (!CanInteractWithWire(eventArgs))
                return;

            SetRayIsAcrossingTheParticle(true);

            if (Input.GetMouseButtonDown(0))
            {
                GrabTheParticle(eventArgs);
            }
        }
        private void OnParticleNone(ObiParticlePicker.ParticlePickEventArgs eventArgs)
        {
            SetRayIsAcrossingTheParticle(false);
        }

        private bool CanInteractWithWire(ObiParticlePicker.ParticlePickEventArgs eventArgs)
        {
            foreach (var wire in wiresManager.GetWires())
            {
                WireRope wireRope = wire.GetWireRope();
                int ppi = eventArgs.particleIndex;

                if (!wireRope.ContainsSolverIndices(ppi))
                {
                    continue;
                }

                if (wire.GetSocket_2() == null)
                {
                    return false;
                }

                if (!wireRope.PossibleToMoveTheParticle(ppi))
                {
                    return false;
                }

                break;
            }
            return true;
        }

        private void GrabTheParticle(ObiParticlePicker.ParticlePickEventArgs eventArgs)
        {
            pickedParticleIndex = eventArgs.particleIndex;
            solver.invMasses[pickedParticleIndex] = 0;
            solver.velocities[pickedParticleIndex] = Vector3.zero;
        }

        private void SetRayIsAcrossingTheParticle(bool value)
        {                       
            RayIsAcrossingTheParticle = value;

            SetCursorType();
        }

        private void SetCursorType()
        {
            Texture2D cursorTexture = null;

            if (RayIsAcrossingTheParticle || (pickedParticleIndex != -1))
            {
                cursorTexture = wireInteractiveCursor;
            }

            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }
}