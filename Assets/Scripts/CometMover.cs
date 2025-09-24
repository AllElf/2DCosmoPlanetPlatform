using UnityEngine;

public class CometMover : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    private Transform[] _targetPoints;
    private Transform _currentTarget;

    private void Awake()
    {
        string randomName = Random.value > 0.5f ? "Comet1" : "Comet2";
        gameObject.name = randomName;
    }

    private void Start()
    {
        // Выбираем нужный тег по имени объекта
        string tagToFind = "";
        if (gameObject.name == "Comet1")
        {
            tagToFind = "MoveCometPoints";
        }
        else if (gameObject.name == "Comet2")
        {
            tagToFind = "MoveCometPoints2";
        }
        else
        {
            Debug.LogError($"Неизвестное имя объекта \"{gameObject.name}\". Установите имя Comet1 или Comet2.");
            enabled = false;
            return;
        }

        // Ищем объекты с нужным тегом
        GameObject[] foundPoints = GameObject.FindGameObjectsWithTag(tagToFind);
        if (foundPoints == null || foundPoints.Length < 1)
        {
            Debug.LogError($"Не найдено ни одной цели с тегом \"{tagToFind}\".");
            enabled = false;
            return;
        }

        // Преобразуем в массив Transform
        _targetPoints = new Transform[foundPoints.Length];
        for (int i = 0; i < foundPoints.Length; i++)
        {
            _targetPoints[i] = foundPoints[i].transform;
        }

        ChooseRandomTarget();
    }

    private void Update()
    {
        if (_currentTarget == null || _targetPoints.Length == 0) return;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = _currentTarget.position;

        float distance = Vector3.Distance(currentPos, targetPos);

        // Двигаем объект
        transform.position = Vector3.MoveTowards(currentPos, targetPos, _speed * Time.deltaTime);

        // Поворачиваем в сторону цели
        Vector3 direction = targetPos - currentPos;
        if (direction.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Если дошли — выбрать новую цель
        if (distance < 0.05f)
        {
            ChooseRandomTarget();
        }
    }

    private void ChooseRandomTarget()
    {
        if (_targetPoints.Length == 0) return;

        if (_targetPoints.Length == 1)
        {
            _currentTarget = _targetPoints[0];
            return;
        }

        Transform newTarget;
        do
        {
            newTarget = _targetPoints[Random.Range(0, _targetPoints.Length)];
        }
        while (newTarget == _currentTarget);

        _currentTarget = newTarget;
    }
}
