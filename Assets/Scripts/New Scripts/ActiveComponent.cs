using UnityEngine;

public class ActiveComponent : MonoBehaviour
{
    private Camera cam;
    private Renderer rend;
    private DestroyIfOutOfView targetComponent;
    private bool activated = false;

    void Start()
    {
        cam = Camera.main;
        rend = GetComponent<Renderer>();
        targetComponent = GetComponent<DestroyIfOutOfView>();

        if (targetComponent != null)
        {
            targetComponent.enabled = false; // отключаем до входа в кадр
        }
    }

    void Update()
    {
        if (activated || cam == null || rend == null || targetComponent == null) return;

        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);

        // Проверяем, находится ли объект в пределах видимой области камеры
        bool isInView =
            viewPos.z > 0f && // перед камерой
            viewPos.x >= 0f && viewPos.x <= 1f &&
            viewPos.y >= 0f && viewPos.y <= 1f;

        if (isInView)
        {
            targetComponent.enabled = true;
            activated = true;
        }
    }
}