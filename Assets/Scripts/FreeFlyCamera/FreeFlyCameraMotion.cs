using UnityEngine;

namespace OscilloscopeSimulation.FreeFlyCamera
{
    internal sealed class FreeFlyCameraMotion : MonoBehaviour
    {
        [SerializeField] private FreeFlyCameraInput cameraInput;

        [SerializeField] private Rigidbody mRigidbody;

        [SerializeField, Range(0, 10)] private float speed = 1;

        [SerializeField] private Transform cameraTransform;

        private Quaternion startLocalRotation;
        private Vector3 startLocalPosition;

        private bool inputIsLocked;

        private void Awake()
        {
            startLocalRotation = transform.localRotation;
            startLocalPosition = transform.localPosition;

            ResetPositionAndRotation();
        }

        private void Update()
        {
            if (inputIsLocked)
            {
                StopMotion();
                return;
            }

            RotateCamera();

            MoveCamera();
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

        internal void ResetPositionAndRotation()
        {
            transform.localPosition = startLocalPosition;
            transform.localRotation = startLocalRotation;
            StopMotion();
        }

        internal void StopMotion()
        {
            mRigidbody.velocity = Vector3.zero;
            mRigidbody.angularVelocity = Vector3.zero;

        }

        internal void SetInputIsLocked(bool value)
        {
            inputIsLocked = value;
        }
    }
}