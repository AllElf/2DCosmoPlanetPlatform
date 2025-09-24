using UnityEngine;

public class SputnikMove2 : MonoBehaviour
{
    [SerializeField] private float _speedSputnik = 1f;
    [SerializeField] private float _distance;
    [SerializeField] private int _layerStart = 0, _layerEnd = 1;
    [SerializeField] private Vector3 _startPos;
    [SerializeField] private Vector3 _endPos;

    private bool movingToEnd = true;
    private SpriteRenderer sputnikRenderer;

    private void Start()
    {
        sputnikRenderer = GetComponent<SpriteRenderer>();
        if (sputnikRenderer == null)
        {
            Debug.LogError("SpriteRenderer не найден!");
            return;
        }

        // ”станавливаем начальную позицию в зависимости от наличи€ родител€
        if (transform.parent != null)
            transform.localPosition = _startPos;
        else
            transform.position = _startPos;
    }

    private void Update()
    {
        Vector3 targetPos = movingToEnd ? _endPos : _startPos;
        bool isChild = transform.parent != null; // ѕровер€ем родител€ динамически

        // »спользуем локальную или глобальную позицию
        _distance = Vector3.Distance(isChild ? transform.localPosition : transform.position, targetPos);
        if (isChild)
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, _speedSputnik * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, targetPos, _speedSputnik * Time.deltaTime);

        // ќбновл€ем пор€док рендеринга
        sputnikRenderer.sortingOrder = movingToEnd ? _layerStart : _layerEnd;

        // ѕровер€ем достижение точки и мен€ем направление
        if (_distance < 0.01f)
        {
            movingToEnd = !movingToEnd;
            Vector3 direction = (movingToEnd ? _endPos : _startPos) - (isChild ? transform.localPosition : transform.position);

            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }
}