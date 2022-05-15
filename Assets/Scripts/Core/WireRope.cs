using System.Linq;

using UnityEngine;

namespace OscilloscopeSimulation
{
    internal sealed class WireRope : MonoBehaviour
    {
        [SerializeField] private Obi.ObiRope obiRope;

        internal bool ContainsSolverIndices(int particleIndex)
        {
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