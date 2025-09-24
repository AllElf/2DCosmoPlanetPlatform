using UnityEngine;

public class ParallaxBackground3 : MonoBehaviour
{
    [Header("Скорости слоёв")]
    [SerializeField] float speed1 = 2f;
    [SerializeField] float speed2 = 1.5f;
    [SerializeField] float speed3 = 1f;
    [SerializeField] float speed4 = 0.75f;
    [SerializeField] float speed5 = 0.5f;

    [Header("Слои фонов")]
    [SerializeField] Transform[] backgrounds1;
    [SerializeField] Transform[] backgrounds2;
    [SerializeField] Transform[] backgrounds3;
    [SerializeField] Transform[] backgrounds4;
    [SerializeField] Transform[] backgrounds5;

    float width1, width2, width3, width4, width5;

    void Start()
    {
        width1 = GetLayerWidth(backgrounds1);
        width2 = GetLayerWidth(backgrounds2);
        width3 = GetLayerWidth(backgrounds3);
        width4 = GetLayerWidth(backgrounds4);
        width5 = GetLayerWidth(backgrounds5);
    }

    void LateUpdate()
    {
        TryScroll(backgrounds1, width1, speed1);
        TryScroll(backgrounds2, width2, speed2);
        TryScroll(backgrounds3, width3, speed3);
        TryScroll(backgrounds4, width4, speed4);
        TryScroll(backgrounds5, width5, speed5);
    }

    void TryScroll(Transform[] layer, float width, float speed)
    {
        if (layer == null || layer.Length < 2 || width <= 0f) return;
        ScrollLayer(layer, width, speed);
    }

    float GetLayerWidth(Transform[] layer)
    {
        if (layer != null && layer.Length > 1 && layer[0] != null && layer[1] != null)
            return Mathf.Abs(layer[1].position.x - layer[0].position.x);
        return 0f;
    }

    void ScrollLayer(Transform[] layer, float width, float speed)
    {
        foreach (var bg in layer)
        {
            if (bg != null)
                bg.position += Vector3.left * speed * Time.deltaTime;
        }

        for (int i = 0; i < layer.Length; i++)
        {
            var bg = layer[i];
            if (bg == null) continue;

            if (bg.position.x <= -width)
            {
                float maxX = float.MinValue;
                foreach (var b in layer)
                {
                    if (b != null && b.position.x > maxX)
                        maxX = b.position.x;
                }

                bg.position = new Vector3(maxX + width, bg.position.y, bg.position.z);
            }
        }
    }
}