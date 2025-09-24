using UnityEngine;
using UnityEngine.UI;

public class HorizontalScrollByWheel : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float scrollSpeed = 0.1f;
    [SerializeField] private float newPos;
    private float scroll;

    void Update()
    {
        scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            newPos = scrollRect.horizontalNormalizedPosition - scroll * scrollSpeed * Time.deltaTime;
            newPos = Mathf.Clamp01(newPos);
            scrollRect.horizontalNormalizedPosition = newPos;
            Debug.Log("Scroll value: " + scroll);
        }
    }
}