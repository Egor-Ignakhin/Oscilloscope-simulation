using UnityEngine;

namespace OscilloscopeSimulation
{
    /// <summary>
    /// ������� ����� ������������� ��������.
    /// </summary>
    internal abstract class Interactable : MonoBehaviour
    {
        /// <summary>
        /// ������� ����� ��������������.
        /// ���������� �� <see cref="PlayerInteractive"/>
        /// </summary>
        internal abstract void Interact();
    }
}