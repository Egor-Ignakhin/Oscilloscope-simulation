using OscilloscopeSimulation.Player;

using UnityEngine;

namespace OscilloscopeSimulation.Menu
{
    internal sealed class FreeFlyCameraEntranceButton : MonoBehaviour
    {
        [SerializeField] private PlayerInteractive playerInteractive;
        public void OnButtonClick()
        {
            playerInteractive.SetPIM(PlayerInteractiveModes.FreeFlight);
        }
    }
}