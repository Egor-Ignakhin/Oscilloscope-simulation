using OscilloscopeSimulation.Player;

using UnityEngine;

namespace OscilloscopeSimulation.Menu
{
    public sealed class FreeFlyCameraEntranceButton : MonoBehaviour
    {
        public void OnButtonClick()
        {
            PlayerInteractive.SetPIM(PlayerInteractiveModes.FreeFlight);
        }
    }
}