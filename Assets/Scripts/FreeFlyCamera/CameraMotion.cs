using UnityEngine;

namespace OscilloscopeSimulation.FreeFlyCamera
{
    [System.Serializable]
    internal sealed class CameraMotion
    {
        [SerializeField] private CameraInput cameraInput;

        [SerializeField] private Rigidbody mRigidbody;

        [SerializeField, Range(1, 10)] private float defaultSpeed = 3;
        private float speed;

        [SerializeField] private Transform transform;

        internal void Update()
        {
            speed = Input.GetKey(KeyCode.LeftShift) ? defaultSpeed * 2 : defaultSpeed;

            RotateCamera();

            MoveCameraByVelocity();
        }

        private void RotateCamera()
        {
            transform.localRotation = Quaternion.AngleAxis(cameraInput.GetRotationX(), Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(cameraInput.GetRotationY(), Vector3.left);
        }

        private void MoveCameraByVelocity()
        {
            mRigidbody.velocity = Vector3.zero;
            mRigidbody.velocity += 1000 * Input.GetAxis("Vertical") * speed * Time.deltaTime * transform.forward;
            mRigidbody.velocity += 1000 * Input.GetAxis("Horizontal") * speed * Time.deltaTime * transform.right;

            if (cameraInput.CanFlyUp())
            {
                mRigidbody.AddForce(transform.up * speed * Time.deltaTime * 1000 * 1.5f);
            }
            else if (cameraInput.CanFlyDown())
            {
                mRigidbody.AddForce(-transform.up * speed * Time.deltaTime * 1000 * 1.5f);
            }
        }
    }
}