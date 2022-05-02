using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace OscilloscopeSimulation
{
    public class ShowHideWiresButton : MonoBehaviour
    {
        private WiresManager wiresManager;

        private void Start()
        {
            wiresManager = FindObjectOfType<WiresManager>();
        }
        public void OnButtonClick()
        {
            wiresManager.SetVisibilityToWires();
        }
    }
}