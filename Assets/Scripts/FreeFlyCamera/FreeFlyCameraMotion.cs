using UnityEngine;

namespace OscilloscopeSimulation.FreeFlyCamera
{
    internal sealed class FreeFlyCameraMotion : MonoBehaviour
    {
        [SerializeField] private FreeFlyCameraInput cameraInput;

        [SerializeField] private Rigidbody mRigidbody;

        [SerializeField, Range(0, 10)] private float defaultSpeed = 1;
        private float speed;

        [SerializeField] private Transform cameraTransform;

        internal void Update()
        {
            speed = CalculateSpeed();

            RotateCamera();

            MoveCamera();
        }

        private float CalculateSpeed()
        {
            return Input.GetKey(KeyCode.LeftShift) ? defaultSpeed * 2 : defaultSpeed;
        }

        private void RotateCamera()
        {
            cameraTransform.localRotation = Quaternion.AngleAxis(cameraInput.GetRotationX(), Vector3.up);
            cameraTransform.localRotation *= Quaternion.AngleAxis(cameraInput.GetRotationY(), Vector3.left);
        }

        private void MoveCamera()
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