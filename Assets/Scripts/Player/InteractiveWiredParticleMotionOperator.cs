using Obi;

using UnityEngine;

namespace OscilloscopeSimulation.Player
{
    internal sealed class WiredParticleMotionOperator
    {
        private readonly ObiSolver solver;

        private readonly WiresManager wiresManager;

        private readonly Texture2D wireInteractiveCursor;

        private int pickedParticleIndex = -1;
        private int particleIndexUnderCursor = -1;
        private bool isThereWireUnderCursor;

        private readonly PlayerInteractive playerInteractive;

        public WiredParticleMotionOperator(ObiSolver solver, ObiParticlePicker particlePicker,
            WiresManager wiresManager, Texture2D wireInteractiveCursor, PlayerInteractive playerInteractive)
        {
            this.solver = solver;
            this.wiresManager = wiresManager;
            this.wireInteractiveCursor = wireInteractiveCursor;
            this.playerInteractive = playerInteractive;

            particlePicker.OnParticleAcrossing.AddListener(OnWireParticleAcrossing);
            particlePicker.OnParticleNone.AddListener(OnParticleNone);
        }

        internal void Update()
        {
            if (pickedParticleIndex == -1)
            {
                return;
            }

            var wireInteractiveMode = playerInteractive.GetPlayerInteractiveMode();
            if ((wireInteractiveMode == PlayerInteractiveModes.InsertingWires) || (!Input.GetMouseButton(0)))
            {
                solver.invMasses[pickedParticleIndex] = 100000;
                pickedParticleIndex = -1;
            }
            else if (wireInteractiveMode == PlayerInteractiveModes.MovingWires)
            {
                MovePickedParticle();
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
            isThereWireUnderCursor = value;

            SetCursorType();
        }

        private void SetCursorType()
        {
            Texture2D cursorTexture = null;

            if ((isThereWireUnderCursor || (pickedParticleIndex != -1)) &&
                (playerInteractive.GetPlayerInteractiveMode() == PlayerInteractiveModes.MovingWires))
            {
                cursorTexture = wireInteractiveCursor;
            }

            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }

        internal void DeleteWireIfPossible()
        {
            if (isThereWireUnderCursor)
            {
                DeleteWireUnderCursor();
            }
            else if (wiresManager.HasActiveWire())
            {
                wiresManager.DeleteActiveWire();
            }
        }

        private void DeleteWireUnderCursor()
        {
            int particleIndexUnderCursor = GetParticleIndexUnderCursor();
            Wire wire = wiresManager.GetWireByParticleIndex(particleIndexUnderCursor);
            wire.DeleteWire();
        }

        internal int GetParticleIndexUnderCursor()
        {
            return particleIndexUnderCursor;
        }
    }
}