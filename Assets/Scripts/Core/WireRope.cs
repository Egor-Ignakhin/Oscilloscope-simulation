using System.Linq;

using UnityEngine;

namespace OscilloscopeSimulation
{
    [System.Serializable]
    internal sealed class WireRope
    {
        [SerializeField] private Obi.ObiRope obiRope;

        internal void Initialize()
        {
            obiRope.AddToSolver();
        }

        internal bool ContainsSolverIndices(int particleIndex)
        {
            if (obiRope.solverIndices == null)
            {
                return false;
            }
            return obiRope.solverIndices.Contains(particleIndex);
        }

        private bool ParticleBelongToInnerInterval(int particleIndex)
        {
            return (particleIndex != GetIndexOfFirstElement()) &&
                (particleIndex != GetIndexOfLastElement());
        }

        private int GetIndexOfFirstElement()
        {
            return obiRope.GetElementAt(0, out _).particle1;

        }
        private int GetIndexOfLastElement()
        {
            return obiRope.GetElementAt(obiRope.particleCount - 1, out _).particle2;
        }

        internal bool PossibleToMoveTheParticle(int ppi)
        {
            return ParticleBelongToInnerInterval(ppi);
        }
    }
}