using UnityEngine;

namespace OscilloscopeSimulation
{
    internal sealed class WireRenderer : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        internal void SetVisible(bool visibility)
        {
            meshRenderer.enabled = visibility;
        }

        internal void SetMaterialColor(Color color)
        {
            meshRenderer.material.color = color;
        }
    }
}