using System.Collections.Generic;

using UnityEngine;
namespace OscilloscopeSimulation
{
    internal sealed class Wire : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        private Transform connector_1;
        private Transform connector_2;
        public bool WireIsActive { get; private set; }
        private PlayerInteractive playerInteractive;
        private void Start()
        {
            playerInteractive = FindObjectOfType<PlayerInteractive>();
        }
        internal void SetConnectorPosition(Transform positionForWireConnector)
        {
            if (!connector_1)
            {
                connector_1 = positionForWireConnector;
                WireIsActive = true;
            }
            else
            {
                connector_2 = positionForWireConnector;
                WireIsActive = false;
            }
        }
        private void Reconnect()
        {
            lineRenderer.positionCount = 2;
            if (!connector_1)
            {
                lineRenderer.SetPosition(0, Vector3.zero);
                lineRenderer.SetPosition(1, Vector3.zero);
                return;
            }
            List<Vector3> v3Array = new List<Vector3>();
            if (WireIsActive)
            {
                v3Array.Add(connector_1.position);
                v3Array.Add(playerInteractive.GetLastHitPoint());
            }
            else
            {
                v3Array.Add(connector_1.position);
                v3Array.Add(connector_2.position);
            }

            lineRenderer.SetPosition(0, v3Array[0]);
            lineRenderer.SetPosition(1, v3Array[1]);
        }
        private void LateUpdate()
        {
            Reconnect();
        }
        internal void Disconnect()
        {
            connector_1 = null;
            connector_2 = null;
            WireIsActive = false;
        }

        internal void DisconnectFromPoint(Transform point)
        {
            if (connector_1.Equals(point))
            {
                connector_1 = null;
            }

            if (connector_2.Equals(point))
            {
                connector_2 = null;
            }

            if (connector_1 == null)
            {
                connector_1 = connector_2;
                connector_2 = null;
            }

            WireIsActive = true;
        }
    }
}