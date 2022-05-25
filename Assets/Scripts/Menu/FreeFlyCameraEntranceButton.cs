using OscilloscopeSimulation.Player;

using UnityEngine;

namespace OscilloscopeSimulation.Menu
{
    internal sealed class FreeFlyCameraEntranceButton : MonoBehaviour
    {
        public void OnButtonClick()
        {
            CameraModesOperator.SetPlayerMode(CameraModesOperator.CameraModes.FreeFly);
        }
    }
}