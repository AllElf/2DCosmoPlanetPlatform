using UnityEngine;

public class DestroyIfOutOfView : MonoBehaviour
{
    private Camera cam;
    private Renderer rend;
    private bool hasEnteredView = false;

    void Start()
    {
        cam = Camera.main;
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (cam == null || rend == null) return;

        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);

        // Если объект впервые стал видимым — активируем отслеживание
        if (!hasEnteredView && rend.isVisible)
        {
            hasEnteredView = true;
        }

        // После входа в кадр — начинаем уничтожать при выходе
        if (hasEnteredView)
        {
            if (viewPos.x < -0.1f || viewPos.x > 1.1f || viewPos.y < -0.1f || viewPos.y > 1.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}