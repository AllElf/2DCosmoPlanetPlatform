using UnityEngine;

public class SputnikMove : MonoBehaviour
{
    [SerializeField] private float _speedSputnik = 1f;
    [SerializeField] private float _distance;
    [SerializeField] private int _layerStart = 0, _layerEnd = 1;
    [SerializeField] private Vector3 _startPos;
    [SerializeField] private Vector3 _endPos;

    private bool movingToEnd = true;
    private SpriteRenderer sputnikRenderer;
    private Transform sputnikTransform;
    private bool isChild; // Флаг, проверяющий родителя

    private void Start()
    {
        // Проверяем, есть ли родитель у объекта
        isChild = transform.parent != null;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.name == "Sputnik2")
            {
                sputnikTransform = child;
                sputnikRenderer = child.GetComponent<SpriteRenderer>();
                break;
            }
        }

        if (sputnikTransform == null || sputnikRenderer == null)
        {
            Debug.LogError("Sputnik2 не найден!");
            return;
        }

        // Устанавливаем начальную позицию в зависимости от типа объекта
        if (isChild)
            transform.localPosition = _startPos;
        else
            transform.position = _startPos;
    }

    private void Update()
    {
        Vector3 targetPos = movingToEnd ? _endPos : _startPos;

        // Используем локальную позицию, если объект вложенный, иначе глобальную
        _distance = isChild ? Vector3.Distance(transform.localPosition, targetPos) : Vector3.Distance(transform.position, targetPos);
        if (isChild)
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, _speedSputnik * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, targetPos, _speedSputnik * Time.deltaTime);

        if (sputnikRenderer != null)
        {
            sputnikRenderer.sortingOrder = movingToEnd ? _layerStart : _layerEnd;
        }

        if (_distance < 0.01f)
        {
            movingToEnd = !movingToEnd;
            Vector3 direction = (movingToEnd ? _endPos : _startPos) - (isChild ? transform.localPosition : transform.position);

            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.localRotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }
}