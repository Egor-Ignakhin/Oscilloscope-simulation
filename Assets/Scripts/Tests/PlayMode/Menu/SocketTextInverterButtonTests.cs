using NUnit.Framework;

using OscilloscopeSimulation.InteractableObjects;
using OscilloscopeSimulation.Menu;

using System.Collections;

using UnityEngine;
using UnityEngine.TestTools;

namespace OscilloscopeSimulation.Tests
{
    public class SocketTextInverterButtonTests
    {
        [UnityTest]
        public IEnumerator OnButtonClickTest()
        {
            GameObject button = new GameObject();
            var component = button.AddComponent<SocketTextInverterButton>();

            bool startSocketsVisible = WireSocketText.GeneralVisibility;

            component.OnButtonClick();

            Assert.AreNotEqual(WireSocketText.GeneralVisibility, startSocketsVisible);

            return null;
        }
    }
}