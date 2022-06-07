using NUnit.Framework;

using OscilloscopeSimulation.Menu;

using System.Collections;

using UnityEngine;
using UnityEngine.TestTools;

namespace OscilloscopeSimulation.Tests
{
    internal sealed class ShowHideWiresButtonTests
    {
        [UnityTest]
        public IEnumerator OnButtonClickTest()
        {
            GameObject button = new GameObject();
            var component = button.AddComponent<ShowHideWiresButton>();

            bool startAllWiresVisible = WiresManager.AllWiresVisible;

            component.OnButtonClick();

            Assert.AreNotEqual(WiresManager.AllWiresVisible, startAllWiresVisible);

            return null;
        }
    }
}