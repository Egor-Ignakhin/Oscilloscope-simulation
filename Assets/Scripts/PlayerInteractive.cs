
using UnityEngine;
namespace OscilloscopeSimulation
{
    /// <summary>
    /// Оператор взаимодействия пользовательского ввода
    /// </summary>
    internal sealed class PlayerInteractive : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        /// <summary>
        /// Координаты точки соприкосновения с физ. объектами сцены
        /// </summary>
        public Vector3 LastHitPoint { get; private set; }

        /// <summary>
        /// Метод, вызываемый каждый кадр
        /// </summary>
        private void Update()
        {
            //Бросаем луч из камеры
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, ~0, QueryTriggerInteraction.Ignore))
            {
                //Если у объект коллайдера является интерактивным
                if (hit.transform.TryGetComponent(out Interactable hitInteractable))
                {
                    //Если нажата левая кнопка мыши
                    if (Input.GetMouseButtonDown(0))
                    {
                        hitInteractable.Interact();
                    }
                }

                //Присваиваем точку соприкосновения из луча в память
                LastHitPoint = hit.point;
            }
        }
    }
}