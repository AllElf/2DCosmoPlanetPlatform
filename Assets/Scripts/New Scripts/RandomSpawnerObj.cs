using UnityEngine;
using System.Collections.Generic;

public class RandomSpawnerObj : MonoBehaviour
{
    [Header("Игрок")]
    [SerializeField] Transform playerTransform;

    [Header("Префаб для спавна")]
    [SerializeField] GameObject prefabToSpawn;
    [SerializeField] GameObject prefabStar;
    [SerializeField] GameObject prefabHeart;

    [Header("Шанс появления сердца (0–1)")]
    [Range(0f, 1f)]
    [SerializeField] float heartSpawnChance = 0.1f;

    [Header("Скорость для префаба")]
    [SerializeField] float speed = 1f;

    [Header("Спавнить при старте")]
    [SerializeField] bool spawnOnStart = true;

    [Header("Количество объектов на сцене")]
    [SerializeField] int spawnCount = 1;
    [SerializeField] int spawnStarCount = 1;

    [Header("Спавнить в точках поинтов")]
    [SerializeField] Transform[] pointersSpawn;
    [SerializeField] Transform[] pointersStarSpawn;
    [SerializeField] bool pointersActive;

    [Header("Виртуальная зона справа")]
    [SerializeField] int virtualScreensRight = 1;

    [Header("Максимум попыток для поиска свободной позиции")]
    [SerializeField] int maxAttempts = 30;

    static List<Rect> occupiedZones = new List<Rect>();
    bool hasStarted = false;

    void Start()
    {
        hasStarted = true;
        if (spawnOnStart)
        {
            MaintainSpawnCount();
        }
    }

    void Update()
    {
        if (!hasStarted) return;
        MaintainSpawnCount();
    }

    void MaintainSpawnCount()
    {
        int missingMain = spawnCount - GameObject.FindGameObjectsWithTag("Prefab").Length;
        int missingStar = spawnStarCount - (GameObject.FindGameObjectsWithTag("Star").Length + GameObject.FindGameObjectsWithTag("Heart").Length);

        if (pointersActive)
        {
            if (missingMain > 0) SpawnAtPointers(prefabToSpawn, pointersSpawn, missingMain);
            if (missingStar > 0) SpawnStarsAtPointers(missingStar);
        }
        else
        {
            for (int i = 0; i < missingMain; i++) SpawnSafe(prefabToSpawn);
            for (int i = 0; i < missingStar; i++) SpawnSafeRandomStarOrHeart();
        }
    }

    void SpawnSafe(GameObject prefab)
    {
        if (prefab == null) return;

        Vector2 prefabSize = GetPrefabSize(prefab);
        Vector2 spawnPos = FindFreePosition(prefabSize, virtualScreensRight);

        GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);

        var move = obj.GetComponent<PrefabMove>();
        if (move != null) move.speedMovePrefab = speed;

        var destroyer = obj.GetComponent<DestroyIfOutOfView>();
        if (destroyer != null) destroyer.enabled = true;

        Rect zone = new Rect(spawnPos - prefabSize / 2f, prefabSize);
        occupiedZones.Add(zone);
    }

    void SpawnSafeRandomStarOrHeart()
    {
        GameObject selected = null;

        if (prefabStar != null && prefabHeart != null)
        {
            selected = Random.value < heartSpawnChance ? prefabHeart : prefabStar;
        }
        else if (prefabStar != null)
        {
            selected = prefabStar;
        }
        else if (prefabHeart != null)
        {
            selected = prefabHeart;
        }

        if (selected == null) return;

        SpawnSafe(selected);
    }

    void SpawnAtPointers(GameObject prefab, Transform[] points, int countToSpawn)
    {
        if (prefab == null || points == null || points.Length == 0) return;

        countToSpawn = Mathf.Min(countToSpawn, points.Length);
        List<int> usedIndices = new List<int>();

        for (int i = 0; i < countToSpawn; i++)
        {
            int index;
            do
            {
                index = Random.Range(0, points.Length);
            } while (usedIndices.Contains(index) && usedIndices.Count < points.Length);

            usedIndices.Add(index);

            Vector2 spawnPos = points[index].position;
            GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);

            var move = obj.GetComponent<PrefabMove>();
            if (move != null) move.speedMovePrefab = speed;

            var destroyer = obj.GetComponent<DestroyIfOutOfView>();
            if (destroyer != null) destroyer.enabled = true;
        }
    }

    void SpawnStarsAtPointers(int countToSpawn)
    {
        if (spawnStarCount <= 0 || pointersStarSpawn == null || pointersStarSpawn.Length == 0)
            return;

        bool hasStar = prefabStar != null;
        bool hasHeart = prefabHeart != null;

        if (!hasStar && !hasHeart)
            return;

        countToSpawn = Mathf.Min(countToSpawn, pointersStarSpawn.Length);
        List<int> usedIndices = new List<int>();

        for (int i = 0; i < countToSpawn; i++)
        {
            int index;
            do
            {
                index = Random.Range(0, pointersStarSpawn.Length);
            } while (usedIndices.Contains(index) && usedIndices.Count < pointersStarSpawn.Length);

            usedIndices.Add(index);

            GameObject selected = null;

            if (hasStar && hasHeart)
            {
                selected = Random.value < heartSpawnChance ? prefabHeart : prefabStar;
            }
            else if (hasStar)
            {
                selected = prefabStar;
            }
            else if (hasHeart)
            {
                selected = prefabHeart;
            }

            if (selected == null) continue;

            Vector2 spawnPos = pointersStarSpawn[index].position;
            GameObject obj = Instantiate(selected, spawnPos, Quaternion.identity);

            var move = obj.GetComponent<PrefabMove>();
            if (move != null)
                move.speedMovePrefab = speed;

            var destroyer = obj.GetComponent<DestroyIfOutOfView>();
            if (destroyer != null)
                destroyer.enabled = true;
        }
    }
    Vector2 FindFreePosition(Vector2 prefabSize, int screensRight)
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector2 candidate = GetVirtualSpawnPosition(prefabSize, screensRight);
            Rect candidateRect = new Rect(candidate - prefabSize / 2f, prefabSize);

            bool overlaps = false;
            foreach (Rect zone in occupiedZones)
            {
                if (zone.Overlaps(candidateRect))
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps && !OverlapsSafePassage(candidateRect))
                return candidate;
        }

        Debug.LogWarning("Не удалось найти свободную позицию без наложения.");
        return GetVirtualSpawnPosition(prefabSize, screensRight);
    }

    bool OverlapsSafePassage(Rect candidateRect)
    {
        if (playerTransform == null) return false;

        Vector2 playerSize = GetPlayerSize();
        Vector2 playerPos = playerTransform.position;

        Rect passageRect = new Rect(
            new Vector2(candidateRect.xMin, playerPos.y - playerSize.y / 2f),
            new Vector2(candidateRect.width, playerSize.y)
        );

        return candidateRect.Overlaps(passageRect);
    }

    Vector2 GetPlayerSize()
    {
        if (playerTransform == null) return Vector2.zero;

        var sr = playerTransform.GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
        {
            Vector2 localSize = sr.sprite.bounds.size;
            Vector3 globalScale = playerTransform.lossyScale;
            return new Vector2(localSize.x * globalScale.x, localSize.y * globalScale.y);
        }

        Debug.LogWarning("SpriteRenderer не найден или спрайт отсутствует на игроке.");
        return Vector2.one;
    }

    Vector2 GetVirtualSpawnPosition(Vector2 prefabSize, int screensRight)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Main Camera не найдена!");
            return Vector2.zero;
        }

        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;
        Vector3 camPos = cam.transform.position;

        float offsetX = camWidth * screensRight;

        float minX = camPos.x - camWidth / 2f + prefabSize.x / 2f + offsetX;
        float maxX = camPos.x + camWidth / 2f - prefabSize.x / 2f + offsetX;
        float minY = camPos.y - camHeight / 2f + prefabSize.y / 2f;
        float maxY = camPos.y + camHeight / 2f - prefabSize.y / 2f;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }

    Vector2 GetPrefabSize(GameObject prefab)
    {
        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
        {
            Vector2 localSize = sr.sprite.bounds.size;
            Vector3 globalScale = prefab.transform.lossyScale;
            return new Vector2(localSize.x * globalScale.x, localSize.y * globalScale.y);
        }

        Debug.LogWarning("SpriteRenderer не найден или спрайт отсутствует. Используется размер по умолчанию.");
        return Vector2.one;
    }
}