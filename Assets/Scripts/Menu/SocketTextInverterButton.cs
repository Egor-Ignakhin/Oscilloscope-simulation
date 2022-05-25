using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;

namespace OscilloscopeSimulation.Menu
{
    internal sealed class SocketTextInverterButton : MonoBehaviour
    {
        public void OnButtonClick()
        {
            SocketText.InvertGeneralVisibility();
        }
    }
}