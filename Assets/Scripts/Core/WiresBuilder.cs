using System.Collections.Generic;

using UnityEngine;

namespace OscilloscopeSimulation
{
    internal struct WiresBuilder
    {
        internal List<Wire> Build(Transform wiresParent, WiresManager wiresManager, int numberOfWiresBeingCreated)
        {
            List<Wire> wires = new List<Wire>();
            for (int i = 0; i < numberOfWiresBeingCreated; i++)
            {
                Wire wire = Object.Instantiate(Resources.Load<Wire>("Wire"), wiresParent);
                wire.transform.localScale = Vector3.one;
                wire.Initialize(wiresManager);       
                wires.Add(wire);
                wire.name = $"Wire_{i}";
                wire.gameObject.SetActive(false);

                WireRenderer wireRenderer = wire.GetWireRenderer();
                Color randColor = Extenions<WiresManager>.
                    GenerateRandomColor(Color.yellow, Color.blue, Color.green, Color.gray, Color.red);
                wireRenderer.SetMaterialColor(randColor);
            }

            return wires;
        }
    }
}