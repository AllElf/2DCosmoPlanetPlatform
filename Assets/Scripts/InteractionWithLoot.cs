using System.Collections;
using System.Linq;
using UnityEngine;

public class InteractionWithLoot : MonoBehaviour
{
    [SerializeField] Hints _hints;
    [SerializeField] PrefabsAll _prefabsAll;
    [SerializeField] string _namePref = "MountainPref";
    [SerializeField] int _count = 0;
    [SerializeField] float _secondsToDestroy;
    [SerializeField] string _nameHints = "ScriptManager";
    [SerializeField] bool isTrigger = false;

    private void Start()
    {
        _hints = GameObject.FindGameObjectWithTag(_nameHints).GetComponent<Hints>();
        _prefabsAll = GameObject.FindGameObjectWithTag(_nameHints).GetComponent<PrefabsAll>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_count == 0 && isTrigger == false)
        {
            _hints._request = true;
            isTrigger = true;
            Destroy(gameObject);
        }
        else if (_count == 1 && isTrigger == false)
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            GameObject targetObject = allObjects.FirstOrDefault(obj => obj.name == "Platfotms3");
            targetObject.SetActive(true);
            _hints._request = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            isTrigger = true;
            StartCoroutine(DestroyObj());
        }
        else if (_count == 2 && isTrigger == false)
        {
            _hints._request = true;
            isTrigger = true;
            StartCoroutine(DestroyObj());
        }
        
        else if (_count == 3 && isTrigger == false)
        {
            _hints._request = true;
            isTrigger = true;
            StartCoroutine(Comet());
            
        }
    }
    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(_secondsToDestroy);
        if(_count == 2)
        {
            StartCoroutine(Comet()); 
        }
        else
        {
            _prefabsAll.newPrefabName = _namePref;
            _hints._request = true;
            Debug.Log("Destroy" + gameObject.name);
            Destroy(gameObject);
        }   
    }
    IEnumerator Comet()
    {
        if (_count == 2)
        {
            for (int i = 0; i < 10; i++)
            {
                _prefabsAll.newPrefabName = (i % 2 == 0) ? "Comet1" : "Comet2";
                _hints._request = true;
                yield return new WaitForSeconds(0.1f);
            }
            _hints._corutinStop = true;
            Debug.Log("Coroutine stopped: " + _hints._corutinStop);
            Destroy(gameObject); // Уничтожаем объект после завершения цикла
        }
        for (int i = 0; i < 10; i++)
        {
            _prefabsAll.newPrefabName = (i % 2 == 0) ? "Comet1" : "Comet2";
            _hints._request = true;
            yield return new WaitForSeconds(_secondsToDestroy);
        }
        _hints._corutinStop = true;
        Debug.Log("Coroutine stopped: " + _hints._corutinStop);
        Destroy(gameObject); // Уничтожаем объект после завершения цикла
    }

}

