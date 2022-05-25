using System;

using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    [Serializable]
    internal sealed class SocketText
    {
        private static bool generalVisibility;
        private static event Action GeneralVisibilityUpdate;

        [SerializeField] private TMPro.TextMeshPro textMeshPro;

        static SocketText()
        {
            InvertGeneralVisibility();
        }

        internal void Initialize(bool itsOutOrToggleSwitchSocket)
        {
            textMeshPro.enabled = false;

            if (itsOutOrToggleSwitchSocket)
            {
                GeneralVisibilityUpdate += OnGeneralVisibilityUpdate;
                OnGeneralVisibilityUpdate();
            }
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
            textMeshPro.enabled = generalVisibility;
        }

        ~SocketText()
        {
            GeneralVisibilityUpdate -= OnGeneralVisibilityUpdate;
        }
    }
}