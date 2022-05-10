
using OscilloscopeSimulation.InteractableObjects;

using UnityEngine;
namespace OscilloscopeSimulation
{
    /// <summary>
    /// Провод, соединяющий сокеты стенда
    /// </summary>
    internal sealed class Wire : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRendererOfWire;

        private WireSocketInteractable socket_1;
        private WireSocketInteractable socket_2;

        private PlayerInteractive playerInteractive;
        private WiresManager wiresManager;
        private bool available = true;

        internal void Initialize(PlayerInteractive playerInteractive, WiresManager wiresManager)
        {
            //Устанавливаем необходимое кол-во вершин провода
            lineRendererOfWire.positionCount = 2;

            this.playerInteractive = playerInteractive;
            this.wiresManager = wiresManager;
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
        }

        private void SetupWireVertexPositions()
        {
            if (!socket_1)
            {
                //Перемещаем вершины провода в начало координат
                lineRendererOfWire.SetPositions
                    (new Vector3[] { Vector3.zero, Vector3.zero });

                return;
            }
            lineRendererOfWire.SetPosition(0, socket_1.GetWireConnectorSetupPosition());
            lineRendererOfWire.SetPosition(1, wiresManager.ActiveWire == this ?
                playerInteractive.LastRaycastPointPosition :
                socket_2.GetWireConnectorSetupPosition());

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

        internal void SetVisible(bool visibility)
        {
            lineRendererOfWire.enabled = visibility;
        }

        internal void SetAvailability(bool availability)
        {
            available = availability;
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
    }
}