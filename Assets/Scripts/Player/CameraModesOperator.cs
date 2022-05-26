
using System;

using UnityEngine;

namespace OscilloscopeSimulation.Player
{
    internal sealed class CameraModesOperator : MonoBehaviour
    {
        [SerializeField] private GameObject UI;
        internal enum CameraModes
        {
            TechInteraction,
            FreeFly
        }
        private static CameraModes cameraMode = CameraModes.TechInteraction;
        internal static event Action<CameraModes> OnCameraModesChanged;

        private void Start()
        {
            OnCameraModesChanged +=OnCameraModeChangedHandler;
        }

        internal static void SetPlayerMode(CameraModes mode)
        {
            cameraMode = mode;
            OnCameraModesChanged?.Invoke(cameraMode);
        }

        private void OnCameraModeChangedHandler(CameraModes cameraMode)
        {
            if (cameraMode == CameraModes.TechInteraction)
            {
                UI.SetActive(true);
            }
            else
            {
                UI.SetActive(false);
            }
        }
    }
}