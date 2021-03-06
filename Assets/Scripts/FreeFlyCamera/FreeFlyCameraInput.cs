using UnityEngine;

namespace OscilloscopeSimulation.FreeFlyCamera
{
    internal sealed class FreeFlyCameraInput : MonoBehaviour
    {
        [SerializeField, Range(0, 10)] private float lookSensitivity = 3;

        private float rotationX = 0;
        private float rotationY = 0;

        private bool canFlyUp;
        private bool canFlyDown;

        private bool inputIsLocked;

        private void Start()
        {
            ResetInput();
        }

        internal void Update()
        {
            if (inputIsLocked)
                return;

            rotationX += Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime * 100;
            rotationY += Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime * 100;
            rotationY = Mathf.Clamp(rotationY, -90, 90);

            canFlyUp = Input.GetKey(KeyCode.E);
            canFlyDown = Input.GetKey(KeyCode.Q);
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

        internal void SetInputIsLocked(bool value)
        {
            inputIsLocked = value;
        }

        internal void ResetInput()
        {
            rotationX = transform.localEulerAngles.y;
            rotationY = -transform.localEulerAngles.x;
        }
    }
}