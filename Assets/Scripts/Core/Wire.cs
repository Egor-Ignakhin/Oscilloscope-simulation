using Obi;

using OscilloscopeSimulation.InteractableObjects;

using System;

using UnityEngine;

namespace OscilloscopeSimulation
{

    /// <summary>
    /// Провод, соединяющий сокеты стенда 3D
    /// </summary>
    internal sealed class Wire : MonoBehaviour
    {
        [SerializeField] private WireRenderer wireRenderer;
        [SerializeField] private ObiRope obiRope;
        [SerializeField] private WireRope wireRope;
        [SerializeField] private Transform startWirePoint;
        [SerializeField] private Transform endWirePoint;
        private WireSocketInteractable socket_1;
        private WireSocketInteractable socket_2;

        private WiresManager wiresManager;

        private bool available = true;

        internal void Initialize(WiresManager wiresManager)
        {
            this.wiresManager = wiresManager;

            obiRope.AddToSolver();
        }

        private void LateUpdate()
        {
            SetupWireVertexPositions();
        }

        internal void InsertWire(WireSocketInteractable socket)
        {
            if (socket_1)
            {
                socket_2 = socket;
            }
            else
            {
                socket_1 = socket;
            }

            gameObject.SetActive(true);
        }

        private void SetupWireVertexPositions()
        {
            if (!socket_1)
            {
                return;
            }

            startWirePoint.position = socket_1.GetWireConnectorSetupPosition();
            endWirePoint.position = wiresManager.EqualsWithActiveWire(this) ?
               PlayerInteractive.GetLastRaycastPointPosition() :
             socket_2.GetWireConnectorSetupPosition();
        }

        internal void DisconnectWire(WireSocketInteractable socket)
        {
            if (socket_1.Equals(socket))
            {
                socket_1 = null;
            }
            else
            {
                socket_2 = null;
            }

            if ((socket_1 == null) && (socket_2 == null))
            {
                wiresManager.ReleaseWire(this);

                return;
            }

            if (socket_1 == null)
            {
                socket_1 = socket_2;
                socket_2 = null;
            }

            wiresManager.SetActiveWire(this);
        }

        internal void SwapSocketReferences()
        {
            Extenions<WireSocketInteractable>.Swap(ref socket_1, ref socket_2);
        }

        internal void SetAvailability(bool availability)
        {
            available = availability;

            gameObject.SetActive(false);
        }

        internal bool IsAvailable()
        {
            return available;
        }

        internal WireSocketInteractable GetSocket_1()
        {
            return socket_1;
        }

        internal WireSocketInteractable GetSocket_2()
        {
            return socket_2;
        }

        internal bool IsFullyConnected()
        {
            return socket_1 && socket_2;
        }

        internal WireRenderer GetWireRenderer()
        {
            return wireRenderer;
        }

        internal WireRope GetWireRope()
        {
            return wireRope;
        }

        internal ObiRope GetObiRope()
        {
            return obiRope;
        }
    }
}