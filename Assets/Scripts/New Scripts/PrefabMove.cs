using UnityEngine;

public class PrefabMove : MonoBehaviour
{
    [Header("Горизонтальное движение")]
    public float speedMovePrefab = 1f;

    [Header("Вертикальное колебание")]
    public bool useRandomVerticalTiming = false;
    public float verticalSpeed = 1f;
    public float verticalRange = 0.5f;

    [Header("Рандомная задержка запуска колебания")]
    public float minDelay = 0.5f;
    public float maxDelay = 2f;

    private float initialY;
    private float verticalOffset;
    private bool verticalActive = false;
    private float delayTimer = 0f;
    private float targetDelay = 0f;

    void Start()
    {
        initialY = transform.position.y;

        if (useRandomVerticalTiming)
        {
            targetDelay = Random.Range(minDelay, maxDelay);
            delayTimer = 0f;
            verticalActive = false;
        }
        else
        {
            verticalActive = true;
        }
    }

    void Update()
    {
        // Горизонтальное движение
        float newX = transform.position.x - speedMovePrefab * Time.deltaTime;

        // Вертикальное движение (если активировано)
        float newY = transform.position.y;

        if (useRandomVerticalTiming && !verticalActive)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= targetDelay)
                verticalActive = true;
        }

        if (verticalActive)
        {
            verticalOffset = Mathf.Sin(Time.time * verticalSpeed) * verticalRange;
            newY = initialY + verticalOffset;
        }

        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}