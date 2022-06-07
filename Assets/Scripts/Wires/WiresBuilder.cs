using System.Collections.Generic;

using UnityEngine;

namespace OscilloscopeSimulation.Wires
{
    internal struct WiresBuilder
    {
        private readonly Transform wiresParent;
        private readonly WiresManager wiresManager;
        
        internal WiresBuilder (Transform wiresParent, WiresManager wiresManager)
        {            
            this.wiresParent = wiresParent;
            this.wiresManager = wiresManager;
        }

        internal List<Wire> Build(int numberOfWiresBeingBuilded)
        {
            List<Wire> wires = new List<Wire>();
            for (int i = 0; i < numberOfWiresBeingBuilded; i++)
            {
                Wire wireInstance = BuildWireInstance($"Wire_{i}");
                wires.Add(wireInstance);
            }

            return wires;
        }
        
        private Wire BuildWireInstance(string name)
        {
            Wire wire = Object.Instantiate(Resources.Load<Wire>("Wire"), wiresParent);
            wire.transform.localScale = Vector3.one;
            wire.Initialize(wiresManager);
            wire.name = name;
            wire.gameObject.SetActive(false);

            WireRenderer wireRenderer = wire.GetWireRenderer();
            Color randColor = Extenions<WiresManager>.
                GenerateRandomColor(Color.yellow, Color.blue, Color.green, Color.gray, Color.red);
            wireRenderer.SetMaterialColor(randColor);

            return wire;
        }
    }
}