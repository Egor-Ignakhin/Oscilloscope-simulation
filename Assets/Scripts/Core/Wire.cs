using OscilloscopeSimulation.InteractableObjects;
using OscilloscopeSimulation.Player;

using UnityEngine;

namespace OscilloscopeSimulation
{

    /// <summary>
    /// Провод, соединяющий сокеты стенда 3D
    /// </summary>
    internal sealed class Wire : MonoBehaviour
    {
        [SerializeField] private WireRenderer wireRenderer;
        [SerializeField] private WireRope wireRope;
        [SerializeField] private Transform startWirePoint;
        [SerializeField] private Transform endWirePoint;
        private WireSocketInteractable socketStart;
        private WireSocketInteractable socketEnd;

        private WiresManager wiresManager;

        private bool isAvailableToUse = true;

        internal void Initialize(WiresManager wiresManager)
        {
            this.wiresManager = wiresManager;

            wireRope.Initialize();
        }

        private void LateUpdate()
        {
            SetupWireVertexPositions();
        }

        internal void InsertWire(WireSocketInteractable socket)
        {
            if (socketStart)
            {
                socketEnd = socket;
            }
            else
            {
                socketStart = socket;
            }

            gameObject.SetActive(true);
        }

        private void SetupWireVertexPositions()
        {
            if (!socketStart)
            {
                return;
            }

            startWirePoint.position = socketStart.GetWireConnectorSetupPosition();
            endWirePoint.position = wiresManager.EqualsWithActiveWire(this) ?
               PlayerInteractive.GetLastRaycastPointPosition() :
             socketEnd.GetWireConnectorSetupPosition();
        }

        internal void DisconnectWireFromSocket(WireSocketInteractable socket)
        {
            if (socketStart.Equals(socket))
            {
                socketStart = null;
            }
            else
            {
                socketEnd = null;
            }

            if ((socketStart == null) && (socketEnd == null))
            {
                wiresManager.ReleaseWire(this);

                return;
            }

            if (socketStart == null)
            {
                socketStart = socketEnd;
                socketEnd = null;
            }

            wiresManager.SetActiveWire(this);
        }

        internal void DeleteWire()
        {
            if (socketEnd != null)
            {
                socketEnd.DisconnectWire();
                socketEnd = null;
            }
            if (socketStart != null)
            {
                socketStart.DisconnectWire();
                socketStart = null;
            }

            wiresManager.ReleaseWire(this);
        }

        internal void SwapSocketReferences()
        {
            Extenions<WireSocketInteractable>.Swap(ref socketStart, ref socketEnd);
        }

        internal void SetUsageAvailability(bool availability)
        {
            isAvailableToUse = availability;

            gameObject.SetActive(false);
        }

        internal bool IsAvailableToUse()
        {
            return isAvailableToUse;
        }

        internal WireSocketInteractable GetSocketStart()
        {
            return socketStart;
        }

        internal WireSocketInteractable GetSocketEnd()
        {
            return socketEnd;
        }

        internal bool IsFullyConnected()
        {
            return socketStart && socketEnd;
        }

        internal WireRenderer GetWireRenderer()
        {
            return wireRenderer;
        }

        internal WireRope GetWireRope()
        {
            return wireRope;
        }
    }
}