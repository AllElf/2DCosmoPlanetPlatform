using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("Background Settings")]
    [SerializeField] RectTransform[] backgrounds;
    [SerializeField] float speed = 200f;
    [SerializeField] float buffer = 50f;

    [Header("Movement Limits")]
    [SerializeField] RectTransform leftBoundary;
    [SerializeField] RectTransform rightBoundary;

    [SerializeField] float backgroundWidth;

    void Start()
    {
        if (backgrounds.Length < 2)
        {
            Debug.LogError("Нужно минимум 2 фона для параллакса.");
            enabled = false;
            return;
        }

        backgroundWidth = Mathf.Abs(backgrounds[1].anchoredPosition.x - backgrounds[0].anchoredPosition.x);
    }

    void Update()
    {
        float direction = Input.GetAxis("Horizontal");
        if (Mathf.Approximately(direction, 0f)) return;

        MoveBackgrounds(direction);
        RepositionOutOfBounds();
    }

    private void MoveBackgrounds(float direction)
    {
        Vector2 movement = Vector2.left * direction * speed * Time.deltaTime;
        foreach (var bg in backgrounds)
            bg.anchoredPosition += movement;
    }

    private void RepositionOutOfBounds()
    {
        float leftX = leftBoundary.anchoredPosition.x + buffer;
        float rightX = rightBoundary.anchoredPosition.x - buffer;

        RectTransform leftmost = GetExtremeBackground(true);
        RectTransform rightmost = GetExtremeBackground(false);

        if (leftmost.anchoredPosition.x + backgroundWidth < leftX)
        {
            leftmost.anchoredPosition = new Vector2(
                rightmost.anchoredPosition.x + backgroundWidth,
                leftmost.anchoredPosition.y
            );
        }

        if (rightmost.anchoredPosition.x - backgroundWidth > rightX)
        {
            rightmost.anchoredPosition = new Vector2(
                leftmost.anchoredPosition.x - backgroundWidth,
                rightmost.anchoredPosition.y
            );
        }
    }

    private RectTransform GetExtremeBackground(bool getLeftmost)
    {
        RectTransform result = backgrounds[0];
        foreach (var bg in backgrounds)
        {
            if (getLeftmost && bg.anchoredPosition.x < result.anchoredPosition.x)
                result = bg;
            else if (!getLeftmost && bg.anchoredPosition.x > result.anchoredPosition.x)
                result = bg;
        }
        return result;
    }
}
