using UnityEngine;

public class PrefabsAll : MonoBehaviour
{
    [SerializeField] RandomSpawner randomSpawner;
    public GameObject[] _prefab;
    public string prefabName, newPrefabName;
    [SerializeField] string[] group;
    [SerializeField] bool _search;
    private void Awake()
    {
        _search = false;
        randomSpawner = GetComponent<RandomSpawner>();
        newPrefabName = prefabName;
        group = new string[_prefab.Length];
        for (int i = 0; i < _prefab.Length; i++)
        {
            group[i] = i.ToString();
        }
        StartCoroutine(DelayedSearch());
    }

    private System.Collections.IEnumerator DelayedSearch()
    {
        yield return null; // ждём 1 кадр
        SearchPref();
    }

    private void Update()
    {
        if (newPrefabName != null && newPrefabName != prefabName)
        {
            prefabName = newPrefabName; // Обновляем только после поиска
            SearchPref();
        }
    }
    public void SearchPref()
    {
        _search = true;
        for (int i = 0; i < _prefab.Length; i++)
        {
            if (_prefab[i].name == prefabName && _search)
            {
                randomSpawner.prefab = _prefab[i];
                randomSpawner.SpawnObject(group[i]);
                _search = false;
                break;
            }
        }
    }
}
