using UnityEngine;
namespace OscilloscopeSimulation.Menu
{
    public sealed class ShowHideWiresButton : MonoBehaviour
    {
        public void OnButtonClick()
        {
            WiresManager.InvertVisibilityToWires();
        }
    }
}