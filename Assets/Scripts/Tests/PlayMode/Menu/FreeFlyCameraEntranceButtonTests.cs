using NUnit.Framework;

using OscilloscopeSimulation.Menu;
using OscilloscopeSimulation.Player;

using System.Collections;

using UnityEngine;
using UnityEngine.TestTools;

namespace OscilloscopeSimulation.Tests
{
    internal sealed class FreeFlyCameraEntranceButtonTests
    {
        [UnityTest]
        public IEnumerator OnButtonClickTest()
        {
            GameObject button = new GameObject();
            var component = button.AddComponent<FreeFlyCameraEntranceButton>();

            component.OnButtonClick();

            Assert.AreEqual(PlayerInteractive.GetPlayerInteractiveModes(), PlayerInteractiveModes.FreeFlight);

            return null;
        }
    }
}