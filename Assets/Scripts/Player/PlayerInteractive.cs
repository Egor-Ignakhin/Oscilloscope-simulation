using Obi;

using OscilloscopeSimulation.Wires;

using System;
using System.Runtime.InteropServices;

using UnityEngine;

namespace OscilloscopeSimulation.Player
{
    public sealed class PlayerInteractive : MonoBehaviour
    {
        private static Vector3 mousePositionInBehindFrame;

        [SerializeField] private PlayerRaycast playerRaycast;

        [Space(15)]

        [SerializeField] private Camera mainCamera;

        [SerializeField] private Texture2D wireInteractiveCursor;

        [SerializeField] private ObiSolver obiSolver;
        [SerializeField] private ObiParticlePicker obiParticlePicker;

        [SerializeField] private WiresManager wiresManager;

        private WiredParticleMotionOperator wiresParticleMotionOperator;

        private static PlayerInteractiveModes playerInteractiveMode;
        private static event EventHandler PIMChanged;

        [SerializeField] private GameObject UI;

        [Space(15)]

        [SerializeField] private FreeFlyCamera.FreeFlyCameraInput mainCameraFFCInput;
        [SerializeField] private FreeFlyCamera.FreeFlyCameraMotion mainCameraFFCMotion;

        private void Start()
        {
            wiresParticleMotionOperator = new WiredParticleMotionOperator(obiSolver, obiParticlePicker,
                wiresManager, wireInteractiveCursor, this);

            PIMChanged += OnPIMChanged;
            
            SetPIM(PlayerInteractiveModes.FreeFlight);
        }

        private void LateUpdate()
        {
            if (playerInteractiveMode == PlayerInteractiveModes.FreeFlight)
            {
                OperateFreeFlight();
                return;
            }

            playerInteractiveMode = CalculatePIM();

            if (playerInteractiveMode == PlayerInteractiveModes.InsertingWires)
            {
                playerRaycast.Update();
            }

            wiresParticleMotionOperator.Update();

            mousePositionInBehindFrame = Input.mousePosition;

            if (Input.GetMouseButtonDown(1))
            {
                wiresParticleMotionOperator.DeleteWireIfPossible();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                SetPIM(PlayerInteractiveModes.FreeFlight);
            }
        }

        private void OperateFreeFlight()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                SetPIM(PlayerInteractiveModes.InsertingWires);
            }
        }

        private PlayerInteractiveModes CalculatePIM()
        {
            return Input.GetKey(KeyCode.LeftControl) ? PlayerInteractiveModes.MovingWires : PlayerInteractiveModes.InsertingWires;
        }

        internal static Vector3 GetMousePositionInBehindFrame()
        {
            return mousePositionInBehindFrame;
        }

        internal PlayerInteractiveModes GetPlayerInteractiveMode()
        {
            return playerInteractiveMode;
        }

        internal static void SetPIM(PlayerInteractiveModes mode)
        {
            playerInteractiveMode = mode;
            PIMChanged?.Invoke(null, null);
        }

        private void OnPIMChanged(object sender, EventArgs eventArgs)
        {
            switch (playerInteractiveMode)
            {
                case PlayerInteractiveModes.FreeFlight:
                    SetupFreeFlightPIM();
                    break;

                default:
                    SetupMovingInsertingWiresPIM();
                    break;
            }

            mainCameraFFCInput.SetInputIsLocked(playerInteractiveMode != PlayerInteractiveModes.FreeFlight);
            mainCameraFFCMotion.SetInputIsLocked(playerInteractiveMode != PlayerInteractiveModes.FreeFlight);
            UI.SetActive(playerInteractiveMode != PlayerInteractiveModes.FreeFlight);
        }

        private void SetupFreeFlightPIM()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            mainCameraFFCMotion.enabled = true;
            mainCameraFFCInput.enabled = true;
        }

        private void SetupMovingInsertingWiresPIM()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            mainCameraFFCMotion.enabled = false;
            mainCameraFFCInput.enabled = false;

            mainCameraFFCMotion.StopMotion();
            mainCameraFFCInput.ResetInput();
        }

        public static PlayerInteractiveModes GetPlayerInteractiveModes()
        {
            return playerInteractiveMode;
        }

        private void OnDestroy()
        {
            PIMChanged -= OnPIMChanged;
        }
    }
}