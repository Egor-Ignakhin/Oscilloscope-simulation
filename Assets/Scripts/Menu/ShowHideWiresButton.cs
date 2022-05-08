using UnityEngine;
namespace OscilloscopeSimulation
{
    internal sealed class ShowHideWiresButton : MonoBehaviour
    {
        [SerializeField] private WiresManager wiresManager;

        public void OnButtonClick()
        {
            wiresManager.SetVisibilityToWires();
        }
    }
}