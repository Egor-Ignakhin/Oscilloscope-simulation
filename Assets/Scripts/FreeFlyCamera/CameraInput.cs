
using OscilloscopeSimulation.Player;

using UnityEngine;

namespace OscilloscopeSimulation.FreeFlyCamera
{
    internal sealed class CameraInput : MonoBehaviour
    {
        [SerializeField, Range(0, 10)] private float lookSensitivity = 3;

        private float rotationX = 0;
        private float rotationY = 0;

        [SerializeField] private CameraMotion cameraMotion;

        private bool canFlyUp;
        private bool canFlyDown;

        private void Update()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            rotationX += Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime * 100;
            rotationY += Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime * 100;
            rotationY = Mathf.Clamp(rotationY, -90, 90);

            canFlyUp = Input.GetKey(KeyCode.E);
            canFlyDown = Input.GetKey(KeyCode.Q);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CameraModesOperator.SetPlayerMode(CameraModesOperator.CameraModes.TechInteraction);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return;
            }

            cameraMotion.Update();
        }

        internal float GetRotationY()
        {
            return rotationY;
        }

        internal float GetRotationX()
        {
            return rotationX;
        }

        internal bool CanFlyUp()
        {
            return canFlyUp;
        }
        internal bool CanFlyDown()
        {
            return canFlyDown;
        }
    }
}