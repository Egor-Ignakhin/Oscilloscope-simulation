using System;

using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    [Serializable]
    public sealed class WireSocketText
    {
        public static bool GeneralVisibility { get; private set; } = true;
        private static event Action GeneralVisibilityUpdate;

        private bool canVisibility = false;

        [SerializeField] private TMPro.TextMeshPro textMeshPro;

        public void Initialize()
        {
            textMeshPro.enabled = false;

            GeneralVisibilityUpdate += OnGeneralVisibilityUpdate;
            OnGeneralVisibilityUpdate();
        }

        public void Write(string text)
        {
            textMeshPro.SetText(text);
        }

        internal static void InvertGeneralVisibility()
        {
            GeneralVisibility = !GeneralVisibility;
            GeneralVisibilityUpdate?.Invoke();
        }

        private void OnGeneralVisibilityUpdate()
        {
            textMeshPro.enabled = GeneralVisibility && canVisibility;
        }

        public void SetCanVisibility(bool value)
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