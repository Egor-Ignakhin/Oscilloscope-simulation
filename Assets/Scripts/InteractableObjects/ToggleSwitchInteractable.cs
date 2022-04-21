using UnityEngine;

namespace OscilloscopeSimulation.InteractableObjects
{
    internal sealed class ToggleSwitchInteractable : Interactable
    {
        private bool isEnabled = false;
        [SerializeField] private Vector3 enabledLocalEulersState;
        [SerializeField] private Vector3 disabledLocalEulersState;

        private void Start()
        {
            Switch(isEnabled);
        }
        internal override void Interact()
        {
            Switch(!isEnabled);
        }
        private void Switch(bool nextState)
        {
            isEnabled = nextState;

            transform.localEulerAngles = isEnabled ? enabledLocalEulersState : disabledLocalEulersState;
        }

    }
}