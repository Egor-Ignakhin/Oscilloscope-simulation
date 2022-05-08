using UnityEngine;
namespace OscilloscopeSimulation
{
    sealed class ShowHideWiresButton : MonoBehaviour
    {
        [SerializeField] private WiresManager wiresManager;

        public void OnButtonClick()
        {
            wiresManager.SetVisibilityToWires();
        }
    }
}