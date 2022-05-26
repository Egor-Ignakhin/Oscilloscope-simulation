using UnityEngine;

namespace OscilloscopeSimulation.FreeFlyCamera
{
    [System.Serializable]
    internal sealed class CameraMotion
    {
        [SerializeField] private CameraInput cameraInput;

        [SerializeField] private Rigidbody mRigidbody;

        [SerializeField, Range(0, 10)] private float defaultSpeed = 1;
        private float speed;

        [SerializeField] private Transform cameraTransform;

        internal void Update()
        {
            speed = Input.GetKey(KeyCode.LeftShift) ? defaultSpeed * 2 : defaultSpeed;

            RotateCamera();

            MoveCameraByVelocity();
        }

        private void RotateCamera()
        {
            cameraTransform.localRotation = Quaternion.AngleAxis(cameraInput.GetRotationX(), Vector3.up);
            cameraTransform.localRotation *= Quaternion.AngleAxis(cameraInput.GetRotationY(), Vector3.left);
        }

        private void MoveCameraByVelocity()
        {
            mRigidbody.velocity = Vector3.zero;
            mRigidbody.velocity += 100 * Input.GetAxis("Vertical") * speed * Time.deltaTime * cameraTransform.forward;
            mRigidbody.velocity += 100 * Input.GetAxis("Horizontal") * speed * Time.deltaTime * cameraTransform.right;

            if (cameraInput.CanFlyUp())
            {
                mRigidbody.velocity += 100 * speed * Time.deltaTime * Vector3.up;
            }
            else if (cameraInput.CanFlyDown())
            {                
                mRigidbody.velocity += 100 * speed * Time.deltaTime * -Vector3.up;
            }
        }
    }
}