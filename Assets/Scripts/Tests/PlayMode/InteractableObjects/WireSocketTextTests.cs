using NUnit.Framework;

using OscilloscopeSimulation.InteractableObjects;

using System.Collections;
using System.Reflection;

using TMPro;

using UnityEngine;
using UnityEngine.TestTools;

namespace OscilloscopeSimulation.Tests
{

    internal sealed class WireSocketTextTests
    {
        [UnityTest]
        public IEnumerator InitializeTest()
        {
            var wireSocketText = CreateFakeInstance();

            FieldInfo tmpFieldInfo = typeof(WireSocketText).GetField("textMeshPro",
               BindingFlags.NonPublic | BindingFlags.Instance);
            var tmProInstance = (TextMeshPro)tmpFieldInfo.GetValue(wireSocketText);

            wireSocketText.Initialize();

            Assert.IsFalse(tmProInstance.enabled);

            return null;
        }

        private WireSocketText CreateFakeInstance()
        {
            WireSocketText wireSocketText = new WireSocketText();

            #region Setup textMeshPro
            FieldInfo tmpFieldInfo = typeof(WireSocketText).GetField("textMeshPro",
                BindingFlags.NonPublic | BindingFlags.Instance);

            TextMeshPro textMeshPro = new GameObject("FakeTMPro").AddComponent<TextMeshPro>();

            tmpFieldInfo.SetValue(wireSocketText, textMeshPro);
            #endregion

            return wireSocketText;
        }

        [UnityTest]
        public IEnumerator WriteTest()
        {
            WireSocketText wireSocketText = CreateFakeInstance();

            FieldInfo tmpFieldInfo = typeof(WireSocketText).GetField("textMeshPro",
               BindingFlags.NonPublic | BindingFlags.Instance);
            var tmProInstance = (TextMeshPro)tmpFieldInfo.GetValue(wireSocketText);

            string inText = "Test string";

            wireSocketText.Write(inText);

            Assert.AreEqual(tmProInstance.text, inText);

            return null;
        }

        [UnityTest]
        public IEnumerator SetCanVisibilityTest()
        {
            var fakeInstance = CreateFakeInstance();

            fakeInstance.SetCanVisibility(true);

            FieldInfo canVisibilityFieldInfo = typeof(WireSocketText).GetField("canVisibility",
               BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsTrue((bool)canVisibilityFieldInfo.GetValue(fakeInstance));

            return null;
        }
    }
}