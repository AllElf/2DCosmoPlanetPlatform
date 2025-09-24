using UnityEngine;
using System.Collections.Generic;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] public GameObject prefab;
    [SerializeField] string groupName;
    [Header("Количество групп:")]
    [SerializeField] private int groupCount = 10;

    [System.Serializable]
    public class SpawnGroup
    {
        public Transform[] spawnPoints;
    }

    [Header("Группы спавна (заполняй только точки):")]
    [SerializeField] private List<SpawnGroup> groups = new List<SpawnGroup>();

    private Dictionary<string, Transform[]> spawnGroups = new Dictionary<string, Transform[]>();

    private void OnValidate()
    {
        // Обновляем список групп в редакторе при изменении groupCount
        if (groups.Count != groupCount)
        {
            while (groups.Count < groupCount)
                groups.Add(new SpawnGroup());

            while (groups.Count > groupCount)
                groups.RemoveAt(groups.Count - 1);
        }
    }

    private void Start()
    {
        spawnGroups = new Dictionary<string, Transform[]>();

        for (int i = 0; i < groups.Count; i++)
        {
            string groupName = i .ToString(); // имена: "1", "2", ...
            spawnGroups[groupName] = groups[i].spawnPoints;
        }
    }


    public void SpawnObject(string groupName)
    {
        if (!spawnGroups.ContainsKey(groupName))
        {
            Debug.LogWarning($"Группа с именем '{groupName}' не найдена!");
            return;
        }

        Transform[] spawnPoints = spawnGroups[groupName];

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning($"Нет точек спавна в группе '{groupName}'!");
            return;
        }

        if (prefab == null)
        {
            Debug.LogWarning("Prefab не установлен!");
            return;
        }

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform chosenSpawnPoint = spawnPoints[randomIndex];

        GameObject spawnedObject = Instantiate(prefab, chosenSpawnPoint.position, Quaternion.identity); 
        //spawnedObject.transform.localRotation = Quaternion.identity;
        //spawnedObject.transform.localScale = Vector3.one;
        spawnedObject.transform.SetParent(chosenSpawnPoint);
    }
}
