using System.Collections.Generic;

using UnityEngine;

namespace OscilloscopeSimulation
{
    public sealed class Extenions<T>
    {
        internal static void Swap(ref T obj1, ref T obj2)
        {
            T obj1Buffer = obj1;

            obj1 = obj2;
            obj2 = obj1Buffer;
        }

        internal static Color GenerateRandomColor(params Color[] colors)
        {            
            int randIndex = Random.Range(0, colors.Length);

            return colors[randIndex];
        }
    }
}
