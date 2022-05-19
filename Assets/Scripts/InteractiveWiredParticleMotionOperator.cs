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

        private readonly WiresManager wiresManager;

        private readonly Texture2D wireInteractiveCursor;

        private int pickedParticleIndex = -1;
        private int particleIndexUnderCursor = -1;
        private bool doesTheBeamIntersectTheParticle;

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
            particleIndexUnderCursor = eventArgs.particleIndex;

            if (!CanInteractWithWire())
                return;

            SetRayIsAcrossingTheParticle(true);

            if (Input.GetMouseButtonDown(0))
            {
                GrabTheParticle();
            }
        }
        private void OnParticleNone(ObiParticlePicker.ParticlePickEventArgs eventArgs)
        {
            SetRayIsAcrossingTheParticle(false);
        }

        private bool CanInteractWithWire()
        {
            foreach (var wire in wiresManager.GetWires())
            {
                WireRope wireRope = wire.GetWireRope();

                if (wireRope.ContainsSolverIndices(particleIndexUnderCursor))
                {
                    if (!wire.IsFullyConnected())
                    {
                        break;
                    }
                    if (wireRope.PossibleToMoveTheParticle(particleIndexUnderCursor))
                    {
                        return true;
                    }
                    break;
                }
            }

            return false;
        }

        private void GrabTheParticle()
        {
            pickedParticleIndex = particleIndexUnderCursor;
            solver.invMasses[pickedParticleIndex] = 0;
            solver.velocities[pickedParticleIndex] = Vector3.zero;
        }

        private void SetRayIsAcrossingTheParticle(bool value)
        {
            doesTheBeamIntersectTheParticle = value;

            SetCursorType();
        }

        private void SetCursorType()
        {
            Texture2D cursorTexture = null;

            if (doesTheBeamIntersectTheParticle || (pickedParticleIndex != -1))
            {
                cursorTexture = wireInteractiveCursor;
            }

            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }

        internal bool DoesTheBeamIntersectTheParticle()
        {
            return doesTheBeamIntersectTheParticle;
        }

        internal int GetParticleIndexUnderCursor()
        {
            return particleIndexUnderCursor;
        }
    }
}