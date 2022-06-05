using System;

using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    [Serializable]
    internal sealed class WireSocketText
    {
        private static bool generalVisibility = true;
        private static event Action GeneralVisibilityUpdate;

        private bool canVisibility = false;

        [SerializeField] private TMPro.TextMeshPro textMeshPro;

        internal void Initialize()
        {
            textMeshPro.enabled = false;

            GeneralVisibilityUpdate += OnGeneralVisibilityUpdate;
            OnGeneralVisibilityUpdate();
        }

        internal void Write(string text)
        {
            textMeshPro.SetText(text);
        }

        internal static void InvertGeneralVisibility()
        {
            generalVisibility = !generalVisibility;
            GeneralVisibilityUpdate?.Invoke();
        }

        private void OnGeneralVisibilityUpdate()
        {
            textMeshPro.enabled = generalVisibility && canVisibility;
        }

        internal void SetCanVisibility(bool value)
        {
            canVisibility = value;
            OnGeneralVisibilityUpdate();
        }

        ~WireSocketText()
        {
            GeneralVisibilityUpdate -= OnGeneralVisibilityUpdate;
        }
    }
}