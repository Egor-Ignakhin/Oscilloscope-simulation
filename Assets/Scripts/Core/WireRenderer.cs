using UnityEngine;

namespace OscilloscopeSimulation
{
    [System.Serializable]
    internal sealed class WireRenderer
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

        internal bool IsVisible()
        {
            return meshRenderer.enabled;
        }
    }
}