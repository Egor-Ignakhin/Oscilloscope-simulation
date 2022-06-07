using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;

namespace OscilloscopeSimulation.Menu
{
    public sealed class SocketTextInverterButton : MonoBehaviour
    {
        public void OnButtonClick()
        {
            WireSocketText.InvertGeneralVisibility();
        }
    }
}