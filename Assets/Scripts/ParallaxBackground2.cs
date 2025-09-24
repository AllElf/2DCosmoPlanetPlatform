using UnityEngine;

public class ParallaxBackground2 : MonoBehaviour
{
    [Header("Скорости слоёв")]
    [SerializeField] float speed1 = 2f;
    [SerializeField] float speed2 = 1.5f;
    [SerializeField] float speed3 = 1f;

    [Header("Слои фонов")]
    [SerializeField] Transform[] backgrounds1;
    [SerializeField] Transform[] backgrounds2;
    [SerializeField] Transform[] backgrounds3;

    [SerializeField] float backgroundWidth1;
    [SerializeField] float backgroundWidth2;
    [SerializeField] float backgroundWidth3;

    [SerializeField] CosmoController cosmoController;
    [SerializeField] bool _range;
    [SerializeField] float direction;
    [SerializeField] float speed = 2f;
    void Start()
    {
        if (backgrounds1 != null && backgrounds1.Length > 0)
        {
            backgroundWidth1 = Mathf.Abs(backgrounds1[1].position.x - backgrounds1[0].position.x);
        }

        if (backgrounds2 != null && backgrounds2.Length > 0)
        {
            backgroundWidth2 = Mathf.Abs(backgrounds2[1].position.x - backgrounds2[0].position.x);
        }

        if (backgrounds3 != null && backgrounds3.Length > 0)
        {
            backgroundWidth3 = Mathf.Abs(backgrounds3[1].position.x - backgrounds3[0].position.x);
        }
    }

    void Update()
    {
        _range = cosmoController.range;
        direction = cosmoController.currentHorizontalSpeed;

        if (_range && Mathf.Abs(direction) > 0.01f)
        {
            ScrollBackground(backgrounds1, backgroundWidth1, speed1, direction);
            ScrollBackground(backgrounds2, backgroundWidth2, speed2, direction);
            ScrollBackground(backgrounds3, backgroundWidth3, speed3, direction);
        }
    }

    void ScrollBackground(Transform[] backgrounds, float width, float speed, float direction)
    {
        if (backgrounds == null || backgrounds.Length == 0) return;

        foreach (var bg in backgrounds)
        {
            bg.position += Vector3.left * direction * speed * Time.deltaTime;
        }

        for (int i = 0; i < backgrounds.Length; i++)
        {
            var bg = backgrounds[i];

            if (direction > 0 && bg.position.x <= -width)
            {
                float maxX = float.MinValue;
                foreach (var b in backgrounds)
                {
                    if (b.position.x > maxX)
                        maxX = b.position.x;
                }

                bg.position = new Vector3(maxX + width, bg.position.y, bg.position.z);
            }
            else if (direction < 0 && bg.position.x >= width)
            {
                float minX = float.MaxValue;
                foreach (var b in backgrounds)
                {
                    if (b.position.x < minX)
                        minX = b.position.x;
                }

                bg.position = new Vector3(minX - width, bg.position.y, bg.position.z);
            }
        }
    }
}
