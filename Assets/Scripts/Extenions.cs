using UnityEngine;

namespace OscilloscopeSimulation
{
    public sealed class Extenions<T>
    {
        public static void Swap(ref T obj1, ref T obj2)
        {
            T obj1Buffer = obj1;

            obj1 = obj2;
            obj2 = obj1Buffer;
        }

        public static Color GenerateRandomColor(params Color[] colors)
        {
            if ((colors == null) || (colors.Length == 0))
            {
                return Color.white;
            }

            int randIndex = Random.Range(0, colors.Length);

            return colors[randIndex];
        }
    }
}
