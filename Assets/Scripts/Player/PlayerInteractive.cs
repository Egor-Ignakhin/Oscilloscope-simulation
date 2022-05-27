using Obi;

using UnityEngine;

namespace OscilloscopeSimulation.Player
{
    internal sealed class PlayerInteractive : MonoBehaviour
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

        private PlayerInteractiveModes playerInteractiveMode;

        [SerializeField] private GameObject FreeFlyCameraGM;

        [SerializeField] private GameObject UI;        

        private void Start()
        {
            wiresParticleMotionOperator = new WiredParticleMotionOperator(obiSolver, obiParticlePicker,
                wiresManager, wireInteractiveCursor, this);
        }

        private void LateUpdate()
        {
            if (playerInteractiveMode == PlayerInteractiveModes.FreeFlight)
            {
                OperateFreeFlightMode();
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
        }

        private void OperateFreeFlightMode()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
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

        internal void SetPIM(PlayerInteractiveModes mode)
        {
            playerInteractiveMode = mode;

            switch (mode)
            {
                case PlayerInteractiveModes.FreeFlight:
                    SetupFreeFlightPIM();
                    break;
                default:
                    SetupMovingInsertingWiresPIM();
                    break;
            }

            UI.SetActive(playerInteractiveMode != PlayerInteractiveModes.FreeFlight);
        }

        private void SetupFreeFlightPIM()
        {
            mainCamera.enabled = false;
            FreeFlyCameraGM.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void SetupMovingInsertingWiresPIM()
        {
            mainCamera.enabled = true;
            FreeFlyCameraGM.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}