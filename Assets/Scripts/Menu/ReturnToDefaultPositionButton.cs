using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OscilloscopeSimulation.Menu
{
    public sealed class ReturnToDefaultPositionButton : MonoBehaviour
    {
        [SerializeField] private FreeFlyCamera.FreeFlyCameraMotion freeFlyCameraMotion;
        [SerializeField] private FreeFlyCamera.FreeFlyCameraInput freeFlyCameraInput;

        public void OnButtonClick()
        {
            freeFlyCameraMotion.ResetPositionAndRotation();
            freeFlyCameraInput.ResetInput();
        }
    }
}