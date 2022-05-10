using UnityEngine;

namespace OscilloscopeSimulation
{
    /// <summary>
    /// Базовый класс интерактивных объектов.
    /// </summary>
    internal abstract class Interactable : MonoBehaviour
    {
        /// <summary>
        /// Базовый метод взаимодействия.
        /// Вызывается из <see cref="PlayerInteractive"/>
        /// </summary>
        internal abstract void Interact();
    }
}