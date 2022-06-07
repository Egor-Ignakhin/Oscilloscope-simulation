using NUnit.Framework;

using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

namespace OscilloscopeSimulation.Tests
{
    internal sealed class ExtensionsTest
    {
        [UnityTest]
        public System.Collections.IEnumerator SwapTest()
        {
            bool value1 = true;
            bool value2 = false;
            Extenions<bool>.Swap(ref value1, ref value2);
            Assert.IsFalse(value1);
            Assert.IsTrue(value2);

            return null;
        }

        [UnityTest]
        public System.Collections.IEnumerator GenerateRandomColorTest()
        {
            Color[] filledColorArray = { Color.blue, Color.black, Color.red };
            Color[] emptyColorArray = { };
            Color[] nullableColorArray = null;

            Color filledColor = Extenions<bool>.GenerateRandomColor(filledColorArray);
            Color emptyColor = Extenions<bool>.GenerateRandomColor(emptyColorArray);
            Color nullableColor = Extenions<bool>.GenerateRandomColor(nullableColorArray);

            Assert.True(filledColorArray.Contains(filledColor));
            Assert.IsNotNull(emptyColor);
            Assert.IsNotNull(nullableColor);

            return null;
        }
    }
}