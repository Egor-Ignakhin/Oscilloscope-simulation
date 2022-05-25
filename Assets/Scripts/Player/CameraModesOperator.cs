
using System;

using UnityEngine;

namespace OscilloscopeSimulation.Player
{
    internal sealed class CameraModesOperator : MonoBehaviour
    {
        internal enum CameraModes
        {
            TechInteraction,
            FreeFly
        }
        private static CameraModes cameraMode = CameraModes.TechInteraction;
        internal static event Action <CameraModes> OnCameraModesChanged;

        internal static void SetPlayerMode(CameraModes mode)
        {
            cameraMode = mode;
            OnCameraModesChanged?.Invoke(cameraMode);
        }
    }
}